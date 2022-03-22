using EFCore3.Many2Many.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace EFCore3.Many2Many.Config
{
    public class GroupConfig : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(t => t.GroupID).HasName("pk_tb_group");

            builder.ToTable("tb_group");

            builder.Property(t => t.GroupID)
              .HasColumnName("GroupID")
              .ValueGeneratedOnAdd()
              .IsRequired()
              .HasColumnType("int");

            builder.Property(t => t.Name)
              .HasColumnName("Name")
              .HasColumnType("varchar")
              .HasMaxLength(100)
              .IsRequired();

            builder.HasMany(t => t.Users)
                   .WithMany(t => t.Groups)
                   .UsingEntity<Dictionary<string, object>>("UserGroup",
                    leftTable => leftTable.HasOne<User>()
                                          .WithMany()
                                          .HasForeignKey("UserID")
                                          .HasConstraintName("fk_user_group_user"),
                    rightTable => rightTable.HasOne<Group>()
                                            .WithMany()
                                            .HasForeignKey("GroupID")
                                            .HasConstraintName("fk_group_group_user"),
                    j =>
                        {
                            j.HasKey("UserID", "GroupID").HasName("pk_group_user");
                            j.ToTable("tb_group_user");
                        });
        }
    }
}