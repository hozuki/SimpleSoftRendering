using System.Diagnostics;
using System.Security.Policy;
using SharpDX;
using SSR.Extensions;

namespace SSR {
    public static class Triangular {

        /// <summary>
        /// Interpolate a <see cref="float"/> according to the position of point P in triangle ABC.
        /// </summary>
        /// <param name="va">The value on vertex A.</param>
        /// <param name="vb">The value on vertex B.</param>
        /// <param name="vc">The value on vertex C.</param>
        /// <param name="p">Normalized value of point P.</param>
        /// <param name="a">Normalized vertex A.</param>
        /// <param name="b">Normalized vertex B.</param>
        /// <param name="c">Normalized vertex C.</param>
        /// <returns></returns>
        public static float Interpolate(float va, float vb, float vc, Vector2 p, Vector4 a, Vector4 b, Vector4 c, out bool successful) {
            float w1, w2, w3;
            (w1, w2, w3, successful) = GetTriangleWeights(p, a, b, c);

            return va * w1 + vb * w2 + vc * w3;
        }

        /// <summary>
        /// Interpolate a <see cref="Vector2"/> according to the position of point P in triangle ABC.
        /// </summary>
        /// <param name="va">The value on vertex A.</param>
        /// <param name="vb">The value on vertex B.</param>
        /// <param name="vc">The value on vertex C.</param>
        /// <param name="p">Normalized position of point P.</param>
        /// <param name="a">Normalized vertex A.</param>
        /// <param name="b">Normalized vertex B.</param>
        /// <param name="c">Normalized vertex C.</param>
        /// <returns></returns>
        public static Vector2 Interpolate(Vector2 va, Vector2 vb, Vector3 vc, Vector2 p, Vector4 a, Vector4 b, Vector4 c, out bool successful) {
            float w1, w2, w3;
            (w1, w2, w3, successful) = GetTriangleWeights(p, a, b, c);

            var x = va.X * w1 + vb.X * w2 + vc.X * w3;
            var y = va.Y * w1 + vb.Y * w2 + vc.Y * w3;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Interpolate a <see cref="Vector2"/> according to the position of point P in triangle ABC, with W-calibration.
        /// </summary>
        /// <param name="va">The value on vertex A.</param>
        /// <param name="vb">The value on vertex B.</param>
        /// <param name="vc">The value on vertex C.</param>
        /// <param name="p">Normalized position of point P.</param>
        /// <param name="wp">W value of point P.</param>
        /// <param name="a">Normalized vertex A.</param>
        /// <param name="b">Normalized vertex B.</param>
        /// <param name="c">Normalized vertex C.</param>
        /// <returns></returns>
        public static Vector2 Interpolate(Vector2 va, Vector2 vb, Vector2 vc, Vector2 p, float wp, Vector4 a, Vector4 b, Vector4 c, out bool successful) {
            var wa = a.W;
            var wb = b.W;
            var wc = c.W;

            float w1, w2, w3;
            (w1, w2, w3, successful) = GetTriangleWeights(p * wp, a * wa, b * wb, c * wc);

            var x = va.X * w1 + vb.X * w2 + vc.X * w3;
            var y = va.Y * w1 + vb.Y * w2 + vc.Y * w3;

            return new Vector2(x, y);
        }

        public static Vector3 Interpolate(Vector3 va, Vector3 vb, Vector3 vc, Vector2 p, float wp, Vector4 a, Vector4 b, Vector4 c, out bool successful) {
            var wa = a.W;
            var wb = b.W;
            var wc = c.W;

            float w1, w2, w3;
            (w1, w2, w3, successful) = GetTriangleWeights(p * wp, a * wa, b * wb, c * wc);

            var x = va.X * w1 + vb.X * w2 + vc.X * w3;
            var y = va.Y * w1 + vb.Y * w2 + vc.Y * w3;
            var z = va.Z * w1 + vb.Z * w2 + vc.Z * w3;

            return new Vector3(x, y, z);
        }

        public static Vector4 Interpolate(Vector4 va, Vector4 vb, Vector4 vc, Vector2 p, float wp, Vector4 a, Vector4 b, Vector4 c, out bool successful) {
            var wa = a.W;
            var wb = b.W;
            var wc = c.W;

            float w1, w2, w3;
            (w1, w2, w3, successful) = GetTriangleWeights(p * wp, a * wa, b * wb, c * wc);

            var x = va.X * w1 + vb.X * w2 + vc.X * w3;
            var y = va.Y * w1 + vb.Y * w2 + vc.Y * w3;
            var z = va.Z * w1 + vb.Z * w2 + vc.Z * w3;
            var w = va.W + w1 + vb.W * w2 + vc.W * w3;

            return new Vector4(x, y, z, w);
        }

        public static Vector4 InterpolateTransformedPosition(Vector2 p, float wp, Vector4 a, Vector4 b, Vector4 c, out bool successful) {
            var wa = a.W;
            var wb = b.W;
            var wc = c.W;

            float w1, w2, w3;
            bool successful1;
            (w1, w2, w3, successful1) = GetTriangleWeights(p / wp, a / wa, b / wb, c / wc);

            var x = a.X * w1 + b.X * w2 + c.X * w3;
            var y = a.Y * w1 + b.Y * w2 + c.Y * w3;
            var z = a.Z * w1 + b.Z * w2 + c.Z * w3;
            // TODO: ???
            //var z = 1 / Interpolate(1 / a.Z, 1 / b.Z, 1 / c.Z, p, a, b, c, out var successful2);
            //var z = Interpolate(a.Z, b.Z, c.Z, p, a, b, c, out var _);
            //var z = wp / (1 / (a.Z * a.W) + 1 / (b.Z * b.W) + 1 / (c.Z * c.W));
            //var z = 1 / (wp * Interpolate(1 / (a.Z * a.W), 1 / (b.Z * b.W), 1 / (c.Z * c.W), p, a, b, c, out var _));

            //if (z < 0) {
            //    Debugger.Break();
            //}

            //successful = successful1 && successful2;
            successful = successful1;

            return new Vector4(x, y, z, wp);

            //// http://zqdevres.qiniucdn.com/data/20100709102006/index.html
            //var za = a.Z;
            //var zb = b.Z;
            //var zc = c.Z;

            //var zp = 1 / Interpolate(1 / za, 1 / zb, 1 / zc, p, a, b, c, out var _);

            //float w1, w2, w3;
            //(w1, w2, w3, successful) = GetTriangleWeights(p / zp, a / za, b / zb, c / zc);

            //var x = a.X * w1 + b.X * w2 + c.X * w3;
            //var y = a.Y * w1 + b.Y * w2 + c.Y * w3;

            //return new Vector4(x, y, zp, wp);
        }

        public static Color4 Interpolate(Color4 va, Color4 vb, Color4 vc, Vector2 p, float wp, Vector4 a, Vector4 b, Vector4 c, out bool successful) {
            var wa = a.W;
            var wb = b.W;
            var wc = c.W;

            float w1, w2, w3;
            (w1, w2, w3, successful) = GetTriangleWeights(p * wp, a * wa, b * wb, c * wc);

            var red = va.Red * w1 + vb.Red * w2 + vc.Red * w3;
            var green = va.Green * w1 + vb.Green * w2 + vc.Green * w3;
            var blue = va.Blue * w1 + vb.Blue * w2 + vc.Blue * w3;
            var alpha = va.Alpha * w1 + vb.Alpha * w2 + vc.Alpha * w3;

            return new Color4(red, green, blue, alpha);
        }

        public static float GetInterpolatedW(Vector2 p, Vector4 a, Vector4 b, Vector4 c, out bool successful) {
            var invW = Interpolate(1 / a.W, 1 / b.W, 1 / c.W, p, a, b, c, out successful);
            return 1 / invW;
        }

        // https://codeplea.com/triangular-interpolation
        // https://www.scratchapixel.com/lessons/3d-basic-rendering/rasterization-practical-implementation/perspective-correct-interpolation-vertex-attributes
        // https://www.scratchapixel.com/lessons/3d-basic-rendering/rasterization-practical-implementation/visibility-problem-depth-buffer-depth-interpolation
        private static (float w1, float w2, float w3, bool Successful) GetTriangleWeights(Vector2 p, Vector4 a, Vector4 b, Vector4 c) {
            var area = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
            var w1 = ((b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y)) / area;
            var w2 = ((c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y)) / area;
            var w3 = 1 - w1 - w2;

            return (w1, w2, w3, w1 >= 0 && w2 >= 0 && w3 >= 0);
        }

    }
}
