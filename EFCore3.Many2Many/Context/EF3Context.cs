using EFCore3.Many2Many.Config;
using EFCore3.Many2Many.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore3.Many2Many.Context
{
    partial class EF3Context : DbContext
    {
        public EF3Context(DbContextOptions<EF3Context> options)
        : base(options)
        { }

        public DbSet<User> User { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new GroupConfig());
            modelBuilder.ApplyConfiguration(new UserGroupConfig());
        }
    }
}
