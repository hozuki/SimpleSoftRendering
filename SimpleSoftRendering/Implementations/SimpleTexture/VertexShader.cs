using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations.SimpleTexture {
    public class VertexShader : VertexShaderBase<VertexShaderInput, PixelShaderInput> {

        protected override PixelShaderInput TransformInternal(MemoryResources resources, VertexShaderInput input) {
            var wvp = WVP;
            var posTransformed = Vector4.Transform(input.Position, wvp);

            return new PixelShaderInput {
                TransformedPosition = posTransformed,
                TexCoords = input.TexCoords
            };
        }

    }
}
