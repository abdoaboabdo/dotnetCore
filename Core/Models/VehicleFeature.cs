using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vega.Core.Models
{
    [Table("VehicleFeatures")]
    public class VehicleFeature
    {
        [ForeignKey("Vehicle")]
         [Key]
         [Column(Order =0)]
        public int VehicleId { get; set; }
        [ForeignKey("Feature")]
        [Key]
        [Column(Order =1)]
        public int FeatureId { get; set; }
        public Vehicle Vehicle { get; set; }
        public Feature Feature { get; set; }

        
    }
}