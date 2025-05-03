using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities;

public class Entity
{
    [Key]
    public Guid Id { get; set; }
}