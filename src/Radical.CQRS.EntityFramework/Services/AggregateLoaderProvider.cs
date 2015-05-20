using System;
using Topics.Radical;

namespace Radical.CQRS.Services
{
	class AggregateLoaderProvider
	{
		readonly IServiceProvider container;

		public AggregateLoaderProvider( IServiceProvider container )
		{
			this.container = container;
		}

		public IAggregateLoader<TAggregate> GetLoader<TAggregate>() where TAggregate : class, IAggregate
		{
			return this.container.GetService<IAggregateLoader<TAggregate>>();
		}

		//public IAggregateAsyncLoader<TAggregate> GetAsyncLoader<TAggregate>() where TAggregate : class, IAggregate
		//{
		//	return this.container.GetService<IAggregateAsyncLoader<TAggregate>>();
		//}
	}
}
