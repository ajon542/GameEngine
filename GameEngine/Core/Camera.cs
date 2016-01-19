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
    }
}
