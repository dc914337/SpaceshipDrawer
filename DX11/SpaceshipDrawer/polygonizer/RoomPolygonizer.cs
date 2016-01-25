using SpaceshipDrawer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                wallMeshes.AddRange(GetWallMeshes(wall));
            }

            return model.GetMeshes();
        }

        private List<Mesh> GetWallMeshes(Polygon plane)
        {

            return null;
        }


    }
}
