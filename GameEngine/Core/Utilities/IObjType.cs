using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Utilities
{
    interface IObjType
    {
        void Deserialize(string input);
        string Serialize();
    }
}
