using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class Entity
{
    [Key]
    public Guid Id { get; set; }
}