using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Configurations;

public class ServiceConfiguration
    : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Organization)
            .WithMany(x => x.Services)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(x => x.Name, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Name")
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Description, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Description")
                .HasMaxLength(1000)
                .IsRequired();
        });

        builder.Property(x => x.EstimatedDurationMinutes)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        builder.HasIndex(x => new
            {
                x.OrganizationId,
                x.Name
            })
            .IsUnique();
    }
}