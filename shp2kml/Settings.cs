using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml
{
    class Settings
    {
        [Option('b', "buildings", Required = true, HelpText = "The shape file containing the 3D buildings.")]
        public string BuildingsFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "The output kmz file (Will be overwritten if it exists).")]
        public string OutputFile { get; set; }

        [Option('f', "filter", Required = false, HelpText = "The shape file containing the areas for filtering the buildings.")]
        public string FilterFile { get; set; }

        [Option('p', "proximity", Required = false, HelpText = "If filter contains a line, sets the proximity for buildings to be included.")]
        public int Proximity { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            /*var help = new HelpText {
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            help.AddPreOptionsLine("Usage: shp2kml --buildings buildings.shp --output buildings.kmz");
            help.AddPreOptionsLine("Warning: It is assumed that the shape files are encoded in WGS 84.");
            help.AddOptions(this);

            return help;*/
        }
    }
}
