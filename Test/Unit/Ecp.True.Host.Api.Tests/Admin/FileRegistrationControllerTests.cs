// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The register file controller tests.
    /// </summary>
    [TestClass]
    public class FileRegistrationControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private FileRegistrationController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IFileRegistrationProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IFileRegistrationProcessor>();
            this.controller = new FileRegistrationController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Registerfileses the asynchronous should invoke processor to register newfile asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RetryPendingTransactionAsyncAsync_ShouldInvokeProcessor_ToRetryPendingTransactionAsync()
        {
            var retryIds = new int[] { 1 };
            this.mockProcessor.Setup(m => m.RetryAsync(It.IsAny<int[]>()));

            var result = await this.controller.RetryPendingTransactionAsync(retryIds).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.RetryAsync(retryIds), Times.Once());
        }

        /// <summary>
        /// Queries the register files asynchronous should invoke processor to return register files asynchronous.
        /// </summary>
        /// <returns>The tasks.</returns>
        [TestMethod]
        public async Task QueryRegisterFilesAsync_ShouldInvokeProcessor_ToReturnRegisterFilesAsync()
        {
            var registerFiles = new[] { new FileRegistrationInfo() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryViewAsync<FileRegistrationInfo>()).ReturnsAsync(registerFiles);

            var result = await this.controller.QueryAllAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, registerFiles);

            this.mockProcessor.Verify(c => c.QueryViewAsync<FileRegistrationInfo>(), Times.Once());
        }

        /// <summary>
        /// Queries the file registration asynchronous should invoke processor to return file registration asynchronous.
        /// </summary>
        /// <returns>The tasks.</returns>
        [TestMethod]
        public async Task QueryAllIntegrationManagementAsync_ShouldInvokeProcessor_ToReturnFileRegistrationAsync()
        {
            var registerFiles = new[] { new FileRegistration() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<FileRegistration>(null)).ReturnsAsync(registerFiles);

            var result = await this.controller.QueryAllIntegrationManagementAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, registerFiles);

            this.mockProcessor.Verify(c => c.QueryAllAsync<FileRegistration>(null), Times.Once());
        }

        /// <summary>
        /// Registerfileses the asynchronous should invoke processor to register newfile asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RegisterfilesAsync_ShouldInvokeProcessor_ToRegisterNewfileAsync()
        {
            var registerFile = new FileRegistration();
            this.mockProcessor.Setup(m => m.RegisterAsync(It.IsAny<FileRegistration>()));

            var result = await this.controller.RegisterfilesAsync(registerFile).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.RegisterAsync(registerFile), Times.Once());
        }

        /// <summary>
        /// Gets the register files by ids should invoke processor to files asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetRegisterFilesByIds_ShouldInvokeProcessor_ToFilesAsync()
        {
            var registerFiles = new[] { new FileRegistration() }.AsQueryable();
            var fileIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
            this.mockProcessor.Setup(m => m.GetFileRegistrationStatusAsync(fileIds)).ReturnsAsync(registerFiles);

            var result = await this.controller.GetFileRegistrationStatusAsync(fileIds).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetFileRegistrationStatusAsync(fileIds), Times.Once());
        }

        /// <summary>
        /// Gets the Read Sas Token for container.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFileRegistration_ShouldInvokeProcess_ToReadFileAsync()
        {
            var fileAccessInfo = new FileAccessInfo
            {
                AccountName = "baseHost",
                ContainerName = "container",
                SasToken = "token",
            };
            this.mockProcessor.Setup(m => m.GetFileRegistrationAccessInfoAsync()).ReturnsAsync(fileAccessInfo);
            var result = await this.controller.GetFileRegistrationAccessInfoAsync().ConfigureAwait(false);
            this.mockProcessor.Verify(a => a.GetFileRegistrationAccessInfoAsync(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Gets the Read Sas Token for container.
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <returns>The task.</returns>
        [TestMethod]
        [DataRow(ContainerName.True)]
        [DataRow(ContainerName.Delta)]
        [DataRow(ContainerName.Ownership)]
        public async Task GetFileRegistrationByContainer_ShouldInvokeProcess_ToReadFileAsync(string containerName)
        {
            var fileAccessInfo = new FileAccessInfo
            {
                AccountName = "baseHost",
                ContainerName = containerName,
                SasToken = "token",
            };
            this.mockProcessor.Setup(m => m.GetFileRegistrationAccessInfoByContainerAsync(containerName)).ReturnsAsync(fileAccessInfo);

            var result = await this.controller.GetFileRegistrationAccessInfoAsync(containerName).ConfigureAwait(false);

            this.mockProcessor.Verify(a => a.GetFileRegistrationAccessInfoByContainerAsync(containerName), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Gets the Read Sas Token for container.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFileRegistration_ShouldInvokeProcess_ToUploadFileAsync()
        {
            var fileAccessInfo = new FileAccessInfo
            {
                AccountName = "baseHost",
                ContainerName = "container",
                SasToken = "token",
            };
            var blobFileName = "movement.xls";
            this.mockProcessor.Setup(m => m.GetFileRegistrationAccessInfoAsync(It.IsAny<string>(), It.IsAny<SystemType>())).ReturnsAsync(fileAccessInfo);
            var result = await this.controller.GetFileRegistrationAccessInfoAsync(blobFileName, It.IsAny<SystemType>()).ConfigureAwait(false);
            this.mockProcessor.Verify(a => a.GetFileRegistrationAccessInfoAsync(It.IsAny<string>(), It.IsAny<SystemType>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }
    }
}
