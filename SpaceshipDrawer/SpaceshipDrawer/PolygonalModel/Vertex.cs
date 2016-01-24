using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SpaceshipDrawer.PolygonalModel
{
    public class Vertex
    {
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public int Index { get; set; }

        public Point3D Position3D { get; set; }

        public Vertex(double x, double y, double z, int index)
        {
            Position3D = new Point3D(x, y, z);
            Index = index;
        }

        public Vertex(Point3D position3D, int index)
        {
            Position3D = position3D;
            Index = index;
        }

        public Edge AddVertex(Vertex nextVertex)
        {
            var newEdge = new Edge(this, nextVertex);
            Edges.Add(newEdge);
            nextVertex.Index = Index++;
            return newEdge;
        }
    }
}
