using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SpaceshipDrawer.polygonizer
{
    class PolygonalModel
    {
        public List<Vertex> Vertexes { get; private set; } = new List<Vertex>();
        public List<int> Indexes { get; private set; } = new List<int>();

        public void AddMesh(Mesh mesh)
        {
            int firstVertexIndex = 0;

            if (Vertexes.Any())
                firstVertexIndex = Indexes.Max() + 1;

            foreach (var vertex in mesh.Vertexes)
            {
                var newIndex = 0;
                if (Vertexes.Any())
                    newIndex = Indexes.Max() + 1;
                Vertexes.Add(vertex);
                Indexes.Add(newIndex);
            }
            Indexes.Add(firstVertexIndex);
        }

    }


    public class Mesh
    {
        private const byte VERTEXES_COUNT = 3;
        public Vertex[] Vertexes = new Vertex[VERTEXES_COUNT];

        public Mesh(Point3D[] vertexes)
        {
            Vertexes = vertexes.Select(a => new Vertex(a)).ToArray();
        }

    }


    public class Vertex
    {
        public Point3D Position;

        public Vertex(double x, double y, double z)
        {
            Position = new Point3D(x, y, z);
        }

        public Vertex(Point3D p)
        {
            Position = p;
        }

    }
}
