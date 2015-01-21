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
        public IEnumerable<SharpKml.Dom.Feature> Filter(IEnumerable<DotSpatial.Data.IFeature> input)
        {
            var output = new List<SharpKml.Dom.Feature>();
            foreach (var feature in input)
            {
                if (feature.BasicGeometry is IPolygon)
                {
                    var polygon = ((IPolygon)feature.BasicGeometry).ToPolygon();
                    var place = new Placemark();
                    place.Name = "Building (Single)";
                    place.Geometry = polygon;
                    place.AddStyle(new Style()
                    {
                        Polygon = new PolygonStyle()
                        {
                            ColorMode = ColorMode.Random,
                            Outline = false
                        }
                    });
                    output.Add(place);
                }
                else if (feature.BasicGeometry is IMultiPolygon)
                {
                    var polygon = ((IMultiPolygon)feature.BasicGeometry).ToMultiPolygon();
                    var place = new Placemark();
                    place.Name = "Building (Multi)";
                    place.Geometry = polygon;
                    place.AddStyle(new Style()
                    {
                        Polygon = new PolygonStyle()
                        {
                            ColorMode = ColorMode.Random,
                            Outline = false
                        }
                    });
                    output.Add(place);
                }
                else
                {
                    Console.WriteLine("Unsupported geometry type");
                }
            }

            return output;
        }
    }
}
