using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Utilities
{
    public abstract class BaseType : ITypeParser
    {
        public abstract string Id { get; }

        public bool CanParse(string id)
        {
            return id == Id;
        }

        public abstract void Parse(string id);
    }
}
