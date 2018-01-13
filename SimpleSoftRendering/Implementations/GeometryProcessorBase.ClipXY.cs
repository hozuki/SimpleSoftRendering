using System;

namespace SSR.Implementations {
    partial class GeometryProcessorBase<TPixelShaderInput> {
        
        protected (TPixelShaderInput[] Vertices, int[] Indices) ClipXY(TPixelShaderInput[] vertices, int[] indices) {
            throw new NotImplementedException();
        }

        private (TPixelShaderInput[] Vertices, int[] Indices) ClipTriangleXY(TPixelShaderInput a, TPixelShaderInput b, TPixelShaderInput c, int indexStart) {
            throw new NotImplementedException();
        }

    }
}
