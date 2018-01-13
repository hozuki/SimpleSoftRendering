using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SSR.Pipeline;
using Timer = System.Threading.Timer;

namespace SSR {
    public abstract class RenderWindow : Form {

        public RenderWindow() {
            InitializeComponents();

            CheckForIllegalCrossThreadCalls = false;

            SetStyle(ControlStyles.UserPaint, true);

            _timer = new Timer(TimerOnElapsed, null, 0, 1000);

            RegisterEventHandlers();

            _fpsArray = new int[FrameCounterBufferSize];
        }

        protected override void Dispose(bool disposing) {
            UnregisterEventHandlers();

            DisposeObjects();

            _timer.Dispose();

            _cacheBitmap?.Dispose();
            _cacheBitmap = null;

            base.Dispose(disposing);
        }

        protected virtual void CreateObjects() {
            _memoryResources = new MemoryResources();
            _inputAssembler = new InputAssembler();
            _outputMerger = new OutputMerger();
        }

        protected virtual void InitialzeObjects() {
            _inputAssembler.Topology = PrimitiveTopology.Triangles;
            _outputMerger.SourceBlending = Blending.SrcAlpha;
            _outputMerger.DestinationBlending = Blending.OneMinusSrcAlpha;
            _outputMerger.DepthTest = ComparingFunction.LessEqual;
        }

        protected virtual void DisposeObjects() {
        }

        protected abstract void OnUpdate();

        protected abstract void OnRender(Graphics graphics, Bitmap cacheBitmap);

        protected abstract void OnResize();

        protected MemoryResources MemoryResources => _memoryResources;

        protected InputAssembler InputAssembler => _inputAssembler;

        protected OutputMerger OutputMerger => _outputMerger;

        private void InitializeComponents() {
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            DoubleBuffered = true;
        }

        private void UnregisterEventHandlers() {
            Resize -= OnResize;
            Paint -= OnPaint;
            Load -= OnLoad;
        }

        private void RegisterEventHandlers() {
            Resize += OnResize;
            Paint += OnPaint;
            Load += OnLoad;
        }

        private void TimerOnElapsed(object state) {
            _fpsArray[_fpsIndex] = _frames;
            _frames = 0;

            _fpsIndex = (_fpsIndex + 1) % FrameCounterBufferSize;

            var fps = _fpsArray.Average(v => (float)v);

            Text = $"FPS: {fps:0.00}";
        }

        private void OnLoad(object sender, EventArgs eventArgs) {
            CreateObjects();
            InitialzeObjects();

            OnResize(this, EventArgs.Empty);
        }

        private void OnPaint(object sender, PaintEventArgs e) {
            Application.DoEvents();

            e.Graphics.Clear(Color.Black);

            OnUpdate();

            if (_cacheBitmap != null) {
                RenderLogic.Clear();

                OnRender(e.Graphics, _cacheBitmap);

                e.Graphics.DrawImageUnscaled(_cacheBitmap, 0, 0);

                ++_frames;
            }

            Invalidate();
        }

        private void OnResize(object sender, EventArgs e) {
            _cacheBitmap?.Dispose();

            var clientSize = ClientSize;

            _cacheBitmap = new Bitmap(clientSize.Width, clientSize.Height);
            RenderLogic.Alloc(clientSize.Width, clientSize.Height);

            OnResize();
        }

        private MemoryResources _memoryResources;
        private InputAssembler _inputAssembler;
        private OutputMerger _outputMerger;

        private Bitmap _cacheBitmap;

        private const int FrameCounterBufferSize = 5;

        private Timer _timer;
        private int[] _fpsArray;
        private int _fpsIndex;
        private int _frames;

    }
}
