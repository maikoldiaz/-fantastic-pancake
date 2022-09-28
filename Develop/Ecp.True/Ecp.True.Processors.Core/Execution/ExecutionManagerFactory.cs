// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutionManagerFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Execution
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// Execution manager.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.FailureHandler.FailureHandlerFactory" />
    public class ExecutionManagerFactory : IExecutionManagerFactory
    {
        /// <summary>
        /// The execution managers.
        /// </summary>
        private readonly IEnumerable<IExecutionManager> executionManagers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionManagerFactory"/> class.
        /// </summary>
        /// <param name="executionManagers">The executionManagers.</param>
        public ExecutionManagerFactory(
                  IEnumerable<IExecutionManager> executionManagers)
        {
            this.executionManagers = executionManagers;
        }

        /// <inheritdoc/>
        public IExecutionManager GetExecutionManager(TicketType ticketType)
        {
            return this.executionManagers.Single(x => x.Type == ticketType);
        }
    }
}