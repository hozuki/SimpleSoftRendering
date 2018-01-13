using System;
using System.Drawing;
using SharpDX;

namespace SSR.Pipeline.Internal {
    public sealed class TextureSampler {

        private TextureSampler(Bitmap texture) {
            var pixels = Helper.GetPixels(texture);

            _pixels = pixels;
            _width = pixels.GetLength(0);
            _height = pixels.GetLength(1);
        }

        public static TextureSampler Create(Bitmap texture) {
            return new TextureSampler(texture);
        }

        // Not used. Actually nearest-point.
        public TextureFiltering MagnifyFiltering { get; set; }

        public TextureFiltering MinifyFiltering { get; set; }

        public int AnisotropicLevel { get; set; }

        public Color4 BorderColor { get; set; } = Color4.Black;

        public TextureAddressing AddressingU { get; set; }

        public TextureAddressing AddressingV { get; set; }

        public Color4 Sample(float u, float v) {
            int x, y;

            if (0 <= u && u <= 1 && 0 <= v && v <= 1) {
                x = (int)(u * (_width - 1));
                y = (int)(v * (_height - 1));

                return _pixels[x, y];
            }

            var adU = AddressingU;
            var adV = AddressingV;

            if (adU == TextureAddressing.Border || adV == TextureAddressing.Border) {
                return BorderColor;
            }

            if (u < 0 || 1 < u) {
                switch (adU) {
                    case TextureAddressing.Wrap:
                        u = Wrap(u);
                        break;
                    case TextureAddressing.Mirror:
                        u = Mirror(u);
                        break;
                    case TextureAddressing.Clamp:
                        u = MathUtil.Clamp(u, 0, 1);
                        break;
                    case TextureAddressing.MirrorOnce:
                        u = MirrorOnce(u);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (v < 0 || 1 < v) {
                switch (adU) {
                    case TextureAddressing.Wrap:
                        v = Wrap(v);
                        break;
                    case TextureAddressing.Mirror:
                        v = Mirror(v);
                        break;
                    case TextureAddressing.Clamp:
                        v = MathUtil.Clamp(v, 0, 1);
                        break;
                    case TextureAddressing.MirrorOnce:
                        v = MirrorOnce(v);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            x = (int)(u * (_width - 1));
            y = (int)(v * (_height - 1));

            return _pixels[x, y];
        }

        private static float Wrap(float value) {
            var floor = (int)Math.Floor(value);
            return value - floor;
        }

        private static float Mirror(float value) {
            value = -value;

            var mod = value % 2;

            return mod < 1 ? mod : 2 - mod;
        }

        private static float MirrorOnce(float value) {
            var v = value < 0 ? -value : value;
            return MathUtil.Clamp(v, 0, 1);
        }

        private readonly Color4[,] _pixels;
        private readonly int _width;
        private readonly int _height;

    }
}
