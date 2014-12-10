using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml.Shape
{
    struct Bounds
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public Bounds(double min, double max)
            : this()
        {
            this.Min = min;
            this.Max = max;
        }

        public override bool Equals(object obj)
        {
            if (obj is Bounds)
            {
                return ((Bounds)obj).Min == this.Min
                    && ((Bounds)obj).Max == this.Max;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            byte[] data1 = BitConverter.GetBytes(this.Min);
            byte[] data2 = BitConverter.GetBytes(this.Max);
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
