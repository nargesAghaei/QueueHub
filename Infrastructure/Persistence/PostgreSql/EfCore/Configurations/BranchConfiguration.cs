using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Configurations;

public class BranchConfiguration
    : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Organization)
            .WithMany(x => x.Branches)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Staff)
            .WithOne(x => x.Branch)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.OwnsOne(x => x.Name, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Name")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Address, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Address")
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
                .HasColumnName("PhoneNumber");
        });

        builder.Property(x => x.City)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired();
    }
}