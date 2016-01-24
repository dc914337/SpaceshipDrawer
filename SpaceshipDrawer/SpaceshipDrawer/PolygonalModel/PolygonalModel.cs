using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceshipDrawer.PolygonalModel
{
    public class PolygonalModel
    {
        private List<Edge> _edges = new List<Edge>();
        private List<Vertex> _vertices = new List<Vertex>();
        private Vertex _maxIndexVertex { get; set; } = null;


        public void AddFragment(Vertex vertex, bool connect)
        {
            AddVertex(vertex, connect);
            Reindex();
        }

        public void AddFragment(PolygonalModel model, bool connect)
        {
            var startVertex = model._vertices.First(a => a.Index == model._vertices.Min(b => b.Index));
            AddFragment(startVertex, connect);
        }

        public void AddVertex(Vertex vertex, bool createEdge)
        {
            Vertex oldLastVertex = _maxIndexVertex;

            if (_maxIndexVertex != null)
            {
                vertex.Index = _maxIndexVertex.Index + 1;
            }

            _maxIndexVertex = vertex;
            _vertices.Add(vertex);

            if (createEdge && oldLastVertex != null)
            {
                _edges.Add(oldLastVertex.AddVertex(vertex));
            }
        }



        public void Reindex()
        {
            bool done;
            do
            {
                var newVertices = GetNotIndexedVertices();
                var orderedByIndexVertices = newVertices.OrderBy(a => a.Index).ToList();
                foreach (var newVertex in orderedByIndexVertices)
                {
                    AddVertex(newVertex, false);
                }

                var newEdges = GetNotIndexedEdges();
                _edges.AddRange(newEdges);

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
