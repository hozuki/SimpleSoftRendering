using System.Drawing;
using System.Drawing.Imaging;
using SharpDX;

namespace SSR {
    public static class Helper {

        public static unsafe Color4[,] GetPixels(Bitmap texture) {
            var textureRect = new System.Drawing.Rectangle(0, 0, texture.Width, texture.Height);
            var bitmapData = texture.LockBits(textureRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            var scan0 = (byte*)bitmapData.Scan0;

            var width = bitmapData.Width;
            var height = bitmapData.Height;
            var pixels = new Color4[width, height];

            for (var j = 0; j < bitmapData.Height; ++j) {
                var pLine = scan0 + j * bitmapData.Stride;

                for (var i = 0; i < bitmapData.Width; ++i) {
                    var p = pLine + i * sizeof(uint);

                    var b = (float)p[0] / byte.MaxValue;
                    var g = (float)p[1] / byte.MaxValue;
                    var r = (float)p[2] / byte.MaxValue;
                    var a = (float)p[3] / byte.MaxValue;

                    pixels[i, j] = new Color4(r, g, b, a);
                }
            }

            texture.UnlockBits(bitmapData);

            return pixels;
        }

        public static unsafe void SetPixels(Color4[,] screen, Bitmap output) {
            var bitmapRect = new System.Drawing.Rectangle(0, 0, output.Width, output.Height);
            var bitmapData = output.LockBits(bitmapRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            var scan0 = (byte*)bitmapData.Scan0;

            for (var j = 0; j < output.Height; ++j) {
                var pLine = scan0 + j * bitmapData.Stride;

                for (var i = 0; i < output.Width; ++i) {
                    var p = pLine + i * sizeof(uint);
                    var color = new SharpDX.Color(screen[i, j]);

                    p[0] = color.B;
                    p[1] = color.G;
                    p[2] = color.R;
                    p[3] = color.A;
                }
            }

            output.UnlockBits(bitmapData);
        }

        public static bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c) {
            return SameSide(p, a, b, c) && SameSide(p, b, c, a) && SameSide(p, c, a, b);
        }

        public static bool PointInTriangle(SharpDX.Point p, SharpDX.Point a, SharpDX.Point b, SharpDX.Point c) {
            return SameSide(p, a, b, c) && SameSide(p, b, c, a) && SameSide(p, c, a, b);
        }

        public static bool IsTriangleCounterclockwise(Vector3 eye, Vector3 a, Vector3 b, Vector3 c) {
            var n = Vector3.Cross(b - a, c - a);
            var d = a - eye;

            return Vector3.Dot(n, d) > 0;
        }

        public static bool IsTriangleCounterclockwise(Vector2 a, Vector2 b, Vector2 c) {
            var v = 0f;

            v += (b.X - a.X) * (b.Y + a.Y);
            v += (c.X - b.X) * (c.Y + b.Y);
            v += (a.X - c.X) * (a.Y + c.Y);

            return v < 0;
        }

        public static bool IsTriangleCounterclockwise(Vector4 a, Vector4 b, Vector4 c) {
            var v = 0f;

            v += (b.X - a.X) * (b.Y + a.Y);
            v += (c.X - b.X) * (c.Y + b.Y);
            v += (a.X - c.X) * (a.Y + c.Y);

            return v < 0;
        }

        private static bool SameSide(SharpDX.Point p1, SharpDX.Point p2, SharpDX.Point a, SharpDX.Point b) {
            // Vector3D is faster (~1.25x) than using a struct Point3D...
            var p1a = new Vector3(p1.X, p1.Y, 0);
            var p2a = new Vector3(p2.X, p2.Y, 0);
            var aa = new Vector3(a.X, a.Y, 0);
            var ba = new Vector3(b.X, b.Y, 0);

            var cp1 = Vector3.Cross(ba - aa, p1a - aa);
            var cp2 = Vector3.Cross(ba - aa, p2a - aa);

            return Vector3.Dot(cp1, cp2) >= 0;
        }

        private static bool SameSide(Vector2 p1, Vector2 p2, Vector2 a, Vector2 b) {
            var p1a = new Vector3(p1, 0);
            var p2a = new Vector3(p2, 0);
            var aa = new Vector3(a, 0);
            var ba = new Vector3(b, 0);

            var cp1 = Vector3.Cross(ba - aa, p1a - aa);
            var cp2 = Vector3.Cross(ba - aa, p2a - aa);

            return Vector3.Dot(cp1, cp2) >= 0;
        }

    }
}
