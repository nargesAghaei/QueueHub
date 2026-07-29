using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Configurations;

public class UserConfiguration:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.UserRoles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        
        builder.OwnsOne(x => x.FirstName, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("FirstName")
                .HasMaxLength(UserConstants.MaxFirstNameLength)
                .IsRequired();
        });
        
        builder.OwnsOne(x => x.LastName, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("LastName")
                .HasMaxLength(UserConstants.MaxLastNameLength)
                .IsRequired();
        });
        
        builder.OwnsOne(x => x.UserName, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("UserName")
                .HasMaxLength(UserConstants.MaxUserNameLength)
                .IsRequired();

            b.HasIndex(x => x.Value).IsUnique();
        });
        
        builder.OwnsOne(x => x.PhoneNumber, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(UserConstants.PhoneNumberLength)
                .IsRequired();
            
            b.HasIndex(x => x.Value).IsUnique();   
        });
        
        builder.OwnsOne(x => x.PasswordHash, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Email, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("Email")
                .HasMaxLength(UserConstants.MaxEmailLength);
        });
    }
}