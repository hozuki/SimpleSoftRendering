# Simple Soft Rendering

A simple programmable soft rendering toy constructed based on Direct3D concepts.
It can be used as a demonstration of how 3D graphics devices work. 

**Warning: some features are still to be fixed.**

## Overview

Direct3D uses a [graphics pipeline](https://msdn.microsoft.com/en-us/library/windows/desktop/ff476882.aspx) to process
graphics. The stages is visualized as the graph below:

![Direct3D Pipeline (Direct3D 10)](https://msdn.microsoft.com/dynimg/IC340510.jpg)

In this project, these "blocks" are implemented:

- Memory Resources
- Input Assembler
- Vertex Shader
- Rasterizer
- Pixel Shader
- Output Merger (depth test and blending)

Due to a clear design, the rest of the "blocks" can also be inserted if you want to.

This project's code base is much larger than [tinyrenderer](https://github.com/ssloy/tinyrenderer), because this project
implemented some advanced features:

- More realistic pipeline. Every stage is separated from the others. The shaders you write behave the same as real shaders.
So in a shader, you cannot access anything outside the shader. And for the graphics device, it requires only a few things
(see `IVertexShaderInput` and `IPixelShaderInput`), exactly matching Direct3D's requirements.
- Culling and clipping.
  - Back face culling: Drops triangles according to their orientation (clockwise, counterclockwise). It is used to
  speed up the performance.
  - Clipping (**TBF**): Znear and Zfar / X-Y
- Blending. Proper blending is necessary for correctly rendering overlapped pixels with transparency.

## Notes

### Direct3D and OpenGL

[The rendering pipeline in OpenGL](https://www.khronos.org/opengl/wiki/Rendering_Pipeline_Overview) is similar to that
in Direct3D. But there is a major difference in coordinate systems. In Direct3D, the transformed vertex coordinates (X, Y, Z)
in the CVV range from -1 to 1, and, traditionally, the input should use a left-hand coordinate system. OpenGL, on the
other hand, requires the coordinate values ranging from 0 to 1, and uses a right-hand coordinate system.

## License

The Unlicense
