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
            Scale = new Vector3(1, 1, 1);
            Rotation = new Quaternion();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Transform return false.
            Transform t = obj as Transform;
            if (t == null)
            {
                return false;
            }

            // Return true if the fields match.
            return Match(t);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="t">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(Transform t)
        {
            // If parameter is null return false:
            if (t == null)
            {
                return false;
            }

            // Return true if the fields match.
            return Match(t);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Position.GetHashCode() * Scale.GetHashCode() * Rotation.GetHashCode() * 17;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="p">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        private bool Match(Transform p)
        {
            bool positionMatch = p.Position == Position;
            bool scaleMatch = p.Scale == Scale;
            bool rotationMatch = p.Rotation == Rotation;
            return positionMatch & scaleMatch & rotationMatch;
        }
    }
}
