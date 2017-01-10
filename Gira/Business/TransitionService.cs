using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Gira.Business.Interfaces;
using Gira.Data;
using Gira.Data.Entities;
using Gira.Data.Enums;
using Gira.Resources;
using Gira.Utilities;
using Microsoft.AspNet.Identity;

namespace Gira.Business
{
    public class TransitionService : ITransitionService
    {
        private readonly IStateMachine<IssueStatusCode, IssueTransition> _stateMachine;
        private readonly IGiraUoW _db;

        public TransitionService(IStateMachine<IssueStatusCode, IssueTransition> stateMachine, IGiraUoW db)
        {
            _stateMachine = stateMachine;
            _db = db;
        }

        public async Task<Issue> Transition(Issue issue, IssueTransition transition, string userId)
        {
            //if transition not in possible transitions
            if(!GetTransitions(issue).Contains(transition))
                throw new BusinessException(BusinessErrors.TransitionIsImpossible);

            issue.IssueStatusCode = _stateMachine.Transition(issue.IssueStatusCode, transition);

            await TransferOwnerShip(issue, transition, userId);

            //todo: add history

            _db.Issues.Update(issue);
            await _db.SaveAsync();

            return issue;
        }

        //should have put this in stateless as well
        private async Task<Issue> TransferOwnerShip(Issue issue, IssueTransition transition, string userId)
        {
            switch (transition)
            {
                case IssueTransition.Assign:
                    var user = await _db.Users.GetAsync(userId);
                    var solverRole = await _db.Roles.SingleOrDefaultAsync(r => r.Name.ToLower().Equals("solver"));

                    if (user?.Roles.FirstOrDefault(r => r.RoleId == solverRole.Id) == null)
                        throw new BusinessException(BusinessErrors.TargetUserInvalid);

                    issue.ResponsibleUserId = userId;
                    break;

                case IssueTransition.Cancel:
                case IssueTransition.Close:
                case IssueTransition.Refuse:
                    issue.ResponsibleUserId = null;
                    break;

                case IssueTransition.Enquire:
                case IssueTransition.Solve:
                    issue.ResponsibleUserId = issue.CreatorId;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(transition), transition, null);
            }
            return null;
        }

        public IEnumerable<IssueTransition> GetTransitions(Issue issue)
        {
            //possible transitions in this state
            var stateTransitions = _stateMachine.GetTransitions(issue.IssueStatusCode);

            //possible transitions for the current role
            var userTransitions = GetRoleTransitions(issue);

            //insersection of both lists to get possible transitions
            return stateTransitions.Intersect(userTransitions);
        }

        // Should've put this in Stateless. Ain't got no time fo that
        private static IEnumerable<IssueTransition> GetRoleTransitions(Issue issue)
        {
            if (HttpContext.Current.User.IsInRole(SecurityRoles.Administrator.ToString()))
            {
                //return all transitions
                return Enum.GetValues(typeof(IssueTransition)).Cast<IssueTransition>();
            }

            //checks
            if (issue.ResponsibleUserId == null || issue.CreatorId == null)
                throw new BusinessException(BusinessErrors.IssueInvalid);

            var isDispatcher = HttpContext.Current.User.IsInRole(SecurityRoles.Manager.ToString());
            var isSolver = HttpContext.Current.User.IsInRole(SecurityRoles.Manager.ToString());

            var userId = HttpContext.Current.User.Identity.GetUserId();
            var responsibleId = issue.ResponsibleUserId;
            var originalUserId = issue.CreatorId;

            //make lost of transitions and return possible transitions
            var transitions = new List<IssueTransition>();

            if (isDispatcher)
                transitions.Add(IssueTransition.Assign);

            if ((isDispatcher || isSolver) && userId == responsibleId)
            {
                transitions.Add(IssueTransition.Solve);
                transitions.Add(IssueTransition.Enquire);
                transitions.Add(IssueTransition.Refuse);
            }

            if (userId == originalUserId)
                transitions.Add(IssueTransition.Close);

            if (isDispatcher || (isSolver && userId == responsibleId) || userId == originalUserId)
                transitions.Add(IssueTransition.Cancel);

            return transitions;
        }
    }
}