using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;

namespace Radical.CQRS.Server.Runtime
{
	class OperationContext : IOperationContext
	{
		readonly Dictionary<String, Object> data = new Dictionary<string, object>();

		public void Dispose()
		{
			this.CorrelationId = null;
			this.data.Clear();
		}

		public IOperationContext ForOperation( string correlationId )
		{
			//Il correlationId non deve essere obbligatorio
			//quello che deve essere obbbligatorio è che non
			//è possibile cambiarlo dopo...
			//Ensure.That( correlationId ).Named( () => correlationId ).IsNotNullNorEmpty();
			Ensure.That( this.CorrelationId ).Is( null );

			this.CorrelationId = correlationId;

			return this;
		}


		public string CorrelationId
		{
			get;
			private set;
		}

		public void Add( string key, object value )
		{
			this.data.Add( key, value );
		}

		public T Get<T>( string key )
		{
			return ( T )this.data[ key ];
		}
	}
}
