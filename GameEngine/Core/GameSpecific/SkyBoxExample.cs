using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GameEngine.Core.Graphics;

using NLog;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// Example to demonstrate a skybox.
    /// </summary>
    // TODO: There is an optimization that needs to be compeleted:
    // http://learnopengl.com/#!Advanced-OpenGL/Cubemaps
    public class SkyBoxExample : Scene
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        private GameObject skyboxGameObject;
        private Renderer skyboxRenderer;

        private GameObject sphereGameObject;
        private Renderer sphereRenderer;
        private Light light;

        private int cubeMapId;

        public override void Initialize()
        {
            logger.Log(LogLevel.Info, "");

            // Create the sphere game object.
            sphereGameObject = new GameObject();
            sphereRenderer = new Renderer();
            light = new Light(new Vector3(10, 0, 0), new Vector4(1, 1, 1, 1));

            sphereRenderer.material = new Material("PerPixelMat", "Core/Shaders/per-pixel-vert.glsl", "Core/Shaders/per-pixel-frag.glsl");
            sphereRenderer.mesh = new Sphere(4, 2);

            sphereRenderer.Initialize();

            List<string> filenames = new List<string>
            {
                "Core/GameSpecific/Assets/Textures/CubeMap/Right.png",
                "Core/GameSpecific/Assets/Textures/CubeMap/Left.png",
                "Core/GameSpecific/Assets/Textures/CubeMap/Top.png",
                "Core/GameSpecific/Assets/Textures/CubeMap/Bottom.png",
                "Core/GameSpecific/Assets/Textures/CubeMap/Back.png",
                "Core/GameSpecific/Assets/Textures/CubeMap/Front.png",
            };
            cubeMapId = Texture.LoadCubeMap(filenames);

            skyboxGameObject = new GameObject();
            skyboxRenderer = new Renderer();

            skyboxRenderer.material = new Material("PerPixelMat", "Core/Shaders/skybox-vert.glsl", "Core/Shaders/skybox-frag.glsl");
            skyboxRenderer.mesh = new SkyBox();

            skyboxRenderer.Initialize();
        }

        public override void Update()
        {
            MainCamera.Update();
            skyboxGameObject.CalculateModelMatrix();

            sphereGameObject.Transform.Position = new Vector3(0, 0, -10);
            sphereGameObject.CalculateModelMatrix();
        }

        public override void Render()
        {
            // Render the sphere.
            DefaultShaderInput shaderInput;
            SetDefaultShaderVariables(out shaderInput, sphereGameObject, MainCamera, light);
            sphereRenderer.material.UseProgram();

            sphereRenderer.material.SetVector4("_Color", new Vector4(1, 1, 0, 0));
            sphereRenderer.material.SetVector4("_SpecColor", new Vector4(1, 1, 1, 0));
            sphereRenderer.material.SetFloat("_Shininess", 100);

            sphereRenderer.Render(shaderInput);

            // Render the skybox.
            SetDefaultShaderVariables(out shaderInput, skyboxGameObject, MainCamera, light);
            skyboxRenderer.material.UseProgram();
            GL.DepthFunc(DepthFunction.Lequal);
            GL.DepthMask(false);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapId);
            skyboxRenderer.material.SetUniform1("skybox", TextureUnit.Texture0 - TextureUnit.Texture0);

            // Remove the translation from the view matrix.
            Matrix3 tmp = new Matrix3(MainCamera.ViewMatrix);
            Matrix4 view = new Matrix4(
                new Vector4(tmp.Row0, 0),
                new Vector4(tmp.Row1, 0),
                new Vector4(tmp.Row2, 0),
                new Vector4(0, 0, 0, 1));
            skyboxRenderer.material.SetMatrix4("_SkyBoxMatrix_VP", view * MainCamera.ProjectionMatrix);

            skyboxRenderer.Render(shaderInput);

            GL.DepthFunc(DepthFunction.Less);
            GL.DepthMask(true);

            // TODO: camera.Render(rootGameObject/scene);
        }

        public override void Shutdown()
        {
            logger.Log(LogLevel.Info, "");
            // TODO: Probably could be done by the material.
            GL.DeleteTexture(cubeMapId);
            skyboxRenderer.Destroy();
        }
    }
}
