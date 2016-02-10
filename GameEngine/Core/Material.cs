using OpenTK;

namespace GameEngine.Core
{
    public class Material
    {
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public float SpecularExponent { get; set; }

        public Material()
        {
            Ambient = new Vector3(0.2f, 0.2f, 0.2f);
            Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
            Specular = new Vector3(1.0f, 1.0f, 1.0f);
            SpecularExponent = 2.0f;
        }
    }
}
