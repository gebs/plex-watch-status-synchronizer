using plexapi.Model.XML.Resources;
using System.Collections.Generic;
using System.Linq;

namespace plexapi
{
    public class PlexMediaServer
    {
        public string Name { get; private set; }
        private readonly string address;
        private readonly int port;
        public string Token { get; private set; }

        public string BaseUrl { get => $"http://{address}:{port}"; }

        public PlexMediaServer(Device container)
        {
            this.Name = container.Name;
            this.address = container.PublicAddress;
            this.port = container.Connection.FirstOrDefault(x => !x.Local).Port;
            this.Token = container.AccessToken;
        }


    }
}
