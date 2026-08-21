using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EFCore.Config;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.CategoryId); // PK
        builder.Property(c => c.CategoryName).IsRequired().HasMaxLength(50);

        builder.HasData(
            new Category { CategoryId = 1, CategoryName = "Fiction" },
            new Category { CategoryId = 2, CategoryName = "Non-Fiction" },
            new Category { CategoryId = 3, CategoryName = "Self-Help" }
        );
    }
}
