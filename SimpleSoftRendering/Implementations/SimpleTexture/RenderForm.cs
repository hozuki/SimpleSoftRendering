using System.Drawing;
using System.Windows.Forms;
using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations.SimpleTexture {
    public class RenderForm : RenderWindow {

        protected override void CreateObjects() {
            base.CreateObjects();

            LoadModel();

            _vertexShader = new VertexShader();
            _geometryProcessor = new GeometryProcessor();
            _rasterizer = new Rasterizer();
            _pixelShader = new PixelShader();

            _geometryProcessor.Culling = Culling.None;

            this.KeyDown += RenderForm_KeyDown;
        }

        private void RenderForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.OemMinus) {
                if (e.KeyCode == Keys.Oemplus) {
                    _dist += 0.5f;
                } else {
                    _dist -= 0.5f;
                }

                _view = Matrix.LookAtLH(new Vector3(_dist), Vector3.Zero, Vector3.UnitZ);
            }
        }

        protected override void InitialzeObjects() {
            base.InitialzeObjects();

            var sampler = MemoryResources.SetSampler(PixelShader.Sampler1, Image.FromFile("Resources/Images/aperture_science.png") as Bitmap);

            sampler.AddressingU = TextureAddressing.Clamp;
            sampler.AddressingV = TextureAddressing.Clamp;
        }

        protected override void OnUpdate() {
            _world = _world * Matrix.RotationZ(0.05f);
            _vertexShader.SetMatrices(_world, _view, _projection);
        }

        protected override void OnRender(Graphics graphics, Bitmap cacheBitmap) {
            RenderLogic.Render(_vertices, _indices, MemoryResources, InputAssembler, OutputMerger, _vertexShader, _geometryProcessor, _rasterizer, _pixelShader, cacheBitmap);
        }

        protected override void OnResize() {
            var clientSize = ClientSize;

            _projection = Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(90), (float)clientSize.Width / clientSize.Height, 0.5f, 20f);
        }

        private void LoadModel() {
            var mesh = MeshBuilder.Box(Vector3.Zero, 9, 9, 9);

            _vertices = mesh.Vertices;
            _indices = mesh.Indices;
        }

        private VertexShader _vertexShader;
        private GeometryProcessor _geometryProcessor;
        private Rasterizer _rasterizer;
        private PixelShader _pixelShader;

        private VertexShaderInput[] _vertices;
        private int[] _indices;

        private Matrix _world = Matrix.Identity;

        private float _dist = 11f;

        //private Matrix _view = Matrix.LookAtLH(new Vector3(2, 2, 2), Vector3.Zero, Vector3.UnitZ);
        private Matrix _view = Matrix.LookAtLH(new Vector3(11, 11, 11), Vector3.Zero, Vector3.UnitZ);
        //        private Matrix _view = Matrix.LookAtLH(Vector3.Zero, -Vector3.One, Vector3.UnitZ);
        //        private Matrix _view = Matrix.LookAtLH(-10 * Vector3.One, -100 * Vector3.One, Vector3.UnitZ);

        private Matrix _projection;

    }
}
