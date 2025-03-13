using System;
using System.IO;
using System.Net.Sockets;

class Program
{
    static void Main()
    {
        Console.WriteLine("TCP Client started.");
        using TcpClient client = new TcpClient("127.0.0.1", 5000);
        using StreamReader reader = new StreamReader(client.GetStream());
        using StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };

        for (int i = 0; i < 3; i++)
        {
            // Ask the user to select a method
            Console.WriteLine("Select method (Random, Add, Subtract):");
            string command = Console.ReadLine();

            // Send method to the server
            writer.WriteLine(command);

            // Receive response from server ("Input numbers")
            string response = reader.ReadLine();
            Console.WriteLine($"Server: {response}");

            // Ask the user to enter two numbers
            Console.WriteLine("Enter two numbers separated by a space (e.g., 5 10):");
            string numbers = Console.ReadLine();

            // Send numbers to the server
            writer.WriteLine(numbers);

            // Receive result from the server
            string result = reader.ReadLine();
            Console.WriteLine($"Server responded with result: {result}");
        }

        Console.WriteLine("Finished.");
    }
}