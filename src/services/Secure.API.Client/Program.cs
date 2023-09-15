using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Secure.API.Client;

// var config = AuthConfig.ReadFromJsonFile("appsettings.json");

await RunAsync();

async Task RunAsync()
{
    var config = AuthConfig.ReadFromJsonFile("appsettings.json");

    var app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
        .WithClientSecret(config.ClientSecret)
        .WithAuthority(new Uri(config.Authority))
        .Build();

    var resourceIds = new[] { config.ResourceID };

    try
    {
        var result = await app.AcquireTokenForClient(resourceIds).ExecuteAsync();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Token acquired \n");
        Console.WriteLine(result.AccessToken);
        Console.ResetColor();

        if (!string.IsNullOrEmpty(result.AccessToken))
        {
            var httpClient = new HttpClient();
            var defaultRequestHeaders = httpClient.DefaultRequestHeaders;

            if (defaultRequestHeaders.Accept.All(m => m.MediaType != "application/json"))
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
            }

            defaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", result.AccessToken);

            var response = await httpClient.GetAsync(config.BaseAddress);
            var json = await response.Content.ReadAsStringAsync();

            string content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(json);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to call the Web Api: {response.StatusCode}");
                Console.WriteLine($"Content: {content}");
            }

            Console.ResetColor();
        }
    }
    catch (MsalClientException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}