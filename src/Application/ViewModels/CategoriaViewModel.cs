using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels;

public class CreateCategoriaViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
    [Display(Name = "Nome")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(300, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; }
}

public class UpdateCategoriaViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid Id { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
    [Display(Name = "Nome")]
    public string? Nome { get; set; }

    [StringLength(300, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres")]
    [Display(Name = "Descrição")]
    public string? Descricao { get; set; }
}