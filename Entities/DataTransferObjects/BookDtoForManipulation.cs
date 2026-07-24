using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects;

public abstract record BookDtoForManipulation
{
    [Required(ErrorMessage = "Title is a required field")]
    [MaxLength(50, ErrorMessage = "Maximum length for the Title is 50 characters")]
    [MinLength(2, ErrorMessage = "Minimum length for the Title is 2 characters")]
    public string Title { get; init; } = null!;

    [Required(ErrorMessage = "Price is a required field")]
    [Range(10, 1000)]
    public decimal Price { get; init; }
}
