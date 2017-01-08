using System;
using System.Web;
using Gira.Data;
using Gira.Data.Entities;
using Gira.Data.Enums;
using Gira.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Business
{
    public class TransitionService
    {
        private readonly IStateMachine<IssueStatusCode, IssueTransition> _stateMachine;
        private readonly IGiraUoW _uoW;

        public TransitionService(IStateMachine<IssueStatusCode, IssueTransition> stateMachine, IGiraUoW uoW)
        {
            _stateMachine = stateMachine;
            _uoW = uoW;
        }

        public Issue Transition(Issue issue, IdentityUser user, IssueTransition transition)
        {
            //checks
            if(issue.ResponsibleUserId == null || issue.ManagerId == null || issue.CreatorId == null)
                  throw new BusinessException(BusinessErrors.IssueInvalid);

            var isManager = HttpContext.Current.User.IsInRole(SecurityRoles.Manager.ToString());
            var isDispatcher = HttpContext.Current.User.IsInRole(SecurityRoles.Manager.ToString());
            var isSolver = HttpContext.Current.User.IsInRole(SecurityRoles.Manager.ToString());
            var isAdmin = HttpContext.Current.User.IsInRole(SecurityRoles.Manager.ToString());

            var userId = HttpContext.Current.User.Identity.GetUserId();
            var managerId = issue.ManagerId;
            var responsibleId = issue.ResponsibleUserId;
            var originalUserId = issue.CreatorId;

            //if admin skip this step
            if (!isAdmin)
            {
                switch (transition)
                {
                    case IssueTransition.Assign:
                        if ((isManager && !managerId.Equals(userId)) || !isManager)
                            throw new BusinessException(BusinessErrors.UserNotAuthorized);
                        break;

                    case IssueTransition.Treat:
                        if ((!isDispatcher && !isSolver) || userId != responsibleId)
                            throw new BusinessException(BusinessErrors.UserNotAuthorized);
                        break;

                    case IssueTransition.Solve:
                        if ((!isDispatcher && !isSolver) || userId != responsibleId)
                            throw new BusinessException(BusinessErrors.UserNotAuthorized);
                        break;

                    case IssueTransition.Enquire:
                        if ((!isDispatcher && !isSolver) || userId != responsibleId)
                            throw new BusinessException(BusinessErrors.UserNotAuthorized);
                        break;

                    case IssueTransition.Refuse:
                        if ((!isDispatcher && !isSolver) || userId != responsibleId)
                            throw new BusinessException(BusinessErrors.UserNotAuthorized);
                        break;

                    case IssueTransition.Close:
                        if (userId != originalUserId)
                            throw new BusinessException(BusinessErrors.UserNotAuthorized);
                        break;

                    case IssueTransition.Cancel:
                        if (userId != originalUserId && ((!isDispatcher && !isSolver) || userId != responsibleId))
                            throw new BusinessException(BusinessErrors.UserNotAuthorized);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(transition), transition, null);
                }
            }

            issue.IssueStatusCode = _stateMachine.Transition(issue.IssueStatusCode, transition);
            issue.ResponsibleUserId = userId;

            _uoW.Issues.Update(issue);
            _uoW.SaveAsync();

            return issue;
        }
    }
}