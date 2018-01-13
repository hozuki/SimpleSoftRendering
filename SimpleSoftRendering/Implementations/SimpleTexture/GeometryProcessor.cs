using SharpDX;

namespace SSR.Implementations.SimpleTexture {
    public class GeometryProcessor : GeometryProcessorBase<PixelShaderInput> {

        public override (PixelShaderInput[] Vertices, int[] Indices) Process(PixelShaderInput[] vertices, int[] indices) {
            return DefaultProcessProc(vertices, indices);
        }

        protected override PixelShaderInput LerpPixelShaderInput(PixelShaderInput from, PixelShaderInput to, float perc) {
            var v = new PixelShaderInput();

            v.TransformedPosition = TransformedPosition.Lerp(from.TransformedPosition, to.TransformedPosition, perc);
            v.TexCoords = Vector2.Lerp(from.TexCoords, to.TexCoords, perc);

            return v;
        }

    }
}
