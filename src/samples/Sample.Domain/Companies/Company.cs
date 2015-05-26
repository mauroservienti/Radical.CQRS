using Radical.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Companies
{
	public class Company : Aggregate<Company.State>
	{
		public class State : AggregateState
		{
			public string Name { get; set; }
		}

		public class Factory
		{
			public Company CreateNew( string nome )
			{
				var state = new Company.State()
				{
					Name = nome
				};

				var aggregate = new Company( state );
				aggregate.SetupCompleted();

				return aggregate;
			}
		}

		private Company()
		{

		}

		private Company( Company.State state )
			: base( state )
		{

		}

		private void SetupCompleted()
		{
			this.RaiseEvent<ICompanyCreated>( e => e.Name = this.Data.Name );
		}
	}
}
