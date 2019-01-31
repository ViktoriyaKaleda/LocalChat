using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LocalChat.Service
{
    public class LocalChatClient : ILocalChatClient
    {
        private readonly UdpClient _udpClient;
        private readonly IPAddress _ipAddress;
        private readonly IPEndPoint _ipEndPoint;
        private string UserName { get; set; }

        public event EventHandler<MessageReceiveEventArgs> MessageReceive;

        public LocalChatClient(string userName)
        {
            _ipAddress = IPAddress.Parse("239.0.0.222"); // one of the reserved for local needs UDP addresses
            _udpClient = new UdpClient();
            _udpClient.JoinMulticastGroup(_ipAddress);
            _ipEndPoint = new IPEndPoint(_ipAddress, 2222);
            UserName = userName;
        }

        public void SendUserMessage(string message)
        {
            Byte[] buffer = Encoding.UTF8.GetBytes($"{this.UserName}: {message}");
            _udpClient.Send(buffer, buffer.Length, _ipEndPoint);
        }

        public void SendSystemMessage(string message)
        {
            Byte[] buffer = Encoding.UTF8.GetBytes(message);
            _udpClient.Send(buffer, buffer.Length, _ipEndPoint);
        }

        public void Listen()
        {
            UdpClient client = new UdpClient
            {
                ExclusiveAddressUse = false
            };
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 2222);

            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false;

            client.Client.Bind(localEP);

            client.JoinMulticastGroup(_ipAddress);

            string formatted_data;

            while (true)
            {
                Byte[] data = client.Receive(ref localEP);
                formatted_data = Encoding.UTF8.GetString(data);
                MessageReceive.Invoke(this, new MessageReceiveEventArgs { Message = new Message { Text = formatted_data, CreatedDate = DateTime.Now } });
            }
        }
    }
}
