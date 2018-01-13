using SharpDX;

namespace SSR.Extensions {
    public static class Vector4Extensions {

        public static Vector2 XY(this Vector4 v) {
            return new Vector2(v.X, v.Y);
        }

        public static Vector3 XYZ(this Vector4 v) {
            return new Vector3(v.X, v.Y, v.Z);
        }

    }
}
