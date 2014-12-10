using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml.Shape.Records
{
    class PointMRecord : IShapeRecord
    {
        public PointM Point { get; set; }

        public static PointMRecord ReadFromStream(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return ReadFromStream(reader);
            }
        }

        public static PointMRecord ReadFromStream(BinaryReader reader)
        {
            PointMRecord record = new PointMRecord();
            record.Point = new PointM(
                reader.ReadDouble(Endianness.Little),
                reader.ReadDouble(Endianness.Little),
                reader.ReadDouble(Endianness.Little)
            );
            return record;
        }
    }
}
