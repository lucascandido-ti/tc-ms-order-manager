﻿using Entities = Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Customer;
using Data.Category;
using Data.Product;
using Domain.Entities;

namespace Data
{
    public class DataDbContext: DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public virtual DbSet<Entities.Customer> Customers { get; set; }
        public virtual DbSet<Entities.Category> Categories { get; set; }
        public virtual DbSet<Entities.Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<Entities.Customer>()
            .Property(e => e.CreatedAt)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder
            .Entity<Entities.Customer>()
            .Property(e => e.LastUpdatedAt)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder.Entity<Entities.Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .UsingEntity(j => j.ToTable("ProductCategory"));

            modelBuilder
                   .ApplyConfiguration(new CustomerConfiguration())
                   .ApplyConfiguration(new CategoryConfiguration())
                   .ApplyConfiguration(new ProductConfiguration());
        }
    }
}
