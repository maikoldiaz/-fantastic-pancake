// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecretData.cs" company="Microsoft">
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
    /// The secret data.
    /// </summary>
    public class SecretData
    {
        /// <summary>
        /// Gets or sets the name of the secret.
        /// </summary>
        /// <value>
        /// The name of the secret.
        /// </value>
        public string SecretName { get; set; }

        /// <summary>
        /// Gets or sets the vault address.
        /// </summary>
        /// <value>
        /// The vault address.
        /// </value>
        public string VaultAddress { get; set; }
    }
}