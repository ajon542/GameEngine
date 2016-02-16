using System;
using OpenTK;

namespace GameEngine.Core.Utilities
{
    public static class VectorExtensions
    {
        /// <summary>
        /// Calculate the scalar projection of b onto a.
        /// </summary>
        /// <remarks>
        /// http://math.oregonstate.edu/home/programs/undergrad/CalculusQuestStudyGuides/vcalc/dotprod/dotprod.html
        /// </remarks>
        /// <param name="a">Vector a.</param>
        /// <param name="b">Vector b.</param>
        /// <returns>The scalar projection of b onto a.</returns>
        public static float Comp(Vector3 a, Vector3 b)
        {
            return Vector3.Dot(a, b) / a.Length;
        }

        /// <summary>
        /// Calculate the vector projection of b onto a.
        /// </summary>
        /// <remarks>
        /// http://math.oregonstate.edu/home/programs/undergrad/CalculusQuestStudyGuides/vcalc/dotprod/dotprod.html
        /// </remarks>
        /// <param name="a">Vector a.</param>
        /// <param name="b">Vector b.</param>
        /// <returns>The vector projection of b onto a.</returns>
        public static Vector3 Proj(Vector3 a, Vector3 b)
        {
            return Comp(a, b) * (a / a.Length);
        }

        /// <summary>
        /// Reflect the vector l about the vector n.
        /// </summary>
        /// <remarks>
        /// https://asalga.wordpress.com/2012/09/23/understanding-vector-reflection-visually/
        /// </remarks>
        /// <param name="l">The vector to reflect.</param>
        /// <param name="n">The vector to reflect about.</param>
        /// <returns>The reflected vector.</returns>
        public static Vector3 Reflect(Vector3 l, Vector3 n)
        {
            return 2 * Vector3.Dot(n, l) * n - l;
        }
    }
}
