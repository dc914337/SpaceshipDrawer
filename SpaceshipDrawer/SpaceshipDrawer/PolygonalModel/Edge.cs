using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceshipDrawer.PolygonalModel
{
    public class Edge
    {
        //sorted by Vertexes' indexes
        public Vertex[] Vertexes { get; set; } = new Vertex[2];

        public Edge(Vertex vertex1, Vertex vertex2)
        {
            if (vertex1.Index < vertex2.Index)
            {
                Vertexes[0] = vertex1;
                Vertexes[1] = vertex2;
            }
            else
            {
                Vertexes[0] = vertex2;
                Vertexes[1] = vertex1;
            }

          
        }

    }
}
