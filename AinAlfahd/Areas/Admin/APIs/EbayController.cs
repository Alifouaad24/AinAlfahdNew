using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EbayController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public EbayController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var clientId = "SaifSami-AinAlFhd-PRD-48e894d8b-1178e54d"; // مفتاحك
        var clientSecret = "PRD-8e9408e1a850-a655-4fb7-906b-9a28"; // مفتاحك

        var credentials = $"{clientId}:{clientSecret}";
        var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);

        var content = new FormUrlEncodedContent(new[]
        {
        new KeyValuePair<string, string>("grant_type", "client_credentials"),
        new KeyValuePair<string, string>("scope", "https://api.ebay.com/oauth/api_scope")
    });

        var response = await _httpClient.PostAsync("https://api.ebay.com/identity/v1/oauth2/token", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get token: {response.StatusCode} - {error}");
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = System.Text.Json.JsonDocument.Parse(json);
        var accessToken = doc.RootElement.GetProperty("access_token").GetString();

        return accessToken;
    }


    [HttpGet("search/{query}")]
    public async Task<IActionResult> SearchItems(string query)
    {
        var accessToken = await GetAccessTokenAsync();

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://api.ebay.com/buy/browse/v1/item_summary/search?q={Uri.EscapeDataString(query)}"
        );
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("X-EBAY-C-MARKETPLACE-ID", "EBAY_US");

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        return Ok(json);
    } 
}
