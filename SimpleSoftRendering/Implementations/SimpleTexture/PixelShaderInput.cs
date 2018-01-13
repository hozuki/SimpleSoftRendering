using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations.SimpleTexture {
    public struct PixelShaderInput : IPixelShaderInput {

        public Vector4 TransformedPosition { get; set; }
        
        public Vector4 Precalculated { get; set; }

        public Vector2 TexCoords { get; set; }

    }
}
