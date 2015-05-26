using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS
{
	public abstract class AggregateState : IAggregateState
	{
		Guid _id = Guid.NewGuid();

		public Guid Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		public int Version { get; set; }
	}
}
