using System;
using System.Collections.Generic;
using System.Text;

namespace traktapi.Model
{
    public class TraktRefreshTokenRequest
    {
        public string refresh_token { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; } = "refresh_token";
    }
}
