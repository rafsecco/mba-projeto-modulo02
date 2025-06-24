using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Domain.Entities
{
    public class Categoria : Entity
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<Produto>? Produtos { get; set; }
    }
}