using CommandLine;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Topology;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shp2kml
{
    enum ExitCode : int
    {
        Success = 0,
        InvalidParameter = 1,
        FileNotFound = 2,
        InvalidFile = 3
    }

    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static int Main(string[] args)
        {
            int code;
            Settings settings = new Settings();
            if (CommandLine.Parser.Default.ParseArguments(args, settings))
            {
                code = (int)Work(settings);
            }
            else
            {
                code = (int)ExitCode.InvalidParameter;
            }

#if DEBUG
            Console.WriteLine("Exit Code = " + code + " " + Enum.GetName(typeof(ExitCode), code));
            Console.WriteLine("-- Press Any Key --");
            Console.ReadKey(true);
#endif

            return code;
        }

        static ExitCode Work(Settings settings)
        {
            if (!File.Exists(settings.BuildingsFile))
            {
                Console.Error.WriteLine("The buildings shape file does not exist!");
                return ExitCode.FileNotFound;
            }

            if (settings.FilterFile != null && !File.Exists(settings.FilterFile))
            {
                Console.Error.WriteLine("The filter shape file does not exist!");
                return ExitCode.FileNotFound;
            }

            IFeatureSet buildings = FeatureSet.OpenFile(settings.BuildingsFile);
            if (buildings.FeatureType != FeatureType.Polygon)
            {
                Console.Error.WriteLine("The buildings shape file does not contain polygons!");
                return ExitCode.InvalidFile;
            }

#if DEBUG
            Console.WriteLine("The buildings shape file has the projection: " + buildings.Projection);
#endif
            if (settings.BuildingsProjection != null)
            {
                buildings.Projection = ProjectionInfo.FromProj4String(settings.BuildingsProjection);
            }
            else if (!File.Exists(Path.ChangeExtension(settings.BuildingsFile, ".prj")))
            {
                buildings.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            }

            if (buildings.Projection != KnownCoordinateSystems.Geographic.World.WGS1984)
            {
                buildings.Reproject(KnownCoordinateSystems.Geographic.World.WGS1984);
            }

            IFeatureSet filter = null;
            if (settings.FilterFile != null)
            {
                filter = FeatureSet.OpenFile(settings.FilterFile);

#if DEBUG
                Console.WriteLine("The filter shape file has the projection: " + buildings.Projection);
#endif

                if (settings.FilterProjection != null)
                {
                    filter.Projection = ProjectionInfo.FromProj4String(settings.FilterProjection);
                }
                else if (!File.Exists(Path.ChangeExtension(settings.FilterFile, ".prj")))
                {
                    filter.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
                }

                if (filter.Projection != KnownCoordinateSystems.Geographic.World.WGS1984)
                {
                    filter.Reproject(KnownCoordinateSystems.Geographic.World.WGS1984);
                }
            }


            
            IFilter filterBlock;
            IEnumerable<DotSpatial.Data.IFeature> filterFeatures = null;
            if (filter != null)
            {
                filterBlock = new IntersectsFilter();
                filterFeatures = filter.Features;
            }
            else
            {
                filterBlock = new NullFilter();
            }

            var output = new Document();
            output.Name = Path.GetFileName(settings.OutputFile);
            output.Open = false;
            foreach (var feature in filterBlock.Filter(buildings.Features, filterFeatures))
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

            return ExitCode.Success;
        }
    }
}
