using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Vega.Core.Models;
using Vega.Core;

namespace Vega.Persistence
{
    public class VegaDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Feature> Features { get; set; }
        public VegaDbContext([NotNullAttribute] DbContextOptions<VegaDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleFeature>()
                .HasKey(vf=> new { vf.VehicleId,vf.FeatureId });
        }
        
    }
}