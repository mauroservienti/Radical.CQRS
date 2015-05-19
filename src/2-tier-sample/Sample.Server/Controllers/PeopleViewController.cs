using System;
using System.Linq;
using System.Web.OData;
using Sample.ViewModels;

namespace Sample.Server.Controllers
{
	public class PeopleViewController : ODataController
	{
		PeopleViewContext db = new PeopleViewContext();

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
