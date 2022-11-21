using AssetManagement.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // any guid
            var adminRoleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var staffRoleId = new Guid("12147FE0-4571-4AD2-B8F7-D2C863EB78A5");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            var staffId = Guid.NewGuid();

            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = staffRoleId,
                Name = "Staff",
                NormalizedName = "staff",
                Description = "Staff role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "12345678"),
                SecurityStamp = string.Empty,
                FirstName = "Toan",
                LastName = "Bach",
                Dob = new DateTime(2020, 01, 31),
                IsLoginFirstTime = true,
                CreatedDate = DateTime.Now,
                Gender = "Male",
                Location = "HCM",
                RoleId = adminRoleId
            });

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = staffId,
                UserName = "staff",
                NormalizedUserName = "staff",
                Email = "staff@gmail.com",
                NormalizedEmail = "staff@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "12345678"),
                SecurityStamp = string.Empty,
                FirstName = "Toan",
                LastName = "Bach",
                Dob = new DateTime(2020, 01, 31),
                IsLoginFirstTime = true,
                CreatedDate = DateTime.Now,
                Gender = "Male",
                Location = "HCM",
                RoleId = staffRoleId
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = adminRoleId,
                UserId = adminId
            });
        }
    }
}
