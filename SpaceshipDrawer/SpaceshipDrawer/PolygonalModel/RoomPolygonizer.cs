using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using SpaceshipDrawer.Model;
using MIConvexHull;

namespace SpaceshipDrawer.PolygonalModel
{
    class RoomPolygonizer
    {
        private Room _realRoom;


        public RoomPolygonizer(Room room)
        {
            _realRoom = room;
        }



        public IEnumerable<Point3D> GetVertices()
        {
            int index = 0;
            List<TriangulableVertex> lst = new List<TriangulableVertex>();
            foreach (var plane in _realRoom.Template.Planes)
            {
                lst.AddRange(plane.Points.Select(a => new TriangulableVertex(a.X, a.Y, a.Z, index++)).ToList());
            }


            var convexHull = ConvexHull.Create<TriangulableVertex, TriangulableFace>(lst);

            var faceTris = new Int32Collection();
            var convexHullVertices = convexHull.Points.ToList();
            foreach (var f in convexHull.Faces)
            {
                // The vertices are stored in clockwise order.
                faceTris.Add(convexHullVertices.IndexOf(f.Vertices[0]));
                faceTris.Add(convexHullVertices.IndexOf(f.Vertices[1]));
                faceTris.Add(convexHullVertices.IndexOf(f.Vertices[2]));
            }
            return faceTris.Select(a => convexHullVertices[a].Position3D).ToList();
        }

    }

    public class TriangulableVertex : Vertex, IVertex
    {
        public TriangulableVertex(double x, double y, double z, int index) : base(x, y, z, index)
        {
            Position = new double[] { x, y, z };
        }

        public TriangulableVertex(Point3D position3D, int index) : base(position3D, index)
        {
            Position = new double[] { position3D.X, position3D.Y, position3D.Z };
        }

        public double[] Position { get; }
    }


    public class TriangulableFace : ConvexFace<TriangulableVertex, TriangulableFace>
    {

    }
}
