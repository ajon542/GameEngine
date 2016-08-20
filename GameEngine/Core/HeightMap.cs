using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core
{
    // Basic height map based on indexed drawing:
    // http://www.mbsoftworks.sk/index.php?page=tutorials&series=1&tutorial=8
    public class HeightMap : Mesh
    {
        private int width;
        private int height;

        private List<Vector3> GenQuad(float x, float z, float tl, float tr, float bl, float br, float scale)
        {
            List<Vector3> vertices = new List<Vector3>
            {
                new Vector3(x + 0,     tl, z + 0),
                new Vector3(x + 0,     bl, z + scale),
                new Vector3(x + scale, br, z + scale),
                new Vector3(x + scale, br, z + scale),
                new Vector3(x + scale, tr, z + 0),
                new Vector3(x + 0,     tl, z + 0),
            };
            return vertices;
        }

        public HeightMap()
        {
            Random rnd = new Random();

            width = 1000;
            height = 1000;

            float[,] heights = new float[width, height];

            for (int col = 0; col < height; ++col)
            {
                for (int row = 0; row < width; ++row)
                {
                    heights[row, col] = rnd.Next(3);
                    //heights[row, col] = 0;
                }
            }

            float scale = 10;
            for (int col = 0; col < height - 1; ++col)
            {
                for (int row = 0; row < width - 1; ++row)
                {
                    Vertices.AddRange(
                        GenQuad(
                            row * scale,
                            col * scale,
                            heights[row, col],
                            heights[row + 1, col],
                            heights[row, col + 1],
                            heights[row + 1, col + 1],
                            scale)
                            );
                }
            }

            int indexCount = 0;
            Indices = new List<int>();
            foreach (Vector3 vertex in Vertices)
            {
                Colours.Add(new Vector3(1f, 0f, 0f));
                Indices.Add(indexCount++);
            }

            GenerateNormals();
        }
    }
}
