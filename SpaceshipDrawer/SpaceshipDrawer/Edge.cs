using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceshipDrawer.PolygonalModel
{
    class Edge
    {
        public Vertex[] Vertexes { get; set; } = new Vertex[2];

        public Edge(Vertex vertex1, Vertex vertex2)
        {
            Vertexes[0] = vertex1;
            Vertexes[1] = vertex2;
        }

    }
}
