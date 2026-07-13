using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bsStoreApp.Repositories.Config;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(
            new Book
            {
                Id = 1,
                Title = "The Lord of the Rings",
                Price = 999,
            },
            new Book
            {
                Id = 2,
                Title = "The Hobbit",
                Price = 899,
            },
            new Book
            {
                Id = 3,
                Title = "The Silmarillion",
                Price = 799,
            }
        );
    }
}
