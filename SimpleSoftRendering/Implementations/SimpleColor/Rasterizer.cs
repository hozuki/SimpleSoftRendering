using SharpDX;

namespace SSR.Implementations.SimpleColor {
    public class Rasterizer : RasterizerBase<PixelShaderInput> {

        protected override PixelShaderInput InterpolatePoint(PixelShaderInput va, PixelShaderInput vb, PixelShaderInput vc, Vector2 p, out bool outOfRange) {
            var v = new PixelShaderInput();

            var vatp = va.TransformedPosition;
            var vbtp = vb.TransformedPosition;
            var vctp = vc.TransformedPosition;

            // 感谢lyj
            var wp = Triangular.GetInterpolatedW(p, vatp, vbtp, vctp, out var successful);

            outOfRange = !successful;

            // No cache, slow.
            v.TransformedPosition = Triangular.InterpolateTransformedPosition(p, wp, vatp, vbtp, vctp, out successful);

            if (!outOfRange) {
                outOfRange = !successful;
            }

            v.Color = Triangular.Interpolate(va.Color, vb.Color, vc.Color, p, wp, vatp, vbtp, vctp, out successful);

            if (!outOfRange) {
                outOfRange = !successful;
            }

            return v;
        }

    }
}
