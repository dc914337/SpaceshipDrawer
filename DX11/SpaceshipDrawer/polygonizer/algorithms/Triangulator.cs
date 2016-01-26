using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIConvexHull;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using Triangulator;
using Triangulator.Geometry;

namespace SpaceshipDrawer.polygonizer.algorithms
{
    public class DXTriangulator
    {
        public static IEnumerable<Mesh> TriangulateNMesh(IEnumerable<Point3D> vertexes)
        {
            int index = 0;



            List<Point> Vertices = vertexes.Select(a => new Translated2DPoint(a, SelectCoordinateToLose(vertexes))).ToList<Point>();
            //Do triangulation
            List<Triangle> tris = Delauney.Triangulate(Vertices);


            return tris.Select(a =>
            new Mesh(new Point3D[] {
                ((Translated2DPoint)Vertices[a.p1]).Point3D,
               ((Translated2DPoint)Vertices[a.p2]).Point3D,
             ((Translated2DPoint)Vertices[a.p3]).Point3D
            }));
        }
        private static LosingCoordinate SelectCoordinateToLose(IEnumerable<Point3D> vertexes)
        {
            LosingCoordinate losingCoordinate;
            int countX = CountDistinctWhenLosingCoord(vertexes, LosingCoordinate.X);
            int countY = CountDistinctWhenLosingCoord(vertexes, LosingCoordinate.Y);
            int countZ = CountDistinctWhenLosingCoord(vertexes, LosingCoordinate.Z);

            int max = new int[] { countX, countY, countZ }.Max();

            if (countX == max)
                return LosingCoordinate.X;
            else if (countY == max)
                return LosingCoordinate.Y;
            else
                return LosingCoordinate.Z;

        }

        private static int CountDistinctWhenLosingCoord(IEnumerable<Point3D> vertexes, LosingCoordinate losingCoordinate)
        {
            return vertexes.
                Select(a => new Translated2DPoint(a, losingCoordinate))
                .GroupBy(b => new { b.X, b.Y })
                .Select(c => c.First()).Count();
        }




        private class Translated2DPoint : Triangulator.Geometry.Point, IEqualityComparer<Translated2DPoint>
        {
            public Point3D Point3D { get; set; }

            public Translated2DPoint(Point3D point3d, LosingCoordinate losingCoordinate) : base(0, 0)
            {
                Point3D = point3d;
                switch (losingCoordinate)
                {
                    case LosingCoordinate.X:
                        base.X = point3d.Y;
                        base.Y = point3d.Z;
                        break;
                    case LosingCoordinate.Y:
                        base.X = point3d.X;
                        base.Y = point3d.Z;
                        break;
                    case LosingCoordinate.Z:
                        base.X = point3d.X;
                        base.Y = point3d.Y;
                        break;
                }
            }




            public bool Equals(Translated2DPoint x, Translated2DPoint y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(Translated2DPoint obj)
            {
                throw new NotImplementedException();
            }
        }


        enum LosingCoordinate
        {
            X,
            Y,
            Z
        }
    }









}
