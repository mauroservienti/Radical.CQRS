using Newtonsoft.Json;
using Radical.CQRS.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;

namespace Radical.CQRS
{
	public class NHibernateDomainEventCommit : DomainEventCommit
	{
		IDomainEvent _event;

		public override IDomainEvent Event
		{
			get
			{
				if( this._event == null )
				{
					var interfaceType = Type.GetType( this.EventType );
					var concreteType = ConcreteProxyCreator.GetConcreteType( interfaceType );
					var eventInstance = JsonConvert.DeserializeObject( this.EventBlob, concreteType );

					this._event = ( IDomainEvent )eventInstance;
				}

				return this._event;
			}
			set
			{
				Ensure.That( value ).IsNotNull();

				this._event = value;

				this.EventType = ConcreteProxyCreator.GetValidTypeName( this._event.GetType() );
				this.EventBlob = JsonConvert.SerializeObject( this._event );
			}
		}

		public string EventType { get; set; }
		public string EventBlob { get; set; }
	}
}
