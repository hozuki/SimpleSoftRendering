using System;
using System.Collections.Generic;

namespace SSR.Pipeline {
    public class InputAssembler {

        public PrimitiveTopology Topology { get; set; }

        public (TVertexShaderInput[] Vertices, int[] Indices) Assemble<TVertexShaderInput>(TVertexShaderInput[] vertices, int[] indices)
            where TVertexShaderInput : struct, IVertexShaderInput {
            var topology = Topology;

            if (topology == PrimitiveTopology.Triangles) {
                return (vertices, indices);
            } else {
                List<int> ri;

                switch (topology) {
                    case PrimitiveTopology.TriangleStrip:
                        ri = new List<int>(vertices.Length + 2);

                        var latest1 = indices[0];
                        var latest2 = indices[1];

                        for (var i = 2; i < indices.Length; ++i) {
                            ri.Add(latest1);
                            ri.Add(latest2);
                            ri.Add(i);

                            latest1 = latest2;
                            latest2 = i;
                        }
                        break;
                    case PrimitiveTopology.TriangleFan:
                        ri = new List<int>(vertices.Length + 2);

                        var first = indices[0];
                        var latest = indices[1];

                        for (var i = 2; i < indices.Length; ++i) {
                            ri.Add(first);
                            ri.Add(latest);
                            ri.Add(i);

                            latest = i;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return (vertices, ri.ToArray());
            }
        }

    }
}
