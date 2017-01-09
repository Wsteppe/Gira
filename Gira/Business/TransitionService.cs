using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IGiraUoW _uoW;

        public TransitionService(IStateMachine<IssueStatusCode, IssueTransition> stateMachine, IGiraUoW uoW)
        {
            _stateMachine = stateMachine;
            _uoW = uoW;
        }

        public Issue Transition(Issue issue, IssueTransition transition)
        {
            //if transition not in possible transitions
            if(!GetTransitions(issue).Contains(transition))
                throw new BusinessException(BusinessErrors.TransitionIsImpossible);

            issue.IssueStatusCode = _stateMachine.Transition(issue.IssueStatusCode, transition);

            //todo: add history

            _uoW.Issues.Update(issue);
            _uoW.SaveAsync();

            return issue;
        }

        public IEnumerable<IssueTransition> GetTransitions(Issue issue)
        {
            //possible transitions in this state
            var stateTransitions = _stateMachine.GetTransitions(issue.IssueStatusCode);

            //possible transitions for the current user
            var userTransitions = GetUserTransitions(issue);

            //insersection of both lists to get possible transitions
            return stateTransitions.Intersect(userTransitions);
        }

        // need method gettransitions with params: issue & userRole
        private static IEnumerable<IssueTransition> GetUserTransitions(Issue issue)
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

            if(isDispatcher)
                transitions.Add(IssueTransition.Assign);

            if ((isDispatcher || isSolver) && userId == responsibleId)
            {
                transitions.Add(IssueTransition.Treat);
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