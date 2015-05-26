using NHibernate;
using System;
using Topics.Radical;

namespace Radical.CQRS.Services
{
	class AggregateFinderProvider : IAggregateFinderProvider<ISession>
	{
		readonly IServiceProvider container;

		public AggregateFinderProvider( IServiceProvider container )
		{
			this.container = container;
		}

		public IAggregateFinder<ISession, TAggregate> GetFinder<TAggregate>() where TAggregate : class, IAggregate
		{
			return this.container.GetService<IAggregateFinder<ISession, TAggregate>>();
		}

		public IDefaultAggregateFinder<ISession> GetDefaultFinder()
		{
			return this.container.GetService<IDefaultAggregateFinder<ISession>>();
		}
	}
}
