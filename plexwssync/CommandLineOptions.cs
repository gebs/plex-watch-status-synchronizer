using CommandLine;

namespace plexwssync
{
    public class CommandLineOptions
    {
        [Option('m', "MediaType", HelpText = "What should be synchronized, allowed Values are Movies, Shows or All", Required = true)]
        public MediaType MediaType { get; set; }

        [Option('t', "PlexToken", HelpText = "Your plex token, if none is available you can use Username/Password to generate one", Required = false)]
        public string PlexToken { get; set; }

        [Option('u', "PlexUsername", HelpText = "If you don't have a Plex Token you can use Username/Password to generate one", Required = false)]
        public string PlexUsername { get; set; }

        [Option('p', "PlexPassword", HelpText = "If you don't have a Plex Token you can use Username/Password to generate one", Required = false)]
        public string PlexPassword { get; set; }

        [Option('d', "DryRun", HelpText = "Shows you what would be changed without changing anything", Required = false)]
        public bool DryRun { get; set; }

        [Option('q', "TraktUsername", HelpText = "To use Trakt as an additional Source for watche statuses you can provide a Trakt slug (Url encoded Username)", Required = false)]
        public string TraktUsername { get; set; }

    }
}
