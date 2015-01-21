using DotSpatial.Topology;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml
{
    class NullFilter : IFilter
    {
        public IEnumerable<DotSpatial.Data.IFeature> Filter(IEnumerable<DotSpatial.Data.IFeature> input, IEnumerable<DotSpatial.Data.IFeature> filter)
        {
            return input;
        }
    }
}
