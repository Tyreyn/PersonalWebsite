using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json;
using PersonalWebsite.Interfaces;
using PersonalWebsite.Models.DataObjects;
using System;
using System.Net.Http.Headers;

namespace PersonalWebsite.Services
{
    public class RepositoryDownloaderService : IRepositoryDownloaderService
    {
        private readonly IHttpClientService _httpClientService;

        public RepositoryDownloaderService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<List<Repositories>> DownloadAllRepositories()
        {
            string url = "https://api.github.com/users/tyreyn/repos";
            string response = await _httpClientService.GetAsync(url);

            if (response == null) return null;

            List<Repositories> repositories = JsonConvert.DeserializeObject<List<Repositories>>(response);
            return repositories ?? new List<Repositories>();
        }

        public async Task<string> DownloadReadme(string projectName)
        {
            string url = $"https://raw.githubusercontent.com/Tyreyn/{projectName}/main/README.md";
            return await _httpClientService.GetAsync(url);
        }

        public async Task<IList<string>> DownloadLanguages(string url)
        {
            string response = await _httpClientService.GetAsync(url);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(response);

            return dictionary?.Keys.ToList() ?? new List<string>();
        }
    }
}
