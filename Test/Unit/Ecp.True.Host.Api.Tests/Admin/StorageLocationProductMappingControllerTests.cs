// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductMappingControllerTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The StorageLocationProductMappingController test class.
    /// </summary>
    [TestClass]
    public class StorageLocationProductMappingControllerTests
    {
        private StorageLocationProductMappingController controller;
        private Mock<IStorageLocationProductMappingProcessor> processorMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.processorMock = new Mock<IStorageLocationProductMappingProcessor>();
            this.processorMock.Setup(
                p => p.CreateStorageLocationProductMappingAsync(
                    It.IsAny<IEnumerable<StorageLocationProductMapping>>()))
                .ReturnsAsync(new List<StorageLocationProductMappingInfo>().AsEnumerable());
            this.controller = new StorageLocationProductMappingController(this.processorMock.Object);
        }

        /// <summary>
        /// CreateStorageLocationProduct_ShouldInvokeProcessorAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateStorageLocationProduct_ShouldInvokeProcessorAsync()
        {
            // Arrange
            var testMappings = GetTestMappings();

            // Act
            await this.controller.CreateStorageLocationProductMappingAsync(testMappings)
                .ConfigureAwait(false);

            // Assert
            this.processorMock.Verify(
                p => p.CreateStorageLocationProductMappingAsync(
                    It.IsAny<IEnumerable<StorageLocationProductMapping>>()), Times.Once);
        }

        /// <summary>
        /// UpdateStorageLocationProductMapping_ShouldInvokeProcessorAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateStorageLocationProductMapping_ShouldInvokeProcessorAsync()
        {
            // Arrange
            var testMappings = GetTestMappings();

            // Act
            await this.controller.DeleteStorageLocationProductMappingAsync(testMappings.FirstOrDefault().StorageLocationId)
                .ConfigureAwait(false);

            // Assert
            this.processorMock.Verify(
                p => p.DeleteStorageLocationProductMappingAsync(
                    testMappings.FirstOrDefault().StorageLocationId), Times.Once);
        }

        private static IEnumerable<StorageLocationProductMapping> GetTestMappings()
        {
            yield return new StorageLocationProductMapping
            {
                ProductId = "1",
                StorageLocationId = "2",
            };
        }
    }
}