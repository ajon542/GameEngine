using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core
{
    /// <summary>
    /// Torus mesh.
    /// </summary>
    class Torus : Mesh
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Torus"/> mesh class.
        /// </summary>
        /// <param name="radius">The radius of the torus.</param>
        /// <param name="tubeRadius">The radius of the tube.</param>
        /// <param name="subDivAround">Sub-divisions around the torus.</param>
        /// <param name="subDivTube">Sub-divisions around the tube.</param>
        public Torus(float radius, float tubeRadius, int subDivAround, int subDivTube)
        {
            GenerateTorus(radius, tubeRadius, subDivAround, subDivTube);
        }

        /// <summary>
        /// Generates a torus mesh.
        /// </summary>
        /// <param name="radius">The radius of the torus.</param>
        /// <param name="tubeRadius">The radius of the tube.</param>
        /// <param name="subDivAround">Sub-divisions around the torus.</param>
        /// <param name="subDivTube">Sub-divisions around the tube.</param>
        public void GenerateTorus(float radius, float tubeRadius, int subDivAround, int subDivTube)
        {
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            Colours = new List<Vector3>();
            UV = new List<Vector2>();
            Indices = new List<int>();

            float fAddAngleAround = 360.0f / (float)subDivAround;
            float fAddAngleTube = 360.0f / (float)subDivTube;

            float fCurAngleAround = 0.0f;
            int iStepsAround = 1;

            int iFacesAdded = 0;

            while (iStepsAround <= subDivAround)
            {
                float fSineAround = (float)Math.Sin(fCurAngleAround / 180.0f * Math.PI);
                float fCosineAround = (float)Math.Cos(fCurAngleAround / 180.0f * Math.PI);
                Vector3 vDir1 = new Vector3(fSineAround, fCosineAround, 0.0f);
                float fNextAngleAround = fCurAngleAround + fAddAngleAround;
                float fNextSineAround = (float)Math.Sin(fNextAngleAround / 180.0f * Math.PI);
                float fNextCosineAround = (float)Math.Cos(fNextAngleAround / 180.0f * Math.PI);
                Vector3 vDir2 = new Vector3(fNextSineAround, fNextCosineAround, 0.0f);
                float fCurAngleTube = 0.0f;
                int iStepsTube = 1;

                while (iStepsTube <= subDivTube)
                {
                    float fSineTube = (float)Math.Sin(fCurAngleTube / 180.0f * Math.PI);
                    float fCosineTube = (float)Math.Cos(fCurAngleTube / 180.0f * Math.PI);
                    float fNextAngleTube = fCurAngleTube + fAddAngleTube;
                    float fNextSineTube = (float)Math.Sin(fNextAngleTube / 180.0f * Math.PI);
                    float fNextCosineTube = (float)Math.Cos(fNextAngleTube / 180.0f * Math.PI);
                    Vector3 vMid1 = vDir1 * new Vector3(radius - tubeRadius / 2), vMid2 = vDir2 * (radius - tubeRadius / 2);
                    Vector3[] vQuadPoints = 
                    {
                        vMid1 + new Vector3(0.0f, 0.0f, -fNextSineTube*tubeRadius) + vDir1*fNextCosineTube*tubeRadius,
                        vMid1 + new Vector3(0.0f, 0.0f, -fSineTube*tubeRadius) + vDir1*fCosineTube*tubeRadius,
                        vMid2 + new Vector3(0.0f, 0.0f, -fSineTube*tubeRadius) + vDir2*fCosineTube*tubeRadius,
                        vMid2 + new Vector3(0.0f, 0.0f, -fNextSineTube*tubeRadius) + vDir2*fNextCosineTube*tubeRadius
                    };

                    Vector3[] vNormals =
                    {
                        new Vector3(0.0f, 0.0f, -fNextSineTube) + vDir1*fNextCosineTube,
                        new Vector3(0.0f, 0.0f, -fSineTube) + vDir1*fCosineTube,
                        new Vector3(0.0f, 0.0f, -fSineTube) + vDir2*fCosineTube,
                        new Vector3(0.0f, 0.0f, -fNextSineTube) + vDir2*fNextCosineTube
                    };

                    Vector2[] vTexCoords = 
                    {
                        new Vector2(fCurAngleAround/360.0f, fNextAngleTube/360.0f),
                        new Vector2(fCurAngleAround/360.0f, fCurAngleTube/360.0f),
                        new Vector2(fNextAngleAround/360.0f, fCurAngleTube/360.0f),
                        new Vector2(fNextAngleAround/360.0f, fNextAngleTube/360.0f)
                    };

                    int[] iIndices = { 0, 1, 2, 2, 3, 0 };

                    for (int i = 0; i < 6; ++i)
                    {
                        int index = iIndices[i];
                        Vertices.Add(vQuadPoints[index]);
                        Normals.Add(vNormals[index]);
                        UV.Add(vTexCoords[index]);
                        Colours.Add(new Vector3(0.9f, 0.9f, 0.9f));
                    }

                    iFacesAdded += 2;
                    fCurAngleTube += fAddAngleTube;
                    iStepsTube++;
                }
                fCurAngleAround += fAddAngleAround;
                iStepsAround++;
            }

            for (int i = 0; i < iFacesAdded * 3; ++i)
            {
                Indices.Add(i);
            }
        }
    }
}
