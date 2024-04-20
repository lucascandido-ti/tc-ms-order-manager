using Entities = Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Customer;

namespace Data
{
    public class DataDbContext: DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public virtual DbSet<Entities.Customer> Customers { get; set; }

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

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }
    }
}
