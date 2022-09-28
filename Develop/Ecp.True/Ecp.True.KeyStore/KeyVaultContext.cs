// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyVaultContext.cs" company="Microsoft">
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
    using Ecp.True.Core.Attributes;
    using Ecp.True.KeyStore.Entities;
    using Ecp.True.KeyStore.Interfaces;

    /// <summary>
    /// The key vault context.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class KeyVaultContext : IKeyVaultContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultContext" /> class.
        /// </summary>
        public KeyVaultContext()
        {
            this.Settings = new KeyVaultConfiguration();
        }

        /// <summary>
        /// Gets a value indicating whether initialized.
        /// </summary>
        /// <value>
        /// A value indicating whether initialized.
        /// </value>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public KeyVaultConfiguration Settings { get; private set; }

        /// <summary>
        /// The initialize settings.
        /// </summary>
        /// <param name="keyVaultConfiguration">
        /// The key vault configuration.
        /// </param>
        public void InitializeSettings(KeyVaultConfiguration keyVaultConfiguration)
        {
            this.Settings = keyVaultConfiguration;
            this.Initialized = true;
        }
    }
}