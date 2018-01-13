namespace SSR.Pipeline {
    public interface IVertexShader<in TVertexShaderIn, out TPixelShaderIn>
        where TVertexShaderIn : struct, IVertexShaderInput
        where TPixelShaderIn : struct, IPixelShaderInput {

        TPixelShaderIn Transform(MemoryResources resources, TVertexShaderIn input);

    }
}
