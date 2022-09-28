// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistrationStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;

    /// <summary>
    /// The registration strategy.
    /// </summary>
    public interface IRegistrationStrategy
    {
        /// <summary>
        /// Registers asynchronously.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="unitOfWork">The unitOfWork.</param>
        void Insert(IEnumerable<object> entities, IUnitOfWork unitOfWork);

        /// <summary>
        /// Registers asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The unitOfWork.</param>
        /// <returns>The task.</returns>
        Task RegisterAsync(object entity, IUnitOfWork unitOfWork);
    }
}
