using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shp2kml
{
    public static class Converter
    {
        #region Primitives

        public static SharpKml.Base.Vector ToVector(this DotSpatial.Topology.Vector vector)
        {
            if (Double.IsNaN(vector.Z))
            {
                return new SharpKml.Base.Vector(vector.Y, vector.X);
            }
            else
            {
                return new SharpKml.Base.Vector(vector.Y, vector.X, vector.Z);
            }
        }

        public static SharpKml.Base.Vector ToVector(this DotSpatial.Topology.Coordinate coord)
        {
            if (Double.IsNaN(coord.Z))
            {
                return new SharpKml.Base.Vector(coord.Y, coord.X);
            }
            else
            {
                return new SharpKml.Base.Vector(coord.Y, coord.X, coord.Z);
            }
        }

        public static DotSpatial.Topology.Vector ToVector(this SharpKml.Base.Vector vector)
        {
            if (vector.Altitude.HasValue)
            {
                return new DotSpatial.Topology.Vector(vector.Latitude, vector.Longitude, vector.Altitude.Value);
            }
            else
            {
                return new DotSpatial.Topology.Vector(vector.Latitude, vector.Longitude, Double.NaN);
            }
        }

        public static DotSpatial.Topology.Coordinate ToCoord(this SharpKml.Base.Vector vector)
        {
            if (vector.Altitude.HasValue)
            {
                return new DotSpatial.Topology.Coordinate(vector.Latitude, vector.Longitude, vector.Altitude.Value);
            }
            else
            {
                return new DotSpatial.Topology.Coordinate(vector.Latitude, vector.Longitude);
            }
        }

        #endregion

        #region Geometry

        public static SharpKml.Dom.Point ToPoint(this DotSpatial.Topology.IPoint point)
        {
            if (Double.IsNaN(point.Z))
            {
                return new SharpKml.Dom.Point()
                {
                    Coordinate = new SharpKml.Base.Vector(point.Y, point.X)
                };
            }
            else
            {
                return new SharpKml.Dom.Point()
                {
                    Coordinate = new SharpKml.Base.Vector(point.Y, point.X, point.Z)
                };
            }
        }

        public static DotSpatial.Topology.IPoint ToPoint(this SharpKml.Dom.Point point)
        {
            return new DotSpatial.Topology.Point(point.Coordinate.ToCoord());
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

        public static SharpKml.Dom.MultipleGeometry ToMultiPolygon(this DotSpatial.Topology.IMultiPolygon poly)
        {
            var result = new SharpKml.Dom.MultipleGeometry();

            foreach (var p in poly.Geometries)
            {
                result.AddGeometry((p as DotSpatial.Topology.IPolygon).ToPolygon());
            }

            return result;
        }

        public static DotSpatial.Topology.IMultiPolygon ToMultiPolygon(this SharpKml.Dom.MultipleGeometry poly)
        {
            var list = new List<DotSpatial.Topology.IPolygon>();
            foreach (var p in poly.Geometry)
            {
                list.Add((p as SharpKml.Dom.Polygon).ToPolygon());
            }
            return new DotSpatial.Topology.MultiPolygon(list.ToArray());
        }

        #endregion

        public static SharpKml.Dom.Feature ToFeature(this DotSpatial.Data.IFeature feature)
        {
            if (feature.BasicGeometry is DotSpatial.Topology.IPoint)
            {
                var point = (feature.BasicGeometry as DotSpatial.Topology.IPoint).ToPoint();
                return new SharpKml.Dom.Placemark()
                {
                    Geometry = point
                };
            }
            if (feature.BasicGeometry is DotSpatial.Topology.IPolygon)
            {
                var polygon = (feature.BasicGeometry as DotSpatial.Topology.IPolygon).ToPolygon();
                return new SharpKml.Dom.Placemark()
                {
                    Geometry = polygon
                };
            }
            if (feature.BasicGeometry is DotSpatial.Topology.IMultiPolygon)
            {
                var mpoly = (feature.BasicGeometry as DotSpatial.Topology.IMultiPolygon).ToMultiPolygon();
                return new SharpKml.Dom.Placemark()
                {
                    Geometry = mpoly
                };
            }

            throw new NotImplementedException();
        }
    }
}
