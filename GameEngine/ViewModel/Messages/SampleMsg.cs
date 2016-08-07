using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Messaging;

namespace GameEngine.ViewModel.Messages
{
    internal class SampleMessage : MessageBase
    {
        public SampleMessage()
            : base()
        {
        }
    }

    internal class CreateGameObjectMessage : MessageBase
    {
        public string Type { get; private set; }

        public CreateGameObjectMessage(string type)
            : base()
        {
            Type = type;
        }
    }
}
