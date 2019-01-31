namespace LocalChat.Service
{
    public interface ILocalChatClient
    {
        void SendUserMessage(string message);

        void SendSystemMessage(string message);

        void Listen();
    }
}
