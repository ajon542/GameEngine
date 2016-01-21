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
        public Torus()
        {
            GenerateTorus(7.0f, 2.0f, 20, 20);
        }

        public void GenerateTorus(float fRadius, float fTubeRadius, int iSubDivAround, int iSubDivTube)
        {
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            Colours = new List<Vector3>();
            UV = new List<Vector2>();
            Indices = new List<int>();

            float fAddAngleAround = 360.0f / (float)iSubDivAround;
            float fAddAngleTube = 360.0f / (float)iSubDivTube;

            float fCurAngleAround = 0.0f;
            int iStepsAround = 1;

            int iFacesAdded = 0;

            while (iStepsAround <= iSubDivAround)
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
                while (iStepsTube <= iSubDivTube)
                {
                    float fSineTube = (float)Math.Sin(fCurAngleTube / 180.0f * Math.PI);
                    float fCosineTube = (float)Math.Cos(fCurAngleTube / 180.0f * Math.PI);
                    float fNextAngleTube = fCurAngleTube + fAddAngleTube;
                    float fNextSineTube = (float)Math.Sin(fNextAngleTube / 180.0f * Math.PI);
                    float fNextCosineTube = (float)Math.Cos(fNextAngleTube / 180.0f * Math.PI);
                    Vector3 vMid1 = vDir1 * new Vector3(fRadius - fTubeRadius / 2), vMid2 = vDir2 * (fRadius - fTubeRadius / 2);
                    Vector3[] vQuadPoints = 
                    {
                        vMid1 + new Vector3(0.0f, 0.0f, -fNextSineTube*fTubeRadius) + vDir1*fNextCosineTube*fTubeRadius,
                        vMid1 + new Vector3(0.0f, 0.0f, -fSineTube*fTubeRadius) + vDir1*fCosineTube*fTubeRadius,
                        vMid2 + new Vector3(0.0f, 0.0f, -fSineTube*fTubeRadius) + vDir2*fCosineTube*fTubeRadius,
                        vMid2 + new Vector3(0.0f, 0.0f, -fNextSineTube*fTubeRadius) + vDir2*fNextCosineTube*fTubeRadius
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

                    iFacesAdded += 2; // Keep count of added faces
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
