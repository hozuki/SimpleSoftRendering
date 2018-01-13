namespace SSR.Pipeline {
    public interface IGeometryProcessor<TPixelShaderInput>
        where TPixelShaderInput : struct, IPixelShaderInput {

        Culling Culling { get; set; }
        
        (TPixelShaderInput[] Vertices, int[] Indices) Process(TPixelShaderInput[] vertices, int[] indices);

    }
}
