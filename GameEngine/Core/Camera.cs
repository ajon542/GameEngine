using System;
using OpenTK;
using OpenTK.Input;

namespace GameEngine.Core
{
    // http://learnopengl.com/#!Getting-started/Camera
    public class Camera
    {
        public Vector3 Position { get; set; }
        public float FieldOfView { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }
        public Vector3 Orientation { get; set; }
        public Vector3 LookAt { get; set; }
        public float AspectRatio { get; set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Camera"/> class.
        /// </summary>
        public Camera()
        {
            Position = Vector3.Zero;
            FieldOfView = (float)((Math.PI / 180) * 45);
            NearPlane = 1.0f;
            FarPlane = 1000.0f;
            AspectRatio = 4 / (float)3;
            Orientation = new Vector3((float)Math.PI, 0f, 0f);
            LookAt = new Vector3(0, 0, -1);
        }

        /// <summary>
        /// Gets the view matrix for the camera.
        /// </summary>
        public Matrix4 ViewMatrix
        {
            get
            {
                return Matrix4.LookAt(Position, LookAt, Vector3.UnitY);
            }
        }

        /// <summary>
        /// Gets the projection matrix for the camera.
        /// </summary>
        public Matrix4 ProjectionMatrix
        {
            get
            {
                Matrix4 projectionMatrix =
                    Matrix4.CreatePerspectiveFieldOfView
                    (
                    FieldOfView, AspectRatio, NearPlane, FarPlane
                    );
                return projectionMatrix;
            }
        }

        /// <summary>
        /// Move the camera by the given amount for each coordinate.
        /// </summary>
        /// <param name="x">The amount to move the x-coordinate.</param>
        /// <param name="y">The amount to move the y-coordinate.</param>
        /// <param name="z">The amount to move the z-coordinate.</param>
        public void Move(float x, float y, float z)
        {
            Position = new Vector3(Position.X + x, Position.Y + y, Position.Z + z);
            LookAt = new Vector3(LookAt.X + x, LookAt.Y + y, LookAt.Z + z);
        }

        private int mouseWheelIndex;
        private int prevX;
        private int prevY;
        private float mouseSensitivity = 0.01f;
        private float rotation = 0.01f;
        private bool mouseLeftDown;
        public void Update()
        {
            var mouse = Mouse.GetState();
            if (mouse[MouseButton.Left])
            {
                if (mouseLeftDown == false)
                {
                    prevX = mouse.X;
                    prevY = mouse.Y;
                }
                mouseLeftDown = true;

                if (prevY > mouse.Y)
                {
                    Move(0, (prevY - mouse.Y) * -mouseSensitivity, 0);
                }
                if (prevY < mouse.Y)
                {
                    Move(0, (mouse.Y - prevY) * mouseSensitivity, 0);
                }
                if (prevX > mouse.X)
                {
                    Move((prevX - mouse.X) * mouseSensitivity, 0, 0);
                }
                if (prevX < mouse.X)
                {
                    Move((mouse.X - prevX) * -mouseSensitivity, 0, 0);
                }
                prevX = mouse.X;
                prevY = mouse.Y;
            }
            else
            {
                mouseLeftDown = false;
            }

            // Handle zoom.
            if (mouseWheelIndex != mouse.Wheel)
            {
                Vector3 vec = LookAt - Position;

                if (mouseWheelIndex > mouse.Wheel)
                {
                    vec *= -0.5f;
                }
                else
                {
                    vec *= 0.5f;
                }

                LookAt = new Vector3(
                    LookAt.X + vec.X,
                    LookAt.Y + vec.Y,
                    LookAt.Z + vec.Z
                    );
                Position = new Vector3(
                    Position.X + vec.X,
                    Position.Y + vec.Y,
                    Position.Z + vec.Z
                    );
                mouseWheelIndex = mouse.Wheel;
                Console.WriteLine("LookAt {0}, Position {1}, Vec {2}", LookAt, Position, vec);
            }

            if (mouse[MouseButton.Right])
            {
                float radius = 10.0f;
                float camX = (float)Math.Sin(rotation) * radius;
                float camZ = (float)Math.Cos(rotation) * radius;
                Position = new Vector3(camX, 0.0f, camZ);
                LookAt = new Vector3(0, 0, 0);
                rotation += 0.01f;
            }
        }
    }
}
