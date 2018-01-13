using SharpDX;

namespace SSR.Pipeline {
    public interface IRasterizer<TPixelShaderInput>
        where TPixelShaderInput : struct, IPixelShaderInput {

        Color4[,] Rasterize(MemoryResources resources, TPixelShaderInput[] vertices, int[] indices, IPixelShader<TPixelShaderInput> pixelShader, OutputMerger outputMerger, int outputWidth, int outputHeight, Color4[,] colorBuffer, float[,] wBuffer);

    }
}
