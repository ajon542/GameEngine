using OpenTK;

namespace GameEngine.Core.Graphics
{
    public class DefaultShaderInput
    {
        public Matrix4 MatrixMVP { get; set; }
        public Matrix4 MatrixMV { get; set; }
        public Matrix4 MatrixV { get; set; }
        public Matrix4 MatrixP { get; set; }
        public Matrix4 MatrixVP { get; set; }
        public Matrix4 Object2World { get; set; }
        public Matrix4 World2Object { get; set; }
        public Vector3 WorldCameraPos { get; set; }

        // TODO: Might be worth separating out.
        public Vector3 LightPos { get; set; }
    }
}
