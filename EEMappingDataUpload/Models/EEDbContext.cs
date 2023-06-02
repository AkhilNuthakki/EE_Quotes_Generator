using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EEMappingDataUpload.Models
{
    public class EEDbContext:DbContext
    {
      
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=ee_master;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Irradiance>().HasKey(nameof(Irradiance.Zone), nameof(Irradiance.Orientation), nameof(Irradiance.Inclination));
        }

        public DbSet<Irradiance> IrradianceDatasets { get; set; }
        public DbSet<RegionMap> RegionMappings { get; set; }

    }
}
