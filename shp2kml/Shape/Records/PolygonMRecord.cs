using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml.Shape.Records
{
    class PolygonMRecord : IShapeRecord
    {
        public Bounds BoundsX { get; set; }
        public Bounds BoundsY { get; set; }
        public Bounds BoundsM { get; set; }

        public IList<ICollection<PointM>> Lines { get; private set; }

        public PolygonMRecord()
        {
            this.Lines = new List<ICollection<PointM>>();
        }

        public static PolygonMRecord ReadFromStream(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return ReadFromStream(reader);
            }
        }

        public static PolygonMRecord ReadFromStream(BinaryReader reader)
        {
            PolygonMRecord record = new PolygonMRecord();
            double tmp1 = reader.ReadDouble(Endianness.Little);
            double tmp2 = reader.ReadDouble(Endianness.Little);
            record.BoundsX = new Bounds(tmp1, reader.ReadDouble(Endianness.Little));
            record.BoundsY = new Bounds(tmp2, reader.ReadDouble(Endianness.Little));

            int partsCount = reader.ReadInt32(Endianness.Little);
            int pointsCount = reader.ReadInt32(Endianness.Little);

            List<int> parts = new List<int>();
            for (int i = 0; i < partsCount; i++)
            {
                parts.Add(reader.ReadInt32(Endianness.Little));
            }
            parts.Sort();

            int index = 0;
            List<Point> tmp = new List<Point>();
            for (int i = 0; i < pointsCount; i++)
            {
                tmp.Add(new Point(
                    reader.ReadDouble(Endianness.Little),
                    reader.ReadDouble(Endianness.Little)
                ));
            }

            record.BoundsM = new Bounds(reader.ReadDouble(Endianness.Little), reader.ReadDouble(Endianness.Little));
            record.Lines.Add(new List<PointM>());
            index = 0;
            for (int i = 0; i < pointsCount; i++)
            {
                if (i == parts[index])
                {
                    index++;
                    record.Lines.Add(new List<PointM>());
                }
                record.Lines[index].Add(new PointM(tmp[i].X, tmp[i].Y, reader.ReadDouble(Endianness.Little)));
            }

            return record;
        }
    }
}
