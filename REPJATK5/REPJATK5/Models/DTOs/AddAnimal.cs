using System.ComponentModel.DataAnnotations;

namespace REPJATK5.Models.DTOs;

public class AddAnimal
{
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Descritpion { get; set; }
    public string Category { get; set; }
    public string Area { get; set; }
}