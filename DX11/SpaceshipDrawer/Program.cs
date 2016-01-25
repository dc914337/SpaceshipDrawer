using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using SpaceshipDrawer.Model;
using Debug = System.Diagnostics.Debug;


namespace SpaceshipDrawer
{
    using System.Windows.Forms;

    using Core;
    using Core.Vertex;

    using SlimDX;

    using Buffer = SlimDX.Direct3D11.Buffer;
    using Effect = SlimDX.Direct3D11.Effect;
    using SpaceshipDrawer.Model;
    class ShapesDemo : D3DApp
    {
        private Buffer _vb;
        private Buffer _ib;

        private Effect _fx;
        private EffectTechnique _tech;
        private EffectMatrixVariable _fxWVP;

        private InputLayout _inputLayout;

        private RasterizerState _wireframeRS;
        private Matrix[] _sphereWorld = new Matrix[10];
        private Matrix[] _cylWorld = new Matrix[10];
        private Matrix _boxWorld;
        private Matrix _gridWorld;
        private Matrix _centerSphere;

        private Matrix _view;
        private Matrix _proj;

        private int _spaceShipVertexOffset;
        private int _spaceShipIndexOffset;
        private int _spaceShipIndexCount;

        private float _theta;
        private float _phi;
        private float _radius;
        private Point _lastMousePos;

        private bool _disposed;
        public ShapesDemo(IntPtr hInstance)
            : base(hInstance)
        {
            _vb = null;
            _ib = null;
            _fx = null;
            _tech = null;
            _fxWVP = null;
            _inputLayout = null;
            _wireframeRS = null;
            _theta = 1.5f * MathF.PI;
            _phi = 0.1f * MathF.PI;
            _radius = 15.0f;

            MainWindowCaption = "Shapes Demo";
            _lastMousePos = new Point(0, 0);

            _gridWorld = Matrix.Identity;
            _view = Matrix.Identity;
            _proj = Matrix.Identity;

            _boxWorld = Matrix.Scaling(2.0f, 1.0f, 2.0f) * Matrix.Translation(0, 0.5f, 0);
            _centerSphere = Matrix.Scaling(2.0f, 2.0f, 2.0f) * Matrix.Translation(0, 2, 0);

            for (int i = 0; i < 5; ++i)
            {
                _cylWorld[i * 2] = Matrix.Translation(-5.0f, 1.5f, -10.0f + i * 5.0f);
                _cylWorld[i * 2 + 1] = Matrix.Translation(5.0f, 1.5f, -10.0f + i * 5.0f);

                _sphereWorld[i * 2] = Matrix.Translation(-5.0f, 3.5f, -10.0f + i * 5.0f);
                _sphereWorld[i * 2 + 1] = Matrix.Translation(5.0f, 3.5f, -10.0f + i * 5.0f);
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Util.ReleaseCom(ref _vb);
                    Util.ReleaseCom(ref _ib);
                    Util.ReleaseCom(ref _fx);
                    Util.ReleaseCom(ref _inputLayout);
                    Util.ReleaseCom(ref _wireframeRS);
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
        public override bool Init()
        {
            if (!base.Init())
            {
                return false;
            }
            BuildGeometryBuffers();
            BuildFX();
            BuildVertexLayout();

            var wireFrameDesc = new RasterizerStateDescription
            {
                FillMode = FillMode.Wireframe,
                CullMode = CullMode.Back,
                IsFrontCounterclockwise = false,
                IsDepthClipEnabled = true
            };

            _wireframeRS = RasterizerState.FromDescription(Device, wireFrameDesc);
            return true;
        }
        public override void OnResize()
        {
            base.OnResize();

            _proj = Matrix.PerspectiveFovLH(0.25f * MathF.PI, AspectRatio, 1.0f, 1000.0f);
        }
        public override void UpdateScene(float dt)
        {
            base.UpdateScene(dt);

            // Get camera position from polar coords
            var x = _radius * MathF.Sin(_phi) * MathF.Cos(_theta);
            var z = _radius * MathF.Sin(_phi) * MathF.Sin(_theta);
            var y = _radius * MathF.Cos(_phi);

            // Build the view matrix
            var pos = new Vector3(x, y, z);
            var target = new Vector3(0);
            var up = new Vector3(0, 1, 0);
            _view = Matrix.LookAtLH(pos, target, up);
        }
        public override void DrawScene()
        {
            base.DrawScene();
            ImmediateContext.ClearRenderTargetView(RenderTargetView, Color.Wheat);
            ImmediateContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);

            ImmediateContext.InputAssembler.InputLayout = _inputLayout;
            ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            ImmediateContext.Rasterizer.State = _wireframeRS;

            ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vb, VertexPC.Stride, 0));
            ImmediateContext.InputAssembler.SetIndexBuffer(_ib, Format.R32_UInt, 0);

            var viewProj = _view * _proj;

            var techDesc = _tech.Description;
            for (int p = 0; p < techDesc.PassCount; p++)
            {
                _fxWVP.SetMatrix(_boxWorld * viewProj);
                _tech.GetPassByIndex(p).Apply(ImmediateContext);
                ImmediateContext.DrawIndexed(_spaceShipIndexCount, _spaceShipIndexOffset, _spaceShipVertexOffset);

            }
            SwapChain.Present(0, PresentFlags.None);
        }
        protected override void OnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            _lastMousePos = mouseEventArgs.Location;
            Window.Capture = true;
        }
        protected override void OnMouseUp(object sender, MouseEventArgs e)
        {
            Window.Capture = false;
        }
        protected override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var dx = MathF.ToRadians(0.25f * (e.X - _lastMousePos.X));
                var dy = MathF.ToRadians(0.25f * (e.Y - _lastMousePos.Y));

                _theta += dx;
                _phi += dy;

                _phi = MathF.Clamp(_phi, 0.1f, MathF.PI - 0.1f);
            }
            else if (e.Button == MouseButtons.Right)
            {
                var dx = 0.01f * (e.X - _lastMousePos.X);
                var dy = 0.01f * (e.Y - _lastMousePos.Y);
                _radius += dx - dy;

                _radius = MathF.Clamp(_radius, 3.0f, 200.0f);
            }
            _lastMousePos = e.Location;
        }

        private void BuildGeometryBuffers()
        {
            var spaceShip = GeometryGenerator.CreateSpaceship();

            _spaceShipVertexOffset = 0;

            _spaceShipIndexCount = spaceShip.Indices.Count;

            _spaceShipIndexOffset = 0;


            var totalVertexCount = spaceShip.Vertices.Count;
            var totalIndexCount = _spaceShipIndexCount;

            var vs = new List<VertexPC>();
            foreach (var vertex in spaceShip.Vertices)
            {
                vs.Add(new VertexPC(vertex.Position, Color.Black));
            }

            var vbd = new BufferDescription(VertexPC.Stride * totalVertexCount,
                ResourceUsage.Immutable, BindFlags.VertexBuffer,
                CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            _vb = new Buffer(Device, new DataStream(vs.ToArray(), false, false), vbd);

            var indices = new List<int>();
            indices.AddRange(spaceShip.Indices);


            var ibd = new BufferDescription(sizeof(int) * totalIndexCount, ResourceUsage.Immutable,
                BindFlags.IndexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            _ib = new Buffer(Device, new DataStream(indices.ToArray(), false, false), ibd);

        }
        private void BuildFX()
        {
            ShaderBytecode compiledShader = null;
            try
            {
                compiledShader = new ShaderBytecode(new DataStream(File.ReadAllBytes("fx/color.fxo"), false, false));
                _fx = new Effect(Device, compiledShader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                Util.ReleaseCom(ref compiledShader);
            }

            _tech = _fx.GetTechniqueByName("ColorTech");
            _fxWVP = _fx.GetVariableByName("gWorldViewProj").AsMatrix();
        }
        private void BuildVertexLayout()
        {
            var vertexDesc = new[] {
                new InputElement("POSITION", 0, Format.R32G32B32_Float,
                    0, 0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float,
                    12, 0, InputClassification.PerVertexData, 0)
            };
            Debug.Assert(_tech != null);
            var passDesc = _tech.GetPassByIndex(0).Description;
            _inputLayout = new InputLayout(Device, passDesc.Signature, vertexDesc);
        }

    }

    class Program
    {
        //using Plane = SpaceshipDrawer.Model.Plane;
        static void Main(string[] args)
        {


            Configuration.EnableObjectTracking = true;
            var app = new ShapesDemo(Process.GetCurrentProcess().Handle);
            if (!app.Init())
            {
                return;
            }
            app.Run();
        }



        static void GetSpaceship()
        {
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
