using EFCore6.Many2Many.Config;
using EFCore6.Many2Many.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore6.Many2Many.Context
{
    partial class EF6Context : DbContext
    {
        public EF6Context(DbContextOptions<EF6Context> options)
        : base(options)
        { }

        public DbSet<User> User { get; set; }
        public DbSet<Group> Group { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new GroupConfig());
        }
    }
}
