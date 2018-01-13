using System;
using System.Collections.Generic;

namespace SSR.Implementations {
    partial class GeometryProcessorBase<TPixelShaderInput> {
        
        protected (TPixelShaderInput[] Vertices, int[] Indices) ClipZ(TPixelShaderInput[] vertices, int[] indices) {
            var rv = new List<TPixelShaderInput>(vertices.Length);
            var ri = new List<int>(indices.Length);

            var indexStart = 0;

            for (var i = 0; i < indices.Length; i += 3) {
                var a = vertices[indices[i]];
                var b = vertices[indices[i + 1]];
                var c = vertices[indices[i + 2]];

                var notInFrustrum = 0;

                if (!IsZVisible(a.TransformedPosition.Z)) {
                    ++notInFrustrum;
                }
                if (!IsZVisible(b.TransformedPosition.Z)) {
                    ++notInFrustrum;
                }
                if (!IsZVisible(c.TransformedPosition.Z)) {
                    ++notInFrustrum;
                }

                switch (notInFrustrum) {
                    case 0: {
                        rv.AddRange(new[] {a, b, c});
                        ri.AddRange(new[] {indexStart, indexStart + 1, indexStart + 2});
                        indexStart += 3;
                        break;
                    }
                    case 1:
                    case 2: {
                        var t = ClipTriangleZ(a, b, c, indexStart);
                        rv.AddRange(t.Vertices);
                        ri.AddRange(t.Indices);
                        indexStart += t.Vertices.Length;
                        break;
                    }
                    case 3:
                        break;
                    default:
                        throw new Exception("Shouldn't happen.");
                }
            }

            return (rv.ToArray(), ri.ToArray());
        }

        private (TPixelShaderInput[] Vertices, int[] Indices) ClipTriangleZ(TPixelShaderInput a, TPixelShaderInput b, TPixelShaderInput c, int indexStart) {
            var (signA, signB, signC) = CalculateSigns(a, b, c);

            TPixelShaderInput[] vertices;
            int[] indices;

            if (signA < 0) {
                if (signB < 0) {
                    if (signC < 0) {
                        throw new Exception("Shouldn't happen.");
                    } else if (signC == 0) {
                        var p1 = GetZPerc(a.TransformedPosition.Z, c.TransformedPosition.Z, ZNear);
                        var p2 = GetZPerc(b.TransformedPosition.Z, c.TransformedPosition.Z, ZNear);
                        vertices = new[] {
                            c, LerpPixelShaderInput(a, c, p1), LerpPixelShaderInput(b, c, p2)
                        };
                        indices = new[] {
                            indexStart, indexStart + 1, indexStart + 2
                        };
                    } else if (signC > 0) {
                        var (p11, p12) = GetZPerc2(a.TransformedPosition.Z, c.TransformedPosition.Z, ZNear, ZFar);
                        var (p21, p22) = GetZPerc2(b.TransformedPosition.Z, c.TransformedPosition.Z, ZNear, ZFar);
                        vertices = new[] {
                            LerpPixelShaderInput(a, c, p11), LerpPixelShaderInput(b, c, p21), LerpPixelShaderInput(a, c, p12), LerpPixelShaderInput(b, c, p22)
                        };
                        indices = new[] {
                            indexStart, indexStart + 2, indexStart + 1,
                            indexStart + 1, indexStart + 2, indexStart + 3
                        };
                    }
                } else if (signB == 0) {
                    if (signC < 0) {
                        var p1 = GetZPerc(a.TransformedPosition.Z, b.TransformedPosition.Z, ZNear);
                        var p2 = GetZPerc(c.TransformedPosition.Z, b.TransformedPosition.Z, ZNear);
                        vertices = new[] {
                            b, LerpPixelShaderInput(a, b, p1), LerpPixelShaderInput(c, b, p2)
                        };
                        indices = new[] {
                            indexStart, indexStart + 1, indexStart + 2
                        };
                    } else if (signC == 0) {
                        var p1 = GetZPerc(a.TransformedPosition.Z, b.TransformedPosition.Z, ZNear);
                        var p2 = GetZPerc(a.TransformedPosition.Z, c.TransformedPosition.Z, ZNear);
                        vertices = new[] {
                            b, c, LerpPixelShaderInput(a, b, p1), LerpPixelShaderInput(a, c, p2)
                        };
                        indices = new[] {
                            indexStart, indexStart + 1, indexStart + 2
                        };
                    } else if (signC > 0) {
                        var p1 = GetZPerc(a.TransformedPosition.Z, b.TransformedPosition.Z, ZNear);
                        var p2 = GetZPerc(c.TransformedPosition.Z, b.TransformedPosition.Z, ZFar);
                        var (p31, p32) = GetZPerc2(a.TransformedPosition.Z, c.TransformedPosition.Z, ZNear, ZFar);
                        vertices = new[] {
                            b, LerpPixelShaderInput(a, b, p1), LerpPixelShaderInput(c, b, p2), LerpPixelShaderInput(a, c, p31), LerpPixelShaderInput(a, c, p32)
                        };
                        indices = new[] {
                            indexStart, indexStart + 1, indexStart + 2,
                            indexStart + 1, indexStart + 3, indexStart + 2,
                            indexStart + 2, indexStart + 3, indexStart + 4
                        };
                    }
                } else if (signB > 0) {
                    if (signC < 0) {
                    } else if (signC == 0) {
                    } else if (signC > 0) {
                    }
                }
            } else if (signA == 0) {
                if (signB < 0) {
                    if (signC < 0) {
                    } else if (signC == 0) {
                    } else if (signC > 0) {
                    }
                } else if (signB == 0) {
                    if (signC < 0) {
                    } else if (signC == 0) {
                        throw new Exception("Shouldn't happen.");
                    } else if (signC > 0) {
                    }
                } else if (signB > 0) {
                    if (signC < 0) {
                    } else if (signC == 0) {
                    } else if (signC > 0) {
                    }
                }
            } else if (signA > 0) {
                if (signB < 0) {
                    if (signC < 0) {
                    } else if (signC == 0) {
                    } else if (signC > 0) {
                    }
                } else if (signB == 0) {
                    if (signC < 0) {
                    } else if (signC == 0) {
                    } else if (signC > 0) {
                    }
                } else if (signB > 0) {
                    if (signC < 0) {
                    } else if (signC == 0) {
                    } else if (signC > 0) {
                        throw new Exception("Shouldn't happen.");
                    }
                }
            }

            throw new NotImplementedException();
        }
        
        private static (int SignA, int SignB, int SignC) CalculateSigns(TPixelShaderInput a, TPixelShaderInput b, TPixelShaderInput c) {
            return (SignOf(a.TransformedPosition.Z), SignOf(b.TransformedPosition.Z), SignOf(c.TransformedPosition.Z));

            int SignOf(float z) {
                if (z < ZNear) {
                    return -1;
                } else if (z > ZFar) {
                    return 1;
                } else {
                    return 0;
                }
            }
        }

        private static bool IsZVisible(float normalizedZ) {
            return ZNear <= normalizedZ && normalizedZ <= ZFar;
        }

        /// <summary>
        /// Get the Z value at <see cref="standard"/>.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="standard"></param>
        /// <returns></returns>
        private static float GetZPerc(float from, float to, float standard) {
            var f = 1 / from;
            var t = 1 / to;
            var s = 1 / standard;
            var perc = (f - s) / (t - f);
            return perc;
        }

        private static (float Perc1, float Perc2) GetZPerc2(float from, float to, float standard1, float standard2) {
            var f = 1 / from;
            var t = 1 / to;
            var s1 = 1 / standard1;
            var s2 = 1 / standard2;
            var perc1 = (f - s1) / (t - f);
            var perc2 = (f - s2) / (t - f);
            return (perc1, perc2);
        }

        private const float ZNear = -1f;

        private const float ZFar = 1f;

    }
}
