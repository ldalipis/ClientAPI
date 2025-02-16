using System.ComponentModel.DataAnnotations;

namespace ClientCRUD.Models;

public class UnifiedResponseModel
{
    [Required]
    public required string Id { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Source { get; set; }
}
