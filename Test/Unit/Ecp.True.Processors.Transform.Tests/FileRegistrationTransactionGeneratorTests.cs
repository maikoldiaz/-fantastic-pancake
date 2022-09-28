// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationTransactionGeneratorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Transform.Homologate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The file registration transaction generator tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Transform.Tests.HomologatorTestBase" />
    [TestClass]
    public class FileRegistrationTransactionGeneratorTests : HomologatorTestBase
    {
        /// <summary>
        /// The blobGenerator.
        /// </summary>
        private FileRegistrationTransactionGenerator registrationTransactionGenerator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.registrationTransactionGenerator = new FileRegistrationTransactionGenerator();
        }

        /// <summary>
        /// Transactions the generator should generate BLOB IDs and file transactions.
        /// </summary>
        [TestMethod]
        public void TransactionGenerator_ShouldGenerate_BlobIdsAndFileTransactions()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Movement,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
                FileRegistration = new Entities.Admin.FileRegistration
                {
                    FileRegistrationId = 123,
                },
            };

            var jobject = this.GetMovementJArray();
            this.registrationTransactionGenerator.UpdateFileRegistrationTransactions(trueMessage, jobject);
            Assert.AreEqual(3, trueMessage.FileRegistration.FileRegistrationTransactions.Count);
            Assert.IsNotNull(trueMessage.FileRegistration.FileRegistrationTransactions.ToArray()[0].RecordId);
            Assert.IsNotNull(trueMessage.FileRegistration.FileRegistrationTransactions.ToArray()[1].RecordId);
            Assert.IsNotNull(trueMessage.FileRegistration.FileRegistrationTransactions.ToArray()[2].RecordId);
            Assert.IsTrue(trueMessage.FileRegistration.FileRegistrationTransactions.ToArray()[0].BlobPath.Contains($"279760396583110_{trueMessage.FileRegistration.FileRegistrationId}_1", System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(trueMessage.FileRegistration.FileRegistrationTransactions.ToArray()[1].BlobPath.Contains($"279760396583110_{trueMessage.FileRegistration.FileRegistrationId}_2", System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(trueMessage.FileRegistration.FileRegistrationTransactions.ToArray()[2].BlobPath.Contains($"279760396583111_{trueMessage.FileRegistration.FileRegistrationId}_1", System.StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Transactions the generator should generate BLOB IDs and file transactions.
        /// </summary>
        [TestMethod]
        public void TransactionGenerator_ShouldGenerate_BlobIdsAndFileTransactions_ForInventories()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
            };

            var jobject = this.GetInventoryWithMultipleEntities();
            var inventoryList = jobject.ToObject<List<InventoryProduct>>();
            this.registrationTransactionGenerator.UpdateFileRegistrationTransactions(trueMessage, jobject);
            Assert.AreEqual(2, trueMessage.FileRegistration.FileRegistrationTransactions.Count);
            Assert.IsNotNull(trueMessage.FileRegistration.FileRegistrationTransactions.ToArray()[0].RecordId);
            Assert.IsNotNull(trueMessage.FileRegistration.FileRegistrationTransactions.ToArray()[1].RecordId);
            Assert.IsTrue(trueMessage.FileRegistration.FileRegistrationTransactions.ToArray()[0].BlobPath.Contains(inventoryList.AsEnumerable().FirstOrDefault().InventoryProductUniqueId, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(trueMessage.FileRegistration.FileRegistrationTransactions.ToArray()[1].BlobPath.Contains(inventoryList.ToList()[1].InventoryProductUniqueId, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
