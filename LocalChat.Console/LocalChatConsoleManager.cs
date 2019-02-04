using LocalChat.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LocalChat.Console
{
    internal class LocalChatConsoleManager
    {
        private List<Message> Messages { get; }

        private readonly LocalChatClient _localChat;

        private string _userInput;

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
            ConsoleKeyInfo e;

            while (true)
            {
                e = System.Console.ReadKey();

                if (e.Key == ConsoleKey.Enter)
                {
                    if (string.IsNullOrEmpty(_userInput) || string.IsNullOrWhiteSpace(_userInput))
                    {
                        _userInput = "";
                        continue;
                    }

                    _localChat.SendUserMessage(_userInput);
                    _userInput = "";
                }

                else
                {
                    _userInput += e.KeyChar;
                }
            }
        }

        private void PrintReceivedMessageToConsole(object sender, MessageReceiveEventArgs messageReceiveEventArgs)
        {
            Messages.Add(messageReceiveEventArgs.Message);

            System.Console.Clear();

            foreach (var m in Messages)
            {
                System.Console.WriteLine($"{m.CreatedDate.ToShortTimeString()} {m.Username}: {m.Text}");
            }

            if (!string.IsNullOrEmpty(_userInput))
                System.Console.Write(_userInput);
        }
    }
}
