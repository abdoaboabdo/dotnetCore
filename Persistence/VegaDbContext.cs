using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Vega.Core.Models;
using Vega.Core;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Options;
using IdentityServer4.EntityFramework.Options;

namespace Vega.Persistence
{
    public class VegaDbContext : ApiAuthorizationDbContext<User>
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<VehicleFeature> VehicleFeature { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public VegaDbContext(DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options,operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating( modelBuilder);
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