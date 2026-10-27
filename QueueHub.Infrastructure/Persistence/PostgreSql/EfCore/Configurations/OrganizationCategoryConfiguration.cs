using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Configurations;

public class OrganizationCategoryConfiguration
    : IEntityTypeConfiguration<OrganizationCategory>
{
    public void Configure(EntityTypeBuilder<OrganizationCategory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasQueryFilter(oc => !oc.IsDeleted);
        
        builder.OwnsOne(x => x.Name, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Name")
                .HasMaxLength(100)
                .IsRequired();

            b.HasIndex(x => x.Value)
                .IsUnique();
        });

        builder.OwnsOne(x => x.Description, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Description")
                .HasMaxLength(500);
        });

        builder.Property(x => x.IsDeleted)
            .IsRequired();
    }
}