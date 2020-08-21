using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using traktapi.Model;

namespace traktapi
{
    public class TraktClient
    {
        private readonly string username;

        public TraktClient(string username)
        {
            this.username = username;
        }

        public async Task<List<TraktMedia>> GetWatchedMedia(string type)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");
            client.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "cb4ea5ef732804fd6df560218b1bbf08e0933b50e1e21545fcb71f3d61cbfeb7");

            Console.WriteLine($"{DateTime.Now} - Reading watched history from trakt");

            var response = await client.GetAsync($"https://api.trakt.tv/users/{username}/history/{type}?page=1&limit=999999");//
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<TraktMedia>>(json);

            Console.WriteLine($"{DateTime.Now} - Found {result.Count} entries in trakt history");
            return result;
        }
    }
}
