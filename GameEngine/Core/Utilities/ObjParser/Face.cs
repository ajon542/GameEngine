using System.Collections.Generic;

namespace GameEngine.Core.Utilities.ObjParser
{
    class Face
    {
        public List<int> Vertices { get; set; }
        public List<int> UVs { get; set; }
        public List<int> Normals { get; set; }
    }
}
