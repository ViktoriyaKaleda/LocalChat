using System;

namespace LocalChat.Service
{
    public class MessageReceiveEventArgs : EventArgs
    {
        public Message Message { get; set; }
    }
}
