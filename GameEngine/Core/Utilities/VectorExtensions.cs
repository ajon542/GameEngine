using System;
using OpenTK;

namespace GameEngine.Core.Utilities
{
    public static class VectorExtensions
    {
        // http://math.oregonstate.edu/home/programs/undergrad/CalculusQuestStudyGuides/vcalc/dotprod/dotprod.html
        public static int Component(Vector3 a, Vector3 b)
        {
            throw new NotImplementedException();
        }

        public static Vector3 Project(Vector3 a, Vector3 b)
        {
            throw new NotImplementedException();
        }

        public static Vector3 Project(Vector3 a, Vector3 b)
        {
            throw new NotImplementedException();
        }
        
        public static Vector3 Reflect(Vector3 l, Vector3 n)
        {
            //R = 2(N dot L)N - L
            Vector3 r = 2 * Vector3.Dot(n, l) * n - l;
            return r;
        }

    }
}
