using System;
using OpenTK;
using OpenTK.Input;

namespace GameEngine.Core
{
    // http://learnopengl.com/#!Getting-started/Camera
    public class Camera
    {
        public const float YAW = -90.0f;
        public const float PITCH = 0.0f;
        public const float SPEED = 3.0f;
        public const float SENSITIVTY = 0.25f;
        public const float ZOOM = 45.0f;

        // Camera Attributes
        public Vector3 Position;
        public Vector3 Front;
        public Vector3 Up;
        public Vector3 Right;
        public Vector3 WorldUp;
        // Eular Angles
        float Yaw = YAW;
        float Pitch = PITCH;
        // Camera options
        float MovementSpeed = SPEED;
        float MouseSensitivity = SENSITIVTY;

        public float FieldOfView { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }
        public float AspectRatio { get; set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Camera"/> class.
        /// </summary>
        public Camera()
        {
            Position = Vector3.Zero;
            Front = new Vector3(0.0f, 0.0f, -1.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            WorldUp = new Vector3(0.0f, 1.0f, 0.0f);


            FieldOfView = (float)((Math.PI / 180) * 45);
            NearPlane = 1.0f;
            FarPlane = 1000.0f;
            AspectRatio = 4 / (float)3;
        }

            // Calculates the front vector from the Camera's (updated) Eular Angles
        private void updateCameraVectors()
        {
            // Calculate the new Front vector
            Vector3 front = new Vector3(
                (float)(Math.Cos(ConvertToRadians(Yaw)) * Math.Cos(ConvertToRadians(Pitch))),
                (float)(Math.Sin(ConvertToRadians(Pitch))),
                (float)(Math.Sin(ConvertToRadians(Yaw)) * Math.Cos(ConvertToRadians(Pitch)))
            );
            front.Normalize();
            Front = front;
            // Also re-calculate the Right and Up vector
            Right = Vector3.Normalize(Vector3.Cross(Front, WorldUp));  // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            Up    = Vector3.Normalize(Vector3.Cross(Right, Front));
        }

        private double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        /// <summary>
        /// Gets the view matrix for the camera.
        /// </summary>
        public Matrix4 ViewMatrix
        {
            get
            {
                return Matrix4.LookAt(Position, Position + Front, Up);
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

        private int prevX;
        private int prevY;
        private bool mouseLeftDown;
        float xoffset;
        float yoffset;

        private int mouseWheelIndex;
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

                xoffset = prevX - mouse.X;
                yoffset = mouse.Y - prevY;

                xoffset *= MouseSensitivity;
                yoffset *= MouseSensitivity;

                Yaw += xoffset;
                Pitch += yoffset;

                if (true)
                {
                    if (Pitch > 89.0f)
                        Pitch = 89.0f;
                    if (Pitch < -89.0f)
                        Pitch = -89.0f;
                }
            }

            prevX = mouse.X;
            prevY = mouse.Y;

            if (mouseWheelIndex != mouse.Wheel)
            {
            }

            if (mouse[MouseButton.Right])
            {
            }

            var keyboard = Keyboard.GetState();
            if (keyboard[Key.W])
                Position += Front * MovementSpeed;
            if (keyboard[Key.S])
                Position -= Front * MovementSpeed;
            if (keyboard[Key.A])
                Position -= Right * MovementSpeed;
            if (keyboard[Key.D])
                Position += Right * MovementSpeed;

            updateCameraVectors();
        }
    }
}
