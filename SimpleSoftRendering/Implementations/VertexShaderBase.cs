using System;
using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations {
    public abstract class VertexShaderBase<TVertexShaderInput, TPixelShaderInput> : IVertexShader<TVertexShaderInput, TPixelShaderInput>
        where TVertexShaderInput : struct, IVertexShaderInput
        where TPixelShaderInput : struct, IPixelShaderInput {

        protected VertexShaderBase() {
            SetMatrices(
                Matrix.Identity,
                Matrix.LookAtLH(Vector3.One, Vector3.Zero, Vector3.UnitZ),
                Matrix.PerspectiveFovLH(60, (float)16 / 9, 0.5f, 100f));
        }

        public TPixelShaderInput Transform(MemoryResources resources, TVertexShaderInput input) {
            var output = TransformInternal(resources, input);
            var pos = output.TransformedPosition;

            var absW = pos.W;
            //var absW = Math.Abs(pos.W);

            // LH to RH
            pos.Y = -pos.Y;

            // Normalize to (-1, 1)
            pos.X /= absW;
            pos.Y /= absW;
            pos.Z /= absW;

            // Z: (0, 1)
            //pos.Z = (pos.Z + 1) * 0.5f;

            output.TransformedPosition = pos;

            return output;
        }

        public void SetMatrices(Matrix world, Matrix view, Matrix projection) {
            World = world;
            View = view;
            Projection = projection;
            WVP = world * view * projection;
        }

        protected abstract TPixelShaderInput TransformInternal(MemoryResources resources, TVertexShaderInput input);

        protected Matrix World { get; private set; }

        protected Matrix View { get; private set; }

        protected Matrix Projection { get; private set; }

        protected Matrix WVP { get; private set; }

    }
}
