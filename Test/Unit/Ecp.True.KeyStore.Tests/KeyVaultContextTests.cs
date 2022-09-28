// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyVaultContextTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.KeyStore.Tests
{
    using System;

    using Ecp.True.KeyStore.Entities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Key Vault Context Tests.
    /// </summary>
    [TestClass]
    public class KeyVaultContextTests
    {
        /// <summary>
        /// Initialize settings should set key vault configuration.
        /// </summary>
        [TestMethod]
        public void InitializeSettingsShouldSetKeyVaultConfiguration()
        {
            // Arrange
            var keyVaultContext = new KeyVaultContext();
            var configurationSettings = new KeyVaultConfiguration { ClientAuthId = Guid.NewGuid().ToString() };

            // Act
            keyVaultContext.InitializeSettings(configurationSettings);

            // Assert
            Assert.IsNotNull(keyVaultContext.Settings);
            Assert.IsTrue(keyVaultContext.Initialized);
            Assert.AreEqual(keyVaultContext.Settings.ClientAuthId, configurationSettings.ClientAuthId);
        }

        /// <summary>
        /// Settings should be initialized when key vault context instance is created.
        /// </summary>
        [TestMethod]
        public void SettingsShouldBeInitializedWhenKeyVaultContextInstanceIsCreated()
        {
            // Arrange

            // Act
            var keyVaultContext = new KeyVaultContext();

            // Assert
            Assert.IsNotNull(keyVaultContext.Settings);
        }
    }
}