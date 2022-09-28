// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionReconciler.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Deadletter.Reconciler
{
    using System.Globalization;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The movement reconciler.
    /// </summary>
    public class NodeConnectionReconciler : ReconcilerBase<OffchainNodeConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConnectionReconciler" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public NodeConnectionReconciler(IUnitOfWorkFactory factory, ITelemetry telemetry, IAzureClientFactory azureClientFactory, IConfigurationHandler configurationHandler)
            : base(factory, telemetry, azureClientFactory, configurationHandler, x => x.Connections, y => y.Id, z => z.NodeConnectionId.ToString(CultureInfo.InvariantCulture))
        {
        }

        /// <inheritdoc />
        public override ServiceType Type => ServiceType.NodeConnection;

        /// <inheritdoc/>
        protected override EventName EventName => EventName.NodeConnectionReconcileFailed;

        /// <inheritdoc/>
        protected override string QueueName => QueueConstants.BlockchainNodeConnectionQueue;
    }
}
