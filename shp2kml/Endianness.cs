using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml
{
    public enum Endianness
    {
        Big, Little
    }

    public static class EndiannessHelper
    {
        public static Int16 ReadInt16(this BinaryReader reader, Endianness endianness)
        {
            if (endianness == Endianness.Little) return reader.ReadInt16();
            byte[] buf = new byte[2];
            if (reader.Read(buf, 0, 2) != 2) throw new InvalidDataException();
            Array.Reverse(buf);
            return BitConverter.ToInt16(buf, 0);
        }

        public static Int32 ReadInt32(this BinaryReader reader, Endianness endianness)
        {
            if (endianness == Endianness.Little) return reader.ReadInt32();
            byte[] buf = new byte[4];
            if (reader.Read(buf, 0, 4) != 4) throw new InvalidDataException();
            Array.Reverse(buf);
            return BitConverter.ToInt32(buf, 0);
        }

        public static Int64 ReadInt64(this BinaryReader reader, Endianness endianness)
        {
            if (endianness == Endianness.Little) return reader.ReadInt64();
            byte[] buf = new byte[8];
            if (reader.Read(buf, 0, 8) != 8) throw new InvalidDataException();
            Array.Reverse(buf);
            return BitConverter.ToInt64(buf, 0);
        }

        public static UInt16 ReadUInt16(this BinaryReader reader, Endianness endianness)
        {
            if (endianness == Endianness.Little) return reader.ReadUInt16();
            byte[] buf = new byte[2];
            if (reader.Read(buf, 0, 2) != 2) throw new InvalidDataException();
            Array.Reverse(buf);
            return BitConverter.ToUInt16(buf, 0);
        }

        public static UInt32 ReadUInt32(this BinaryReader reader, Endianness endianness)
        {
            if (endianness == Endianness.Little) return reader.ReadUInt32();
            byte[] buf = new byte[4];
            if (reader.Read(buf, 0, 4) != 4) throw new InvalidDataException();
            Array.Reverse(buf);
            return BitConverter.ToUInt32(buf, 0);
        }

        public static UInt64 ReadUInt64(this BinaryReader reader, Endianness endianness)
        {
            if (endianness == Endianness.Little) return reader.ReadUInt64();
            byte[] buf = new byte[8];
            if (reader.Read(buf, 0, 8) != 8) throw new InvalidDataException();
            Array.Reverse(buf);
            return BitConverter.ToUInt64(buf, 0);
        }

        public static Single ReadSingle(this BinaryReader reader, Endianness endianness)
        {
            if (endianness == Endianness.Little) return reader.ReadSingle();
            byte[] buf = new byte[4];
            if (reader.Read(buf, 0, 4) != 4) throw new InvalidDataException();
            Array.Reverse(buf);
            return BitConverter.ToSingle(buf, 0);
        }

        public static Double ReadDouble(this BinaryReader reader, Endianness endianness)
        {
            if (endianness == Endianness.Little) return reader.ReadDouble();
            byte[] buf = new byte[8];
            if (reader.Read(buf, 0, 8) != 8) throw new InvalidDataException();
            Array.Reverse(buf);
            return BitConverter.ToDouble(buf, 0);
        }
    }
}
