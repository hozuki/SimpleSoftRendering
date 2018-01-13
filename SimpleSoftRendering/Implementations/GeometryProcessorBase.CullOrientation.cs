using System;
using System.Collections.Generic;
using SSR.Extensions;
using SSR.Pipeline;

namespace SSR.Implementations {
    partial class GeometryProcessorBase<TPixelShaderInput> {

        protected (TPixelShaderInput[] Vertices, int[] Indices) CullOrientation(TPixelShaderInput[] vertices, int[] indices) {
            var culling = Culling;

            if (culling == Culling.None) {
                return (vertices, indices);
            }

            var rv = new List<TPixelShaderInput>(vertices.Length);
            var ri = new List<int>(indices.Length);

            var indexStart = 0;

            for (var i = 0; i < indices.Length; i += 3) {
                var a = vertices[indices[i]];
                var b = vertices[indices[i + 1]];
                var c = vertices[indices[i + 2]];

                var ccw = Helper.IsTriangleCounterclockwise(a.TransformedPosition, b.TransformedPosition, c.TransformedPosition);

                switch (culling) {
                    case Culling.Clockwise:
                        if (ccw) {
                            continue;
                        }
                        break;
                    case Culling.Counterclockwise:
                        if (!ccw) {
                            continue;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                rv.AddRange(new[] {a, b, c});
                ri.AddRange(new[] {indexStart, indexStart + 1, indexStart + 2});
                indexStart += 3;
            }

            return (rv.ToArray(), ri.ToArray());
        }

    }
}
