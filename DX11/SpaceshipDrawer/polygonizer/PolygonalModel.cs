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
        List<IndexedVertex> _vertexes = new List<IndexedVertex>();
        Mesh _lastAddedMesh = null;

        public void AddMesh(Mesh mesh)
        {
            var firstVertex = mesh.Vertexes.
                First(a => a.Index == mesh.Vertexes.
                Min(b => b.Index));

            foreach (var vertex in mesh.Vertexes.OrderBy(a => a.Index))
            {
                var newIndex = 0;
                if (_vertexes.Any())
                    newIndex = _vertexes.Last().Index + 1;
                _vertexes.Add(vertex);
                vertex.Index = newIndex;
            }
            _vertexes.Add(new IndexedVertex(_vertexes.Last().Index + 1, firstVertex.Vertex));
        }

    }


    class Mesh
    {
        private const byte VERTEXES_COUNT = 3;
        public IndexedVertex[] Vertexes = new IndexedVertex[VERTEXES_COUNT];

    }

    class IndexedVertex
    {
        public int Index;
        public Vertex Vertex;

        public IndexedVertex(int index, Vertex vertex)
        {
            Index = index;
            Vertex = vertex;
        }
    }

    class Vertex
    {
        public Point3D Position;
    }
}
