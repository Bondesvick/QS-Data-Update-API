using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using QSDataUpdateAPI.Data;
using QSDataUpdateAPI.Core.Domain.Entities;
using QSDataUpdateAPI.Data.Data.Configurations;
using Microsoft.Extensions.Configuration;
using QSDataUpdateAPI.Domain.Services.Helpers;

namespace QSDataUpdateAPI.Data.Data
{
    public partial class QuickServiceDbContext : DbContext
    {
        public QuickServiceDbContext()
        {
        }

        public QuickServiceDbContext(DbContextOptions<QuickServiceDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataUpdateDetails> AddAccOpeningDetails { get; set; }
        public virtual DbSet<DataUpdateDocument> AddAccOpeningDocs { get; set; }
        public virtual DbSet<CustomerRequest> CustomerRequest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DataUpdateDocumentEntityConfig());
            modelBuilder.ApplyConfiguration(new DataUpdateDetailsEntityConfig());
            modelBuilder.ApplyConfiguration(new CustomerRequestEntityConfig());
            modelBuilder.ApplyConfiguration(new AuditConfiguration());
            OnModelCreatingPartial(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = ServiceResolver.Resolve<IConfiguration>().GetConnectionString("QuickServiceDbConn");
            optionsBuilder.UseSqlServer(connStr).EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}