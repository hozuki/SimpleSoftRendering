using SharpDX;

namespace SSR.Extensions {
    public static class Vector3Extensions {

        public static Vector2 XY(this Vector3 v) {
            return new Vector2(v.X, v.Y);
        }

    }
}
