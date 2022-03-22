using EFCore3.Many2Many.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore3.Many2Many.Config
{
    public class UserGroupConfig : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasKey(t => new { t.UserID, t.GroupID }).HasName("pk_user_group");

            builder.ToTable("tb_user_group");

            builder.Property(t => t.UserID)
                .HasColumnName("UserID")
                .ValueGeneratedOnAdd()
                .IsRequired()
                .HasColumnType("int");

            builder.Property(t => t.GroupID)
                .HasColumnName("GroupID")
                .ValueGeneratedOnAdd()
                .IsRequired()
                .HasColumnType("int");

            builder.HasOne(t => t.User)
                .WithMany(t => t.UserGroup)
                .HasForeignKey(t => t.UserID)
                .HasConstraintName("fk_user_group_user");

            builder.HasOne(t => t.Group)
                .WithMany(t => t.UserGroup)
                .HasForeignKey(t => t.GroupID)
                .HasConstraintName("fk_group_group_user");
        }
    }
}
