using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GameEngine.Core.Graphics;

using NLog;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// Example to demonstrate Phong shading.
    /// </summary>
    public class SkyBoxExample : Scene
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        private GameObject gameObject;
        private Renderer renderer;
        private Light light;

        int cubeMapId;

        public override void Initialize()
        {
            logger.Log(LogLevel.Info, "");

            List<string> filenames = new List<string>
            {
                @"C:\development\GitHub\GameEngine\GameEngine\Core\GameSpecific\Assets\Textures\CubeMap\Right.png",
                @"C:\development\GitHub\GameEngine\GameEngine\Core\GameSpecific\Assets\Textures\CubeMap\Left.png",
                @"C:\development\GitHub\GameEngine\GameEngine\Core\GameSpecific\Assets\Textures\CubeMap\Top.png",
                @"C:\development\GitHub\GameEngine\GameEngine\Core\GameSpecific\Assets\Textures\CubeMap\Bottom.png",
                @"C:\development\GitHub\GameEngine\GameEngine\Core\GameSpecific\Assets\Textures\CubeMap\Back.png",
                @"C:\development\GitHub\GameEngine\GameEngine\Core\GameSpecific\Assets\Textures\CubeMap\Front.png",
            };
            cubeMapId = Texture.LoadCubeMap(filenames);

            // Create the objects here as the contructor runs quite early on before the GL context has been created.
            // This actually highlights some issues with the intialization procedure. I should be able to use common
            // construction patterns and not be limited to calling these constructors in the Initialize method.
            gameObject = new GameObject();
            renderer = new Renderer();
            light = new Light(new Vector3(10, 0, 0), new Vector4(1, 1, 1, 1));

            renderer.material = new Material("PerPixelMat", "Core/Shaders/skybox-vert.glsl", "Core/Shaders/skybox-frag.glsl");
            renderer.mesh = new SkyBox();

            renderer.Initialize();
        }

        public override void Update()
        {
            MainCamera.Update();
            gameObject.CalculateModelMatrix();
        }

        public override void Render()
        {
            GL.DepthMask(false);

            // Set the program object for the current rendering state.
            renderer.material.UseProgram();

            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapId);

            // Set the default shader variables.
            DefaultShaderInput shaderInput;
            SetDefaultShaderVariables(out shaderInput, gameObject, MainCamera, light);

            // Remove the translation from the view matrix.
            Matrix3 tmp = new Matrix3(MainCamera.ViewMatrix);
            Matrix4 view = new Matrix4(
                new Vector4(tmp.Row0, 0),
                new Vector4(tmp.Row1, 0),
                new Vector4(tmp.Row2, 0),
                new Vector4(0, 0, 0, 1));
            renderer.material.SetMatrix4("_SkyBoxMatrix_VP", view * MainCamera.ProjectionMatrix);

            renderer.Render(shaderInput);

            GL.DepthMask(true);

            // TODO: camera.Render(rootGameObject/scene);
        }

        public override void Shutdown()
        {
            logger.Log(LogLevel.Info, "");
            renderer.Destroy();
        }
    }
}
