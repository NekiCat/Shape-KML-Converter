using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml.Shape.Records
{
    class MultiPointMRecord : IShapeRecord
    {
        public Bounds BoundsX { get; set; }
        public Bounds BoundsY { get; set; }
        public Bounds BoundsM { get; set; }
        public ICollection<PointM> Points { get; private set; }

        public MultiPointMRecord()
        {
            this.Points = new List<PointM>();
        }

        public static MultiPointMRecord ReadFromStream(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return ReadFromStream(reader);
            }
        }

        public static MultiPointMRecord ReadFromStream(BinaryReader reader)
        {
            MultiPointMRecord record = new MultiPointMRecord();
            double tmp1 = reader.ReadDouble(Endianness.Little);
            double tmp2 = reader.ReadDouble(Endianness.Little);
            record.BoundsX = new Bounds(tmp1, reader.ReadDouble(Endianness.Little));
            record.BoundsY = new Bounds(tmp2, reader.ReadDouble(Endianness.Little));

            int count = reader.ReadInt32(Endianness.Little);
            List<Point> tmp = new List<Point>();
            for (int i = 0; i < count; i++) tmp.Add(new Point(
                reader.ReadDouble(Endianness.Little),
                reader.ReadDouble(Endianness.Little)
            ));

            record.BoundsM = new Bounds(reader.ReadDouble(Endianness.Little), reader.ReadDouble(Endianness.Little));
            for (int i = 0; i < count; i++)
            {
                record.Points.Add(new PointM(tmp[i].X, tmp[i].Y, reader.ReadDouble(Endianness.Little)));
            }

            return record;
        }
    }
}
