// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialLogisticsFailureHandlerTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.HandleFailure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OfficialLogisticsFailureHandlerTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<OfficialLogisticsFailureHandler>> loggerMock = new Mock<ITrueLogger<OfficialLogisticsFailureHandler>>();

        /// <summary>
        /// Theofficial logistics failure handler.
        /// </summary>
        private OfficialLogisticsFailureHandler officialLogisticsFailureHandler;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.officialLogisticsFailureHandler = new OfficialLogisticsFailureHandler(this.loggerMock.Object);
        }

        /// <summary>
        /// Types the should return ticket type official logistics when invoked.
        /// </summary>
        [TestMethod]
        public void Type_ShouldReturnTicketTypeDelta_WhenInvoked()
        {
            var result = this.officialLogisticsFailureHandler.TicketType;

            Assert.AreEqual(TicketType.OfficialLogistics, result);
        }
    }
}
