﻿using System.Collections.Generic;
using OpenTK;

namespace GameEngine.Core.GameSpecific
{
    class CubeBehaviour : BehaviourComponent
    {
        private Mesh mesh;

        public override void Initialize()
        {
            mesh = new Mesh();
            mesh.Vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
            mesh.Vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
            mesh.Vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
            mesh.Vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
            mesh.Vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
            mesh.Vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
            mesh.Vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
            mesh.Vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));

            int[] indices =
            {
                //left
                0, 2, 1,
                0, 3, 2,
                //back
                1, 2, 6,
                6, 5, 1,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //front
                0, 7, 3,
                0, 4, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
            };

            mesh.Triangles = new List<int>(indices);

            gameObject.AddComponent<Mesh>(mesh);

            gameObject.Transform.Position = new Vector3(0, 0, -3.0f);
            gameObject.Transform.Rotation = new Quaternion(1, 1, 1, 0.5f);
        }

        private float x;
        public override void Update()
        {
            x += 0.01f;
            gameObject.Transform.Position = new Vector3(x, 0, -3.0f);
        }
    }
}