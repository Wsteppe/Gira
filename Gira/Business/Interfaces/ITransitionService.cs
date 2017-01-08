using System.Collections.Generic;
using Gira.Data.Entities;
using Gira.Data.Enums;

namespace Gira.Business.Interfaces
{
    public interface ITransitionService
    {
        /// <summary>
        /// Checks if transition is possible and sets new status, and creates history
        /// </summary>
        /// <param name="issue"></param>
        /// <param name="transition"></param>
        /// <returns></returns>
        Issue Transition(Issue issue, IssueTransition transition);

        /// <summary>
        /// wrapper method for statemachine
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        IEnumerable<IssueTransition> GetTransitions(Issue issue);
    }
}
