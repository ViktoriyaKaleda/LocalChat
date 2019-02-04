using DevExpress.Mvvm;
using LocalChat.Service;
using LocalChat.Wpf.ViewModels;
using LocalChat.Wpf.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace LocalChat.Wpf
{
    public class MainViewModel : BindableBase
    {
        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        public string CurrentMessage
        {
            get { return GetProperty(() => CurrentMessage); }
            set { SetProperty(() => CurrentMessage, value); }
        }

        private LocalChatClient _localChat;
        private Thread _listeningThread;

        public ICommand WindowLoadedCommand
            => new DelegateCommand(async () =>
            {
                var view = new LoginDialog
                {
                    DataContext = new LoginViewModel()
                };

                var username = (string) await DialogHost.Show(view, "RootDialog");

                _localChat = new LocalChatClient(username);

                _localChat.SendSystemMessage($"{username} joined the chat.");

                _localChat.MessageReceive += OnMessageReceived;

                _listeningThread = new Thread(new ThreadStart(_localChat.Listen));
                _listeningThread.Start();
            });

        public ICommand SendMessage
            => new DelegateCommand(() =>
            {
                if (string.IsNullOrEmpty(CurrentMessage) || string.IsNullOrWhiteSpace(CurrentMessage))
                    return;

                _localChat.SendUserMessage(CurrentMessage);

                CurrentMessage = "";
            });

        public ICommand WindowClosingCommand
            => new DelegateCommand(() => Environment.Exit(0));

        public void OnMessageReceived(object sender, MessageReceiveEventArgs messageReceiveEventArgs)
        {
            System.Windows.Application.Current.Dispatcher
                .Invoke(() => Messages.Add(messageReceiveEventArgs.Message));            
        }
    }
}
