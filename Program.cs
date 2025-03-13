using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        Console.WriteLine("Server started on port 5000.");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("New client connected.");

            // Start a new task for each client (concurrent handling)
            Task.Run(() => HandleClient(client));
        }
    }

    static void HandleClient(TcpClient client)
    {
        using StreamReader reader = new StreamReader(client.GetStream());
        using StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };

        while (true)
        {
            try
            {
                // Receive command from client
                string command = reader.ReadLine();
                Console.WriteLine($"Received command: {command}");

                writer.WriteLine("Input numbers");

                // Receive numbers from client
                string numbers = reader.ReadLine();
                Console.WriteLine($"Received numbers: {numbers}");

                string[] parts = numbers.Split(' ');
                int num1 = int.Parse(parts[0]);
                int num2 = int.Parse(parts[1]);
                string result = "";

                switch (command.ToLower())
                {
                    case "random":
                        Random rnd = new Random();
                        int randomNumber = rnd.Next(Math.Min(num1, num2), Math.Max(num1, num2) + 1);
                        result = randomNumber.ToString();
                        break;
                    case "add":
                        result = (num1 + num2).ToString();
                        break;
                    case "subtract":
                        result = (num1 - num2).ToString();
                        break;
                    default:
                        result = "Unknown command";
                        break;
                }

                writer.WriteLine(result);
                Console.WriteLine($"Sent result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client disconnected. {ex.Message}");
                break;
            }
        }
        client.Close();
    }
}