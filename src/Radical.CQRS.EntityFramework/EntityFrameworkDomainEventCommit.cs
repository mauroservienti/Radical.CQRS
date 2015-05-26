using Newtonsoft.Json;
using Radical.CQRS.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;

namespace Radical.CQRS
{
	class EntityFrameworkDomainEventCommit : DomainEventCommit
	{
		IDomainEvent _event;

		[NotMapped]
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

		string EventType { get; set; }
		string EventBlob { get; set; }
	}
}
