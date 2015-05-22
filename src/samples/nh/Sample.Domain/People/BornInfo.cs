using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.People
{
	class BornInfo
	{
		public virtual string Where { get; set; }

		public virtual DateTimeOffset When { get; set; }
	}
}
