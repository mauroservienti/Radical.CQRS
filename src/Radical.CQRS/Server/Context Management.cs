using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS.Server
{
	public interface IOperationContextManager
	{
		IOperationContext GetCurrent();
	}

	public interface IOperationContext : IDisposable
	{
		IOperationContext ForOperation( String correlationId );
		String CorrelationId { get; }

		void Add( string key, Object value );
		T Get<T>( string key );
	}
}
