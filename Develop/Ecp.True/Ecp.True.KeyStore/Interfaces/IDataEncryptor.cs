// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataEncryptor.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.KeyStore.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Data encryption interface.
    /// </summary>
    public interface IDataEncryptor
    {
        /// <summary>
        /// Decrypt the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Decrypted string.</returns>
        Task<string> DecryptAsync(string data);

        /// <summary>
        /// Decrypt the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="keyIdentifier">The key identifier.</param>
        /// <returns>Decrypted string.</returns>
        Task<string> DecryptAsync(string data, string keyIdentifier);

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Encrypted string.</returns>
        Task<string> EncryptAsync(string data);

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="keyIdentifier">The key identifier.</param>
        /// <returns>Encrypted string.</returns>
        Task<string> EncryptAsync(string data, string keyIdentifier);
    }
}