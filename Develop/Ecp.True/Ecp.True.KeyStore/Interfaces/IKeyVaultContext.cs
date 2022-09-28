// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyVaultContext.cs" company="Microsoft">
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
    using Ecp.True.KeyStore.Entities;

    /// <summary>
    /// The KeyVaultContext interface.
    /// </summary>
    public interface IKeyVaultContext
    {
        /// <summary>
        /// Gets a value indicating whether initialized.
        /// </summary>
        /// <value>
        /// A value indicating whether initialized.
        /// </value>
        bool Initialized { get; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        KeyVaultConfiguration Settings { get; }

        /// <summary>
        /// The initialize settings.
        /// </summary>
        /// <param name="keyVaultConfiguration">
        /// The key vault configuration.
        /// </param>
        void InitializeSettings(KeyVaultConfiguration keyVaultConfiguration);
    }
}