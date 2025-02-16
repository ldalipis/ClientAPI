using System.ComponentModel.DataAnnotations;

namespace ClientCRUD.Models;

public interface IUnifiedRequestModel
{
    string Id { get; set; }
    string? Address { get; set; }
    string? Description { get; set; }
}

public class UnifiedRequestModel : IUnifiedRequestModel
{
    [Required]
    public required string Id { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
}
