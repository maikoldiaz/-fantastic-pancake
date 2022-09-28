// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISqlTokenProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql
{
    using System.Threading.Tasks;

    /// <summary>
    /// The SQL data access.
    /// </summary>
    public interface ISqlTokenProvider
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        string AccessToken { get; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        Task InitializeAsync();
    }
}
