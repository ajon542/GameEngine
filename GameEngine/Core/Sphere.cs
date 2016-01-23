using System;
using System.Collections.Generic;
using OpenTK;

namespace GameEngine.Core
{
    class Sphere : Mesh
    {
        private static Vector3[] directions = {
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 1),
        };

        public Sphere(int subdivisions, float radius)
        {
            Create(subdivisions, radius);
        }

        public void Create(int subdivisions, float radius)
        {
            if (subdivisions < 0)
            {
                subdivisions = 0;
            }
            else if (subdivisions > 6)
            {
                subdivisions = 6;
            }

            int resolution = 1 << subdivisions;
            Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1) * 4 - (resolution * 2 - 1) * 3];
            int[] triangles = new int[(1 << (subdivisions * 2 + 3)) * 3];
            CreateOctahedron(vertices, triangles, resolution);

            Vector3[] normals = new Vector3[vertices.Length];
            Normalize(vertices, normals);

            Vector2[] uv = new Vector2[vertices.Length];
            CreateUV(vertices, uv);

            Vector4[] tangents = new Vector4[vertices.Length];
            CreateTangents(vertices, tangents);

            if (radius != 1f)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] *= radius;
                }
            }

            Vertices = new List<Vector3>(vertices);
            Normals = new List<Vector3>(normals);
            UV = new List<Vector2>(uv);
            Indices = new List<int>(triangles);
        }

        private void CreateOctahedron(Vector3[] vertices, int[] triangles, int resolution)
        {
            int v = 0, vBottom = 0, t = 0;

            for (int i = 0; i < 4; i++)
            {
                vertices[v++] = new Vector3(0, -1, 0);
            }

            for (int i = 1; i <= resolution; i++)
            {
                float progress = (float)i / resolution;
                Vector3 from, to;
                vertices[v++] = to = Vector3.Lerp(new Vector3(0, -1, 0), new Vector3(0, 0, 1), progress);
                for (int d = 0; d < 4; d++)
                {
                    from = to;
                    to = Vector3.Lerp(new Vector3(0, -1, 0), directions[d], progress);
                    t = CreateLowerStrip(i, v, vBottom, t, triangles);
                    v = CreateVertexLine(from, to, i, v, vertices);
                    vBottom += i > 1 ? (i - 1) : 1;
                }
                vBottom = v - 1 - i * 4;
            }

            for (int i = resolution - 1; i >= 1; i--)
            {
                float progress = (float)i / resolution;
                Vector3 from, to;
                vertices[v++] = to = Vector3.Lerp(new Vector3(0, 1, 0), new Vector3(0, 0, 1), progress);
                for (int d = 0; d < 4; d++)
                {
                    from = to;
                    to = Vector3.Lerp(new Vector3(0, 1, 0), directions[d], progress);
                    t = CreateUpperStrip(i, v, vBottom, t, triangles);
                    v = CreateVertexLine(from, to, i, v, vertices);
                    vBottom += i + 1;
                }
                vBottom = v - 1 - i * 4;
            }

            for (int i = 0; i < 4; i++)
            {
                triangles[t++] = vBottom;
                triangles[t++] = v;
                triangles[t++] = ++vBottom;
                vertices[v++] = new Vector3(0, 1, 0);
            }
        }

        private int CreateVertexLine(Vector3 from, Vector3 to, int steps, int v, Vector3[] vertices)
        {
            for (int i = 1; i <= steps; i++)
            {
                vertices[v++] = Vector3.Lerp(from, to, (float)i / steps);
            }
            return v;
        }

        private int CreateLowerStrip(int steps, int vTop, int vBottom, int t, int[] triangles)
        {
            for (int i = 1; i < steps; i++)
            {
                triangles[t++] = vBottom;
                triangles[t++] = vTop - 1;
                triangles[t++] = vTop;

                triangles[t++] = vBottom++;
                triangles[t++] = vTop++;
                triangles[t++] = vBottom;
            }
            triangles[t++] = vBottom;
            triangles[t++] = vTop - 1;
            triangles[t++] = vTop;
            return t;
        }

        private int CreateUpperStrip(int steps, int vTop, int vBottom, int t, int[] triangles)
        {
            triangles[t++] = vBottom;
            triangles[t++] = vTop - 1;
            triangles[t++] = ++vBottom;
            for (int i = 1; i <= steps; i++)
            {
                triangles[t++] = vTop - 1;
                triangles[t++] = vTop;
                triangles[t++] = vBottom;

                triangles[t++] = vBottom;
                triangles[t++] = vTop++;
                triangles[t++] = ++vBottom;
            }
            return t;
        }

        private void Normalize(Vector3[] vertices, Vector3[] normals)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normalize();
                normals[i] = vertices[i];
            }
        }

        private void CreateUV(Vector3[] vertices, Vector2[] uv)
        {
            float previousX = 1f;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];
                if (v.X == previousX)
                {
                    uv[i - 1].X = 1f;
                }
                previousX = v.X;
                Vector2 textureCoordinates;
                textureCoordinates.X = (float)(Math.Atan2(v.X, v.Z) / (-2f * Math.PI));
                if (textureCoordinates.X < 0f)
                {
                    textureCoordinates.X += 1f;
                }
                textureCoordinates.Y = (float)(Math.Asin(v.Y) / Math.PI + 0.5f);
                uv[i] = textureCoordinates;
            }
            uv[vertices.Length - 4].X = uv[0].X = 0.125f;
            uv[vertices.Length - 3].X = uv[1].X = 0.375f;
            uv[vertices.Length - 2].X = uv[2].X = 0.625f;
            uv[vertices.Length - 1].X = uv[3].X = 0.875f;
        }

        private void CreateTangents(Vector3[] vertices, Vector4[] tangents)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];
                v.Y = 0f;
                v.Normalize();
                Vector4 tangent;
                tangent.X = -v.Z;
                tangent.Y = 0f;
                tangent.Z = v.X;
                tangent.W = -1f;
                tangents[i] = tangent;
            }

            tangents[0] = new Vector4(-1f, 0, -1f, -1f);
            tangents[0].Normalize();
            tangents[1] = new Vector4(1f, 0f, -1f, -1f);
            tangents[1].Normalize();
            tangents[2] = new Vector4(1f, 0f, 1f, -1f);
            tangents[2].Normalize();
            tangents[3] = new Vector4(-1f, 0f, 1f, -1f);
            tangents[3].Normalize();

            tangents[vertices.Length - 4] = tangents[0];
            tangents[vertices.Length - 3] = tangents[1];
            tangents[vertices.Length - 2] = tangents[2];
            tangents[vertices.Length - 1] = tangents[3];
            for (int i = 0; i < 4; i++)
            {
                tangents[vertices.Length - 1 - i].W = tangents[i].W = -1f;
            }
        }
    }
}
