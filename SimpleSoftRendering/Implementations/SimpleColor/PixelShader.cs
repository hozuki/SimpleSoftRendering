using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations.SimpleColor {
    public class PixelShader : PixelShaderBase<PixelShaderInput> {

        public override Color4 Transform(MemoryResources resources, PixelShaderInput input, out bool discarded) {
            discarded = false;

            //return input.Color;

            var z = input.TransformedPosition.Z;
            return new Color4(z, z, z, z);
        }

    }
}
