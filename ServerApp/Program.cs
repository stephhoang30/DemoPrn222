// Server
using System.Net;
using System.Net.Sockets;
using System.Text;

try
{
    // create server
    TcpListener server = new TcpListener(IPAddress.Any, 5000);
    server.Start();
    Console.WriteLine("Server is running!");

    // ready listen connect from any client
    while (true)
    {
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Client connected");


        // ready a line stream to store data between client and server
        NetworkStream stream = client.GetStream();    

        // receive message from client
        byte[] buffer = new byte[2048];
        int bytesRead;
        string message;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            if(message.Trim().ToLower() == "exit")
            {
                break;
            }
            Console.WriteLine("Receive from client: " + message);
        }

        // send message to client

    }
}
catch (Exception ex)
{
    Console.WriteLine("Server fail:" + ex.Message);
}