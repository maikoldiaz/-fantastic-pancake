// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExecutor.cs" company="Microsoft">
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
    using Ecp.True.Processors.Core.Execution;

    /// <summary>
    /// The interface IExecutor.
    /// </summary>
    public interface IExecutor
    {
        /// <summary>
        /// Gets the number denoting order of executor.
        /// </summary>
        /// <value>
        /// The number denoting order of executor.
        /// This order denotes the sequence of the executor in the chain.
        /// </value>
        int Order { get; }

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        ProcessType ProcessType { get; }

        /// <summary>
        /// Executes the incoming request.
        /// </summary>
        /// <param name="input">The input object.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        Task ExecuteAsync(object input);

        /// <summary>
        /// Sets the next executor in chain.
        /// </summary>
        /// <param name="nextExecutor">The next executor.</param>
        void SetNext(IExecutor nextExecutor);
    }
}
