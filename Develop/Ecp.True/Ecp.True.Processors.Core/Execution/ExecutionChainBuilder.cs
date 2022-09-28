// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutionChainBuilder.cs" company="Microsoft">
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
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The class Execution Chain Builder.
    /// </summary>
    public class ExecutionChainBuilder : IExecutionChainBuilder
    {
        /// <summary>
        /// The list of executors.
        /// </summary>
        private readonly IEnumerable<IExecutor> executors;

        /// <summary>
        /// The chain configuration.
        /// </summary>
        private readonly IDictionary<ProcessType, IDictionary<ChainType, (int, int)>> chainConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionChainBuilder"/> class.
        /// </summary>
        /// <param name="executors"> the list of executors.</param>
        public ExecutionChainBuilder(IEnumerable<IExecutor> executors)
        {
            this.executors = executors;
            this.chainConfig = new Dictionary<ProcessType, IDictionary<ChainType, (int, int)>>
            {
                { ProcessType.Ownership, GetOwnershipChain() },
                { ProcessType.Delta, GetDeltaChain() },
                { ProcessType.OfficialDelta, GetOfficialDeltaChain() },
            };
        }

        /// <inheritdoc/>
        public IExecutor Build(ProcessType processType, ChainType chainType)
        {
            var (startOrder, endOrder) = this.chainConfig[processType][chainType];
            var initiator = this.executors.Single(x => x.Order == startOrder && x.ProcessType == processType);
            var currentExecutor = initiator;
            var nextOrder = startOrder + 1;

            while (currentExecutor != null && nextOrder <= endOrder)
            {
                var nextExecutor = this.executors.SingleOrDefault(x => x.Order == nextOrder && x.ProcessType == processType);
                currentExecutor.SetNext(nextExecutor);
                currentExecutor = nextExecutor;
                nextOrder += 1;
            }

            return initiator;
        }

        private static IDictionary<ChainType, (int, int)> GetOwnershipChain()
        {
            return new Dictionary<ChainType, (int, int)>
                    {
                        { ChainType.ProcessAnalytics, (1, 1) },
                        { ChainType.RequestOwnershipData, (2, 6) },
                        { ChainType.Register, (7, 9) },
                        { ChainType.CalculateOwnershipData, (10, 11) },
                    };
        }

        private static IDictionary<ChainType, (int, int)> GetDeltaChain()
        {
            return new Dictionary<ChainType, (int, int)>
                    {
                        { ChainType.GetDelta, (1, 2) },
                        { ChainType.RequestDelta, (3, 3) },
                        { ChainType.ProcessDelta, (4, 5) },
                        { ChainType.CompleteDelta, (6, 7) },
                    };
        }

        private static IDictionary<ChainType, (int, int)> GetOfficialDeltaChain()
        {
            return new Dictionary<ChainType, (int, int)>
                    {
                        { ChainType.RequestOfficialDelta, (1, 2) },
                        { ChainType.ProcessOfficialDelta, (3, 4) },
                        { ChainType.RegisterMovementsOfficialDelta, (5, 5) },
                        { ChainType.CalculateOfficialDelta, (6, 6) },
                    };
        }
    }
}
