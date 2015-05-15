using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;
using Topics.Radical;

namespace Radical.CQRS.Server.Services
{
	class OperationContextManager : IOperationContextManager
	{
		readonly IServiceProvider container;

		public OperationContextManager( IServiceProvider container )
		{
			Ensure.That( container ).Named( () => container ).IsNotNull();

			this.container = container;
		}

		public IOperationContext GetCurrent()
		{
			return this.container.GetService<IOperationContext>();
		}
	}
}
