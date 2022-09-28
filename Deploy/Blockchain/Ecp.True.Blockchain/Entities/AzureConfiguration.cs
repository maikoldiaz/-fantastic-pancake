// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureConfiguration.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Entities
{
    using Ecp.True.Blockchain.SetUp;

    /// <summary>
    /// The AzureConfiguration.
    /// </summary>
    public class AzureConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureConfiguration"/> class.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        public AzureConfiguration(string storageConnectionString)
        {
            this.StorageConnectionString = storageConnectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureConfiguration"/> class.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <param name="appSecret">The application secret.</param>
        public AzureConfiguration(string appId, string appSecret)
        {
            this.AppId = appId;
            this.AppSecret = appSecret;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureConfiguration" /> class.
        /// </summary>
        /// <param name="quorumProfile">The quorum profile.</param>
        public AzureConfiguration(QuorumProfile quorumProfile)
        {
            ArgumentValidators.ThrowIfNull(quorumProfile, nameof(quorumProfile));

            this.QuorumProfile = quorumProfile;
        }

        /// <summary>
        /// Gets or sets the storage connection string.
        /// </summary>
        /// <value>
        /// The storage connection string.
        /// </value>
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the application secret.
        /// </summary>
        /// <value>
        /// The application secret.
        /// </value>
        public string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets the quorum profile.
        /// </summary>
        /// <value>
        /// The quorum profile.
        /// </value>
        public QuorumProfile QuorumProfile { get; set; }
    }
}
