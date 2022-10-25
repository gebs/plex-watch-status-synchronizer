using CommandLine;
using plexapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using traktapi;
using traktapi.Model;

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
                    PlexSynchronizer synchronizer = new PlexSynchronizer(o.PlexToken, o.PlexUsername, o.PlexPassword, o.WaitTime);
                    Console.WriteLine(DateTime.Now + " - Starting synchronization");
                    if (o.MediaType == MediaType.Movies || o.MediaType == MediaType.All)
                    {
                        List<PlexMovie> traktwatchedMovies = null;

                        if (!string.IsNullOrEmpty(o.TraktUsername))
                        {
                            var tmp = await traktClient.GetWatchedMedia(type: "movies");
                            traktwatchedMovies = tmp.Select(x => new PlexMovie(x.Movie.Ids.Imdb, x.Movie.Title)).ToList();
                        }
                        await synchronizer.Synchronize(o.DryRun, (v, s) => new PlexMovie(v, s), traktwatchedMovies);

                        var watchedOnlyPlex = await synchronizer.GetTraktDelta((v, s) => new PlexMovie(v, s), traktwatchedMovies);
                        var traktIds = watchedOnlyPlex.Select(x => new TraktMovie() { Ids = new TraktIds() { Imdb = x.Id } }).Take(2).ToList();

                        await traktClient.SetMoviesWatched(traktIds);
                    }

                    if (o.MediaType == MediaType.Shows || o.MediaType == MediaType.All)
                    {
                        List<PlexEpisode> traktwatchedEpisodes = null;
                        if (!string.IsNullOrEmpty(o.TraktUsername))
                        {
                            var tmp = await traktClient.GetWatchedMedia(type: "episodes");
                            traktwatchedEpisodes = tmp.Select(x => new PlexEpisode(x.Show.Ids.Tvdb, x.Episode.Title, x.Episode.Season ?? 0, x.Episode.Number, x.Show.Title)).ToList();
                        }
                           await synchronizer.Synchronize(o.DryRun, (v, s) => new PlexEpisode(v, s), traktwatchedEpisodes);

                        var watchedOnlyPlex = await synchronizer.GetTraktDelta((v, s) => new PlexEpisode(v, s), traktwatchedEpisodes);

                        List<TraktShow> shows = BuildTraktShowTree(watchedOnlyPlex);

                        await traktClient.SetShowsWatched(shows.Take(2).ToList());
                    }
                    Console.WriteLine(DateTime.Now + " - Finished synchronization");
                });
        }

        static List<TraktShow> BuildTraktShowTree(List<PlexEpisode> episodes)
        {
            List<TraktShow> shows = new List<TraktShow>();
            foreach (var item in episodes)
            {
                if (shows.Any(x => x.Ids.Tvdb == item.Id))
                {
                    var show = shows.FirstOrDefault(x => x.Ids.Tvdb == item.Id);
                    if (show.Seasons.Any(x => x.Number == item.Season))
                    {
                        var season = show.Seasons.FirstOrDefault(x => x.Number == item.Season);
                        season.Episodes.Add(new TraktEpisode(item.Episode));
                    }
                    else
                    {
                        show.Seasons.Add(new TraktSeason(item.Season, item.Episode));
                    }
                }
                else
                {
                    shows.Add(new TraktShow(item.Title, item.Id, item.Season, item.Episode));
                }
            }
            return shows;
        }

    }
}
