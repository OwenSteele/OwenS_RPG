using Engine.EventArgs;
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Services
{
    public class MessageBroker
    {
        private static readonly MessageBroker s_messageBroker = new MessageBroker();

        private MessageBroker() { }

        public event EventHandler<GameMessageEventArgs> OnMessageRaised;
        public event EventHandler<PlayerMessageEventArgs> OnPlayerMessageRaised;

        public static MessageBroker GetInstance() =>
            s_messageBroker;

        internal void RaiseMessage(string message, bool playerMessage = false, GameMessageEventArgs.ColorCategory color = GameMessageEventArgs.ColorCategory.None)
        {
            if (!playerMessage)
                OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message, color));
            else
                OnPlayerMessageRaised?.Invoke(this, new PlayerMessageEventArgs(message));
        }
    }
}
