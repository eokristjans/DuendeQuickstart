using IdentityModel.Client;
using System.Text.Json;

var client = new HttpClient();
// Discover endpoints from metadata
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    Console.ReadKey();
    return;
}

// Use the information from the discovery document to request a token from IdentityServer to access api1:
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,

    ClientId = "client",
    ClientSecret = "secret",
    Scope = "api1"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Console.ReadKey();
    return;
}

// Copy and paste the access token from the console to https://jwt.ms to inspect the raw token.
Console.WriteLine(tokenResponse.AccessToken);
Console.WriteLine();

// To send the access token to the API you typically use the HTTP Authorization header.
// This is done using the SetBearerToken extension method:
var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);

var response = await apiClient.GetAsync("https://localhost:44394/identity");
if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}
Console.ReadKey();
