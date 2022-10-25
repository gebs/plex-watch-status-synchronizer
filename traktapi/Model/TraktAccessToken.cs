using System;

namespace traktapi.Model
{
    public class TraktAccessToken
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }
        public string Refresh_token { get; set; }
        public string Scope { get; set; }
        public int Created_at { get; set; }

        public bool IsExpired
        {
            get
            {
                return TraktUtil.UnixTimeStampToDateTime(Created_at).AddSeconds(Expires_in) < DateTime.Now;
            }
        }
    }
}
