using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PersonalWebsite.Interfaces;
using PersonalWebsite.Models.DataObjects;

namespace PersonalWebsite.Services
{
    public class RepositoryDownloaderService(IHttpClientService httpClientService, IMemoryCache memoryCache) : IRepositoryDownloaderService
    {
        public async Task<List<Repositories>?> DownloadAllRepositories()
        {
            const string cacheKey = "repositories";
            if (memoryCache.TryGetValue(cacheKey, out List<Repositories>? cachedRepositories))
            {
                return cachedRepositories;
            }

            string url = "https://api.github.com/users/tyreyn/repos";
            var response = await httpClientService.GetAsync(url);

            if (response == null) return null;

            List<Repositories> repositories = JsonConvert.DeserializeObject<List<Repositories>>(response) ?? [];

            memoryCache.Set(cacheKey, repositories, TimeSpan.FromDays(1));

            return repositories;
        }

        public async Task<string?> DownloadReadme(string projectName)
        {
            var url = $"https://raw.githubusercontent.com/Tyreyn/{projectName}/main/README.md";
            return await httpClientService.GetAsync(url);
        }

        public async Task<IList<string>> DownloadLanguages(string url)
        {
            var response = await httpClientService.GetAsync(url);
            if (response == null) return [];

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(response);

            return dictionary?.Keys.ToList() ?? [];
        }
    }
}
