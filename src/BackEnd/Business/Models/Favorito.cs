using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Business.Models;

public class Favorito : Entity
{
    public Guid ClienteId { get; set; }
    public Guid ProdutoId { get; set; }

    [NotMapped]
    [JsonIgnore]
    public Cliente Cliente { get; set; }

    [NotMapped]
    public Produto Produto { get; set; }
}