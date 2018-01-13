using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations.SimpleColor {
    public struct VertexShaderInput : IVertexShaderInput {

        public Vector4 Position { get; set; }
        
        public Color4 Color { get; set; }

    }
}
