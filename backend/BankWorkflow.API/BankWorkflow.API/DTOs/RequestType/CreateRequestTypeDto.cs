using System.ComponentModel.DataAnnotations;

namespace BankWorkflow.API.DTOs.RequestType;

public class CreateRequestTypeDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}