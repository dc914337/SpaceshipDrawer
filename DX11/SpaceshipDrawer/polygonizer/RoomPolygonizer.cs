using SpaceshipDrawer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;
using System.Windows.Media;
using SpaceshipDrawer.polygonizer.algorithms;

namespace SpaceshipDrawer.polygonizer
{
    class RoomPolygonizer
    {
        private Room _room;
        public RoomPolygonizer(Room room)
        {
            _room = room;
        }

        public PolygonalModel GetMeshes()
        {
            PolygonalModel model = new PolygonalModel();

            List<Mesh> wallMeshes = new List<Mesh>();
            foreach (var wall in _room.Template.Planes)
            {
                foreach (var mesh in GetWallMeshes(wall))
                {
                    model.AddMesh(mesh);
                }

            }
            return model;
        }

        private IEnumerable<Mesh> GetWallMeshes(Polygon plane)
        {
            return DXTriangulator.TriangulateNMesh(plane.Points);
        }



    }
}
