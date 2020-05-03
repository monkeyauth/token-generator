using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace token_generator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                Console.WriteLine("***********************************************");
                Console.WriteLine("******** Monkey Auth - Token Generator ********");
                Console.WriteLine("***********************************************");

                Console.Write("Enter Client ID: ");
                var clientId = Console.ReadLine();

                Console.Write("Enter Client Secret: ");
                var clientSecret = Console.ReadLine();

                Console.Write("Enter API Scope: ");
                var apiScope = Console.ReadLine();

                var disco = await client.GetDiscoveryDocumentAsync(
                    new DiscoveryDocumentRequest
                    {
                        Address = "http://monkeyauth.com",
                        Policy = new DiscoveryPolicy
                        {
                            RequireHttps = false
                        }
                    });

                if (disco.IsError)
                {
                    Console.WriteLine($"Discovery error: {disco.Error}");
                    return;
                }

                // request token
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = clientId,
                        ClientSecret = clientSecret,
                        Scope = apiScope
                    });

                if (tokenResponse.IsError)
                {
                    Console.WriteLine($"Token error: {tokenResponse.Error}");
                    return;
                }

                Console.WriteLine("Bearer token: ");
                Console.WriteLine(tokenResponse.Json);
            }
        }
    }
}
