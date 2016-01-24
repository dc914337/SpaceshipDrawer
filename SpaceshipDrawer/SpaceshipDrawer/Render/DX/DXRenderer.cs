using SlimDX.Direct3D11;
using SlimDX.DXGI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FeatureLevel = SlimDX.Direct3D11.FeatureLevel;

namespace SpaceshipDrawer.Render.DX
{
    using Buffer = SlimDX.Direct3D11.Buffer;
    using Device = SlimDX.Direct3D11.Device;
    using Debug = System.Diagnostics.Debug;
    using SlimDX;
    using SlimDX.D3DCompiler;
    using System.IO;
    using Util;
    class DXRenderer
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

        private int _boxVertexOffset;
        private int _gridVertexOffset;
        private int _sphereVertexOffset;
        private int _cylinderVertexOffset;

        private int _boxIndexOffset;
        private int _gridIndexOffset;
        private int _sphereIndexOffset;
        private int _cylinderIndexOffset;

        private int _boxIndexCount;
        private int _gridIndexCount;
        private int _sphereIndexCount;
        private int _cylinderIndexCount;

        private float _theta;
        private float _phi;
        private float _radius;
        private Point _lastMousePos;


        public Form Window { get; protected set; }
        public IntPtr AppInst { get; protected set; }
        public float AspectRatio { get { return (float)ClientWidth / ClientHeight; } }
        public bool GammaCorrectedBackBuffer { get; set; }


        protected bool AppPaused;
        protected bool Minimized;
        protected bool Maximized;
        protected bool Resizing;
        protected int Msaa4XQuality;
        //protected GameTimer Timer;

        protected Device Device;
        protected DeviceContext ImmediateContext;
        protected SwapChain SwapChain;
        protected Texture2D DepthStencilBuffer;
        protected RenderTargetView RenderTargetView;
        protected DepthStencilView DepthStencilView;
        protected Viewport Viewport;
        protected string MainWindowCaption;
        protected DriverType DriverType;
        protected int ClientWidth;
        protected int ClientHeight;
        protected bool Enable4XMsaa;
        private bool _running;
        private int _frameCount;
        private float _timeElapsed;




        public DXRenderer(IntPtr hInstance)
        {
            AppInst = hInstance;
            MainWindowCaption = "D3D11 Application";
            DriverType = DriverType.Hardware;
            ClientWidth = 800;
            ClientHeight = 600;
            Enable4XMsaa = false;
            Window = null;
            AppPaused = false;
            Minimized = false;
            Maximized = false;
            Resizing = false;
            Msaa4XQuality = 0;
            Device = null;
            ImmediateContext = null;
            SwapChain = null;
            DepthStencilBuffer = null;
            RenderTargetView = null;
            DepthStencilView = null;
            Viewport = new Viewport();
            //Timer = new GameTimer();


            _vb = null;
            _ib = null;
            _fx = null;
            _tech = null;
            _fxWVP = null;
            _inputLayout = null;
            _wireframeRS = null;
            _theta = 1.5f * (float)Math.PI;
            _phi = 0.1f * (float)Math.PI;
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





        public bool TryInitAll(string MainWindowCaption, int ClientWidth, int ClientHeight, MyWndProc WndProc)
        {
            if (!TryInitWindow(MainWindowCaption, ClientWidth, ClientHeight, WndProc))
                return false;
            if (!TryInitDirect3D())
                return false;

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


        public bool TryInitWindow(string MainWindowCaption, int ClientWidth, int ClientHeight, MyWndProc WndProc)
        {
            try
            {
                Window = new DxForm
                {
                    Text = MainWindowCaption,
                    Name = "D3DWndClassName",
                    FormBorderStyle = FormBorderStyle.Sizable,
                    ClientSize = new Size(ClientWidth, ClientHeight),
                    StartPosition = FormStartPosition.CenterScreen,
                    MyWndProc = WndProc,
                    MinimumSize = new Size(200, 200),
                };
                Window.Show();
                Window.Update();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error");
                return false;
            }
        }


        private void BuildGeometryBuffers()
        {


            var vs = new List<VertexPC>();



            vs.Add(new VertexPC(new Vector3(0, 0, 0), Color.Black));
            vs.Add(new VertexPC(new Vector3(0, 0, 1), Color.Black));
            vs.Add(new VertexPC(new Vector3(1, 1, 1), Color.Black));
            vs.Add(new VertexPC(new Vector3(0, 1, 1), Color.Black));


            var totalVertexCount = vs.Count;




            var vbd = new BufferDescription(VertexPC.Stride * totalVertexCount,
                ResourceUsage.Immutable, BindFlags.VertexBuffer,
                CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            _vb = new Buffer(Device, new DataStream(vs.ToArray(), false, false), vbd);

            var indices = new List<int>
            { 0,2,1,0,
               0,2,3,0,
               3,1,0
            };
            var totalIndexCount = indices.Count;

            var ibd = new BufferDescription(sizeof(int) * totalIndexCount, ResourceUsage.Immutable,
                BindFlags.IndexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            _ib = new Buffer(Device, new DataStream(indices.ToArray(), false, false), ibd);

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


        public bool TryInitDirect3D()
        {
            var creationFlags = DeviceCreationFlags.None;
#if DEBUG
            creationFlags |= DeviceCreationFlags.Debug;
#endif
            try
            {
                Device = new Device(DriverType, creationFlags);
            }
            catch (Exception ex)
            {
                MessageBox.Show("D3D11Device creation failed\n" + ex.Message + "\n" + ex.StackTrace, "Error");
                return false;
            }
            ImmediateContext = Device.ImmediateContext;
            if (Device.FeatureLevel != FeatureLevel.Level_11_0)
            {
                Console.WriteLine("Direct3D Feature Level 11 unsupported\nSupported feature level: " + Enum.GetName(Device.FeatureLevel.GetType(), Device.FeatureLevel));
                //return false;
            }
            //Debug.Assert((Msaa4XQuality = Device.CheckMultisampleQualityLevels(Format.R8G8B8A8_UNorm, 4)) > 0);
            try
            {
                var format = GammaCorrectedBackBuffer ? Format.R8G8B8A8_UNorm_SRGB : Format.R8G8B8A8_UNorm;
                var sd = new SwapChainDescription
                {
                    ModeDescription = new ModeDescription(ClientWidth, ClientHeight, new Rational(60, 1), format)
                    {
                        ScanlineOrdering = DisplayModeScanlineOrdering.Unspecified,
                        Scaling = DisplayModeScaling.Unspecified
                    },
                    SampleDescription = Enable4XMsaa && Device.FeatureLevel >= FeatureLevel.Level_10_1 ? new SampleDescription(4, Msaa4XQuality - 1) : new SampleDescription(1, 0),
                    Usage = Usage.RenderTargetOutput,
                    BufferCount = 1,
                    OutputHandle = Window.Handle,
                    IsWindowed = true,
                    SwapEffect = SwapEffect.Discard,
                    Flags = SwapChainFlags.None

                };
                SwapChain = new SwapChain(Device.Factory, Device, sd);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SwapChain creation failed\n" + ex.Message + "\n" + ex.StackTrace, "Error");
                return false;
            }
            OnResize();

            // Sprite = new SpriteRenderer(Device);
            //FontCache = new FontCache(Sprite);
            return true;
        }


        public virtual void OnResize()
        {
            Debug.Assert(ImmediateContext != null);
            Debug.Assert(Device != null);
            Debug.Assert(SwapChain != null);

            utils.ReleaseCom(ref RenderTargetView);
            utils.ReleaseCom(ref DepthStencilView);
            utils.ReleaseCom(ref DepthStencilBuffer);

            var format = GammaCorrectedBackBuffer ? Format.R8G8B8A8_UNorm_SRGB : Format.R8G8B8A8_UNorm;
            SwapChain.ResizeBuffers(1, ClientWidth, ClientHeight, format, SwapChainFlags.None);
            using (var resource = SlimDX.Direct3D11.Resource.FromSwapChain<Texture2D>(SwapChain, 0))
            {
                RenderTargetView = new RenderTargetView(Device, resource);
                RenderTargetView.Resource.DebugName = "main render target";
            }

            var depthStencilDesc = new Texture2DDescription
            {
                Width = ClientWidth,
                Height = ClientHeight,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D24_UNorm_S8_UInt,
                SampleDescription = (Enable4XMsaa && Device.FeatureLevel >= FeatureLevel.Level_10_1) ? new SampleDescription(4, Msaa4XQuality - 1) : new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };
            DepthStencilBuffer = new Texture2D(Device, depthStencilDesc) { DebugName = "DepthStencilBuffer" };
            DepthStencilView = new DepthStencilView(Device, DepthStencilBuffer);

            ImmediateContext.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);

            Viewport = new Viewport(0, 0, ClientWidth, ClientHeight, 0.0f, 1.0f);

            ImmediateContext.Rasterizer.SetViewports(Viewport);
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
                utils.ReleaseCom(ref compiledShader);
            }

            _tech = _fx.GetTechniqueByName("ColorTech");
            _fxWVP = _fx.GetVariableByName("gWorldViewProj").AsMatrix();
        }

        public void UpdateScene(float dt)
        {
            // Get camera position from polar coords
            var x = _radius * Math.Sin(_phi) * Math.Cos(_theta);
            var z = _radius * Math.Sin(_phi) * Math.Sin(_theta);
            var y = _radius * Math.Cos(_phi);

            // Build the view matrix
            var pos = new Vector3((float)x, (float)y, (float)z);
            var target = new Vector3(0);
            var up = new Vector3(0, 1, 0);
            _view = Matrix.LookAtLH(pos, target, up);
        }


        public void DrawScene()
        {
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
                _fxWVP.SetMatrix(_gridWorld * viewProj);
                _tech.GetPassByIndex(p).Apply(ImmediateContext);
                ImmediateContext.DrawIndexed(_gridIndexCount, _gridIndexOffset, _gridVertexOffset);

                _fxWVP.SetMatrix(_boxWorld * viewProj);
                _tech.GetPassByIndex(p).Apply(ImmediateContext);
                ImmediateContext.DrawIndexed(_boxIndexCount, _boxIndexOffset, _boxVertexOffset);

                _fxWVP.SetMatrix(_centerSphere * viewProj);
                _tech.GetPassByIndex(p).Apply(ImmediateContext);
                ImmediateContext.DrawIndexed(_sphereIndexCount, _sphereIndexOffset, _sphereVertexOffset);

                foreach (var matrix in _cylWorld)
                {
                    _fxWVP.SetMatrix(matrix * viewProj);
                    _tech.GetPassByIndex(p).Apply(ImmediateContext);
                    ImmediateContext.DrawIndexed(_cylinderIndexCount, _cylinderIndexOffset, _cylinderVertexOffset);
                }
                foreach (var matrix in _sphereWorld)
                {
                    _fxWVP.SetMatrix(matrix * viewProj);
                    _tech.GetPassByIndex(p).Apply(ImmediateContext);
                    ImmediateContext.DrawIndexed(_sphereIndexCount, _sphereIndexOffset, _sphereVertexOffset);
                }
            }
            SwapChain.Present(0, PresentFlags.None);
        }






    }
}
