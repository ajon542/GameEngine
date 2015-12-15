using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using OpenTK;


namespace GameEngine.Core
{
    public class Transform
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transform"/> class.
        /// </summary>
        public Transform()
        {
            Position = new Vector3();
            Scale = new Vector3();
            Rotation = new Quaternion();
        }
    }
}
