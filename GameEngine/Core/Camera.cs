using System;
using OpenTK;

namespace GameEngine.Core
{
    public class Camera
    {
        // TODO: Complete the camera class implementation.
        // position
        // field of view
        // near plane
        // far plane
        // orientation
        // look at
        // viewport aspect ratio
        // view matrix
        // projection matrix
        // view projection matrix

        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.01f;

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
            FieldOfView = 1.0f;
            NearPlane = 1.0f;
            FarPlane = 1000.0f;
            Orientation = new Vector3((float)Math.PI, 0f, 0f);
            LookAt = new Vector3(0, 0, -10);
        }

        public Matrix4 ViewMatrix
        {
            get
            {
                Vector3 lookat = new Vector3();

                lookat.X = (float)(Math.Sin(Orientation.X) * Math.Cos(Orientation.Y));
                lookat.Y = (float)Math.Sin(Orientation.Y);
                lookat.Z = (float)(Math.Cos(Orientation.X) * Math.Cos(Orientation.Y));

                return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
            }
        }

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

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin(Orientation.X), 0, (float)Math.Cos(Orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            Position += offset;
        }

        public void AddRotation(float x, float y)
        {
            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            Vector3 orientation = new Vector3(
                (Orientation.X + x) % ((float)Math.PI * 2.0f),
                Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f),
                0f);

            Orientation = orientation;
        }
    }
}
