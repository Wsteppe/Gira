using System.Collections.Generic;

namespace Gira.Business.Interfaces
{
    public interface IStateMachine<T, TU>
    {
        T Transition(T state, TU transition);

        IEnumerable<TU> GetTransitions(T code);
    }
}
