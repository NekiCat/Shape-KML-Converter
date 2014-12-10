using shp2kml.Shape.Records;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml.Shape
{
    class ShapeFile
    {
        public enum ShapeType
        {
            NullShape = 0,
            Point = 1,
            Polyline = 3,
            Polygon = 5,
            MultiPoint = 8,
            PointZ = 11,
            PolylineZ = 13,
            PolygonZ = 15,
            MultiPointZ = 18,
            PointMeasured = 21,
            PolylineMeasured = 23,
            PolygonMeasured = 25,
            MultiPointMeasured = 28,
            MultiPatch = 31
        }

        public int Version { get; private set; }
        public ShapeType Type { get; set; }
        public double BoundsMinX { get; set; }
        public double BoundsMinY { get; set; }
        public double BoundsMaxX { get; set; }
        public double BoundsMaxY { get; set; }
        public double BoundsMinZ { get; set; }
        public double BoundsMaxZ { get; set; }
        public double BoundsMinM { get; set; }
        public double BoundsMaxM { get; set; }

        public ICollection<IShapeRecord> Shapes { get; private set; }

        private ShapeFile()
        {
            this.Shapes = new List<IShapeRecord>();
        }

        private static IShapeRecord RecordFromType(ShapeType type, BinaryReader reader)
        {
            switch (type)
            {
                case ShapeType.NullShape: return new NullRecord();
                case ShapeType.Point: return PointRecord.ReadFromStream(reader);
                case ShapeType.MultiPoint: return MultiPointRecord.ReadFromStream(reader);
                case ShapeType.Polygon: return PolygonRecord.ReadFromStream(reader);
                case ShapeType.PointMeasured: return PointMRecord.ReadFromStream(reader);
                case ShapeType.MultiPointMeasured: return MultiPointMRecord.ReadFromStream(reader);
                case ShapeType.PolygonMeasured: return PolygonMRecord.ReadFromStream(reader);
                default: throw new ArgumentException("Unsupported shape type");
            }
        }

        public static ShapeFile ReadFromStream(Stream stream)
        {
            ShapeFile file = new ShapeFile();
            using (BinaryReader reader = new BinaryReader(stream))
            {
                byte[] buf = new byte[4];

                if (reader.ReadInt32(Endianness.Big) != 0x0000270A) throw new InvalidDataException("Invalid file code");
                reader.BaseStream.Seek(20, SeekOrigin.Current);
                int length = reader.ReadInt32(Endianness.Big);
                file.Version = reader.ReadInt32(Endianness.Little);
                file.Type = (ShapeType)reader.ReadInt32(Endianness.Little);

                file.BoundsMinX = reader.ReadDouble(Endianness.Little);
                file.BoundsMinY = reader.ReadDouble(Endianness.Little);
                file.BoundsMaxX = reader.ReadDouble(Endianness.Little);
                file.BoundsMaxY = reader.ReadDouble(Endianness.Little);
                file.BoundsMinZ = reader.ReadDouble(Endianness.Little);
                file.BoundsMaxZ = reader.ReadDouble(Endianness.Little);
                file.BoundsMinM = reader.ReadDouble(Endianness.Little);
                file.BoundsMaxM = reader.ReadDouble(Endianness.Little);

                do
                {
                    int number = reader.ReadInt32(Endianness.Big);
                    int clength = reader.ReadInt32(Endianness.Big);
                    ShapeType type = (ShapeType)reader.ReadInt32(Endianness.Little);

                    file.Shapes.Add(RecordFromType(type, reader));
                } while (reader.PeekChar() != -1);
            }

            return file;
        }

        public void WriteToStream(Stream stream)
        {

        }
    }
}
