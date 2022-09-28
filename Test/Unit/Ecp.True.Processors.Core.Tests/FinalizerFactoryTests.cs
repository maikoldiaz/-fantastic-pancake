// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FinalizerFactoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Tests
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// FailureHandlerFactory tests.
    /// </summary>
    [TestClass]
    public class FinalizerFactoryTests
    {
        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFinalizer> cutoffFinalizerMock = new Mock<IFinalizer>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFinalizer> ownershipFinalizerMock = new Mock<IFinalizer>();

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private FinalizerFactory finalizerFactory;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.cutoffFinalizerMock.SetupGet(x => x.Type).Returns(TicketType.Cutoff);
            this.ownershipFinalizerMock.SetupGet(x => x.Type).Returns(TicketType.Ownership);
            var finalizerFactoryList = new List<IFinalizer>() { this.cutoffFinalizerMock.Object, this.ownershipFinalizerMock.Object };
            this.finalizerFactory = new FinalizerFactory(finalizerFactoryList);
        }

        /// <summary>
        /// Gets the ownership failure hanler.
        /// </summary>
        [TestMethod]
        public void GetOwnership_Finalizer()
        {
            var ownershipHandler = this.finalizerFactory.GetFinalizer(TicketType.Ownership);

            Assert.IsNotNull(ownershipHandler);
            Assert.IsTrue(ownershipHandler.Type == TicketType.Ownership);
        }

        [TestMethod]
        public void GetCutOff_Finalizer()
        {
            var cutOffHandler = this.finalizerFactory.GetFinalizer(TicketType.Cutoff);

            Assert.IsNotNull(cutOffHandler);
            Assert.IsTrue(cutOffHandler.Type == TicketType.Cutoff);
        }
    }
}
