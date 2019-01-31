using LocalChat.Service;
using System.Collections.Generic;
using System.Threading;

namespace LocalChat.Console
{
    internal class LocalChatConsoleManager
    {
        private List<Message> Messages { get; }

        private readonly LocalChatClient _localChat;

        public LocalChatConsoleManager(string userName)
        {
            Messages = new List<Message>();

            _localChat = new LocalChatClient(userName);

            _localChat.SendSystemMessage($"{userName} joined the chat.");

            _localChat.MessageReceive += PrintReceivedMessageToConsole;            
        }

        public void StartChating()
        {
            Thread ListenThread = new Thread(new ThreadStart(_localChat.Listen));
            ListenThread.Start();
            ShowMessageInput();
        }

        private void ShowMessageInput()
        {
            while(true)
            {
                string message = System.Console.ReadLine();
                _localChat.SendUserMessage(message);
            }            
        }

        private void PrintReceivedMessageToConsole(object sender, MessageReceiveEventArgs messageReceiveEventArgs)
        {
            Messages.Add(messageReceiveEventArgs.Message);

            System.Console.Clear();

            foreach (var m in Messages)
            {
                System.Console.WriteLine($"{m.CreatedDate.ToShortTimeString()} {m.Text}");
            }
        }
    }
}
