using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml
{
    enum FilterMode
    {
        Inside,
        Touching,
        Proximity
    }

    class Settings
    {
        [Option('b', "buildings", Required = true, HelpText = "The shape file containing the 3D buildings.")]
        public string BuildingsFile { get; set; }

        [Option('f', "filter", Required = false, HelpText = "The shape file containing the areas for filtering the buildings.")]
        public string FilterFile { get; set; }

        [Option('m', "mode", Required = false, HelpText = "The mode with which the buildings get filtered.")]
        public FilterMode Mode { get; set; }

        [Option('p', "proximity", Required = false, HelpText = "If mode is proximity, sets the proximity for buildings to be included.")]
        public int Proximity { get; set; }

        [Option('o', "output", Required = true, HelpText = "The output kmz file.")]
        public string OutputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText {
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            help.AddPreOptionsLine("Usage: shp2kml --buildings buildings.shp --output buildings.kmz");
            help.AddOptions(this);

            return help;
        }
    }
}
