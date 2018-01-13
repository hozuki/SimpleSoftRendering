using System;
using SharpDX;
using SSR.Pipeline;

namespace SSR.Implementations {
    public abstract partial class RasterizerBase<TPixelShaderInput> : IRasterizer<TPixelShaderInput>
        where TPixelShaderInput : struct, IPixelShaderInput {

        public Color4[,] Rasterize(MemoryResources resources, TPixelShaderInput[] vertices, int[] indices,
            IPixelShader<TPixelShaderInput> pixelShader, OutputMerger outputMerger,
            int outputWidth, int outputHeight, Color4[,] outputBuffer, float[,] wBuffer) {
            for (var i = 0; i < indices.Length; i += 3) {
                var a = vertices[indices[i]];
                var b = vertices[indices[i + 1]];
                var c = vertices[indices[i + 2]];
                var tri = new Triangle<TPixelShaderInput> {
                    A = a,
                    B = b,
                    C = c
                };

                RasterizeTriangle(tri, resources, pixelShader, outputMerger, outputWidth, outputHeight, outputBuffer, wBuffer);
            }

            return outputBuffer;
        }

        protected abstract TPixelShaderInput InterpolatePoint(TPixelShaderInput va, TPixelShaderInput vb, TPixelShaderInput vc, Vector2 p, out bool outOfRange);

        private void RasterizeTriangle(Triangle<TPixelShaderInput> tri, MemoryResources resources, IPixelShader<TPixelShaderInput> pixelShader, OutputMerger outputMerger, int outputWidth, int outputHeight, Color4[,] colorBuffer, float[,] wBuffer) {
            var posA = tri.A.TransformedPosition;
            var posB = tri.B.TransformedPosition;
            var posC = tri.C.TransformedPosition;
            var bitmapCoordA = MapNdcToScreen(posA.X, posA.Y, outputWidth, outputHeight);
            var bitmapCoordB = MapNdcToScreen(posB.X, posB.Y, outputWidth, outputHeight);
            var bitmapCoordC = MapNdcToScreen(posC.X, posC.Y, outputWidth, outputHeight);

            // Triangle bounds (in [-1, 1] space)
            var boundsLeft = Math.Min(bitmapCoordA.X, Math.Min(bitmapCoordB.X, bitmapCoordC.X));
            var boundsTop = Math.Min(bitmapCoordA.Y, Math.Min(bitmapCoordB.Y, bitmapCoordC.Y));
            var boundsRight = Math.Max(bitmapCoordA.X, Math.Max(bitmapCoordB.X, bitmapCoordC.X));
            var boundsBottom = Math.Max(bitmapCoordA.Y, Math.Max(bitmapCoordB.Y, bitmapCoordC.Y));

            if (boundsLeft == boundsRight || boundsTop == boundsBottom) {
                return;
            }

            for (var j = boundsTop; j <= boundsBottom; ++j) {
                if (j < 0 || outputHeight <= j) {
                    continue;
                }

                for (var i = boundsLeft; i <= boundsRight; ++i) {
                    if (i < 0 || outputWidth <= i) {
                        continue;
                    }

                    if (!Helper.PointInTriangle(new Point(i, j), bitmapCoordA, bitmapCoordB, bitmapCoordC)) {
                        continue;
                    }

                    var pixCoord = MapScreenToNdc(i, j, outputWidth, outputHeight);
                    var interpolated = InterpolatePoint(tri.A, tri.B, tri.C, pixCoord, out var outOfRange);

                    //if (outOfRange) {
                    //    continue;
                    //}

                    var pos = interpolated.TransformedPosition;

                    if (pos.Z < -1 || 1 < pos.Z || pos.Y < -1 || 1 < pos.Y || pos.X < -1 || 1 < pos.X) {
                        continue;
                    }

                    var depthTestPassed = outputMerger.IsDepthTestPassed(pos.W, wBuffer[i, j]);

                    if (!depthTestPassed && colorBuffer[i, j].Alpha.Equals(1)) {
                        continue;
                    }

                    var pixel = pixelShader.Transform(resources, interpolated, out var discarded);

                    if (discarded) {
                        continue;
                    }

                    pixel = Color4.Premultiply(pixel);

                    if (depthTestPassed) {
                        wBuffer[i, j] = pos.W;
                        colorBuffer[i, j] = outputMerger.Blend(colorBuffer[i, j], pixel);
                    } else if (colorBuffer[i, j].Alpha < 1) {
                        colorBuffer[i, j] = outputMerger.Blend(pixel, colorBuffer[i, j]);
                    }
                }
            }
        }

    }
}
