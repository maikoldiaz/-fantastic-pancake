// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExecutionManager.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The execution manager.
    /// </summary>
    public interface IExecutionManager
    {
        /// <summary>
        /// Gets the variable type.
        /// </summary>
        /// <value>
        /// The variable type.
        /// </value>
        TicketType Type { get; }

        /// <summary>
        /// Executes the chain asynchronous.
        /// </summary>
        /// <param name="executor">The executor.</param>
        void Initialize(IExecutor executor);

        /// <summary>
        /// Executes the chain asynchronous.
        /// </summary>
        /// <param name="input">The input object.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        Task<object> ExecuteChainAsync(object input);
    }
}
