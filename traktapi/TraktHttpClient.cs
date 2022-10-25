using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using traktapi.Model;

namespace traktapi
{
    public class TraktHttpClient : HttpClient
    {
        private const string CLIENTID = "cb4ea5ef732804fd6df560218b1bbf08e0933b50e1e21545fcb71f3d61cbfeb7";
        private const string CLIENTSECRET = "8c543f6fed7d9caf258b8bea1d1a62d1b3845a3e410001e176a17fa50e8f7800";
        private const string BASEURI = "https://api.trakt.tv";
        private const string TOKENFILE = "myToken.txt";
        private TraktAccessToken token;

        public TraktHttpClient() : base()
        {
            BaseAddress = new Uri(BASEURI);
            DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");
            DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", CLIENTID);
        }

        public async Task<T> GetPublicAsync<T>(string path)
        {
            var response = await GetAsync(path);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(json);

            return result;
        }

        public async Task<T> GetPrivateAsync<T>(string path)
        {
            await EnsureTraktToken();
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Access_token);

            var response = await GetAsync(path);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(json);

            return result;
        }

        public async Task<T> PostPrivateAsync<T, G>(string path, G obj)
        {
            await EnsureTraktToken();
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Access_token);

            var response = await this.PostAsJsonAsync(path, obj);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }

        private async Task EnsureTraktToken()
        {
            if (null != token && !token.IsExpired)
                return;

            if (File.Exists(TOKENFILE))
            {
                var content = File.ReadAllText(TOKENFILE);
                token = JsonConvert.DeserializeObject<TraktAccessToken>(content);
                if (!token.IsExpired)
                    return;
            }

            if (null != token && token.IsExpired)
                await RefreshToken();
            else
                await GetToken();
        }

        private async Task RefreshToken()
        {
            TraktRefreshTokenRequest request = new TraktRefreshTokenRequest()
            {
                refresh_token = token.Refresh_token,
                client_id = CLIENTID,
                client_secret = CLIENTSECRET
            };

            var response = await this.PostAsJsonAsync("/oauth/device/token", request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<TraktAccessToken>(json);
        }

        private async Task GetToken()
        {
            var code = await GetCode();
            Console.WriteLine($"Please visit {code.Verification_url} and enter the following Code: {code.User_code}");

            CancellationTokenSource cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(code.Expires_in));

            while (true)
            {
                token = await PollAccessToken(code.Device_code);

                if (null != token)
                    break;

                Task task = Task.Delay(TimeSpan.FromSeconds(code.Interval), cancellation.Token);
                try
                {
                    await task;
                }
                catch (TaskCanceledException)
                {
                    throw new Exception("Error in Trakt authentication");
                }

            }

            File.WriteAllText(TOKENFILE, JsonConvert.SerializeObject(token));
        }

        private async Task<TraktAccessToken> PollAccessToken(string deviceCode)
        {
            TraktGetTokenRequest request = new TraktGetTokenRequest()
            {
                code = deviceCode,
                client_id = CLIENTID,
                client_secret = CLIENTSECRET
            };

            var response = await this.PostAsJsonAsync("/oauth/device/token", request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
                return null;

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TraktAccessToken>(json);

            return result;
        }

        private async Task<TraktGetCodeResponse> GetCode()
        {
            TraktGetCodeRequest request = new TraktGetCodeRequest() { client_id = CLIENTID };

            var response = await this.PostAsJsonAsync("/oauth/device/code", request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<TraktGetCodeResponse>(json);

            return result;
        }
    }
}
