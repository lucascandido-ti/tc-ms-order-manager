using Entities = Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Product
{
    public class ProductConfiguration : IEntityTypeConfiguration<Entities.Product>
    {
        public void Configure(EntityTypeBuilder<Entities.Product> builder)
        {
            builder.HasKey(e => e.Id);
            builder.OwnsOne(e => e.Price)
                   .Property(e => e.Currency);
            builder.OwnsOne(e => e.Price)
                .Property(e => e.Value);
        }
    }
}
