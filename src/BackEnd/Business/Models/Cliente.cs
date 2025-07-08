using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class Cliente
    {
        public Guid UserId { get; set; }
        [DisplayName("Ativo?")]
        public bool Ativo { get; set; } = true;
        public List<Produto>? Produtos;
    }
}
