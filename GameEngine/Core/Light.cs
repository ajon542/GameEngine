using OpenTK;

namespace GameEngine.Core
{
    class Light
    {
        public Light(Vector3 position, Vector4 color, float diffuseintensity = 0.75f, float ambientintensity = 0.01f)
        {
            Position = position;
            Color = color;

            DiffuseIntensity = diffuseintensity;
            AmbientIntensity = ambientintensity;
        }

        public Vector3 Position;
        public Vector4 Color;
        public float DiffuseIntensity;
        public float AmbientIntensity;
    }
}
