using System.Collections.Generic;
using SharpDX;

namespace SSR.Implementations.SimpleTexture {
    public static class MeshBuilder {

        public static (VertexShaderInput[] Vertices, int[] Indices) Box(Vector3 center, float width, float height, float depth) {
            var vertices = new List<VertexShaderInput>();
            var indices = new List<int>();
            var indexStart = 0;

            var p1 = new Vector3(center.X - width / 2, center.Y - height / 2, center.Z - depth / 2);
            var p2 = new Vector3(center.X + width / 2, center.Y - height / 2, center.Z - depth / 2);
            var p3 = new Vector3(center.X - width / 2, center.Y + height / 2, center.Z - depth / 2);
            var p4 = new Vector3(center.X + width / 2, center.Y + height / 2, center.Z - depth / 2);
            var p5 = new Vector3(center.X - width / 2, center.Y - height / 2, center.Z + depth / 2);
            var p6 = new Vector3(center.X + width / 2, center.Y - height / 2, center.Z + depth / 2);
            var p7 = new Vector3(center.X - width / 2, center.Y + height / 2, center.Z + depth / 2);
            var p8 = new Vector3(center.X + width / 2, center.Y + height / 2, center.Z + depth / 2);

            var faces = new (Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4)[] {
                (p1, p2, p3, p4), // up
                (p7, p8, p5, p6), // down
                (p1, p3, p5, p7), // left
                (p4, p2, p8, p6), // right
                (p2, p1, p6, p5), // back
                (p3, p4, p7, p8) // front
            };

            foreach (var face in faces) {
                var (v, i) = AddFace(face.P1, face.P2, face.P3, face.P4, indexStart);

                vertices.AddRange(v);
                indices.AddRange(i);

                indexStart += v.Length;
            }

            return (vertices.ToArray(), indices.ToArray());
        }

        private static (VertexShaderInput[] Vertices, int[] Indices) AddFace(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int indexStart) {
            var v1 = new VertexShaderInput();
            var v2 = new VertexShaderInput();
            var v3 = new VertexShaderInput();
            var v4 = new VertexShaderInput();

            v1.Position = new Vector4(p1, 1);
            v2.Position = new Vector4(p2, 1);
            v3.Position = new Vector4(p3, 1);
            v4.Position = new Vector4(p4, 1);

            v1.TexCoords = Vector2.Zero;
            v2.TexCoords = Vector2.UnitX;
            v3.TexCoords = Vector2.UnitY;
            v4.TexCoords = Vector2.One;

            var indices = new[] {
                indexStart, indexStart + 2, indexStart + 1,
                indexStart + 1, indexStart + 2, indexStart + 3
            };

            return (new[] {v1, v2, v3, v4}, indices);
        }

    }
}
