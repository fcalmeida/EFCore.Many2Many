using EFCore6.Many2Many.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore6.Many2Many.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(t => t.UserID).HasName("pk_tb_user");

            builder.ToTable("tb_user");

            builder.Property(t => t.UserID)
              .HasColumnName("UserID")
              .ValueGeneratedOnAdd()
              .IsRequired()
              .HasColumnType("int");

            builder.Property(t => t.UserName)
              .HasColumnName("UserName")
              .HasColumnType("varchar")
              .HasMaxLength(100)
              .IsRequired();
        }
    }
}
