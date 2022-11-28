using AssetManagement.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Data.Extensions;
using Microsoft.Extensions.Configuration;

namespace AssetManagement.Data.EF
{
    public class AssetManagementDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AssetManagementDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);
            modelBuilder.Entity<Category>().ToTable("Categories").HasKey(x => x.Id);
            modelBuilder.Entity<AppUser>().HasIndex(x => x.UserName).IsUnique();
            modelBuilder.Entity<AppUser>().HasIndex(x => x.RoleId).IsUnique(false);
            //modelBuilder.Entity<Asset>().ToTable("Assets", t=>t.ExcludeFromMigrations());
            //modelBuilder.Entity<Category>().ToTable("Categories", t=>t.ExcludeFromMigrations());

            modelBuilder.Seed();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //    .AddJsonFile("appsettings.json")
        //    .Build();
        //    optionsBuilder.UseSqlServer(configuration.GetConnectionString("AssetManagement"));
        //    base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<AppRole> AppRoles { get; set; }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Assignment> Assignments { get; set; }
    }
}