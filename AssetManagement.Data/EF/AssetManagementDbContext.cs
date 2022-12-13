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
            modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<AppRole> AppRoles { get; set; }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<ReturnRequest> ReturnRequests { get; set; }
    }
}