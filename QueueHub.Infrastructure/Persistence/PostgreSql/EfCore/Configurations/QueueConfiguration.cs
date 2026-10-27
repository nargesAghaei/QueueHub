using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Configurations;

public class QueueConfiguration : IEntityTypeConfiguration<Queue>
{
    public void Configure(EntityTypeBuilder<Queue> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasQueryFilter(q => !q.IsDeleted);
        
        builder.HasOne(x => x.BranchService)
            .WithMany(x => x.Queues)
            .HasForeignKey(x => x.BranchServiceId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.QueueDate)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        builder.HasIndex(x => new
            {
                x.BranchServiceId,
                x.QueueDate
            })
            .IsUnique();
    }
}