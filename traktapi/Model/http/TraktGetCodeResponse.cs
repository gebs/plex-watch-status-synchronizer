namespace traktapi.Model
{
    public class TraktGetCodeResponse
    {
        public string Device_code { get; set; }
        public string User_code { get; set; }
        public string Verification_url { get; set; }
        public int Expires_in { get; set; }
        public int Interval { get; set; }
    }
}
