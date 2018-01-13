using SharpDX;

namespace SSR {
    public static class TransformedPosition {

        public static Vector4 Lerp(Vector4 from, Vector4 to, float amount) {
            var v = new Vector4();

            v.X = MathUtil.Lerp(from.X, to.X, amount);
            v.Y = MathUtil.Lerp(from.Y, to.Y, amount);
            v.Z = 1 / MathUtil.Lerp(1 / from.Z, 1 / to.Z, amount);
            v.W = MathUtil.Lerp(from.W, to.W, amount);

            return v;
        }

    }
}
