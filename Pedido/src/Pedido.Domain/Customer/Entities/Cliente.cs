using Pedido.Domain.Custumer.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pedido.Domain.Custumer.Entities
{
    public class Cliente
    {
        public Guid Id { get; }
        public string Cpf { get; }
        public Name Nome { get; private set; }

        protected Cliente() { }


        public Cliente(Guid id, string cpf, Name nome)
        {
            Id = id;
            Cpf = cpf;
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        }

        private Cliente(Name name)
        {
            Id = Guid.NewGuid();
            Nome = name;
        }
        public static Cliente Create(Name name)
        {
            return new Cliente(name);
        }

    }
}
