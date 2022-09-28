// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Connection.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ConfigurationManager.Services
{
    using Ecp.True.ConfigurationManager.Console.Repositories.Interface;

    /// <summary>
    /// The connection.
    /// </summary>
    public class Connection : IConnection
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// The initialization.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public void Initialize(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
