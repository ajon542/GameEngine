using System;
using System.Collections.Generic;
using OpenTK;

namespace GameEngine.Core
{
    class Sphere : Mesh
    {
        public Sphere()
        {
            CalculateVertices(1, 1, 25, 25);
            CalculateElements(1, 1, 25, 25);
        }

        public void CalculateVertices(float radius, float height, byte segments, byte rings)
        {
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            UV = new List<Vector2>();
            Colours = new List<Vector3>();

            for (double y = 0; y < rings; y++)
            {
                double phi = (y / (rings - 1)) * Math.PI; //was /2 
                for (double x = 0; x < segments; x++)
                {
                    double theta = (x / (segments - 1)) * 2 * Math.PI;

                    Vector3 v = new Vector3()
                    {
                        X = (float)(radius * Math.Sin(phi) * Math.Cos(theta)),
                        Y = (float)(height * Math.Cos(phi)),
                        Z = (float)(radius * Math.Sin(phi) * Math.Sin(theta)),
                    };
                    Vector3 n = Vector3.Normalize(v);
                    Vector2 uv = new Vector2()
                    {
                        X = (float)(x / (segments - 1)),
                        Y = (float)(y / (rings - 1))
                    };
                    Vertices.Add(v);
                    Normals.Add(n);
                    UV.Add(uv);
                    Colours.Add(new Vector3(0.9f, 0.9f, 0.9f));
                }
            }
        }

        public void CalculateElements(float radius, float height, byte segments, byte rings)
        {
            Indices = new List<int>();

            ushort i = 0;

            for (byte y = 0; y < rings - 1; y++)
            {
                for (byte x = 0; x < segments - 1; x++)
                {
                    Indices.Add((y + 0) * segments + x);
                    Indices.Add((ushort)((y + 1) * segments + x));
                    Indices.Add((ushort)((y + 1) * segments + x + 1));
                    Indices.Add((ushort)((y + 1) * segments + x + 1));
                    Indices.Add((ushort)((y + 0) * segments + x + 1));
                    Indices.Add((ushort)((y + 0) * segments + x));
                }
            }

            // Verify that we don't access any vertices out of bounds:
            foreach (int index in Indices)
                if (index >= segments * rings)
                    throw new IndexOutOfRangeException();
        }
    }
}
