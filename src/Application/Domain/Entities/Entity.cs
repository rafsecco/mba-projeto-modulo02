using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Domain.Entities;

public class Entity
{
    [Key]
    [JsonIgnore]
    public Guid Id { get; set; }
}