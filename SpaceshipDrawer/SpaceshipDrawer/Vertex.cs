using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceshipDrawer.PolygonalModel
{
    class Vertex
    {
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Vertex(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Edge AddVertex(Vertex nextVertex)
        {
            var newVertex = new Edge(this, nextVertex);
            Edges.Add(newVertex);
            return newVertex;
        }
    }
}
