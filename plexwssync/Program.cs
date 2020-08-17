using CommandLine;
using plexapi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using traktapi;

namespace plexwssync
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsedAsync(async o =>
                {
                    TraktClient traktClient = new TraktClient(o.TraktUsername);
                    PlexSynchronizer synchronizer = new PlexSynchronizer(o.PlexToken, o.PlexUsername, o.PlexPassword);

                    if (o.MediaType == MediaType.Movies || o.MediaType == MediaType.All)
                    {
                        List<PlexMovie> traktwatchedMovies = null;

                        if (!string.IsNullOrEmpty(o.TraktUsername))
                        {
                            var tmp = await traktClient.GetWatchedMedia(type: "movies");
                            traktwatchedMovies = tmp.Select(x => new PlexMovie(x.Movie.Ids.Imdb, x.Movie.Title)).ToList();
                        }
                        await synchronizer.Synchronize(o.DryRun, (v, s) => new PlexMovie(v, s), traktwatchedMovies);
                    }

                    if (o.MediaType == MediaType.Shows || o.MediaType == MediaType.All)
                    {
                        List<PlexEpisode> traktwatchedEpisodes = null;
                        if (!string.IsNullOrEmpty(o.TraktUsername))
                        {
                            var tmp = await traktClient.GetWatchedMedia(type: "episodes");
                            traktwatchedEpisodes = tmp.Select(x => new PlexEpisode(x.Show.Ids.Tvdb, x.Episode.Title, x.Episode.Season, x.Episode.Number, x.Show.Title)).ToList();
                        }
                        await synchronizer.Synchronize(o.DryRun, (v, s) => new PlexEpisode(v, s), traktwatchedEpisodes);
                    }
                });
        }

    }
}
