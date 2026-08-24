using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public record CategoryDtoForUpdate : CategoryDtoForManipulation
{
    [Required]
    public int Id { get; init; }
}
