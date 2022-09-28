// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionFactoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Tests
{
    using Ecp.True.Entities.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The unit of work tests.
    /// </summary>
    [TestClass]
    public class ConnectionFactoryTests
    {
        /// <summary>
        /// The SQL configuration.
        /// </summary>
        private readonly SqlConnectionSettings sqlConfig = new SqlConnectionSettings { ConnectionString = "ConnString" };

        /// <summary>
        /// The unit of work instance.
        /// </summary>
        private ConnectionFactory connectionFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.connectionFactory = new ConnectionFactory();
        }

        /// <summary>
        /// Creates the repository should create repository from repository factory when invoked.
        /// </summary>
        [TestMethod]
        public void SqlConnectionConfig_ShouldReturnSqlConfig_WhenInvoked()
        {
            this.connectionFactory.SetupSqlConfig(this.sqlConfig);
            this.connectionFactory.SetupStorageConnection("ConfigConnString");

            // Act
            var config = this.connectionFactory.SqlConnectionConfig;

            // Assert
            Assert.IsNotNull(config);
            Assert.AreEqual(this.sqlConfig, config);
        }

        /// <summary>
        /// Creates the repository should create repository from repository factory when invoked.
        /// </summary>
        [TestMethod]
        public void ConnectionConfig_ShouldReturnNull_WhenConnectionFactoryIsEmpty()
        {
            // Assert
            Assert.AreEqual(default(SqlConnectionSettings), this.connectionFactory.SqlConnectionConfig);
            Assert.AreEqual(default(string), this.connectionFactory.NoSqlConnectionString);
        }

        /// <summary>
        /// Creates the repository should create repository from repository factory when invoked.
        /// </summary>
        [TestMethod]
        public void NoSqlConnection_ShouldReturnConnectionString_WhenConnectionFactoryIsInitialized()
        {
            this.connectionFactory.SetupSqlConfig(this.sqlConfig);
            this.connectionFactory.SetupStorageConnection("ConfigConnString");

            // Assert
            Assert.AreEqual("ConfigConnString", this.connectionFactory.NoSqlConnectionString);
        }
    }
}
