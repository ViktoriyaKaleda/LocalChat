using System;

namespace LocalChat.Service
{
    public class Message
    {
        public string Username { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public Message(string message, DateTime createdDate)
        {
            int index = message.IndexOf(':');
            if (index == -1)
                Username = "";
            else
                Username = message.Substring(0, index);

            Text = message.Substring(message.IndexOf(':') + 1);

            CreatedDate = createdDate;
        }
    }
}
