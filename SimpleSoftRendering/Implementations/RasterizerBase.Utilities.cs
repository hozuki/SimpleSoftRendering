using SharpDX;

namespace SSR.Implementations {
    partial class RasterizerBase<TPixelShaderInput> {

        private static Vector2 MapScreenToNdc(int x, int y, int outputWidth, int outputHeight) {
            var cx = (float)x / (outputWidth - 1) * 2 - 1;
            var cy = (float)y / (outputHeight - 1) * 2 - 1;

            return new Vector2(cx, cy);
        }

        private static Point MapNdcToScreen(float x, float y, int outputWidth, int outputHeight) {
            var cx = (int)((x + 1) * 0.5f * (outputWidth - 1));
            var cy = (int)((y + 1) * 0.5f * (outputHeight - 1));

            return new Point(cx, cy);
        }

    }
}
