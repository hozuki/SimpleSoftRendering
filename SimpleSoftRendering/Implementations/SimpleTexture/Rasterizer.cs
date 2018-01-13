using SharpDX;

namespace SSR.Implementations.SimpleTexture {
    public class Rasterizer : RasterizerBase<PixelShaderInput> {

        protected override PixelShaderInput InterpolatePoint(PixelShaderInput va, PixelShaderInput vb, PixelShaderInput vc, Vector2 p, out bool outOfRange) {
            var v = new PixelShaderInput();

            var vatp = va.TransformedPosition;
            var vbtp = vb.TransformedPosition;
            var vctp = vc.TransformedPosition;

            var wp = Triangular.GetInterpolatedW(p, vatp, vbtp, vctp, out var successful);

            outOfRange = !successful;

            v.TransformedPosition = Triangular.InterpolateTransformedPosition(p, wp, vatp, vbtp, vctp, out successful);

            if (!outOfRange) {
                outOfRange = !successful;
            }

            v.TexCoords = Triangular.Interpolate(va.TexCoords, vb.TexCoords, vc.TexCoords, p, wp, vatp, vbtp, vctp, out successful);

            if (!outOfRange) {
                outOfRange = !successful;
            }

            return v;
        }

    }
}
