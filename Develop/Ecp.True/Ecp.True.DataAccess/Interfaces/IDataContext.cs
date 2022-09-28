// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataContext.cs" company="Microsoft">
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
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The data context for data store.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDataContext : IDisposable
    {
        /// <summary>
        /// Saves the context asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// Number of rows effected.
        /// </returns>
        Task<int> SaveAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();
    }
}