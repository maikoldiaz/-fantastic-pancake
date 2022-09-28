// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Auth.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// The AD auth provider.
    /// </summary>
    public interface IAuthProvider
    {
        /// <summary>
        /// Gets the user access token asynchronous.
        /// </summary>
        /// <returns>The user access token.</returns>
        Task<string> GetCurrentUserAccessTokenAsync();

        /// <summary>
        /// Gets the user access token asynchronous.
        /// </summary>
        /// <param name="scopes">The scopes.</param>
        /// <returns>The user access token.</returns>
        Task<string> GetUserAccessTokenAsync(string[] scopes);
    }
}
