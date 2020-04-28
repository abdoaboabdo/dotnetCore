using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Vega.Models
{
    public class Feature
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public ICollection<VehicleFeature> VehicleFeatures { get; set; }
        public Feature()
        {
            VehicleFeatures = new Collection<VehicleFeature>();
        }
    }
}