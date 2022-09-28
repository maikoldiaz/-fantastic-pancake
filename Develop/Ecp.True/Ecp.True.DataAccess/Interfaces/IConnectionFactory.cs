// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnectionFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Interfaces
{
    using Ecp.True.Entities.Configuration;

    /// <summary>
    /// The connection factory to host connection details.
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Gets a value indicating whether this instance is ready.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </value>
        bool IsReady { get; }

        /// <summary>
        /// Gets the SQL connection configuration.
        /// </summary>
        /// <value>
        /// The SQL connection configuration.
        /// </value>
        SqlConnectionSettings SqlConnectionConfig { get; }

        /// <summary>
        /// Gets the no SQL connection string.
        /// </summary>
        /// <value>
        /// The no SQL connection string.
        /// </value>
        string NoSqlConnectionString { get; }

        /// <summary>
        /// Initializes the specified storage connection string.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        void SetupStorageConnection(string storageConnectionString);

        /// <summary>
        /// Setups the SQL configuration.
        /// </summary>
        /// <param name="sqlConnectionConfig">The SQL connection configuration.</param>
        void SetupSqlConfig(SqlConnectionSettings sqlConnectionConfig);
    }
}
