using System.Collections.Generic;
using System.Threading.Tasks;
using traktapi.Model;

namespace traktapi
{
    public class TraktClient
    {
        private readonly string username;
        private readonly TraktHttpClient httpClient;


        public TraktClient(string username)
        {
            this.username = username;
            httpClient = new TraktHttpClient();
        }

        public async Task<List<TraktMedia>> GetWatchedMedia(string type)
        {
            return await httpClient.GetPublicAsync<List<TraktMedia>>($"/users/{username}/history/{type}?page=1&limit=999999");
        }

        private async Task SetWatched(List<TraktMovie> movies = null, List<TraktShow> shows = null)
        {
            var request = new TraktSetWatchedRequest() { Movies = movies, Shows = shows };

            await httpClient.PostPrivateAsync<string, TraktSetWatchedRequest>("sync/history", request);

        }
        public Task SetMoviesWatched(List<TraktMovie> movies) => SetWatched(movies: movies);

        public Task SetShowsWatched(List<TraktShow> shows) => SetWatched(shows: shows);

    }
}
