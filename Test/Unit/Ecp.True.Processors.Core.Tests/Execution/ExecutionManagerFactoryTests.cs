// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutionManagerFactoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Tests.Execution
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// FailureHandlerFactory tests.
    /// </summary>
    [TestClass]
    public class ExecutionManagerFactoryTests
    {
        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IExecutionManager> officialDeltaExecutionMock = new Mock<IExecutionManager>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IExecutionManager> ownershipExecutionMock = new Mock<IExecutionManager>();

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private ExecutionManagerFactory executionManagerFactory;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.officialDeltaExecutionMock.Setup(x => x.Type).Returns(TicketType.OfficialDelta);
            this.ownershipExecutionMock.Setup(x => x.Type).Returns(TicketType.Ownership);
            var executionManagerFactoryList = new List<IExecutionManager>() { this.officialDeltaExecutionMock.Object, this.ownershipExecutionMock.Object };
            this.executionManagerFactory = new ExecutionManagerFactory(executionManagerFactoryList);
        }

        /// <summary>
        /// Gets the ownership failure hanler.
        /// </summary>
        [TestMethod]
        public void GetOwnership_FailureHanler()
        {
            var ownershipExecutionManager = this.executionManagerFactory.GetExecutionManager(TicketType.Ownership);

            Assert.IsNotNull(ownershipExecutionManager);
            Assert.IsTrue(ownershipExecutionManager.Type == TicketType.Ownership);
        }

        [TestMethod]
        public void GetOfficialDelta_FailureHanler()
        {
            var officialDeltaExecutionManager = this.executionManagerFactory.GetExecutionManager(TicketType.OfficialDelta);

            Assert.IsNotNull(officialDeltaExecutionManager);
            Assert.IsTrue(officialDeltaExecutionManager.Type == TicketType.OfficialDelta);
        }
    }
}
