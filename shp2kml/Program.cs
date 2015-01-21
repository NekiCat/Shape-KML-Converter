using CommandLine;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Topology;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shp2kml
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(string[] args)
        {
            var settings = new Settings();
            if (CommandLine.Parser.Default.ParseArguments(args, settings))
            {
                var data = FeatureSet.OpenFile(settings.BuildingsFile);
                if (data.FeatureType != FeatureType.Polygon)
                {
                    Console.WriteLine("The buildings shape file does not contain polygons!");
                    return;
                }

                data.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;

                var filterFile = FeatureSet.OpenFile(settings.FilterFile);
                filterFile.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;

                var output = new Document();
                output.Name = "test.kml";
                output.Open = false;

                var filter = new NullFilter();
                //var filter = new IntersectsFilter();
                foreach (var feature in filter.Filter(data.Features, filterFile.Features))
                {
                    var f = feature.ToFeature();
                    f.AddStyle(new Style()
                    {
                        Polygon = new PolygonStyle()
                        {
                            ColorMode = ColorMode.Random,
                            Outline = false
                        }
                    });
                    output.AddFeature(f);
                }

                Kml root = new Kml();
                root.Feature = output;
                KmlFile kml = KmlFile.Create(root, false);
                KmzFile kmz = KmzFile.Create(kml);
                kmz.Save(settings.OutputFile);
            }

#if DEBUG
            Console.WriteLine("-- Press Any Key --");
            Console.ReadKey(true);
#endif
        }
    }
}
