using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Models;

namespace WebApi.Data.Configurations
{
    public class EntityConfiguration : IEntityTypeConfiguration<Entity>
    {
        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("Entity");

            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
