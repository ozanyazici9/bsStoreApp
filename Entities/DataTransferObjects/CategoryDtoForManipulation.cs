using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public abstract record CategoryDtoForManipulation
{
    [Required(ErrorMessage = "CategoryName is a required field")]
    [MaxLength(50, ErrorMessage = "Maximum length for the CategoryName is 50 characters")]
    [MinLength(2, ErrorMessage = "Minimum length for the CategoryName is 2 characters")]
    public int CategoryName { get; init; }
}
