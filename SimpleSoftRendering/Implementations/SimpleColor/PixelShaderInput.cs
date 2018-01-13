using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations.SimpleColor {
    public struct PixelShaderInput : IPixelShaderInput {

        public Vector4 TransformedPosition { get; set; }
        
        public Vector4 Precalculated { get; set; }

        public Color4 Color { get; set; }

    }
}
