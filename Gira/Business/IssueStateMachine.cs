using System;
using System.Collections.Generic;
using Gira.Business.Interfaces;
using Gira.Data.Enums;
using Gira.Utilities;
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

            //dispatcher
            _stateMachine.Configure(IssueStatusCode.New)
                .Permit(IssueTransition.Assign, IssueStatusCode.Processing)
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
                .Permit(IssueTransition.Assign, IssueStatusCode.Processing)
                .Permit(IssueTransition.Cancel, IssueStatusCode.Canceled);

            _stateMachine.Configure(IssueStatusCode.Enquiring)
                .Permit(IssueTransition.Cancel, IssueStatusCode.Canceled)
                .Permit(IssueTransition.Respond, IssueStatusCode.New);
        }
    }
}