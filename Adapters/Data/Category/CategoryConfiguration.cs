using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Domain.Entities;

namespace Data.Category
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Entities.Category>
    {
        public void Configure(EntityTypeBuilder<Entities.Category> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
