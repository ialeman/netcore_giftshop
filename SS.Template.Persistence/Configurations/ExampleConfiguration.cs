using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SS.Template.Core;
using SS.Template.Domain.Entities;

namespace SS.Template.Persistence.Configurations
{
    public sealed class ExampleConfiguration : IEntityTypeConfiguration<Example>
    {
        public void Configure(EntityTypeBuilder<Example> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AppConstants.StandardValueLength);

            builder.Property(x => x.NormalizedName)
                .IsRequired()
                .HasMaxLength(AppConstants.StandardValueLength);

            builder.HasIndex(x => x.NormalizedName)
                .IsUnique();

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(AppConstants.EmailLength);
        }
    }
}
