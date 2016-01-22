using System;
using System.Collections.Generic;
using OpenTK;

namespace GameEngine.Core
{
    /// <summary>
    /// Sphere mesh.
    /// </summary>
    class Sphere : Mesh
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere"/> class.
        /// </summary>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="height">The height of the sphere.</param>
        /// <param name="segments">The segments of the sphere.</param>
        /// <param name="rings">The rings of the sphere.</param>
        public Sphere(int radius, int height, int segments, int rings)
        {
            CalculateVertices(radius, height, segments, rings);
            CalculateElements(segments, rings);
        }

        public void CalculateVertices(float radius, float height, int segments, int rings)
        {
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            UV = new List<Vector2>();
            Colours = new List<Vector3>();

            for (double y = 0; y < rings; y++)
            {
                double phi = (y / (rings - 1)) * Math.PI;
                for (double x = 0; x < segments; x++)
                {
                    double theta = (x / (segments - 1)) * 2 * Math.PI;

                    Vector3 vertex = new Vector3
                    {
                        X = (float)(radius * Math.Sin(phi) * Math.Cos(theta)),
                        Y = (float)(height * Math.Cos(phi)),
                        Z = (float)(radius * Math.Sin(phi) * Math.Sin(theta)),
                    };

                    Vector3 normal = Vector3.Normalize(vertex);
                    Vector2 uv = new Vector2
                    {
                        X = (float)(x / (segments - 1)),
                        Y = (float)(y / (rings - 1))
                    };
                    Vertices.Add(vertex);
                    Normals.Add(normal);
                    UV.Add(uv);
                    Colours.Add(new Vector3(0.9f, 0.9f, 0.9f));
                }
            }
        }

        public void CalculateElements(int segments, int rings)
        {
            Indices = new List<int>();

            for (byte y = 0; y < rings - 1; y++)
            {
                for (byte x = 0; x < segments - 1; x++)
                {
                    Indices.Add((y + 0) * segments + x);
                    Indices.Add((y + 1) * segments + x);
                    Indices.Add((y + 1) * segments + x + 1);
                    Indices.Add((y + 1) * segments + x + 1);
                    Indices.Add((y + 0) * segments + x + 1);
                    Indices.Add((y + 0) * segments + x);
                }
            }

            // Verify that we don't access any vertices out of bounds:
            foreach (int index in Indices)
            {
                if (index >= segments * rings)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
    }
}
