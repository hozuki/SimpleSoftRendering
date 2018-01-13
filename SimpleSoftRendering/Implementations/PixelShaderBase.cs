using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations {
    public abstract class PixelShaderBase<TPixelShaderInput> : IPixelShader<TPixelShaderInput>
        where TPixelShaderInput : struct, IPixelShaderInput {

        public abstract Color4 Transform(MemoryResources resources, TPixelShaderInput input, out bool discarded);

    }
}
