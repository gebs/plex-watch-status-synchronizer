using System;
using System.Collections.Generic;
using System.Text;

namespace traktapi.Model
{
    public class TraktGetTokenRequest
    {
        public string code { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }

    }
}
