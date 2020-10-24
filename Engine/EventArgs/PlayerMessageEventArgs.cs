using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.EventArgs
{
    public class PlayerMessageEventArgs : System.EventArgs
    {
        public string Message { get; private set; }

        public PlayerMessageEventArgs(string message) => Message = message;
    }
}
