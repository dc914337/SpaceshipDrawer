using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceshipDrawer.PolygonalModel
{
    class PolygonalModel
    {
        private List<Edge> _edges = new List<Edge>();
        private List<Vertex> _vertices = new List<Vertex>();

        public void AddFragment(Vertex vertex)
        {
            _vertices.Add(vertex);
            Reindex();
        }


        public void Reindex()
        {
            bool done;
            do
            {
                var newEdges = GetNotIndexedEdges();
                _edges.AddRange(newEdges);
                var newVertices = GetNotIndexedVertices();
                _vertices.AddRange(newVertices);
                done = !(newEdges.Any() || newVertices.Any());
            } while (!done);
        }

        private IEnumerable<Edge> GetNotIndexedEdges()
        {
            List<Edge> res = new List<Edge>();
            foreach (var vertex in _vertices)
            {
                res.AddRange(vertex.Edges.Where(a => !_edges.Contains(a)).Distinct());
            }
            return res.Distinct();
        }
        private IEnumerable<Vertex> GetNotIndexedVertices()
        {
            List<Vertex> res = new List<Vertex>();
            foreach (var edge in _edges)
            {
                res.AddRange(edge.Vertexes.Where(a => !_vertices.Contains(a)).Distinct());
            }
            return res.Distinct();
        }

    }
}
