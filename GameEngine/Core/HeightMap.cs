using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core
{
    // Basic height map based on indexed drawing:
    // http://www.mbsoftworks.sk/index.php?page=tutorials&series=1&tutorial=8
    class HeightMap : Mesh
    {
        private int width;
        private int height;

        // Should be viewed from:
        // MainCamera.Position = new Vector3(0, 60, 30);
        // MainCamera.LookAt = new Vector3(0, 10, 0);
        // Light activeLight = new Light(new Vector3(0, 20, 0), new Vector3(1.0f, 1.0f, 1.0f));
        //
        // int width = 4;
        // int heigth = 4;
        // GL.Enable(EnableCap.PrimitiveRestart);
        // GL.PrimitiveRestartIndex(width*height);
        //
        // GL.DrawElements(PrimitiveType.TriangleStrip, width*(height-1)*2 + height - 1, DrawElementsType.UnsignedInt, 0); 
        public HeightMap()
        {
            RenderType = PrimitiveType.TriangleStrip;

            Vertices = new List<Vector3>();

            width = 4;
            height = 4;

            float[] fHeights = 
            { 
               4.0f, 2.0f, 3.0f, 1.0f,
               3.0f, 5.0f, 8.0f, 2.0f,
               7.0f, 10.0f, 12.0f, 6.0f,
               4.0f, 6.0f, 8.0f, 3.0f
            };

            float fSizeX = 40.0f, fSizeZ = 40.0f;

            for (int i = 0; i < width * height; ++i)
            {
                float x = (float)(i % width), z = (float)(i / height);
                Vertices.Add(
                    new Vector3(
                       -fSizeX / 2 + fSizeX * x / (float)(width - 1),
                       fHeights[i],
                       -fSizeZ / 2 + fSizeZ * z / (float)(height - 1)
                   ));
            }

            int[] iIndices = 
            { 
                0, 4, 1, 5, 2, 6, 3, 7, 16, // First row, then restart
                4, 8, 5, 9, 6, 10, 7, 11, 16, // Second row, then restart
                8, 12, 9, 13, 10, 14, 11, 15 // Third row, no restart
            };

            Indices = new List<int>(iIndices);

            Normals = new List<Vector3>();
            Colours = new List<Vector3>();
            foreach (Vector3 vertex in Vertices)
            {
                Normals.Add(new Vector3(0, 1, 0));
                Colours.Add(new Vector3(0.9f, 0.9f, 0.9f));
            }
        }
    }
}
