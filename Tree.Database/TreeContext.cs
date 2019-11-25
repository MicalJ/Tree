using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tree.Database.Consts;
using Tree.Database.DbModels;

namespace Tree.Database
{
    public partial class TreeContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public TreeContext(DbContextOptions<TreeContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            OnModelCreatingIdentity(builder);
        }

        private void OnModelCreatingIdentity(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(e =>
            {
                e.HasKey(h => h.Id);

                e.HasIndex(h => h.NormalizedUserName).HasName("UserNameIndex").IsUnique();
                e.HasIndex(h => h.NormalizedEmail).HasName("EmailIndex");

                e.ToTable(TableConsts.IdentityUser, TableConsts.IdentitySchema);

                e.Property(p => p.ConcurrencyStamp).IsConcurrencyToken();

                e.Property(p => p.UserName).HasMaxLength(256);
                e.Property(p => p.NormalizedUserName).HasMaxLength(256);
                e.Property(p => p.Email).HasMaxLength(256);
                e.Property(p => p.NormalizedEmail).HasMaxLength(256);

                e.HasMany<ApplicationUserClaim>().WithOne().HasForeignKey(h => h.UserId).IsRequired();

                e.HasMany<ApplicationUserLogin>().WithOne().HasForeignKey(h => h.UserId).IsRequired();

                e.HasMany<ApplicationUserToken>().WithOne().HasForeignKey(h => h.UserId).IsRequired();

                e.HasMany<ApplicationUserRole>().WithOne().HasForeignKey(h => h.UserId).IsRequired();
            });

            builder.Entity<ApplicationRole>(e =>
            {
                e.HasKey(h => h.Id);

                e.HasIndex(h => h.NormalizedName).HasName("RoleNameIndex").IsUnique();

                e.ToTable(TableConsts.IdentityRole, TableConsts.IdentitySchema);

                e.Property(p => p.ConcurrencyStamp).IsConcurrencyToken();
                
                e.Property(p => p.Name).HasMaxLength(256);
                e.Property(p => p.NormalizedName).HasMaxLength(256);

                e.HasMany<ApplicationUserRole>().WithOne().HasForeignKey(h => h.RoleId).IsRequired();

                e.HasMany<ApplicationRoleClaim>().WithOne().HasForeignKey(h => h.RoleId).IsRequired();
            });

            builder.Entity<ApplicationRoleClaim>(e =>
            {
                e.HasKey(h => h.Id);

                e.ToTable(TableConsts.IdentityRoleClaim, TableConsts.IdentitySchema);
            });

            builder.Entity<ApplicationUserRole>(e =>
            {
                e.HasKey(h => new { h.UserId, h.RoleId });

                e.ToTable(TableConsts.IdentityUserRole, TableConsts.IdentitySchema);
            });

            builder.Entity<ApplicationUserToken>(e =>
            {
                e.HasKey(h => new { h.UserId, h.LoginProvider, h.Name });

                e.Property(p => p.LoginProvider).HasMaxLength(50);
                e.Property(p => p.Name).HasMaxLength(180);

                e.ToTable(TableConsts.IdentityUserToken, TableConsts.IdentitySchema);
            });

            builder.Entity<ApplicationUserClaim>(e =>
            {
                e.HasKey(h => h.Id);

                e.ToTable(TableConsts.IdentityUserClaim, TableConsts.IdentitySchema);
            });

            builder.Entity<ApplicationUserLogin>(e =>
            {
                e.HasKey(h => new { h.LoginProvider, h.ProviderKey });

                e.Property(p => p.LoginProvider).HasMaxLength(128);
                e.Property(p => p.ProviderKey).HasMaxLength(128);

                e.ToTable(TableConsts.IdentityUserLogin, TableConsts.IdentitySchema);
            });
        }
    }
}
