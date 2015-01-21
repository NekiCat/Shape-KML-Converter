using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml
{
    public static class Converter
    {
        public static SharpKml.Base.Vector ToVector(this DotSpatial.Topology.Vector vector)
        {
            return new SharpKml.Base.Vector(vector.Y, vector.X, vector.Z);
        }

        public static SharpKml.Base.Vector ToVector(this DotSpatial.Topology.Coordinate coord)
        {
            return new SharpKml.Base.Vector(coord.Y, coord.X, coord.Z);
        }

        public static DotSpatial.Topology.Vector ToVector(this SharpKml.Base.Vector vector)
        {
            if (vector.Altitude.HasValue)
            {
                return new DotSpatial.Topology.Vector(vector.Latitude, vector.Longitude, vector.Altitude.Value);
            }
            else
            {
                return new DotSpatial.Topology.Vector(vector.Latitude, vector.Longitude, 0);
            }
        }

        public static DotSpatial.Topology.Coordinate ToCoord(this SharpKml.Base.Vector vector)
        {
            if (vector.Altitude.HasValue)
            {
                return new DotSpatial.Topology.Coordinate(vector.Latitude, vector.Longitude);
            }
            else
            {
                return new DotSpatial.Topology.Coordinate(vector.Latitude, vector.Longitude, vector.Altitude.Value);
            }
        }

        public static SharpKml.Dom.LinearRing ToLinearRing(this DotSpatial.Topology.ILinearRing ring)
        {
            var result = new SharpKml.Dom.LinearRing();
            result.Coordinates = new SharpKml.Dom.CoordinateCollection();
            result.AltitudeMode = SharpKml.Dom.AltitudeMode.RelativeToGround;

            foreach (var item in ring.Coordinates)
            {
                result.Coordinates.Add(item.ToVector());
            }

            return result;
        }

        public static DotSpatial.Topology.ILinearRing ToLinearRing(this SharpKml.Dom.LinearRing ring)
        {
            var list = new List<DotSpatial.Topology.Coordinate>();

            foreach (var item in ring.Coordinates)
            {
                list.Add(item.ToCoord());
            }

            return new DotSpatial.Topology.LinearRing(list);
        }

        public static SharpKml.Dom.Polygon ToPolygon(this DotSpatial.Topology.IPolygon poly)
        {
            var result = new SharpKml.Dom.Polygon();
            result.AltitudeMode = SharpKml.Dom.AltitudeMode.RelativeToGround;

            result.OuterBoundary = new SharpKml.Dom.OuterBoundary()
            {
                LinearRing = poly.Shell.ToLinearRing()
            };
            foreach (var hole in poly.Holes)
            {
                result.AddInnerBoundary(new SharpKml.Dom.InnerBoundary()
                {
                    LinearRing = hole.ToLinearRing()
                });
            }
            return result;
        }

        public static DotSpatial.Topology.IPolygon ToPolygon(this SharpKml.Dom.Polygon poly)
        {
            var list = new List<DotSpatial.Topology.ILinearRing>();
            foreach (var ring in poly.InnerBoundary)
            {
                list.Add(ring.LinearRing.ToLinearRing());
            }
            return new DotSpatial.Topology.Polygon(poly.OuterBoundary.LinearRing.ToLinearRing(), list.ToArray());
        }
    }
}
