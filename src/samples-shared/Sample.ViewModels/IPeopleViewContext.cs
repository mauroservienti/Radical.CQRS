using System;
using System.Linq;
namespace Sample.ViewModels
{
	public interface IPeopleViewContext: IDisposable
	{
		IQueryable<PersonView> PeopleView { get; }
	}
}
