using DotSpatial.Data;
using DotSpatial.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml
{
    class IntersectsFilter : IFilter
    {
        public IEnumerable<DotSpatial.Data.IFeature> Filter(IEnumerable<DotSpatial.Data.IFeature> input, IEnumerable<DotSpatial.Data.IFeature> filter)
        {
            yield return filter.FirstOrDefault();

            foreach (var feature in input)
            {
                if (Contains(feature.BasicGeometry as IGeometry, filter))
                {
                    yield return feature;
                }
            }
        }

        private bool Contains(IGeometry item, IEnumerable<IFeature> filter)
        {
            foreach (var f in filter)
            {
                if (item.Intersects((IGeometry)f.BasicGeometry)) return true;
            }
            return false;
        }
    }
}
