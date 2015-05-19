using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sample.ViewModels
{
	public class PersonView
	{
		private PersonView()
		{
			this.Addresses = new List<AddressView>();
		}

		public Guid Id { get; private set; }

		public String Name { get; private set; }

		public int Version { get; private set; }

		public ICollection<AddressView> Addresses { get; private set; }

		public BornInfoView BornInfo { get; private set; }
	}

	public class AddressView
	{
		private AddressView()
		{

		}

		public Guid AddressId { get; private set; }

		public Guid PersonId { get; private set; }

		public String Street { get; private set; }
	}

	public class BornInfoView
	{
		private BornInfoView()
		{

		}

		public string Where { get; private set; }

		public DateTimeOffset When { get; private set; }
	}
}
