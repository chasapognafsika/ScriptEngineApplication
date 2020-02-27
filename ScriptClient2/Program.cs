using NLog;
using System;
using System.Threading.Tasks;
using WebApp;

namespace ScriptClient
{
    class Program
    {
        static string clientUrl = ""; 
        static string serverUrl = "";
        static int Main(string[] args)
        {
            if (args.Length != 4 || args[0] != "-listen" || args[2] != "-connect") {
                return 1;
            }
            clientUrl = args[1];
            serverUrl = args[3];

            using (Microsoft.Owin.Hosting.WebApp.Start<Startup>(clientUrl))
            {
                var response = RegisterClient();
                Console.WriteLine($"registration status: {response.Status}");
                Console.WriteLine("Waiting for server to send commands");
                Console.WriteLine("Press <ENTER> twice to terminate this client");
                Console.ReadLine();
                Console.ReadLine();
            }
            Console.WriteLine("WebApp disposed.");
            Console.ReadLine();

            return 0;
        }

        static async Task<string> RegisterClient()
        {
            Console.WriteLine($"connecting to server: {serverUrl}");
            RestClient restclient = new RestClient(serverUrl);
            Console.WriteLine($"registering this client to server: {serverUrl}");
            var response = await restclient.PostAsync<string, RequestViewModel>(
                "api/serveclient/Register", 
                new RequestViewModel() {url = clientUrl });
            return response;
        }
    }

    public class RequestViewModel
    {
        public string url { get; set; }
    }
}



