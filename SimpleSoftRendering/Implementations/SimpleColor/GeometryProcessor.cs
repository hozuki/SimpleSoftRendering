using SharpDX;

namespace SSR.Implementations.SimpleColor {
    public class GeometryProcessor : GeometryProcessorBase<PixelShaderInput> {

        public override (PixelShaderInput[] Vertices, int[] Indices) Process(PixelShaderInput[] vertices, int[] indices) {
            return DefaultProcessProc(vertices, indices);
        }

        protected override PixelShaderInput LerpPixelShaderInput(PixelShaderInput from, PixelShaderInput to, float perc) {
            var v = new PixelShaderInput();

            v.TransformedPosition = TransformedPosition.Lerp(from.TransformedPosition, to.TransformedPosition, perc);
            v.Color = Color4.Lerp(from.Color, to.Color, perc);

            return v;
        }

    }
}
