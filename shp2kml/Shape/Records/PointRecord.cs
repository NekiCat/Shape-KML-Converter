using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml.Shape.Records
{
    class PointRecord : IShapeRecord
    {
        public Point Point { get; set; }
        
        public static PointRecord ReadFromStream(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return ReadFromStream(reader);
            }
        }

        public static PointRecord ReadFromStream(BinaryReader reader)
        {
            PointRecord record = new PointRecord();
            record.Point = new Point(
                reader.ReadDouble(Endianness.Little),
                reader.ReadDouble(Endianness.Little)
            );
            return record;
        }
    }
}
