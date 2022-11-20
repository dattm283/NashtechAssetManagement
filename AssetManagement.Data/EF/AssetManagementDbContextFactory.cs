using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Data.EF
{
    public class AssetManagementDbContextFactory : IDesignTimeDbContextFactory<AssetManagementDbContext>
    {
        public AssetManagementDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("AssetManagement");

            var optionsBuilder = new DbContextOptionsBuilder<AssetManagementDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AssetManagementDbContext(optionsBuilder.Options);
        }
    }
}
