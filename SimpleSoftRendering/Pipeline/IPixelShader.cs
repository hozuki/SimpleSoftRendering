using SharpDX;

namespace SSR.Pipeline {
    public interface IPixelShader<in TPixelShaderIn>
        where TPixelShaderIn : struct, IPixelShaderInput {

        /// <summary>
        /// Calculates the color of a pixel according to given input. 
        /// </summary>
        /// <param name="resources">Memory resources.</param>
        /// <param name="input">Pixel shader input.</param>
        /// <param name="discarded">When returns, this value signals whether the pixel is discarded.</param>
        /// <returns>Output pixel color, premultiplied.</returns>
        Color4 Transform(MemoryResources resources, TPixelShaderIn input, out bool discarded);

    }
}
