using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Vega.Controllers.Resources
{
    public class FeatureResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<int> VehicleFeatures { get; set; }
        public FeatureResource()
        {
            VehicleFeatures = new Collection<int>();
        }
    }
}