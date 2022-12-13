using AssetManagement.Domain.Enums.Asset;
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
            var adminHcmId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            var adminHNId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00BF");
            var staffAbleId1 = new Guid("70BD714F-9576-45BA-B5B7-F00649BE00DE");
            var staffAbleId2 = new Guid("70BD814F-9576-45BA-B5B7-F00649BE00DE");
            var staffUnableId = new Guid("73BD714F-9576-45BA-B5B7-F00649BE00DE");

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
                Id = adminHcmId,
                UserName = "adminhcm",
                NormalizedUserName = "adminhcm",
                Email = "adminhcm@gmail.com",
                NormalizedEmail = "adminhcm@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "12345678"),
                SecurityStamp = string.Empty,
                FirstName = "Toan",
                LastName = "Bach",
                Dob = new DateTime(2000, 01, 31),
                IsLoginFirstTime = false,
                CreatedDate = DateTime.Now,
                Gender = Domain.Enums.AppUser.UserGender.Male,
                Location = Domain.Enums.AppUser.AppUserLocation.HoChiMinh,
                StaffCode = "SD0001"
            });

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminHNId,
                UserName = "adminhn",
                NormalizedUserName = "adminhn",
                Email = "adminhn@gmail.com",
                NormalizedEmail = "adminhn@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "12345678"),
                SecurityStamp = string.Empty,
                FirstName = "Toan",
                LastName = "Bach",
                Dob = new DateTime(2000, 01, 31),
                IsLoginFirstTime = true,
                CreatedDate = DateTime.Now,
                Gender = Domain.Enums.AppUser.UserGender.Male,
                Location = Domain.Enums.AppUser.AppUserLocation.HaNoi,
                StaffCode = "SD0002"
            });

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = staffAbleId1,
                UserName = "staff1",
                NormalizedUserName = "staff1",
                Email = "staff@gmail.com",
                NormalizedEmail = "staff@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "12345678"),
                SecurityStamp = string.Empty,
                FirstName = "Toan",
                LastName = "Bach",
                Dob = new DateTime(2000, 01, 31),
                IsLoginFirstTime = true,
                CreatedDate = DateTime.Now,
                Gender = Domain.Enums.AppUser.UserGender.Female,
                Location = Domain.Enums.AppUser.AppUserLocation.HaNoi,
                StaffCode = "SD0003"
            });

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = staffAbleId2,
                UserName = "staff2",
                NormalizedUserName = "staff2",
                Email = "staff@gmail.com",
                NormalizedEmail = "staff@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "12345678"),
                SecurityStamp = string.Empty,
                FirstName = "Toan",
                LastName = "Bach",
                Dob = new DateTime(2000, 01, 31),
                IsLoginFirstTime = true,
                CreatedDate = DateTime.Now,
                Gender = Domain.Enums.AppUser.UserGender.Female,
                Location = Domain.Enums.AppUser.AppUserLocation.HaNoi,
                StaffCode = "SD0004"
            });

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = staffUnableId,
                UserName = "staffDis",
                NormalizedUserName = "staffdis",
                Email = "staffdis@gmail.com",
                NormalizedEmail = "staffdis@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "12345678"),
                SecurityStamp = string.Empty,
                FirstName = "Toan",
                LastName = "Bach",
                Dob = new DateTime(2000, 01, 31),
                IsLoginFirstTime = true,
                CreatedDate = DateTime.Now,
                Gender = Domain.Enums.AppUser.UserGender.Female,
                Location = Domain.Enums.AppUser.AppUserLocation.HaNoi,
                StaffCode = "SD0005",
                IsDeleted = true,
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = adminRoleId,
                UserId = adminHcmId
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = adminRoleId,
                UserId = adminHNId,
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = staffRoleId,
                UserId = staffUnableId
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = staffRoleId,
                UserId = staffAbleId1
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = staffRoleId,
                UserId = staffAbleId2
            });

            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 1,
                Name = "Laptop",
                Prefix = "LA",
                IsDeleted = false,
            });

            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 2,
                Name = "Monitor",
                Prefix = "MO",
                IsDeleted = false,
            });

            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 3,
                Name = "Personal Computer",
                Prefix = "PC",
                IsDeleted = false,
            });

            for (int i = 1; i <= 10; i++)
            {
                modelBuilder.Entity<Asset>().HasData(new Asset
                {
                    Id = i,
                    Name = "Laptop " + i,
                    AssetCode = "LA10000" + i,
                    Specification = $"Core i{i}, {i}GB RAM, {i}50 GB HDD, Window {i}",
                    CategoryId = i % 2 == 0 ? 1 : 2,
                    InstalledDate = DateTime.Now,
                    State = i % 2 == 0 ? State.Available : State.NotAvailable,
                    IsDeleted = i % 2 == 0 ? true : false,
                });
            }

            for (int i = 11; i <= 15; i++)
            {
                modelBuilder.Entity<Asset>().HasData(new Asset
                {
                    Id = i,
                    Name = "Laptop " + i,
                    AssetCode = "LA10000" + i,
                    Specification = $"Core i{i}, {i}GB RAM, {i}50 GB HDD, Window {i}",
                    CategoryId = i % 2 == 0 ? 1 : 2,
                    InstalledDate = DateTime.Now,
                    State = i % 2 == 0 ? State.Assigned : State.NotAvailable,
                    IsDeleted = i % 2 == 0 ? true : false,
                });
            }

            for (int i = 1; i <= 10; i++)
            {
                modelBuilder.Entity<Assignment>().HasData(new Assignment
                {
                    Id = i,
                    Note = $"Note for assignment {i}",
                    AssignedDate = DateTime.Today,
                    ReturnedDate = DateTime.Today.AddDays(i),
                    AssetId = i,
                    State = i % 2 == 0 ? Domain.Enums.Assignment.State.Accepted : Domain.Enums.Assignment.State.WaitingForAcceptance,
                    AssignedTo = staffAbleId1,
                    AssignedBy = adminHcmId,
                });
            }

            modelBuilder.Entity<Assignment>().HasData(new Assignment
            {
                Id = 11,
                Note = $"Note for assignment {11}",
                AssignedDate = DateTime.Today,
                ReturnedDate = DateTime.Today.AddDays(11),
                AssetId = 4,
                State = Domain.Enums.Assignment.State.WaitingForAcceptance,
                AssignedTo = staffAbleId1,
                AssignedBy = adminHcmId,
            });

            for (int i = 12; i <= 15; i++)
            {
                modelBuilder.Entity<Assignment>().HasData(new Assignment
                {
                    Id = i,
                    Note = $"Note for assignment {i}",
                    AssignedDate = DateTime.Today,
                    ReturnedDate = DateTime.Today.AddDays(i),
                    AssetId = i,
                    State = i % 2 == 0 ? Domain.Enums.Assignment.State.WaitingForReturning : Domain.Enums.Assignment.State.Returned,
                    AssignedTo = staffAbleId1,
                    AssignedBy = adminHcmId,
                });
            }

            for (int i = 1; i <= 10; i++)
            {
                modelBuilder.Entity<ReturnRequest>().HasData(new ReturnRequest
                {
                    Id = i,
                    AssignedBy = staffAbleId1,
                    AssignedDate = DateTime.Today,
                    ReturnedDate = DateTime.Today,
                    State = Domain.Enums.ReturnRequest.State.WaitingForReturning,
                    AssignmentId = i,
                });
            }

            for (int i = 11; i <= 15; i++)
            {
                modelBuilder.Entity<ReturnRequest>().HasData(new ReturnRequest
                {
                    Id = i,
                    AssignedBy = staffAbleId1,
                    AssignedDate = DateTime.Today,
                    ReturnedDate = DateTime.Today,
                    State = Domain.Enums.ReturnRequest.State.WaitingForReturning,
                    AssignmentId = i,
                });
            }
        }
    }
}
