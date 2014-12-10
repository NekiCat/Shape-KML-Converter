using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml.Shape.Records
{
    class MultiPointRecord : IShapeRecord
    {
        public Bounds BoundsX { get; set; }
        public Bounds BoundsY { get; set; }
        public ICollection<Point> Points { get; private set; }

        public MultiPointRecord()
        {
            this.Points = new List<Point>();
        }

        public static MultiPointRecord ReadFromStream(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return ReadFromStream(reader);
            }
        }

        public static MultiPointRecord ReadFromStream(BinaryReader reader)
        {
            MultiPointRecord record = new MultiPointRecord();
            double tmp1 = reader.ReadDouble(Endianness.Little);
            double tmp2 = reader.ReadDouble(Endianness.Little);
            record.BoundsX = new Bounds(tmp1, reader.ReadDouble(Endianness.Little));
            record.BoundsY = new Bounds(tmp2, reader.ReadDouble(Endianness.Little));

            int count = reader.ReadInt32(Endianness.Little);
            for (int i = 0; i < count; i++) record.Points.Add(new Point(
                reader.ReadDouble(Endianness.Little),
                reader.ReadDouble(Endianness.Little)
            ));

            return record;
        }
    }
}
