using System.ComponentModel;

namespace Business.Models
{
    public class Cliente : Entity
    {
        public Guid UserId { get; set; }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; } = true;

        public List<Favorito> Favoritos;
    }
}