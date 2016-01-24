﻿using System;
using System.Windows.Forms;

namespace Minimap {
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Threading;

    using Core;
    using Core.Camera;
    using Core.FX;
    using Core.Terrain;
    using Core.Vertex;

    using SlimDX;
    using SlimDX.DXGI;
    using SlimDX.Direct3D11;

    internal class RandomTerrainDemo : D3DApp {
        private Sky _sky;
        private Terrain _terrain;
        private readonly DirectionalLight[] _dirLights;

        private readonly LookAtCamera _camera;

        private bool _camWalkMode;
        private Point _lastMousePos;
        private bool _disposed;

        private Ssao _ssao;

        #region UI Elements

        private FlowLayoutPanel _panel;
        private Button _generateButton;
        private Label _lblSeed;
        private NumericUpDown _txtSeed;
        private Label _lblNoise1;
        private NumericUpDown _txtNoise1;
        private Label _lblPersistence1;
        private NumericUpDown _txtPersistence1;
        private Label _lblOctaves1;
        private NumericUpDown _txtOctaves1;
        private Label _lblNoise2;
        private NumericUpDown _txtNoise2;
        private Label _lblPersistence2;
        private NumericUpDown _txtPersistence2;
        private Label _lblOctaves2;
        private NumericUpDown _txtOctaves2;
        private PictureBox _hmImg;
        private TableLayoutPanel _tblLayout;
        private ShaderResourceView _whiteTex;

        #endregion

        private BoundingSphere _sceneBounds;

        private const int SMapSize = 4096;
        private const int MinimapSize = 512;
        private ShadowMap _sMap;
        private Matrix _lightView;
        private Matrix _lightProj;
        private Matrix _shadowTransform;
        private readonly Vector3[] _originalLightDirs;
        private float _lightRotationAngle;

        private Minimap _minimap;

        private RandomTerrainDemo(IntPtr hInstance)
            : base(hInstance) {
            MainWindowCaption = "Minimap Demo";
            //Enable4xMsaa = true;
            _lastMousePos = new Point();

            _camera = new LookAtCamera {
                Position = new Vector3(0, 20, 100)
            };
            _dirLights = new[] {
                new DirectionalLight {
                    Ambient = new Color4(0.8f, 0.8f, 0.8f),
                    Diffuse = new Color4(0.6f, 0.6f, 0.5f),
                    Specular = new Color4(0.8f, 0.8f, 0.7f),
                    Direction = new Vector3(0, 0, 0.707f)
                },
                new DirectionalLight {
                    Ambient = new Color4(0,0,0),
                    Diffuse = new Color4( 0.4f, 0.4f, 0.4f),
                    Specular = new Color4( 0.2f, 0.2f, 0.2f),
                    Direction = new Vector3(0.707f, -0.707f, 0)
                },
                new DirectionalLight {
                    Ambient = new Color4(0,0,0),
                    Diffuse = new Color4(0.2f, 0.2f, 0.2f),
                    Specular = new Color4(0.2f,0.2f,0.2f),
                    Direction = new Vector3(0, 0, -1)
                }
            };
            _originalLightDirs = _dirLights.Select(l => l.Direction).ToArray();
        }

        protected override void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    ImmediateContext.ClearState();
                    Util.ReleaseCom(ref _sky);
                    Util.ReleaseCom(ref _terrain);
                    Util.ReleaseCom(ref _minimap);
                    Util.ReleaseCom(ref _sMap);
                    Util.ReleaseCom(ref _ssao);
                    Util.ReleaseCom(ref _whiteTex);



                    Effects.DestroyAll();
                    InputLayouts.DestroyAll();
                    RenderStates.DestroyAll();
                    Patch.DestroyPatchIndexBuffers();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        public override bool Init() {
            if (!base.Init()) return false;

            Effects.InitAll(Device);
            InputLayouts.InitAll(Device);
            RenderStates.InitAll(Device);
            Patch.InitPatchData(Terrain.CellsPerPatch, Device);

            _sky = new Sky(Device, "Textures/grasscube1024.dds", 5000.0f);

            var tii = new InitInfo {
                HeightMapFilename = null,
                LayerMapFilename0 = "textures/grass.dds",
                LayerMapFilename1 = "textures/darkdirt.dds",
                LayerMapFilename2 = "textures/stone.dds",
                LayerMapFilename3 = "Textures/lightdirt.dds",
                LayerMapFilename4 = "textures/snow.dds",
                BlendMapFilename = null,
                HeightScale = 50.0f,
                HeightMapWidth = 2049,
                HeightMapHeight = 2049,
                CellSpacing = 0.5f,

                Seed = 0,
                NoiseSize1 = 3.0f,
                Persistence1 = 0.7f,
                Octaves1 = 7,
                NoiseSize2 = 2.5f,
                Persistence2 = 0.8f,
                Octaves2 = 3,


            };
            _terrain = new Terrain();
            _terrain.Init(Device, ImmediateContext, tii);

            _camera.Height = _terrain.Height;

            AddUIElements();

            _camera.SetLens(0.25f * MathF.PI, AspectRatio, 1.0f, 1000.0f);
            _ssao = new Ssao(Device, ImmediateContext, ClientWidth, ClientHeight, _camera.FovY, _camera.FarZ);

            _whiteTex = ShaderResourceView.FromFile(Device, "Textures/white.dds");


            _sMap = new ShadowMap(Device, SMapSize, SMapSize);

            _sceneBounds = new BoundingSphere(new Vector3(), MathF.Sqrt(_terrain.Width * _terrain.Width + _terrain.Depth * _terrain.Depth) / 2);

            _minimap = new Minimap(Device, ImmediateContext, MinimapSize, MinimapSize, _terrain, _camera);

            return true;
        }


        private void AddUIElements() {
            _panel = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };

            _generateButton = new Button {
                Text = "Generate Terrain",
                AutoSize = true
            };
            _generateButton.Click += (sender, args) => {
                Window.Cursor = Cursors.WaitCursor;
                Util.ReleaseCom(ref _terrain);
                _terrain = new Terrain();
                var tii = new InitInfo {
                    HeightMapFilename = null,
                    LayerMapFilename0 = "textures/grass.dds",
                    LayerMapFilename1 = "textures/darkdirt.dds",
                    LayerMapFilename2 = "textures/stone.dds",
                    LayerMapFilename3 = "Textures/lightdirt.dds",
                    LayerMapFilename4 = "textures/snow.dds",
                    BlendMapFilename = null,
                    HeightScale = 50.0f,
                    HeightMapWidth = 2049,
                    HeightMapHeight = 2049,
                    CellSpacing = 0.5f,

                    Seed = (int)_txtSeed.Value,
                    NoiseSize1 = (float)_txtNoise1.Value,
                    Persistence1 = (float)_txtPersistence1.Value,
                    Octaves1 = (int)_txtOctaves1.Value,
                    NoiseSize2 = (float)_txtNoise2.Value,
                    Persistence2 = (float)_txtPersistence2.Value,
                    Octaves2 = (int)_txtOctaves2.Value
                };
                _terrain.Init(Device, ImmediateContext, tii);
                _camera.Height = _terrain.Height;
                _hmImg.Image = _terrain.HeightMapImg;
                Util.ReleaseCom(ref _minimap);
                _minimap = new Minimap(Device, ImmediateContext, MinimapSize, MinimapSize, _terrain, _camera);
                Window.Cursor = Cursors.Default;
            };

            var labelPadding = new Padding(0, 6, 0, 0);
            _lblSeed = new Label {
                Text = "Seed:",
                AutoSize = true,
                Padding = labelPadding

            };
            _txtSeed = new NumericUpDown {
                Value = 0,
                AutoSize = true,
                Maximum = int.MaxValue
            };

            _lblNoise1 = new Label {
                Text = "Noise:",
                AutoSize = true,
                Padding = labelPadding,


            };
            _txtNoise1 = new NumericUpDown {
                Value = 3.0m,
                DecimalPlaces = 2,
                Minimum = 0m,
                Maximum = 10m,
                Increment = 0.1m,
                AutoSize = true
            };
            _lblPersistence1 = new Label {
                Text = "Persistence:",
                AutoSize = true,
                Padding = labelPadding
            };
            _txtPersistence1 = new NumericUpDown {
                Value = 0.7m,
                DecimalPlaces = 2,
                Minimum = 0m,
                Maximum = 10m,
                Increment = 0.1m,
                AutoSize = true
            };
            _lblOctaves1 = new Label {
                Text = "Octaves:",
                AutoSize = true,
                Padding = labelPadding

            };
            _txtOctaves1 = new NumericUpDown {
                Value = 7,
                AutoSize = true,
                Minimum = 1,
                Maximum = 20,

            };

            _lblNoise2 = new Label {
                Text = "Noise:",
                AutoSize = true,
                Padding = labelPadding

            };
            _txtNoise2 = new NumericUpDown {
                Value = 2.5m,
                DecimalPlaces = 2,
                Minimum = 0m,
                Maximum = 10m,
                Increment = 0.1m,
                AutoSize = true
            };
            _lblPersistence2 = new Label {
                Text = "Persistence:",
                AutoSize = true,
                Padding = labelPadding
            };
            _txtPersistence2 = new NumericUpDown {
                Value = 0.8m,
                DecimalPlaces = 2,
                Minimum = 0m,
                Maximum = 10m,
                Increment = 0.1m,
                AutoSize = true
            };
            _lblOctaves2 = new Label {
                Text = "Octaves:",
                AutoSize = true,
                Padding = labelPadding

            };
            _txtOctaves2 = new NumericUpDown {
                Value = 3,
                AutoSize = true,
                Minimum = 1,
                Maximum = 20
            };

            _hmImg = new PictureBox {
                Image = _terrain.HeightMapImg,
                MaximumSize = new Size(64, 64),
                MinimumSize = new Size(64, 64),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.White
            };


            _panel.Controls.Add(_lblNoise1);
            _panel.Controls.Add(_txtNoise1);
            _panel.Controls.Add(_lblPersistence1);
            _panel.Controls.Add(_txtPersistence1);
            _panel.Controls.Add(_lblOctaves1);
            _panel.Controls.Add(_txtOctaves1);


            _panel.Controls.Add(_lblNoise2);
            _panel.Controls.Add(_txtNoise2);
            _panel.Controls.Add(_lblPersistence2);
            _panel.Controls.Add(_txtPersistence2);
            _panel.Controls.Add(_lblOctaves2);
            _panel.Controls.Add(_txtOctaves2);

            _panel.SetFlowBreak(_txtOctaves2, true);

            _panel.Controls.Add(_lblSeed);
            _panel.Controls.Add(_txtSeed);

            _panel.Controls.Add(_generateButton);


            _tblLayout = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true
            };
            _tblLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _tblLayout.Controls.Add(_panel, 0, 0);
            _tblLayout.Controls.Add(_hmImg, 1, 0);


            Window.Controls.Add(_tblLayout);


        }

        public override void OnResize() {
            base.OnResize();
            _camera.SetLens(0.25f * MathF.PI, AspectRatio, 1.0f, 1000.0f);
            if (_ssao != null) {
                _ssao.OnSize(ClientWidth, ClientHeight, _camera.FovY, _camera.FarZ);
            }
        }

        public override void UpdateScene(float dt) {
            base.UpdateScene(dt);
            if (Util.IsKeyDown(Keys.Up)) {
                _camera.Walk(10.0f * dt);
            }
            if (Util.IsKeyDown(Keys.Down)) {
                _camera.Walk(-10.0f * dt);
            }
            if (Util.IsKeyDown(Keys.Left)) {
                _camera.Strafe(-10.0f * dt);
            }
            if (Util.IsKeyDown(Keys.Right)) {
                _camera.Strafe(10.0f * dt);
            }
            if (Util.IsKeyDown(Keys.PageUp)) {
                _camera.Zoom(-dt * 10.0f);
            }
            if (Util.IsKeyDown(Keys.PageDown)) {
                _camera.Zoom(+dt * 10.0f);
            }
            if (Util.IsKeyDown(Keys.D2)) {
                _camWalkMode = true;
            }
            if (Util.IsKeyDown(Keys.D3)) {
                _camWalkMode = false;
            }
            if (Util.IsKeyDown(Keys.Space)) {
                _tblLayout.Visible = !_tblLayout.Visible;
                Thread.Sleep(100);
            }
            if (_camWalkMode) {
                var camPos = _camera.Target;
                var y = _terrain.Height(camPos.X, camPos.Z);
                _camera.Target = new Vector3(camPos.X, y, camPos.Z);

            }

            //_lightRotationAngle += 0.1f * dt;
            _lightRotationAngle = MathF.PI - MathF.PI / 16;

            var r = Matrix.RotationX(_lightRotationAngle);
            for (var i = 0; i < 3; i++) {
                var lightDir = _originalLightDirs[i];
                lightDir = Vector3.TransformNormal(lightDir, r);
                _dirLights[i].Direction = lightDir;
            }

            BuildShadowTransform();

            _camera.UpdateViewMatrix();
        }

        private void BuildShadowTransform() {
            var lightDir = _dirLights[0].Direction;
            var lightPos = -2.0f * _sceneBounds.Radius * lightDir;
            var targetPos = _sceneBounds.Center;
            var up = new Vector3(0, 1, 0);

            var v = Matrix.LookAtLH(lightPos, targetPos, up);

            var sphereCenterLS = Vector3.TransformCoordinate(targetPos, v);

            var l = sphereCenterLS.X - _sceneBounds.Radius;
            var b = sphereCenterLS.Y - _sceneBounds.Radius;
            var n = sphereCenterLS.Z - _sceneBounds.Radius;
            var r = sphereCenterLS.X + _sceneBounds.Radius;
            var t = sphereCenterLS.Y + _sceneBounds.Radius;
            var f = sphereCenterLS.Z + _sceneBounds.Radius;


            //var p = Matrix.OrthoLH(r - l, t - b+5, n, f);
            var p = Matrix.OrthoOffCenterLH(l, r, b, t, n, f);
            var T = new Matrix {
                M11 = 0.5f,
                M22 = -0.5f,
                M33 = 1.0f,
                M41 = 0.5f,
                M42 = 0.5f,
                M44 = 1.0f
            };



            var s = v * p * T;
            _lightView = v;
            _lightProj = p;
            _shadowTransform = s;
        }

        public override void DrawScene() {
            Effects.TerrainFX.SetSsaoMap(_whiteTex);
            Effects.TerrainFX.SetShadowMap(_sMap.DepthMapSRV);
            Effects.TerrainFX.SetShadowTransform(_shadowTransform);
            _minimap.RenderMinimap(_dirLights);

            var viewProj = _lightView * _lightProj;

            _terrain.Renderer.DrawToShadowMap(ImmediateContext, _sMap, viewProj);

            ImmediateContext.Rasterizer.State = null;

            ImmediateContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
            ImmediateContext.Rasterizer.SetViewports(Viewport);

            _terrain.Renderer.ComputeSsao(ImmediateContext, _camera, _ssao, DepthStencilView);




            ImmediateContext.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);
            ImmediateContext.Rasterizer.SetViewports(Viewport);

            ImmediateContext.ClearRenderTargetView(RenderTargetView, Color.Silver);

            ImmediateContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);

            if (Util.IsKeyDown(Keys.W)) {
                ImmediateContext.Rasterizer.State = RenderStates.WireframeRS;
            }

            if (Util.IsKeyDown(Keys.S)) {
                Effects.TerrainFX.SetSsaoMap(_whiteTex);
            } else {

                Effects.TerrainFX.SetSsaoMap(_ssao.AmbientSRV);
            }
            Effects.TerrainFX.SetShadowMap(_sMap.DepthMapSRV);
            Effects.TerrainFX.SetShadowTransform(_shadowTransform);

            if (!Util.IsKeyDown(Keys.A)) {
                _terrain.Renderer.Shadows = true;
            } else {
                _terrain.Renderer.Shadows = false;
            }



            _terrain.Renderer.Draw(ImmediateContext, _camera, _dirLights);



            ImmediateContext.Rasterizer.State = null;



            _sky.Draw(ImmediateContext, _camera);


            ImmediateContext.Rasterizer.State = null;
            ImmediateContext.OutputMerger.DepthStencilState = null;
            ImmediateContext.OutputMerger.DepthStencilReference = 0;

            DrawScreenQuad(_ssao.AmbientSRV);
            DrawScreenQuad2(_ssao.NormalDepthSRV);
            DrawScreenQuad3(_sMap.DepthMapSRV);
            _minimap.Draw(ImmediateContext);
            SwapChain.Present(0, PresentFlags.None);


        }

        private void DrawScreenQuad(ShaderResourceView srv) {
            var stride = Basic32.Stride;
            const int Offset = 0;

            ImmediateContext.InputAssembler.InputLayout = InputLayouts.Basic32;
            ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(ScreenQuadVB, stride, Offset));
            ImmediateContext.InputAssembler.SetIndexBuffer(ScreenQuadIB, Format.R32_UInt, 0);

            var world = new Matrix {
                M11 = 0.25f,
                M22 = 0.25f,
                M33 = 1.0f,
                M41 = 0.75f,
                M42 = -0.75f,
                M44 = 1.0f
            };
            var tech = Effects.DebugTexFX.ViewRedTech;
            for (var p = 0; p < tech.Description.PassCount; p++) {
                Effects.DebugTexFX.SetWorldViewProj(world);
                Effects.DebugTexFX.SetTexture(srv);
                tech.GetPassByIndex(p).Apply(ImmediateContext);
                ImmediateContext.DrawIndexed(6, 0, 0);
            }
        }
        private void DrawScreenQuad2(ShaderResourceView srv) {
            var stride = Basic32.Stride;
            const int Offset = 0;

            ImmediateContext.InputAssembler.InputLayout = InputLayouts.Basic32;
            ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(ScreenQuadVB, stride, Offset));
            ImmediateContext.InputAssembler.SetIndexBuffer(ScreenQuadIB, Format.R32_UInt, 0);

            var world = new Matrix {
                M11 = 0.25f,
                M22 = 0.25f,
                M33 = 1.0f,
                M41 = -0.75f,
                M42 = -0.75f,
                M44 = 1.0f
            };
            var tech = Effects.DebugTexFX.ViewArgbTech;
            for (var p = 0; p < tech.Description.PassCount; p++) {
                Effects.DebugTexFX.SetWorldViewProj(world);
                Effects.DebugTexFX.SetTexture(srv);
                tech.GetPassByIndex(p).Apply(ImmediateContext);
                ImmediateContext.DrawIndexed(6, 0, 0);
            }
        }
        private void DrawScreenQuad4(ShaderResourceView srv) {
            var stride = Basic32.Stride;
            const int Offset = 0;

            ImmediateContext.InputAssembler.InputLayout = InputLayouts.Basic32;
            ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(ScreenQuadVB, stride, Offset));
            ImmediateContext.InputAssembler.SetIndexBuffer(ScreenQuadIB, Format.R32_UInt, 0);

            var world = new Matrix {
                M11 = 0.25f,
                M22 = 0.25f,
                M33 = 1.0f,
                M41 = -0.25f,
                M42 = -0.75f,
                M44 = 1.0f
            };
            var tech = Effects.DebugTexFX.ViewArgbTech;
            for (var p = 0; p < tech.Description.PassCount; p++) {
                Effects.DebugTexFX.SetWorldViewProj(world);
                Effects.DebugTexFX.SetTexture(srv);
                tech.GetPassByIndex(p).Apply(ImmediateContext);
                ImmediateContext.DrawIndexed(6, 0, 0);
            }
        }
        private void DrawScreenQuad3(ShaderResourceView srv) {
            var stride = Basic32.Stride;
            const int Offset = 0;

            ImmediateContext.InputAssembler.InputLayout = InputLayouts.Basic32;
            ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(ScreenQuadVB, stride, Offset));
            ImmediateContext.InputAssembler.SetIndexBuffer(ScreenQuadIB, Format.R32_UInt, 0);

            var world = new Matrix {
                M11 = 0.25f,
                M22 = 0.25f,
                M33 = 1.0f,
                M41 = 0.25f,
                M42 = -0.75f,
                M44 = 1.0f
            };
            var tech = Effects.DebugTexFX.ViewRedTech;
            for (var p = 0; p < tech.Description.PassCount; p++) {
                Effects.DebugTexFX.SetWorldViewProj(world);
                Effects.DebugTexFX.SetTexture(srv);
                tech.GetPassByIndex(p).Apply(ImmediateContext);
                ImmediateContext.DrawIndexed(6, 0, 0);
            }
        }


        protected override void OnMouseDown(object sender, MouseEventArgs mouseEventArgs) {
            _minimap.OnClick(mouseEventArgs);

            _lastMousePos = mouseEventArgs.Location;
            Window.Capture = true;
        }

        protected override void OnMouseUp(object sender, MouseEventArgs e) {
            Window.Capture = false;
        }

        protected override void OnMouseMove(object sender, MouseEventArgs e) {
            var x = (float)e.X / Window.ClientSize.Width;
            var y = (float)e.Y / Window.ClientSize.Height;
            var p = new Vector2(x, y);
            if (e.Button == MouseButtons.Left && _minimap.Contains(ref p)) {
                var terrainX = _terrain.Width * p.X - _terrain.Width / 2;
                var terrainZ = -_terrain.Depth * p.Y + _terrain.Depth / 2;
                _camera.Target = new Vector3(terrainX, _terrain.Height(terrainX, terrainZ), terrainZ);
                return;
            }
            if (e.Button == MouseButtons.Left) {
                var dx = MathF.ToRadians(0.25f * (e.X - _lastMousePos.X));
                var dy = MathF.ToRadians(0.25f * (e.Y - _lastMousePos.Y));

                _camera.Pitch(dy);
                _camera.Yaw(dx);

            }
            _lastMousePos = e.Location;
        }

        private static void Main() {
            Configuration.EnableObjectTracking = true;
            var app = new RandomTerrainDemo(Process.GetCurrentProcess().Handle);
            if (!app.Init()) {
                return;
            }
            app.Run();
        }
    }
}
