// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FICORuleshelper.cs" company="Microsoft">
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
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public static class FicoRuleshelper
    {
        public static async Task<string> PosttoFetchTokenAsync(string endpoint, string content)
        {
            using (HttpClient client = new HttpClient())
            {
                var data = new StringContent(content, Encoding.UTF8, "application/json");
                Uri uri = new Uri(endpoint);
                var response = await client.PostAsync(uri, data).ConfigureAwait(false);
                data.Dispose();
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        public static async Task<string> PostWithJwtAsync(string endpoint, string content, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var data = new StringContent(content, Encoding.UTF8, "application/json");
                Uri uri = new Uri(endpoint);
                var response = await client.PostAsync(uri, data).ConfigureAwait(false);
                data.Dispose();
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}