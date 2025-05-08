using Core.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Domain.Entities
{
    public class Product : Entity
    {
        public Guid SellerId { get; set; }

        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Imagem")]
        public string Image { get; set; }

        [Currency]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0.00, 9999.00, ErrorMessage = "O campo {0} deve ser maior que {1} e menor que {2}")]
        [Display(Name = "Preço")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, 9999, ErrorMessage = "O campo {0} deve ser maior que {1} e menor que {2}")]
        [Display(Name = "Quantidade")]
        public int Stock { get; set; }

        [NotMapped]
        [JsonIgnore]
        public Seller Seller { get; set; }

        [NotMapped]
        public Category Category { get; set; }
    }
}