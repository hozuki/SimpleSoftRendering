using SharpDX;

namespace SSR.Implementations.SimpleColor {
    public static class MeshBuilder {

        public static (VertexShaderInput[] Vertices, int[] Indices) Box(Vector3 center, float width, float height, float depth) {
            var vertices = new VertexShaderInput[8];

            vertices[0].Position = new Vector4(center.X - width / 2, center.Y - height / 2, center.Z + depth / 2, 1);
            vertices[1].Position = new Vector4(center.X + width / 2, center.Y - height / 2, center.Z + depth / 2, 1);
            vertices[2].Position = new Vector4(center.X - width / 2, center.Y + height / 2, center.Z + depth / 2, 1);
            vertices[3].Position = new Vector4(center.X + width / 2, center.Y + height / 2, center.Z + depth / 2, 1);
            vertices[4].Position = new Vector4(center.X - width / 2, center.Y - height / 2, center.Z - depth / 2, 1);
            vertices[5].Position = new Vector4(center.X + width / 2, center.Y - height / 2, center.Z - depth / 2, 1);
            vertices[6].Position = new Vector4(center.X - width / 2, center.Y + height / 2, center.Z - depth / 2, 1);
            vertices[7].Position = new Vector4(center.X + width / 2, center.Y + height / 2, center.Z - depth / 2, 1);

            vertices[0].Color = Color.Red.WithAlpha(Alpha);
            vertices[1].Color = Color.Green.WithAlpha(Alpha);
            vertices[2].Color = Color.Blue.WithAlpha(Alpha);
            vertices[3].Color = Color.White.WithAlpha(Alpha);
            vertices[4].Color = Color.Cyan.WithAlpha(Alpha);
            vertices[5].Color = Color.Magenta.WithAlpha(Alpha);
            vertices[6].Color = Color.Yellow.WithAlpha(Alpha);
            vertices[7].Color = Color.Black.WithAlpha(Alpha);

            var indices = new[] {
                0, 2, 1, 1, 2, 3,
                2, 6, 3, 3, 6, 7,
                3, 7, 1, 1, 7, 5,
                1, 5, 0, 0, 5, 4,
                0, 4, 2, 2, 4, 6,
                7, 6, 5, 5, 6, 4
            };

            return (vertices, indices);
        }

        private static Color4 WithAlpha(this Color color, float alpha) {
            var c = (Color4)color;
            return new Color4(c.Red, c.Green, c.Blue, alpha);
        }

        private static readonly float Alpha = 1f;

    }
}
