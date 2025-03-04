using Newtonsoft.Json;
using PersonalWebsite.Models.DataObjects;
using System.Net.Http.Headers;

namespace PersonalWebsite.Services.GithubApi
{
    public class RepositoryDownloaderService
    {
        public async Task<List<Repositories>> DownloadAllRepositories()
        {
            string url = "https://api.github.com/users/tyreyn/repos";
            string response = await GetResponseFromApi(url);
            if (response == null) return null;
            List<Repositories> readmeObject = JsonConvert.DeserializeObject<List<Repositories>>(response);

            if (readmeObject.Count >= 0)
            {
                return readmeObject;
            }

            return null;
        }

        public async Task<string> DownloadReadme(string projectName)
        {
            string url = string.Format("https://raw.githubusercontent.com/Tyreyn/{0}/main/README.md", projectName);
            string response = await GetResponseFromApi(url);
            return response;
        }

        private async Task<string> GetResponseFromApi(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                var response = await client.GetAsync(url);
                try
                {
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine($"URL: {url} - STATUS {response.StatusCode}");
                }
                catch (HttpRequestException httpReqExc)
                {
                    Console.WriteLine($"URL: {url} - STATUS {httpReqExc.StatusCode}");
                    return null;
                }

                return await response.Content.ReadAsStringAsync();

            }

            return null;

        }
    }
}
