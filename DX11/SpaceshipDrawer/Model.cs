using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml;

namespace SpaceshipDrawer.Model
{
    
    /// <summary>
    /// Материал корпуса
    /// </summary>
    public class HullMaterial
    {
        public Color Color { get; set; }
        public string Name { get; set; }

        public HullMaterial(string name, Color color)
        {
            this.Name = name;
            this.Color = color;
        }

        public static HullMaterial Unserial(XmlElement el)
        {
            if (el.Name != "HullMaterial")
                throw new ArgumentException();

            return new HullMaterial(
                el.Attributes["Name"].Value,
                (Color)ColorConverter.ConvertFromString(el.Attributes["Color"].Value)
            );
        }

        public void SerialTo(XmlElement targetElement)
        {
            var el = targetElement.OwnerDocument.CreateElement("HullMaterial");
            el.SetAttribute("Name", this.Name);
            el.SetAttribute("Color", this.Color.ToString());
            targetElement.AppendChild(el);
        }
    }

    /// <summary>
    /// Вид балки скелета
    /// </summary>
    public class FixtureKind
    {
        public string Name { get; set; }
        public HullMaterial Material { get; set; }

        /// <summary>
        /// Размер сечения балки
        /// </summary>
        public double Size { get; private set; }

        public FixtureKind(string name, HullMaterial material, double size)
        {
            this.Name = name;
            this.Size = size;
            this.Material = material;
        }

        public static FixtureKind Unserial(XmlElement el, Hull hull)
        {
            if (el.Name != "FixtureKind")
                throw new ArgumentException();

            var materialName = el.Attributes["Material"].Value;

            return new FixtureKind(
               el.Attributes["Name"].Value,
               hull.Materials.First(m => m.Name == materialName),
               double.Parse(el.Attributes["Size"].Value)
            );
        }

        public void SerialTo(XmlElement targetElement)
        {
            var el = targetElement.OwnerDocument.CreateElement("FixtureKind");
            el.SetAttribute("Name", this.Name);
            el.SetAttribute("Size", this.Size.ToString());
            el.SetAttribute("Material", this.Material.Name);
            targetElement.AppendChild(el);
        }
    }

    /// <summary>
    /// Балка скелета
    /// </summary>
    public class Fixture 
    {
        static readonly Point3DConverter _pointsConverter = new Point3DConverter();

        public Point3D From { get; set; }
        public Point3D To { get; set; }

        public FixtureKind Kind { get; private set; }

        public Fixture(FixtureKind kind, Point3D from, Point3D to)
        {
            this.Kind = kind;
            this.From = from;
            this.To = to;
        }

        public static Fixture Unserial(XmlElement el, Hull hull)
        {
            if (el.Name != "Fixture")
                throw new ArgumentException();

            var fixtureKindName = el.Attributes["Kind"].Value;

            return new Fixture(
               hull.FixtureKinds.First(m => m.Name == fixtureKindName),
               (Point3D)_pointsConverter.ConvertFromString(el.Attributes["From"].Value),
               (Point3D)_pointsConverter.ConvertFromString(el.Attributes["To"].Value)
            );
        }

        public void SerialTo(XmlElement targetElement)
        {
            var el = targetElement.OwnerDocument.CreateElement("Fixture");
            el.SetAttribute("Kind", this.Kind.Name);
            el.SetAttribute("From", _pointsConverter.ConvertToString(this.From));
            el.SetAttribute("To", _pointsConverter.ConvertToString(this.To));
            targetElement.AppendChild(el);
        }


        public PolygonalModel.PolygonalModel GeneratePolygonalModel()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Режим интерполяции точек фрагмента обшивки
    /// </summary>
    public enum CoverFragmentInterpolation
    {
        /// <summary>
        /// Строить ломаную через заданные точки
        /// </summary>
        None,
        /// <summary>
        /// Строить приближенную кривую, интерпполируя промежуточные точки с помощью кривой Безье на основе заданных
        /// </summary>
        Bezier
    }

    /// <summary>
    /// Фрагмент обшивки
    /// </summary>
    public class CoverFragment : IPoligonable
    {
        List<Point3D> _points;
        public IList<Point3D> Points { get { return _points; } }

        public HullMaterial Material { get; set; }
        public CoverFragmentInterpolation Interpolation { get; set; }

        public CoverFragment(HullMaterial material, params Point3D[] points)
        {
            _points = new List<Point3D>(points);
            this.Material = material;
            this.Interpolation = CoverFragmentInterpolation.None;
        }

        public PolygonalModel.PolygonalModel GeneratePolygonalModel()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Форма шлюза
    /// </summary>
    public abstract class GateJointShape
    {
        /// <summary>
        /// Глубина шлюза
        /// </summary>
        public double Depth { get; set; }

        public GateJointShape(double depth)
        {
            this.Depth = depth;
        }
    }

    /// <summary>
    /// Круг\Эллипсоид
    /// </summary>
    public class EllipsoidGateJointShape : GateJointShape
    {
        public double InternalRadius { get; set; }
        public double ExternalRadius { get; set; }

        public EllipsoidGateJointShape(double internalRadius, double externalRadius, double depth)
            : base(depth)
        {
            this.InternalRadius = internalRadius;
            this.ExternalRadius = externalRadius;
        }
    }

    /// <summary>
    /// Параллелепипед
    /// </summary>
    public class RectGateJointShape : GateJointShape
    {
        public double Height { get; set; }
        public double Width { get; set; }

        /// <summary>
        /// Радиус скругления углов шлюза, если задано.
        /// </summary>
        public double? RoundRadius { get; set; }

        public RectGateJointShape(double height, double width, double depth)
            : base(depth)
        {
            this.Height = height;
            this.Width = width;
            this.RoundRadius = null;
        }
    }

    /// <summary>
    /// Многоугольный шлюз
    /// </summary>
    public class PolyGateJointShape : RectGateJointShape
    {
        /// <summary>
        /// Количество уголов.
        /// <remarks>
        /// Для нечетного непарная вершина всегда сверху.
        /// Вершины располагаются по полокружиям радиуса Height, расстояние меж полукружиями Width.
        /// </remarks>
        /// </summary>
        public int VerticesCount { get; set; }

        public PolyGateJointShape(double height, double width, int verticesCount, double depth)
            : base(height, width, depth)
        {
            this.VerticesCount = verticesCount;
        }
    }

    /// <summary>
    /// Расположение шлюза отсека
    /// </summary>
    public class GateJoint
    {
        public HullMaterial Material { get; set; }
        public GateJointShape Shape { get; set; }

        public Point3D Position { get; set; }

        public GateJoint(HullMaterial material)
        {
            this.Material = material;
        }
    }

    /// <summary>
    /// Плоскость в пространстве
    /// </summary>
    public class Plane
    {
        public Vector3D Normal { get; private set; }
        public double Distance { get; private set; }

        public Plane(Point3D a, Point3D b, Point3D c)
        {
            var n = Vector3D.CrossProduct(b - a, c - b);
            n.Normalize();
            Distance = (a.X * n.X + a.Y * n.Y + a.Z * n.Z);
            Normal = n;
        }

        public Plane(Vector3D normal, double distance)
        {
            this.Normal = normal;
            this.Distance = distance;
        }
    }

    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    /// <summary>
    /// Полигон на плоскости в пространстве.
    /// Все точки принадлежат первоначально заданной плоскости.
    /// </summary>
    public class Polygon
    {
        List<Point3D> _points = new List<Point3D>();
        public IEnumerable<Point3D> Points { get { return _points; } }

        List<GateJoint> _gates = new List<GateJoint>();
        public IEnumerable<GateJoint> Gates { get { return _gates; } }

        public HullMaterial Material { get; set; }

        public Plane Plane { get; private set; }


        public Polygon(HullMaterial material, IEnumerable<Point3D> points)
        {
            this.Material = material;
            var planePoints = points.Take(3).ToArray();
            this.Plane = new Plane(planePoints[0], planePoints[1], planePoints[2]);
            _points.AddRange(points);
        }


        public Polygon(HullMaterial material, Point3D a, Point3D b, Point3D c)
        {
            this.Material = material;
            this.Plane = new Plane(a, b, c);
        }



        public void Add(Point3D p)
        {
            // project point to plane
            var n = this.Plane.Normal;
            var d = (p.X * n.X + p.Y * n.Y + p.Z * n.Z); // distance from zero to plane throug point
            var dst = d - this.Plane.Distance; // distance between planes
            var off = n * dst; // projection vector
            var pt = p + off;
            _points.Add(pt);
        }

        public void Add(GateJoint gate)
        {
            // форма шлюза должна помещаться внутри полигона
            _gates.Add(gate);
        }
    }


    /// <summary>
    /// Шаблон отсека
    /// </summary>
    public class RoomTemplate
    {
        public string Name { get; set; }

        List<Polygon> _planes = new List<Polygon>();
        public IList<Polygon> Planes { get { return _planes; } }

        // XoYPlane, YoZPlane, ZoXPlane - плоскости разбиения, разделяющие группы вершин в направлениях масштабирования
        // При масштабировании в некотором направлении, соответствующие группы вершин раздвигаются в противоположные стороны с сохранением полигональной сетки.

        public Plane XoYPlane { get; set; }
        public Plane YoZPlane { get; set; }
        public Plane ZoXPlane { get; set; }

        public RoomTemplate(string name)
        {
            this.Name = name;
        }
    }





    /// <summary>
    /// Отсек
    /// </summary>
    public class Room
    {
        public string Name { get; set; }

        public Point3D Position { get; set; }

        /// <summary>
        /// Добавочный размер, соответствующий масштабированию шаблона через раздвижение в соответствиями с плоскостями разбиения
        /// </summary>
        public Vector3D ExtraSize { get; set; }

        public RoomTemplate Template { get; private set; }

        public Room(string name, RoomTemplate template, Point3D position)
        {
            this.Name = name;
            this.Template = template;
            this.Position = position;
        }

    }

    /// <summary>
    /// Корпус
    /// </summary>
    public class Hull : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string Name { get; set; }

        // словари применены только для упрощения индексации при редактировании структуры модели
        Dictionary<string, HullMaterial> _materials = new Dictionary<string, HullMaterial>();
        public IEnumerable<HullMaterial> Materials { get { return _materials.Values; } }

        Dictionary<string, FixtureKind> _fixtureKinds = new Dictionary<string, FixtureKind>();
        public IEnumerable<FixtureKind> FixtureKinds { get { return _fixtureKinds.Values; } }

        Dictionary<string, RoomTemplate> _roomTemplates = new Dictionary<string, RoomTemplate>();
        public IEnumerable<RoomTemplate> RoomTemplates { get { return _roomTemplates.Values; } }


        //objects to create polygon model from
        IList<Fixture> _fixtures = new List<Fixture>();
        public IList<Fixture> Fixtures { get { return _fixtures; } }

        IList<Room> _rooms = new List<Room>();
        public IList<Room> Rooms { get { return _rooms; } }

        IList<CoverFragment> _coverings = new List<CoverFragment>();
        public IList<CoverFragment> Coverings { get { return _coverings; } }


        public Hull(string name)
        {
            this.Name = name;
        }

        private void RaizePropertyChanged(string propName)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void AddMaterial(HullMaterial material)
        {
            _materials.Add(material.Name, material);
            this.RaizePropertyChanged("Materials");
        }

        public void AddFixtureKind(FixtureKind kind)
        {
            _fixtureKinds.Add(kind.Name, kind);
            this.RaizePropertyChanged("FixtureKinds");
        }

        public void AddFixture(Fixture fixture)
        {
            _fixtures.Add(fixture);
            this.RaizePropertyChanged("Fixtures");
        }

        public void AddRoom(Room room)
        {
            _rooms.Add(room);
            this.RaizePropertyChanged("Rooms");
        }

        public void AddRoomTemplate(RoomTemplate roomTemplate)
        {
            _roomTemplates.Add(roomTemplate.Name, roomTemplate);
            this.RaizePropertyChanged("RoomTemplates");
        }

        public void AddCover(GeometryModel3D model, CoverFragment cover)
        {
            _coverings.Add(cover);
            this.RaizePropertyChanged("Coverings");
        }

        public static Hull Unserial(XmlElement root)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            try
            {

                if (root.Name != "Hull")
                    throw new ArgumentException();

                var hull = new Hull(root.Attributes["Name"].Value);

                foreach (XmlElement el in root.GetElementsByTagName("HullMaterial"))
                    hull.AddMaterial(HullMaterial.Unserial(el));

                foreach (XmlElement el in root.GetElementsByTagName("FixtureKind"))
                    hull.AddFixtureKind(FixtureKind.Unserial(el, hull));

                foreach (XmlElement el in root.GetElementsByTagName("Fixture"))
                {
                    var f = Fixture.Unserial(el, hull);
                    //hull.AddFixture(f.MakeGeometry(), f);
                }

                //foreach (XmlElement el in root.GetElementsByTagName("RoomTemplate"))
                //    hull.AddRoomTemplate(RoomTemplate.Unserial(el, hull));

                //foreach (XmlElement el in root.GetElementsByTagName("Room"))
                //{
                //    var r = Room.Unserial(el, hull);
                //    hull.AddRoom(r.MakeGeometry(), r);
                //}

                //foreach (XmlElement el in root.GetElementsByTagName("CoverFragment"))
                //{
                //    var c = CoverFragment.Unserial(el, hull);
                //    hull.AddCover(c.MakeGeometry(), c);
                //}
                return hull;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = culture;
            }
        }

        public XmlDocument Serial()
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; ;
            try
            {
                var doc = new XmlDocument();
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
                var root = doc.CreateElement("Hull");
                doc.AppendChild(root);
                root.SetAttribute("Name", this.Name);

                foreach (var item in _materials.Values)
                    item.SerialTo(root);

                foreach (var item in _fixtureKinds.Values)
                    item.SerialTo(root);

                //  foreach (var item in _fixtures.Values)
                //  item.SerialTo(root);

                //foreach (var item in _roomTemplates.Values)
                //    item.SerialTo(root);

                //foreach (var item in _rooms.Values)
                //    item.SerialTo(root);

                //foreach (var item in _coverings.Values)
                //    item.SerialTo(root);

                return doc;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = culture;
            }
        }

    }
}