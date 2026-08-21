using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EFCore.Config;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "1",
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "11111111-1111-1111-1111-111111111111",
            },
            new IdentityRole
            {
                Id = "2",
                Name = "Editor",
                NormalizedName = "EDITOR",
                ConcurrencyStamp = "22222222-2222-2222-2222-222222222222",
            },
            new IdentityRole
            {
                Id = "3",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "33333333-3333-3333-3333-333333333333",
            }
        );
    }
}
