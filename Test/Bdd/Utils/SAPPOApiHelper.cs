// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SAPPOApiHelper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using global::Bdd.Core.Utils;
    using Newtonsoft.Json.Linq;

    public static class SapPoApiHelper
    {
        private static readonly NameValueCollection Settings = ConfigurationManager.GetSection("SAPPOApi") as NameValueCollection;

        public static async Task<string> PostWithJwtAsync(string endpoint, string content)
        {
            var result = await TokenGenerateAsync(Settings.GetValue("TokenEndpoint").Replace("~", Settings.GetValue("TenantId"))).ConfigureAwait(false);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result["access_token"].ToString());
                var data = new StringContent(content, Encoding.UTF8, "application/json");
                Uri uri = new Uri(endpoint);
                var response = await client.PostAsync(uri, data).ConfigureAwait(false);
                data.Dispose();
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        public static async Task<JObject> TokenGenerateAsync(string endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", Settings.GetValue("grant-type") },
                    { "client_id", Settings.GetValue("ClientId") },
                    { "client_secret", Settings.GetValue("ClientSecret") },
                    { "scope", Settings.GetValue("Scope") },
                });
                Uri uri = new Uri(endpoint);
                HttpResponseMessage response = await client.PostAsync(uri, content).ConfigureAwait(false);
                content.Dispose();
                return JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }
    }
}