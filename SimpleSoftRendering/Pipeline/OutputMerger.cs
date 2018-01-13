using System;
using SharpDX;

namespace SSR.Pipeline {
    public class OutputMerger {

        public Blending SourceBlending { get; set; }

        public Blending DestinationBlending { get; set; }

        public ComparingFunction DepthTest { get; set; }

        public bool IsDepthTestPassed(float value, float compareTo) {
            var func = DepthTest;

            switch (func) {
                case ComparingFunction.Never:
                    return false;
                case ComparingFunction.Less:
                    return value < compareTo;
                case ComparingFunction.Equal:
                    return value.Equals(compareTo);
                case ComparingFunction.LessEqual:
                    return value <= compareTo;
                case ComparingFunction.Greater:
                    return value > compareTo;
                case ComparingFunction.NotEqual:
                    return !value.Equals(compareTo);
                case ComparingFunction.GreaterEqual:
                    return value >= compareTo;
                case ComparingFunction.Always:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Color4 Blend(Color4 originalColor, Color4 newColor) {
            return Blend(newColor, originalColor, SourceBlending, DestinationBlending);
        }

        private static Color4 Blend(Color4 src, Color4 dst, Blending srcBlend, Blending dstBlend) {
            Vector3 srcColor, dstColor;

            var srcAlpha = src.Alpha;
            var dstAlpha = dst.Alpha;

            if (srcAlpha.Equals(0)) {
                srcColor = Vector3.Zero;
            } else {
                srcColor = Vector3.Clamp(src.ToVector3() / srcAlpha, Vector3.Zero, Vector3.One);
            }

            if (dstAlpha.Equals(0)) {
                dstColor = Vector3.Zero;
            } else {
                dstColor = Vector3.Clamp(dst.ToVector3() / dstAlpha, Vector3.Zero, Vector3.One);
            }

            var f1 = GetValue(src, dst, srcBlend);
            var f2 = GetValue(src, dst, dstBlend);

            var alpha = srcAlpha * f1 + dstAlpha * f2;
            var color = srcColor * f1 + dstColor * f2;

            alpha = MathUtil.Clamp(alpha, 0, 1);
            color = Vector3.Clamp(color, Vector3.Zero, Vector3.One);

            return new Color4(color * alpha, alpha);
        }

        private static float GetValue(Color4 src, Color4 dst, Blending blend) {
            switch (blend) {
                case Blending.Zero:
                    return 0;
                case Blending.SrcAlpha:
                    return src.Alpha;
                case Blending.OneMinusSrcAlpha:
                    return 1 - src.Alpha;
                case Blending.DstAlpha:
                    return dst.Alpha;
                case Blending.OneMinusDstAlpha:
                    return 1 - dst.Alpha;
                default:
                    throw new ArgumentOutOfRangeException(nameof(blend), blend, null);
            }
        }

    }
}
