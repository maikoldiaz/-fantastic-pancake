// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailureHandlerFactoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Tests.FailureHandler
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// FailureHandlerFactory tests.
    /// </summary>
    [TestClass]
    public class FailureHandlerFactoryTests
    {
        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFailureHandler> cutoffFailureHandlerMock = new Mock<IFailureHandler>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFailureHandler> ownershipFailureHandlerMock = new Mock<IFailureHandler>();

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private FailureHandlerFactory failureHandlerFactory;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.cutoffFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.Cutoff);
            this.ownershipFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.Ownership);
            var failureHandlerFactoryList = new List<IFailureHandler>() { this.cutoffFailureHandlerMock.Object, this.ownershipFailureHandlerMock.Object };
            this.failureHandlerFactory = new FailureHandlerFactory(failureHandlerFactoryList);
        }

        /// <summary>
        /// Gets the ownership failure hanler.
        /// </summary>
        [TestMethod]
        public void GetOwnership_FailureHanler()
        {
            var ownershipHandler = this.failureHandlerFactory.GetFailureHandler(TicketType.Ownership);

            Assert.IsNotNull(ownershipHandler);
            Assert.IsTrue(ownershipHandler.TicketType == TicketType.Ownership);
        }

        [TestMethod]
        public void GetCutOff_FailureHanler()
        {
            var cutOffHandler = this.failureHandlerFactory.GetFailureHandler(TicketType.Cutoff);

            Assert.IsNotNull(cutOffHandler);
            Assert.IsTrue(cutOffHandler.TicketType == TicketType.Cutoff);
        }
    }
}
