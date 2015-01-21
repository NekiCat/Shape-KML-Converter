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

                var output = new Document();
                output.Name = "test.kml";
                output.Open = false;

                foreach (var feature in data.Features)
                {
                    IPolygon polygon = null;
                    if (feature.BasicGeometry is IPolygon)
                    {
                        polygon = (IPolygon)feature.BasicGeometry;
                    }
                    else if (feature.BasicGeometry is IMultiPolygon)
                    {
                        foreach (var mpoly in (feature.BasicGeometry as IMultiPolygon).Geometries)
                        {
                            var kpoly = (mpoly as IPolygon).ToPolygon();
                            kpoly.Extrude = false;
                            kpoly.Tessellate = false;

                            var pfeat = new Placemark();
                            pfeat.Name = "Multitest";
                            pfeat.Geometry = kpoly;
                            output.AddFeature(pfeat);
                        }
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Unsupported geometry type");
                        continue;
                    }

                    var kmlPolygon = polygon.ToPolygon();
                    kmlPolygon.Extrude = true;
                    kmlPolygon.Tessellate = true;

                    var feat = new Placemark();
                    feat.Name = "test";
                    feat.Geometry = kmlPolygon;
                    output.AddFeature(feat);
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
