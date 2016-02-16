using System;
using OpenTK;

namespace GameEngine.Core.Utilities
{
    public static class VectorExtensions
    {
        // http://math.oregonstate.edu/home/programs/undergrad/CalculusQuestStudyGuides/vcalc/dotprod/dotprod.html
        public static float Comp(Vector3 a, Vector3 b)
        {
            return Vector3.Dot(a, b) / a.Length;
        }

        public static Vector3 Proj(Vector3 a, Vector3 b)
        {
            return Comp(a, b) * (a / a.Length);
        }
        
        public static Vector3 Reflect(Vector3 l, Vector3 n)
        {
            Vector3 r = 2 * Vector3.Dot(n, l) * n - l;
            return r;
        }
    }
}
