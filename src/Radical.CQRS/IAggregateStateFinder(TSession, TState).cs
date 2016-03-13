using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;

namespace Radical.CQRS
{
    public interface IAggregateStateFinder<TSession>
    {
        IAggregateState FindById(TSession session, AggregateQuery aggregateQuery);

        IEnumerable<IAggregateState> FindById(TSession session, params AggregateQuery[] aggregateQueries);
    }

    [Contract]
    public interface IAggregateStateFinder<TSession, TState> : IAggregateStateFinder<TSession> where TState : class, IAggregateState
    {

    }
}
