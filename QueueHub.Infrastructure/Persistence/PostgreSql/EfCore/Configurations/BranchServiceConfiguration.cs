using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Configurations;

public class BranchServiceConfiguration
    : IEntityTypeConfiguration<BranchService>
{
    public void Configure(EntityTypeBuilder<BranchService> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Branch)
            .WithMany(x => x.BranchServices)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Service)
            .WithMany(x => x.BranchServices)
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => new
            {
                x.BranchId,
                x.ServiceId
            })
            .IsUnique();
    }
}