using PersonalWebsite.Interfaces;
using System.Net.Http.Headers;

namespace PersonalWebsite.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient? _httpClient;
        public HttpClientService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            var token = configuration["GithubToken"];
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["GithubToken"]);
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp/1.0");
            }
        }

        public async Task<string> GetAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"URL: {url} - STATUS {response.StatusCode}");
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException httpReqExc)
            {
                Console.WriteLine($"URL: {url} - STATUS {httpReqExc.StatusCode}");
                return null;
            }
        }
    }
}
