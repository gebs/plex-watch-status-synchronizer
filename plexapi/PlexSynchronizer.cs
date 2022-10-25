using Newtonsoft.Json;
using plexapi.Model.XML.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace plexapi
{
    public class PlexSynchronizer
    {
        private string token;
        private readonly string username;
        private readonly string password;
        private readonly int waittime;
        private readonly PlexMediaComparer mediaComparer;
        private List<PlexMedia> watchedOnlyPlex;

        public PlexSynchronizer(string token, string username, string password, int waittime)
        {
            this.token = token;
            this.username = username;
            this.password = password;
            this.waittime = waittime;
            mediaComparer = new PlexMediaComparer();

            //Disable Server Certificate Validation
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public async Task<bool> Synchronize<T>(bool dryrun, Func<Video, PlexMediaServer, T> add, List<T> traktWatchedMedia = null) where T : PlexMedia
        {
            var (all, watched) = await GetPlexMediaLists(add, traktWatchedMedia);

            watchedOnlyPlex = watched.Except(traktWatchedMedia, mediaComparer).ToList();

            var unwatched = all.Where(x => !x.Watched).ToList();

            var media2sync = unwatched.Intersect(watched, mediaComparer).ToList();

            Console.WriteLine($"{DateTime.Now} - Found Total: {all.Count}, watched: {watched.Count}, unwatched: {unwatched.Count}, tosync: {media2sync.Count}");

            foreach (var item in media2sync)
            {
                if (dryrun)
                    Console.WriteLine($"{DateTime.Now} - {item.Title} would be set watched on Server {item.MediaServer.Name}");
                else
                {
                    Console.WriteLine($"{DateTime.Now} - Setting {item.Title} watched on server {item.MediaServer.Name}");

                    await item.SetWatched();

                    Thread.Sleep(waittime);
                }
            }

            return true;
        }

        public async Task<List<T>> GetTraktDelta<T>(Func<Video, PlexMediaServer, T> add, List<T> traktWatchedMedia) where T : PlexMedia
        {
            //if (null == watchedOnlyPlex)
            //{
            var (_, watched) = await GetPlexMediaLists(add, traktWatchedMedia);
            return watched.Except(traktWatchedMedia, mediaComparer).Select(x => (T)x).ToList();
            // watchedOnlyPlex = watched.Except(traktWatchedMedia, mediaComparer).Select(x => (T)x).ToList();
            //}

            //  return watchedOnlyPlex;
        }

        private async Task<(List<T> all, List<T> watched)> GetPlexMediaLists<T>(Func<Video, PlexMediaServer, T> add, List<T> traktWatchedMedia = null) where T : PlexMedia
        {
            await EnsurePlexToken();

            Console.WriteLine($"{DateTime.Now} - Reading Media from Plex");
            var all = await GetPlexMedia(add);
            var tmp = all.Where(x => x.Watched).ToList();

            if (traktWatchedMedia != null)
                tmp.AddRange(traktWatchedMedia);

            var watched = tmp.Distinct(mediaComparer).Select(x => (T)x).ToList();

            return (all, watched);
        }

        private async Task EnsurePlexToken()
        {
            if (!string.IsNullOrEmpty(token))
                return;
            else if (string.IsNullOrEmpty(token) && (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)))
                throw new Exception("No Token supplied");


            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Plex-Client-Identifier", Guid.NewGuid().ToString());
            client.DefaultRequestHeaders.Add("X-Plex-Product", "plexwssync");
            client.DefaultRequestHeaders.Add("X-Plex-Version", "0.1");

            var formVariables = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("user[login]", username),
                new KeyValuePair<string, string>("user[password]", password)
            };
            var formContent = new FormUrlEncodedContent(formVariables);

            var response = await client.PostAsync("https://plex.tv/users/sign_in.json", formContent);
            response.EnsureSuccessStatusCode();

            dynamic json = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            token = json.user.authToken;

            Console.WriteLine("A new plex token has been issued. Use this token for your next run (-t " + token + ")");
        }

        private async Task<List<T>> GetPlexMedia<T>(Func<Video, PlexMediaServer, T> add)
        {
            List<T> result = new List<T>();

            foreach (var server in (await PlexRequest("https://plex.tv", "/api/resources?includeHttps=1", token)).Device)
            {
                if (!server.Connection.Any(x => !x.Local))
                    continue;

                PlexMediaServer mediaServer = new PlexMediaServer(server);

                foreach (var library in (await PlexRequest(mediaServer.BaseUrl, "/library/sections", mediaServer.Token)).Directory)
                {
                    if ((library.Type == "show" && typeof(T) == typeof(PlexEpisode))
                        || (library.Type == "movie" && typeof(T) == typeof(PlexMovie)))
                    {
                        string type = "";
                        if (library.Type == "show")
                            type = "?type=4";

                        var tmp = await PlexRequest(mediaServer.BaseUrl, $"/library/sections/{library.Key}/all{type}", mediaServer.Token);
                        result.AddRange(tmp.Video.Select(x => add(x, mediaServer)));
                    }
                }
            }
            return result;
        }

        private async Task<MediaContainer> PlexRequest(string baseUrl, string path, string token)
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync($"{baseUrl}{path}{(path.Contains("?") ? "&" : "?")}X-Plex-Token={token}");
            response.EnsureSuccessStatusCode();

            return Utilities.DeserializeXml<MediaContainer>(await response.Content.ReadAsStringAsync());
        }
    }
}
