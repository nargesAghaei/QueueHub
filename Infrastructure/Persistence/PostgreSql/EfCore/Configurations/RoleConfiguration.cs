using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.SqlServer.EfCore.Configurations;

public class RoleConfiguration:IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.UserRoles)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId);
        
        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(
            Role.Seed(
                id: RoleIds.Citizen,
                name: RoleNames.Citizen),
            Role.Seed(
                id: RoleIds.Manager,
                name: RoleNames.Manager),
            Role.Seed(
                id: RoleIds.Staff,
                name: RoleNames.Staff),
            Role.Seed(
                id: RoleIds.Admin,
                name: RoleNames.Admin)
        );
    }
}