using Radical.CQRS.Data;
using System;
using Topics.Radical;

namespace Radical.CQRS.Services
{
	class AggregateFinderProvider : IAggregateFinderProvider<DomainContext>
	{
		readonly IServiceProvider container;

		public AggregateFinderProvider( IServiceProvider container )
		{
			this.container = container;
		}

		public IAggregateFinder<DomainContext, TAggregate> GetFinder<TAggregate>() where TAggregate : class, IAggregate
		{
			return this.container.GetService<IAggregateFinder<DomainContext, TAggregate>>();
		}

		public IDefaultAggregateFinder<DomainContext> GetDefaultFinder()
		{
			return this.container.GetService<IDefaultAggregateFinder<DomainContext>>();
		}
	}
}
