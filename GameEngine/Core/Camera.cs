using System;
using OpenTK;
using OpenTK.Input;

using NLog;

namespace GameEngine.Core
{
    // http://learnopengl.com/#!Getting-started/Camera
    public class Camera
    {
        // Eular Angles
        private float Yaw = -90.0f;
        private float Pitch = 0.0f;

        // Camera options
        private float MovementSpeed = 1.0f;
        private float MouseSensitivity = 0.15f;

        // Mouse state.
        private int mouseWheelIndex;
        private int prevX;
        private int prevY;
        private bool mouseLeftDown;
        private float xoffset;
        private float yoffset;

        // Camera Attributes
        public Vector3 Position { get; set; }
        public Vector3 Front { get; set; }
        public Vector3 Up { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 WorldUp { get; set; }

        public float FieldOfView { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }
        public float AspectRatio { get; set; }

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

        /// <summary>
        /// Initialize a new instance of the <see cref="Camera"/> class.
        /// </summary>
        public Camera()
        {
            Position = Vector3.Zero;
            Front = new Vector3(0.0f, 0.0f, -1.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            WorldUp = new Vector3(0.0f, 1.0f, 0.0f);

            FieldOfView = (float)ConvertToRadians(45);
            NearPlane = 0.1f;
            FarPlane = 10000.0f;
            AspectRatio = 4 / (float)3;
        }

        /// <summary>
        /// Calculates the front vector from the Camera's (updated) Eular Angles.
        /// </summary>
        private void UpdateCameraVectors()
        {
            // Calculate the new Front vector.
            Front = new Vector3(
                (float)(Math.Cos(ConvertToRadians(Yaw)) * Math.Cos(ConvertToRadians(Pitch))),
                (float)(Math.Sin(ConvertToRadians(Pitch))),
                (float)(Math.Sin(ConvertToRadians(Yaw)) * Math.Cos(ConvertToRadians(Pitch)))
            );
            Front.Normalize();

            // Re-calculate the Right and Up vector.
            // Normalize the vectors, because their length gets closer to 0
            // the more you look up or down which results in slower movement.
            Right = Vector3.Normalize(Vector3.Cross(Front, WorldUp));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }

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

                xoffset = (prevX - mouse.X) * MouseSensitivity;
                yoffset = (mouse.Y - prevY) * MouseSensitivity;

                Yaw += xoffset;
                Pitch += yoffset;

                Clamp(ref Pitch, -89.0f, 89.0f);
            }

            prevX = mouse.X;
            prevY = mouse.Y;

            // Move camera back or forward based on the scroll wheel state.
            if (mouseWheelIndex != mouse.Wheel)
            {
                if (mouseWheelIndex > mouse.Wheel)
                {
                    Position -= Front * MovementSpeed;
                }
                if (mouseWheelIndex < mouse.Wheel)
                {
                    Position += Front * MovementSpeed;
                }
                mouseWheelIndex = mouse.Wheel;
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

            UpdateCameraVectors();
        }

        /// <summary>
        /// Helper method to convert degrees to radians.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <returns>The result in radians.</returns>
        private double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        /// <summary>
        /// Clamp the value.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value to clamp to.</param>
        /// <param name="max">The maximum value to clamp to.</param>
        private void Clamp(ref float value, float min, float max)
        {
            if (value > max)
            {
                value = max;
            }
            if (value < min)
            {
                value = min;
            }
        }
    }
}
