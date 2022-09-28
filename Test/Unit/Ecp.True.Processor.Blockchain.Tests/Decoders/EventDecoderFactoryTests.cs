// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventDecoderFactoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Blockchain.Tests.Decoders
{
    using System.Collections.Generic;
    using Ecp.True.Processors.Blockchain.Decoders;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The event decoder factory tests.
    /// </summary>
    [TestClass]
    public class EventDecoderFactoryTests
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private EventDecoderFactory factory;

        /// <summary>
        /// The mock decoder v1.
        /// </summary>
        private Mock<IEventDecoder> mockDecoderV1;

        /// <summary>
        /// The mock decoder v2.
        /// </summary>
        private Mock<IEventDecoder> mockDecoderV2;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var decoders = new List<IEventDecoder>();

            this.mockDecoderV1 = new Mock<IEventDecoder>();
            this.mockDecoderV1.SetupGet(m => m.Version).Returns(1);

            this.mockDecoderV2 = new Mock<IEventDecoder>();
            this.mockDecoderV2.SetupGet(m => m.Version).Returns(2);

            decoders.Add(this.mockDecoderV1.Object);
            decoders.Add(this.mockDecoderV2.Object);

            this.factory = new EventDecoderFactory(decoders);
        }

        /// <summary>
        /// Gets the decoder should return versioned decoder when invoked.
        /// </summary>
        [TestMethod]
        public void GetDecoder_ShouldReturnVersionedDecoder_WhenInvoked()
        {
            var decoder = this.factory.GetDecoder(1);
            Assert.AreEqual(this.mockDecoderV1.Object, decoder);

            decoder = this.factory.GetDecoder(2);
            Assert.AreEqual(this.mockDecoderV2.Object, decoder);
        }
    }
}
