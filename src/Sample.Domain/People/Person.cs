using Radical.CQRS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.People
{
    public class Person: Aggregate
    {
		protected override Guid Id { get; set; }

        internal Person()
        {

        }

        public static Person CreateNew(string nome)
        {
            var p = new Person();
            p.Nome = nome;
            p.SetupCompleted();
            return p;            
        }

		private void SetupCompleted()
		{
			this.RaiseEvent<IPersonaCreata>( e => e.NuovoNome = this.Nome );
		}

        private string Nome { get; set; }

        public void CambiaNome(string nome)
        {
            this.Nome = nome;
            this.RaiseEvent<INomeCambiato>(e => e.NuovoNome = nome);
        }
	}
}
