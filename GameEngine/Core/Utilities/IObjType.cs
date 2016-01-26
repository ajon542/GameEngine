using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Utilities
{
    interface IObjType
    {
        bool CanParse(string id);
        void Parse(string input);
    }
}
