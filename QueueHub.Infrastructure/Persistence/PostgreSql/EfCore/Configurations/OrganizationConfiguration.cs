using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Configurations;

public class OrganizationConfiguration
    : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasQueryFilter(o => !o.IsDeleted);
        
        builder.HasOne(x => x.OrganizationCategory)
            .WithMany(x => x.Organizations)
            .HasForeignKey(x => x.OrganizationCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Manager)
            .WithOne(x => x.Organization)
            .HasForeignKey<User>(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        
        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.OwnsOne(x => x.Name, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Name")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Description, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Description")
                .IsRequired();
        });

        builder.OwnsOne(x => x.PhoneNumber, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("PhoneNumber")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Email, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Email")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Address, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Address");
        });

        builder.Property(x => x.OrganizationCode)
            .IsRequired();

        builder.HasIndex(x => x.OrganizationCode)
            .IsUnique();

        builder.Property(x => x.Website);

        builder.Property(x => x.CityId);

        builder.Property(x => x.LogoUrl);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired();
    }
}