// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Executors
{
    using System.Threading.Tasks;

    using global::Bdd.Core.Api.Executors;
    using global::Bdd.Core.Entities;

    public class ApiBase : ApiExecutor
    {
        public override async Task<string> GetToken(Credentials credentials, bool graphApi = false)
        {
            var azureAdBase = new AzureAdBase();
            var token = await azureAdBase.GetAuthTokenSilentAsync(credentials).ConfigureAwait(false);
            return token;
        }
    }
}