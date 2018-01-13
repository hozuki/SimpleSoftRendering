using System.Collections.Generic;
using System.Drawing;
using SSR.Pipeline.Internal;

namespace SSR.Pipeline {
    public sealed class MemoryResources {

        public TextureSampler SetSampler(int index, Bitmap texture) {
            var sampler = TextureSampler.Create(texture);

            _samplers[index] = sampler;

            return sampler;
        }

        public TextureSampler GetSampler(int index) {
            return _samplers[index];
        }

        private readonly Dictionary<int, TextureSampler> _samplers = new Dictionary<int, TextureSampler>();

    }
}
