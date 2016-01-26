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
        public List<Vertex> Vertices { get; set; }
        public List<UV> UVs { get; set; }
        public List<Normal> Normals { get; set; }
    }
}
