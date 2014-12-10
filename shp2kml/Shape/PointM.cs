using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml.Shape
{
    struct PointM
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double M { get; set; }

        public PointM(double x, double y, double m)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.M = m;
        }

        public override bool Equals(object obj)
        {
            if (obj is PointM)
            {
                return ((PointM)obj).X == this.X
                    && ((PointM)obj).Y == this.Y
                    && ((PointM)obj).M == this.M;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            byte[] data1 = BitConverter.GetBytes(this.X);
            byte[] data2 = BitConverter.GetBytes(this.Y);
            byte[] data3 = BitConverter.GetBytes(this.M);
            return BitConverter.ToInt32(data1, 0) ^ BitConverter.ToInt32(data1, 4)
                ^ BitConverter.ToInt32(data2, 0) ^ BitConverter.ToInt32(data2, 4)
                ^ BitConverter.ToInt32(data3, 0) ^ BitConverter.ToInt32(data3, 4);
        }

        public static bool operator ==(PointM a, PointM b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PointM a, PointM b)
        {
            return !a.Equals(b);
        }
    }
}
