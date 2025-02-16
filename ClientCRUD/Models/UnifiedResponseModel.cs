using System.ComponentModel.DataAnnotations;

namespace ClientCRUD.Models;

public interface IUnifiedResponseModel
{
    string Id { get; set; }
    string? Address { get; set; }
    string? Description { get; set; }
    string Source { get; set; }
}

public class UnifiedResponseModel : IUnifiedResponseModel
{
    [Required]
    public required string Id { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public required string Source { get; set; }
}
