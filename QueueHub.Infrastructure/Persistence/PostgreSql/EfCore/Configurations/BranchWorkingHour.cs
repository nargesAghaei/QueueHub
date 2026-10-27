using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Configurations;

public class BranchWorkingHourConfiguration
    : IEntityTypeConfiguration<BranchWorkingHour>
{
    public void Configure(EntityTypeBuilder<BranchWorkingHour> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasQueryFilter(wh => !wh.IsDeleted);
        
        builder.HasOne(x => x.Branch)
            .WithMany(x => x.WorkingHours)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.DayOfWeek)
            .IsRequired();

        builder.Property(x => x.StartTime)
            .HasColumnType("time");

        builder.Property(x => x.EndTime)
            .HasColumnType("time");

        builder.Property(x => x.IsClosed)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        builder.HasIndex(x => new
            {
                x.BranchId,
                x.DayOfWeek
            })
            .IsUnique();
    }
}