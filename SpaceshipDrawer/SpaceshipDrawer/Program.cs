using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using SpaceshipDrawer.Common;
using SpaceshipDrawer.Model;
using SpaceshipDrawer.PolygonalModel;
using SpaceshipDrawer.Render;
using SpaceshipDrawer.Render.DX;
using System.Diagnostics;

namespace SpaceshipDrawer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DXRenderer renderer = new DXRenderer(Process.GetCurrentProcess().Handle);
            renderer.TryInitAll("test", 800, 800, null);
            renderer.UpdateScene(0.00106699858f);
            renderer.DrawScene();


            while (true)
            {
                Application.DoEvents();
                renderer.DrawScene();
            }




            Plane pl = new Plane(new Point3D(0, 0, 0), new Point3D(0, 1, 1), new Point3D(1, 0, 0));
            var pnt = new Point3D(0, 1, 0);

            HullMaterial material = new HullMaterial("Default", Color.FromRgb(150, 251, 100));
            GateJoint joint = new GateJoint(material);
            joint.Shape = new RectGateJointShape(10, 12, 5);

            RoomTemplate templateRoom1 = new RoomTemplate("left room");
            templateRoom1.XoYPlane = new Plane(
                new Point3D(1, 1, 1),
                new Point3D(2, 1, 1),
                new Point3D(1, 2, 1));
            templateRoom1.YoZPlane = new Plane(
               new Point3D(1, 1, 1),
               new Point3D(1, 1, 2),
               new Point3D(1, 2, 2));
            templateRoom1.ZoXPlane = new Plane(
               new Point3D(1, 1, 1),
               new Point3D(1, 1, 2),
               new Point3D(2, 1, 1));
            //room 1
            var p1 = new Point3D(0, 0, 0);
            var p2 = new Point3D(10, 0, 0);
            var p3 = new Point3D(10, 0, 10);
            var p4 = new Point3D(0, 0, 10);
            var p5 = new Point3D(0, 10, 0);
            var p6 = new Point3D(10, 10, 0);
            var p7 = new Point3D(10, 10, 10);
            var p8 = new Point3D(0, 10, 10);

            //1
            templateRoom1.Planes.Add(new Polygon(material, new List<Point3D>()
            {
                p1,
               p2,
               p3,
               p4
            }));
            //2
            templateRoom1.Planes.Add(new Polygon(material, new List<Point3D>()
            {
              p5,
                p8,
                p7,
                p6
            }));
            //3
            templateRoom1.Planes.Add(new Polygon(material, new List<Point3D>()
            {
              p1,p4,p8,p5
            }));
            //4
            var polygon4 = new Polygon(material, new List<Point3D>()
            {
               p2,p6,p7,p3
            });
            polygon4.Add(joint);
            templateRoom1.Planes.Add(polygon4);
            //5
            templateRoom1.Planes.Add(new Polygon(material, new List<Point3D>()
            {
                p1,p5,p6,p2
            }));
            //6
            templateRoom1.Planes.Add(new Polygon(material, new List<Point3D>()
            {
                p3,p7,p8,p4
            }));
            //room 2
            RoomTemplate templateRoom2 = new RoomTemplate("right room");
            templateRoom2.XoYPlane = new Plane(
              new Point3D(1, 1, 1),
              new Point3D(2, 1, 1),
              new Point3D(1, 2, 1));
            templateRoom2.YoZPlane = new Plane(
               new Point3D(1, 1, 1),
               new Point3D(1, 1, 2),
               new Point3D(1, 2, 2));
            templateRoom2.ZoXPlane = new Plane(
               new Point3D(1, 1, 1),
               new Point3D(1, 1, 2),
               new Point3D(2, 1, 1));

            p1 = new Point3D(0 + 15, 0, 0);
            p2 = new Point3D(10 + 15, 0, 0);
            p3 = new Point3D(10 + 15, 0, 10);
            p4 = new Point3D(0 + 15, 0, 10);
            p5 = new Point3D(0 + 15, 10, 0);
            p6 = new Point3D(10 + 15, 10, 0);
            p7 = new Point3D(10 + 15, 10, 10);
            p8 = new Point3D(0 + 15, 10, 10);
            //7
            templateRoom2.Planes.Add(new Polygon(material, new List<Point3D>()
            {
                p1,
               p2,
               p3,
               p4
            }));
            //8
            templateRoom2.Planes.Add(new Polygon(material, new List<Point3D>()
            {
              p5,
                p8,
                p7,
                p6
            }));
            //9
            var polygon9 = new Polygon(material, new List<Point3D>()
            {
                p1,p4,p8,p5
            });
            polygon9.Add(joint);
            templateRoom2.Planes.Add(polygon9);
            //10
            templateRoom2.Planes.Add(new Polygon(material, new List<Point3D>()
            {
           p2,p6,p7,p3
            }));
            //11
            templateRoom2.Planes.Add(new Polygon(material, new List<Point3D>()
            {
                p1,p5,p6,p2
               }));
            //12
            templateRoom2.Planes.Add(new Polygon(material, new List<Point3D>()
            {
               p3,p7,p8,p4
            }));


            Hull model = new Hull("Ship1");
            model.AddRoom(new Room("Left room", templateRoom1, new Point3D(5, 5, 5)));
            model.AddRoom(new Room("Right room", templateRoom2, new Point3D(20, 5, 5)));


            RoomPolygonizer polygonizer = new RoomPolygonizer(new Room("Left room", templateRoom1, new Point3D(5, 5, 5)));
            var v = polygonizer.GetVertices();



        }
    }
}
