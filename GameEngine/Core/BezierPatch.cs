using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core
{
    public class BezierPatch : Mesh
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

        /// <summary>
        /// Simple method of generating "n choose k".
        /// </summary>
        /// <remarks>
        /// This is used in order to generate the N degree polynomial of:
        ///     (a+b)^n
        /// In the expanded form:
        ///     (n choose 0)(a^n) + (n choose 1)(a^n-1)(b^1) + ... + (n choose k)(b^n)
        /// </remarks>
        /// <param name="n">The number of items to choose from.</param>
        /// <param name="k">The number of items to choose.</param>
        /// <returns>The number of possible way to choose the items.</returns>
        private float Choose(int n, int k)
        {
            if (k > n) return 0;
            if (k * 2 > n) k = n - k;
            if (k == 0) return 1;

            float result = n;
            for (int i = 2; i <= k; ++i)
            {
                result *= (n - i + 1);
                result /= i;
            }
            return result;
        }

        /// <summary>
        /// Determines the interpolation point given all the control points.
        /// </summary>
        /// <example>
        /// If the method is given 6 control points, the resulting polynomial will be of degree 5.
        /// 
        /// The following polynomial describes the x and y components, where "a=1-t" and "b=t".
        ///     a^5 + 5(a^4)(b) + 10(a^3)(b^2) + 10(a^2)(b^3) + 5(a)(b^4) + b^5
        /// 
        /// </example>
        /// <param name="controls">The control points including the end points.</param>
        /// <param name="t">The percent of the interpolation.</param>
        /// <returns>The resulting point.</returns>
        private Vector2 BezierDegreeN(List<Vector2> controls, float t, float s)
        {
            Vector2 result = new Vector2(0, 0);
            int n = controls.Count - 1;
            for (int k = 0; k < controls.Count; ++k)
            {
                float mult = Choose(n, k);
                float aExp = n - k;
                float bExp = k;
                float a = (float)Math.Pow(1 - t, aExp) * (float)Math.Pow(1 - s, aExp);
                float b = (float)Math.Pow(t, bExp) * (float)Math.Pow(s, aExp);

                result.X += mult * a * b * controls[k].X;
                result.Y += mult * a * b * controls[k].Y;
            }
            return result;
        }

        public BezierPatch()
        {
            Random rnd = new Random();

            width = 1000;
            height = 1000;

            float[,] heights = new float[width, height];

            List<Vector2> yControls = new List<Vector2>
                {
                    new Vector2(0, -1400),
                    new Vector2(10, 500),
                    new Vector2(30, 700),
                    new Vector2(40, 0)
                };

            float s = 0;
            for (int col = 0; col < height; ++col)
            {
                float t = 0;
                for (int row = 0; row < width; ++row)
                {
                    Vector2 point = BezierDegreeN(yControls, t, s);

                    //heights[row, col] = rnd.Next(3);
                    heights[row, col] = point.Y - 100;
                    t += 0.001f;
                }
                s += 0.001f;
            }

            float scale = 0.1f;
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
