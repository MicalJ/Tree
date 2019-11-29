using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Tree.Database.DbModels;

namespace Tree.Database
{
    public class TreeContext : DbContext
    {
        public TreeContext(DbContextOptions<TreeContext> options) : base(options)
        {
        }
                
        public virtual DbSet<Node> Nodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Node>(e =>
            {
                e.HasIndex(h => h.Name).IsUnique(false);
                e.Property(p => p.Name).HasMaxLength(500);

                e.HasIndex(h => h.ParentId).IsUnique(false);

                e.Property(p => p.IsDeleted).HasDefaultValue(false);
            });
        }
    }
}
