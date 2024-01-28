using Grpc.Net.Client;
using System.Threading.Channels;
using TimeServerClient;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Press any key to continue... and ESC to exit");
        Console.ReadLine();

        using var notSecureChannel = GrpcChannel.ForAddress("http://localhost:5000");
        using var secureChannel = GrpcChannel.ForAddress("https://localhost:5001");

        var client = new TimeService.TimeServiceClient(notSecureChannel);
        var secureClient = new TimeService.TimeServiceClient(secureChannel);

        var reply = await client.GetCurrentTimeAsync(new TimeRequest { });

        Console.WriteLine("Press 1 -> To Log Time Entry & 2 -> To Retrieve Time Entries");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    await CallGetCurrentTimeAsync(client);
                    break;
                case ConsoleKey.D2:
                    await RetrieveTimesSecurely(secureClient);
                    break;
                case ConsoleKey.Escape:
                    Console.WriteLine("Exiting the console app.");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose 1, 2, or ESC.");
                    break;
            }
        }

    }

    private static async Task RetrieveTimesSecurely(TimeService.TimeServiceClient secureClient)
    {
        Console.WriteLine("Gathering all of the times");

        var reply = await secureClient.QueryTimeDatabaseAsync(new Certificate {});

        Console.WriteLine($"{reply}");
    }

    private static async Task CallGetCurrentTimeAsync(TimeService.TimeServiceClient client)
    {
        Console.WriteLine("Logging the time...");

        var reply = await client.GetCurrentTimeAsync(new TimeRequest { });

        Console.WriteLine($"{reply}");
    }
}