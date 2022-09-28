// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.KeyStore
{
    /// <summary>
    /// The constants store.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The key vault token cache expiry interval in seconds.
        /// </summary>
        public const int KeyVaultTokenCacheExpiryIntervalInSeconds = 60;

        /// <summary>
        /// The key vault access token key.
        /// </summary>
        public const string KeyVaultAccessTokenKey = "KeyVaultADToken";

        /// <summary>
        /// Error constant for secret has been created.
        /// </summary>
        public const string SecretHasBeenCreated = "The secret has been created";

        /// <summary>
        /// Error constant for secret has been deleted.
        /// </summary>
        public const string SecretHasBeenDeleted = "The secret has been deleted";

        /// <summary>
        /// The fail to load certificate from path.
        /// </summary>
        public const string FailToLoadCertificateFromPath = "Failed to load certificate from path.";

        /// <summary>
        /// The default HTTP client name.
        /// </summary>
        public const string DefaultHttpClientName = "Default";
    }
}