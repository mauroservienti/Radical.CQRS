using Sample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace Sample.Server.Controllers
{
	public class PeopleViewController : ODataController
	{
		PeopleViewDbContext db = new PeopleViewDbContext();

		protected override void Dispose( bool disposing )
		{
			this.db.Dispose();
			base.Dispose( disposing );
		}

		[EnableQuery]
		public IQueryable<PersonView> Get()
		{
			return this.db.PeopleView;
		}

		[EnableQuery]
		public PersonView Get( [FromODataUri]Guid key )
		{
			var result = this.db.PeopleView.Where( p => p.Id == key )
				.SingleOrDefault();
			return result;
		}
	}
}
