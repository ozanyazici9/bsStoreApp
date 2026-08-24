using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EFCore.Config;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id); // PK
        builder.Property(c => c.CategoryName).IsRequired().HasMaxLength(50);

        builder.HasData(
            new Category { Id = 1, CategoryName = "Fiction" },
            new Category { Id = 2, CategoryName = "Non-Fiction" },
            new Category { Id = 3, CategoryName = "Self-Help" }
        );
    }
}
