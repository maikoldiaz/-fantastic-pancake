// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReconcilerServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Deadletter.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Deadletter;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The reconciler service test.
    /// </summary>
    [TestClass]
    public class ReconcilerServiceTests
    {
        /// <summary>
        /// The node reconciler mock.
        /// </summary>
        private Mock<IReconciler> nodeReconcilerMock;

        /// <summary>
        /// The connection reconciler mock.
        /// </summary>
        private Mock<IReconciler> connectionReconcilerMock;

        /// <summary>
        /// The service.
        /// </summary>
        private ReconcileService service;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.nodeReconcilerMock = new Mock<IReconciler>();
            this.connectionReconcilerMock = new Mock<IReconciler>();

            this.nodeReconcilerMock.Setup(s => s.ReconcileAsync());
            this.nodeReconcilerMock.SetupGet(s => s.Type).Returns(ServiceType.Node);
            this.nodeReconcilerMock.Setup(s => s.GetFailuresAsync(It.IsAny<bool>(), null)).ReturnsAsync(new List<FailedRecord> { new FailedRecord { Name = nameof(OffchainNode), RecordId = 1 } });
            this.nodeReconcilerMock.Setup(s => s.ResetAsync(It.IsAny<BlockchainFailures>()));

            this.connectionReconcilerMock.Setup(s => s.ReconcileAsync());
            this.connectionReconcilerMock.SetupGet(s => s.Type).Returns(ServiceType.NodeConnection);
            this.connectionReconcilerMock.Setup(s => s.GetFailuresAsync(It.IsAny<bool>(), null)).ReturnsAsync(new List<FailedRecord> { new FailedRecord { Name = nameof(OffchainNodeConnection), RecordId = 2 } });
            this.connectionReconcilerMock.Setup(s => s.ResetAsync(It.IsAny<BlockchainFailures>()));

            var arr = new[] { this.nodeReconcilerMock.Object, this.connectionReconcilerMock.Object };
            this.service = new ReconcileService(arr);
        }

        /// <summary>
        /// Reconciles the should call reconcile on children when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Reconcile_ShouldCallReconcileOnChildren_WhenInvokedAsync()
        {
            await this.service.ReconcileAsync().ConfigureAwait(false);

            this.nodeReconcilerMock.Verify(s => s.ReconcileAsync(), Times.Once);
            this.connectionReconcilerMock.Verify(s => s.ReconcileAsync(), Times.Once);
        }

        /// <summary>
        /// Reconciles the should call reconcile on children when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Reset_ShouldCallResetOnChildren_WhenInvokedAsync()
        {
            var failures = new BlockchainFailures();
            await this.service.ResetAsync(failures).ConfigureAwait(false);

            this.nodeReconcilerMock.Verify(s => s.ResetAsync(It.Is<BlockchainFailures>(x => x == failures)), Times.Once);
            this.connectionReconcilerMock.Verify(s => s.ResetAsync(It.Is<BlockchainFailures>(x => x == failures)), Times.Once);
        }

        /// <summary>
        /// Reconciles the should call reconcile on children when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFailures_ShouldGetFailuresFromAllChildren_WhenInvokedAsync()
        {
            var result = await this.service.GetFailuresAsync(true, null).ConfigureAwait(false);

            this.nodeReconcilerMock.Verify(s => s.GetFailuresAsync(true, null), Times.Once);
            this.connectionReconcilerMock.Verify(s => s.GetFailuresAsync(true, null), Times.Once);

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(true, result.Any(r => r.Name == nameof(OffchainNode)));
            Assert.AreEqual(1, result.First(r => r.Name == nameof(OffchainNode)).RecordId);

            Assert.AreEqual(true, result.Any(r => r.Name == nameof(OffchainNodeConnection)));
            Assert.AreEqual(2, result.First(r => r.Name == nameof(OffchainNodeConnection)).RecordId);
        }

        /// <summary>
        /// Reconciles the should call reconcile on children when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldCallReconcileOnNode_WhenInvokedWithNodeAsync()
        {
            var message = new OffchainMessage { Type = ServiceType.Node };

            await this.service.ReconcileAsync(message).ConfigureAwait(false);

            this.nodeReconcilerMock.Verify(s => s.ReconcileAsync(message), Times.Once);
            this.connectionReconcilerMock.Verify(s => s.ReconcileAsync(message), Times.Never);
        }

        /// <summary>
        /// Reconciles the should call reconcile on children when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldCallReconcileOnNodeConnection_WhenInvokedWithNodeConnectionAsync()
        {
            var message = new OffchainMessage { Type = ServiceType.NodeConnection };

            await this.service.ReconcileAsync(message).ConfigureAwait(false);

            this.nodeReconcilerMock.Verify(s => s.ReconcileAsync(message), Times.Never);
            this.connectionReconcilerMock.Verify(s => s.ReconcileAsync(message), Times.Once);
        }

        /// <summary>
        /// Reconciles the should call reconcile on children when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldNotCallReconcile_WhenInvokedWithUnknownServiceTypeAsync()
        {
            var message = new OffchainMessage { Type = ServiceType.Unbalance };

            await this.service.ReconcileAsync(message).ConfigureAwait(false);

            this.nodeReconcilerMock.Verify(s => s.ReconcileAsync(message), Times.Never);
            this.connectionReconcilerMock.Verify(s => s.ReconcileAsync(message), Times.Never);
        }
    }
}
