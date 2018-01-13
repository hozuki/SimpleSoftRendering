using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations.SimpleTexture {
    public class PixelShader : PixelShaderBase<PixelShaderInput> {

        public const int Sampler1 = 1;

        public override Color4 Transform(MemoryResources resources, PixelShaderInput input, out bool discarded) {
            discarded = false;

            return resources.GetSampler(Sampler1).Sample(input.TexCoords.X, input.TexCoords.Y);
        }

    }
}
