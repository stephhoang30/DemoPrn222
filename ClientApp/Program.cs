// Client
using System.Net.Sockets;
using System.Text;

try
{
    using (TcpClient client = new TcpClient("localhost",5000))
    {
        NetworkStream stream = client.GetStream();
        Console.WriteLine("Client is connected to server");

        // send message to server
        while (true)
        {
            Console.Write("Enter message: ");
            string message = Console.ReadLine();

            if(message.Trim().ToLower() == "exit")
            {
                break;
            }
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
            Console.WriteLine("Sent to server: " + message);
        }

        // receive message from server

    }
}
catch (Exception ex)
{
    Console.WriteLine("Client fail:" + ex.Message);
}