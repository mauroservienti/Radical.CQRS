using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.People
{
	internal class Address
	{
		public virtual Guid Id { get; set; }

		public virtual Guid PersonId { get; set; }

		public virtual String Street { get; set; }
	}
}
