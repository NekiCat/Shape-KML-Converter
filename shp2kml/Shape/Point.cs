using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml.Shape
{
    struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y) : this()
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                return ((Point)obj).X == this.X
                    && ((Point)obj).Y == this.Y;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            byte[] data1 = BitConverter.GetBytes(this.X);
            byte[] data2 = BitConverter.GetBytes(this.Y);
            return BitConverter.ToInt32(data1, 0) ^ BitConverter.ToInt32(data1, 4) ^ BitConverter.ToInt32(data2, 0) ^ BitConverter.ToInt32(data2, 4);
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b)
        {
            return !a.Equals(b);
        }
    }
}
