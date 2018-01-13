using SSR.Pipeline;

namespace SSR.Implementations {
    public abstract partial class GeometryProcessorBase<TPixelShaderInput> : IGeometryProcessor<TPixelShaderInput>
        where TPixelShaderInput : struct, IPixelShaderInput {

        public Culling Culling { get; set; }

        public abstract (TPixelShaderInput[] Vertices, int[] Indices) Process(TPixelShaderInput[] vertices, int[] indices);

        protected abstract TPixelShaderInput LerpPixelShaderInput(TPixelShaderInput from, TPixelShaderInput to, float perc);

        /// <summary>
        /// The default processing procedure.
        /// May perform orientation culling (clockwise, counterclockwise), clippings (Z and XY), depending on the default configuration.
        /// </summary>
        /// <param name="vertices">Vertex array before geometry processing.</param>
        /// <param name="indices">Index array of corresponding vertices.</param>
        /// <returns>Vertex and index array after geometry processing. The topology is actually triangles.</returns>
        protected (TPixelShaderInput[] Vertices, int[] Indices) DefaultProcessProc(TPixelShaderInput[] vertices, int[] indices) {
            if (EnableCullOrientation) {
                (vertices, indices) = CullOrientation(vertices, indices);
            }

            return (vertices, indices);
        }

        /// <summary>
        /// Returns the original vertex and index array.
        /// </summary>
        /// <param name="vertices">Vertex array before geometry processing.</param>
        /// <param name="indices">Index array of corresponding vertices.</param>
        /// <returns>Vertex and index array after geometry processing.</returns>
        protected (TPixelShaderInput[] Vertices, int[] Indices) Original(TPixelShaderInput[] vertices, int[] indices) {
            return (vertices, indices);
        }

        private const bool EnableCullOrientation = true;

    }
}
