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
		public Guid Id { get; set; }

		public Guid PersonId { get; set; }

		public String Street { get; set; }
	}
}
