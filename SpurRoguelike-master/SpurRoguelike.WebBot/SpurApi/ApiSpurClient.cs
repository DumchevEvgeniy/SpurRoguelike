using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot {
    public class ApiSpurClient {
        private readonly String HostName = "http://e03078:666/";
        private readonly HttpClient httpClient;

        public ApiSpurClient() {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Spur-Token", "qlpqxtul1");
        }

        public async Task<LevelViewInfo> ExecutePostAsync(String apiAction) {
            var response = await httpClient.PostAsync($"{HostName}{apiAction}", new StringContent("qlpqxtul"));
            return await TryParseResponseAsync<LevelViewInfo>(response);
        }

        public async Task<LevelViewInfo> ExecuteGetAsync(String apiAction) {
            var response = await httpClient.GetAsync($"{HostName}{apiAction}");
            return await TryParseResponseAsync<LevelViewInfo>(response);
        }

        private async static Task<T> TryParseResponseAsync<T>(HttpResponseMessage response) where T : class {
            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = await response.Content.ReadAsStringAsync();
            if (String.IsNullOrEmpty(responseBody))
                return null;
            try {
                return JsonConvert.DeserializeObject<T>(responseBody);
            }
            catch {
                return null;
            }
        }
    }
}
