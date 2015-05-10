using Radical.CQRS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Reflection;

namespace Radical.CQRS.Data
{
    public class StateContext: DbContext
    {
        readonly Assembly[] domains;
        public StateContext(params Assembly[] domains )
        {
            this.domains = domains;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var aggregates = this.domains.SelectMany(a => a.DefinedTypes).Where(t => t.Is<IAggregate>());

            foreach (var item in aggregates)
            {
                modelBuilder.RegisterEntityType(item);                
            }

            modelBuilder.RegisterEntityType(typeof(DomainEventCommit));
                
            modelBuilder
              .Types()
              .Configure(c =>
              {
                  var nonPublicProperties = GetProperties(c.ClrType);

                  foreach (var p in nonPublicProperties)
                  {
                      c.Property(p).HasColumnName(p.Name);
                      if (p.Name == "Id")
                      {
                          c.Property(p).IsKey();
                      }
                  }
              });
        }

        IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            var all = new List<Type>();
            all.Add(type);
            var current = type;
            while (current.BaseType != null)
            {
                all.Add(current.BaseType);
                current = current.BaseType;
            }

            var props = all.SelectMany(t => t.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)).Where(p =>             
            {
                return !p.IsAttributeDefined<NotMappedAttribute>();
            });

            return props;

        }


    }
}
