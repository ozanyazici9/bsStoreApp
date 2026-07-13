using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bsStoreApp.Repositories.Config;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace bsStoreApp.Repositories;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfig());
    }
}
