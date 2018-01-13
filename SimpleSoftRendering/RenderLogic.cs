using System;
using System.Drawing;
using SharpDX;
using SSR.Pipeline;

namespace SSR {
    public static class RenderLogic {

        public static void Alloc(int width, int height) {
            _outputBuffer = new Color4[width, height];

            var wBuffer = new float[width, height];

            for (var j = 0; j < height; ++j) {
                for (var i = 0; i < width; ++i) {
                    wBuffer[i, j] = float.MaxValue;
                }
            }

            _wBuffer = wBuffer;
        }

        public static void Clear() {
            if (_outputBuffer != null) {
                Array.Clear(_outputBuffer, 0, _outputBuffer.Length);
            }

            if (_wBuffer != null) {
                var wBuffer = _wBuffer;
                var w = wBuffer.GetLength(0);
                var h = wBuffer.GetLength(1);

                for (var j = 0; j < h; ++j) {
                    for (var i = 0; i < w; ++i) {
                        wBuffer[i, j] = float.MaxValue;
                    }
                }
            }
        }

        public static void Render<TVertexShaderIn, TPixelShaderIn>(
            TVertexShaderIn[] vertices,
            int[] indices,
            MemoryResources resources,
            InputAssembler inputAssembler,
            OutputMerger outputMerger,
            IVertexShader<TVertexShaderIn, TPixelShaderIn> vertexShader,
            IGeometryProcessor<TPixelShaderIn> geometryProcessor,
            IRasterizer<TPixelShaderIn> rasterizer,
            IPixelShader<TPixelShaderIn> pixelShader,
            Bitmap output)
            where TVertexShaderIn : struct, IVertexShaderInput
            where TPixelShaderIn : struct, IPixelShaderInput {
            if (vertices.Length == 0 || indices.Length == 0) {
                return;
            }

            (vertices, indices) = inputAssembler.Assemble(vertices, indices);

            var vertexShaderOutput = new TPixelShaderIn[vertices.Length];

            for (var i = 0; i < vertices.Length; ++i) {
                vertexShaderOutput[i] = vertexShader.Transform(resources, vertices[i]);
            }

            (vertexShaderOutput, indices) = geometryProcessor.Process(vertexShaderOutput, indices);

            var pixelShaderOutput = rasterizer.Rasterize(resources, vertexShaderOutput, indices, pixelShader, outputMerger, output.Width, output.Height, _outputBuffer, _wBuffer);

            Helper.SetPixels(pixelShaderOutput, output);
        }

        private static Color4[,] _outputBuffer;
        private static float[,] _wBuffer;

    }
}
