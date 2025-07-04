using System.ComponentModel.DataAnnotations;

namespace Business.Domain.Entities;

public class Entity
{
    [Key]
    public Guid Id { get; set; }
}