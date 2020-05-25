using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Vega.Core.Models;
using Vega.Core;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Vega.Persistence
{
    public class VegaDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<VehicleFeature> VehicleFeature { get; set; }
        public VegaDbContext([NotNullAttribute] DbContextOptions<VegaDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleFeature>()
                .HasKey(vf=> new { vf.VehicleId,vf.FeatureId });
            modelBuilder.Entity<VehicleFeature>()
                .HasOne(F => F.Feature)
                .WithMany(Fs => Fs.VehicleFeatures)
                .HasForeignKey(bc => bc.FeatureId);  
            modelBuilder.Entity<VehicleFeature>()
                .HasOne(V => V.Vehicle)
                .WithMany(Ve => Ve.VehicleFeatures)
                .HasForeignKey(bc => bc.VehicleId);
        }
        
        
    }
}