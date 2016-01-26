using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace GameEngine.Core.Utilities
{
    class Face
    {
        public List<int> Vertices { get; set; }
        public List<int> UVs { get; set; }
        public List<int> Normals { get; set; }
    }
}
