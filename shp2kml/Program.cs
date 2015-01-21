using CommandLine;
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
                Console.WriteLine("Working");
            }

#if DEBUG
            Console.ReadKey(true);
#endif
        }
    }
}
