namespace LocalChat.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Enter your username please: ");
            string username = System.Console.ReadLine();

            System.Console.WriteLine("Let's start!");
            new LocalChatConsoleManager(username).StartChating();            
        }        
    }
}
