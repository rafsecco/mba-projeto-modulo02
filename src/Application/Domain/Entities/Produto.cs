using Core.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Domain.Entities
{
    public class Produto : Entity
    {
        public Guid VendedorId { get; set; }

        public Guid CategoriaId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Imagem")]
        public string Imagem { get; set; }

        [Currency]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0.00, 9999.00, ErrorMessage = "O campo {0} deve ser maior que {1} e menor que {2}")]
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, 9999, ErrorMessage = "O campo {0} deve ser maior que {1} e menor que {2}")]
        [Display(Name = "Quantidade")]
        public int Estoque { get; set; }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; } = true;

        [NotMapped]
        [JsonIgnore]
        public Vendedor Vendedor { get; set; }

        [NotMapped]
        public Categoria Categoria { get; set; }
    }
}