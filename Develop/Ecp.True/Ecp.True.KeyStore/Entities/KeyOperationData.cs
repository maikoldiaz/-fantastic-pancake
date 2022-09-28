// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyOperationData.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.KeyStore.Entities
{
    /// <summary>
    /// The key operation result.
    /// </summary>
    public class KeyOperationData
    {
        /// <summary>
        /// The data.
        /// </summary>
        private readonly byte[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyOperationData"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="configuration">The configuration.</param>
        public KeyOperationData(byte[] data, KeyVaultConfiguration configuration)
        {
            this.KeyVaultConfiguration = configuration;
            this.data = data;
        }

        /// <summary>
        /// Gets the key vault configuration.
        /// </summary>
        /// <value>
        /// The key vault configuration.
        /// </value>
        public KeyVaultConfiguration KeyVaultConfiguration { get; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>The data.</returns>
        public byte[] GetData()
        {
            return this.data;
        }
    }
}