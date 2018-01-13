using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations.SimpleTexture {
    public struct VertexShaderInput : IVertexShaderInput {

        public Vector4 Position { get; set; }

        public Vector2 TexCoords { get; set; }

    }
}
