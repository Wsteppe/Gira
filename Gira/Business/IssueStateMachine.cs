using System;
using System.Collections.Generic;
using Gira.Data.Enums;
using Stateless;

namespace Gira.Business
{
    public class IssueStateMachine : IStateMachine<IssueStatusCode,IssueTransition>
    {
        private StateMachine<IssueStatusCode, IssueTransition> _stateMachine;

        public IssueStatusCode Transition(IssueStatusCode code, IssueTransition transition)
        {
            ConfigureStateMachine(code);

            if(!_stateMachine.CanFire(transition))
                throw new BusinessException("Transition is impossible");

            _stateMachine.Fire(transition);
            return _stateMachine.State;
        }
        public IEnumerable<IssueTransition> GetTransitions(IssueStatusCode code)
        {
            ConfigureStateMachine(code);

            return _stateMachine.PermittedTriggers;
        }

        private void ConfigureStateMachine(IssueStatusCode code)
        {
            _stateMachine = new StateMachine<IssueStatusCode,IssueTransition>(code);

            //manager
            _stateMachine.Configure(IssueStatusCode.New)
                .Permit(IssueTransition.Assign, IssueStatusCode.Assigned)
                .Permit(IssueTransition.Cancel, IssueStatusCode.Canceled);

            //dispatcher
            _stateMachine.Configure(IssueStatusCode.Assigned)
                .Permit(IssueTransition.Enquire, IssueStatusCode.Enquiring)
                .Permit(IssueTransition.Refuse, IssueStatusCode.Refused)
                .Permit(IssueTransition.Treat, IssueStatusCode.Processing)
                .Permit(IssueTransition.Cancel, IssueStatusCode.Canceled);

            //solver or dispatcher
            _stateMachine.Configure(IssueStatusCode.Processing)
                .Permit(IssueTransition.Enquire, IssueStatusCode.Enquiring)
                .Permit(IssueTransition.Solve, IssueStatusCode.Solved)
                .Permit(IssueTransition.Refuse, IssueStatusCode.Refused)
                .Permit(IssueTransition.Cancel, IssueStatusCode.Canceled);

            //client
            _stateMachine.Configure(IssueStatusCode.Solved)
                .Permit(IssueTransition.Close, IssueStatusCode.Closed)
                .Permit(IssueTransition.Cancel, IssueStatusCode.Canceled);

            //solver
            _stateMachine.Configure(IssueStatusCode.Refused)
                .Permit(IssueTransition.Assign, IssueStatusCode.Assigned)
                .Permit(IssueTransition.Cancel, IssueStatusCode.Canceled);
        }
    }
}