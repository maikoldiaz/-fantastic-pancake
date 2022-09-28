// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobGeneratorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Tests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Services;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class BlobGeneratorTests : HomologatorTestBase
    {
        /// <summary>
        /// The BLOB client.
        /// </summary>
        private readonly Mock<IBlobStorageSasClient> mockBlobSASClient = new Mock<IBlobStorageSasClient>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Mock<ITrueLogger<BlobGenerator>> mockLogger = new Mock<ITrueLogger<BlobGenerator>>();

        /// <summary>
        /// The blobGenerator.
        /// </summary>
        private BlobGenerator blobGenerator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockAzureClientFactory.Setup(m => m.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobSASClient.Object);
            this.blobGenerator = new BlobGenerator(this.mockLogger.Object, this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// Generates the blobs asynchronous should generate BLOB and update errors when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GenerateBlobsAsync_ShouldGenerateBlob_WhenInvokedAsync()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
            };

            trueMessage.FileRegistration.FileRegistrationTransactions.Add(new FileRegistrationTransaction
            {
                FileRegistrationId = 1,
                FileRegistrationTransactionId = 1,
                BlobPath = "somePath",
            });

            var jobject = this.GetMovementJArray();
            this.mockBlobSASClient.Setup(m => m.CreateBlobAsync(It.IsAny<Stream>())).Returns(Task.FromResult(0));
            this.mockBlobSASClient.Setup(m => m.CreateBlobAsync(It.IsAny<string>())).Returns(Task.FromResult(0));

            await this.blobGenerator.GenerateBlobsArrayAsync(jobject, trueMessage).ConfigureAwait(false);

            Assert.AreEqual(1, trueMessage.FileRegistration.FileRegistrationTransactions.Count);
            this.mockBlobSASClient.Verify(m => m.CreateBlobAsync(It.IsAny<string>()), Times.Exactly(3));
        }

        [TestMethod]
        public async Task GenerateBlobsAsync_ShouldGenerateBlobForInventoryJArray_WhenInvokedAsync()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
            };

            trueMessage.FileRegistration.FileRegistrationTransactions.Add(new FileRegistrationTransaction
            {
                FileRegistrationId = 1,
                FileRegistrationTransactionId = 1,
                BlobPath = "excel/json/inventory/c630a99d-9ce3-451f-b9b6-9e7978b07b18/51395bc2f614b767a28c1639e12b45252686469cbf117c9acb/51395bc2f614b767a28c1639e12b45252686469cbf117c9acb_9349_1",
                SessionId = "51395bc2f614b767a28c1639e12b45252686469cbf117c9acb",
            });

            var jobject = this.GetInventoryJArray();
            this.mockBlobSASClient.Setup(m => m.CreateBlobAsync(It.IsAny<string>())).Returns(Task.FromResult(0));

            await this.blobGenerator.GenerateBlobsArrayAsync(jobject, trueMessage).ConfigureAwait(false);

            Assert.AreEqual(1, trueMessage.FileRegistration.FileRegistrationTransactions.Count);
            foreach (var item in jobject)
            {
                Assert.AreEqual("9423eef46e38102c127300615be0d0fcf0762d26738355e1b3", item[Constants.InventoryProductUniqueId].ToString());
            }

            foreach (var item in trueMessage.FileRegistration.FileRegistrationTransactions)
            {
                Assert.AreEqual("9423eef46e38102c127300615be0d0fcf0762d26738355e1b3", item.SessionId);
                Assert.AreEqual("excel/json/inventory/c630a99d-9ce3-451f-b9b6-9e7978b07b18/9423eef46e38102c127300615be0d0fcf0762d26738355e1b3/9423eef46e38102c127300615be0d0fcf0762d26738355e1b3_9349_1", item.BlobPath);
            }

            this.mockBlobSASClient.Verify(m => m.CreateBlobAsync(It.IsAny<string>()), Times.Exactly(1));
        }

        /// <summary>
        /// Generates the blobs asynchronous should generate errors when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GenerateBlobsAsync_ShouldGenerateErrors_WhenInvokedAsync()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
            };

            trueMessage.FileRegistration.FileRegistrationTransactions.Add(new FileRegistrationTransaction
            {
                FileRegistrationId = 1,
                FileRegistrationTransactionId = 1,
                BlobPath = "somePath",
            });

            var jobject = this.GetMovementJArray();
            this.mockBlobSASClient.Setup(m => m.CreateBlobAsync(It.IsAny<Stream>())).Throws(new System.Exception(string.Empty));

            await this.blobGenerator.GenerateBlobsArrayAsync(jobject, trueMessage).ConfigureAwait(false);

            Assert.AreEqual(1, trueMessage.FileRegistration.FileRegistrationTransactions.Count);
            this.mockBlobSASClient.Verify(m => m.CreateBlobAsync(It.IsAny<string>()), Times.Exactly(3));
        }

        /// <summary>
        /// Generates the blobs asynchronous should create BLOB when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GenerateBlobsAsync_ShouldCreateBlob_WhenInvokedAsync()
        {
            var jobject = this.GetMovementJArray();
            var blobPath = "somePath";
            var containerName = SystemType.TRUE.ToString();
            this.mockBlobSASClient.Setup(m => m.CreateBlobAsync(It.IsAny<string>())).Returns(Task.FromResult(0));

            await this.blobGenerator.GenerateBlobsArrayAsync(jobject, blobPath, containerName).ConfigureAwait(false);

            this.mockBlobSASClient.Verify(m => m.CreateBlobAsync(It.IsAny<string>()), Times.Exactly(1));
        }

        /// <summary>
        /// Generates the blobs asynchronous should raise exception when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task GenerateBlobsAsync_ShouldRaiseException_WhenInvokedAsync()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
            };

            trueMessage.FileRegistration = null;
            var jobject = this.GetInventoryJArray();

            await this.blobGenerator.GenerateBlobsArrayAsync(jobject, trueMessage).ConfigureAwait(false);
        }
    }
}
