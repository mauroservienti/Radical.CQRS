using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.ViewModels.Services
{
	class PeopleViewContextFactory : IViewContextFactory<IPeopleViewContext>
	{
		public IPeopleViewContext Create() 
		{
			return new PeopleViewContext();
		}
	}
}
