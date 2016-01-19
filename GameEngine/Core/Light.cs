﻿using OpenTK;

namespace GameEngine.Core
{
    class Light
    {
        public Light(Vector3 position, Vector3 color, float diffuseintensity = 1.0f, float ambientintensity = 1.0f)
        {
            Position = position;
            Color = color;

            DiffuseIntensity = diffuseintensity;
            AmbientIntensity = ambientintensity;
        }

        public Vector3 Position;
        public Vector3 Color = new Vector3();
        public float DiffuseIntensity = 1.0f;
        public float AmbientIntensity = 0.1f;
    }
}
