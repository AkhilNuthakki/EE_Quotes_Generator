using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EEQuoteGenerator.Models;

namespace EEQuoteGenerator.Models
{
    public class EEDbContext:DbContext
    {
        public EEDbContext(DbContextOptions<EEDbContext> options):base(options)
        {
            //this.ChangeTracker.LazyLoadingEnabled = false;       
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(user => user.UserEmail).IsUnique();
            modelBuilder.Entity<User>().HasIndex(user => user.UserId).IsUnique();
            modelBuilder.Entity<User>().Property(user => user.UserId).HasComputedColumnSql("SUBSTRING([FirstName], 1, 1) + SUBSTRING([LastName], 1, 1) + CAST([Id] As varchar(16))");

            modelBuilder.Entity<Quote>().Property(quote => quote.QuoteReference).HasComputedColumnSql("CAST(YEAR(GETDATE()) As varchar(16)) + '_' + CAST([QuoteId] As varchar(16)) + '_' + [MasterProjectType] + '_' + SUBSTRING([QuotedGeneratedBy], 1, 2)");

            modelBuilder.Entity<CostProjection>().HasKey(nameof(CostProjection.QuoteId), nameof(CostProjection.Year));

            modelBuilder.Entity<Irradiance>().HasKey(nameof(Irradiance.Zone), nameof(Irradiance.Orientation), nameof(Irradiance.Inclination));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Irradiance> IrradianceDatasets { get; set; }
        public DbSet<RegionMap> RegionMappings { get; set; }
        public DbSet<InvestmentParameters> QuoteInvestmentParameters { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<QuoteCostSummary> Summary { get; set; }
        public DbSet<CostProjection> Projections { get; set; }
        public DbSet<EEQuoteGenerator.Models.Components> Components { get; set; }

    }
}
