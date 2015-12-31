using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;

namespace GameEngine.Core.GameSpecific
{
    class CubeBehaviour : Behaviour
    {
        public Vector3 Colour { get; set; }

        private Mesh mesh;
        
        public override void Initialize()
        {
            mesh = new Mesh();

            mesh.Vertices = new List<Vector3>
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f)
            };

            mesh.Triangles = new List<int>
            {
                0, 2, 1, 0, 3, 2, // left
                1, 2, 6, 6, 5, 1, // back
                4, 5, 6, 6, 7, 4, // right
                2, 3, 6, 6, 3, 7, // top
                0, 7, 3, 0, 4, 7, // front
                0, 1, 5, 0, 5, 4  // bottom
            };

            mesh.Colours = new List<Vector3>
            {
                Colour,
                new Vector3(0, 0, 0),
                Colour,
                new Vector3(0, 0, 0),
                Colour,
                new Vector3(0, 0, 0),
                Colour,
                new Vector3(0, 0, 0),
            };

            gameObject.AddComponent<Mesh>(mesh);

            gameObject.Transform.Position = new Vector3(0, 0, -5.0f);
            gameObject.Transform.Rotation = new Quaternion(1, 1, 1, 0.5f);
        }

        public override void Update()
        {
        }

        private float x;
        public override void KeyDown(KeyEventArgs key)
        {
            x += 0.01f;
            gameObject.Transform.Position = new Vector3(x, 0, -5.0f);
        }
    }
}
