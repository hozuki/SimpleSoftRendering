using SharpDX;

namespace SSR.Pipeline {
    public interface IPixelShaderInput {

        /// <summary>
        /// Transformed position. Not world position.
        /// Calculated by <see cref="IVertexShader{T1,T2}"/> Do not use in custom shader.
        /// </summary>
        Vector4 TransformedPosition { get; set; }

        Vector4 Precalculated { get; set; }

    }
}
