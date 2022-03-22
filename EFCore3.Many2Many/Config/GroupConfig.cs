using EFCore3.Many2Many.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        }
    }
}