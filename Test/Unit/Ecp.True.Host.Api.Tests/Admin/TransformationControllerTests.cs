// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformationControllerTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The transformation controller test class.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public class TransformationControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private TransformationController controller;

        /// <summary>
        /// The processor.
        /// </summary>
        private Mock<ITransformationProcessor> processor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.processor = new Mock<ITransformationProcessor>();
            this.controller = new TransformationController(this.processor.Object);
        }

        /// <summary>
        /// Creates the transformation asynchronous should invoke processor to return200 success asynchronous.
        /// </summary>
        /// <returns>The create transformation result.</returns>
        [TestMethod]
        public async Task CreateTransformationAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var transformation = new TransformationDto();
            this.processor.Setup(m => m.CreateTransformationAsync(transformation));

            var result = await this.controller.CreateTransformationAsync(transformation).ConfigureAwait(false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.processor.Verify(c => c.CreateTransformationAsync(transformation), Times.Once());
        }

        /// <summary>
        /// Updates the transformation asynchronous should invoke processor to return200 success asynchronous.
        /// </summary>
        /// <returns>The create transformation result.</returns>
        [TestMethod]
        public async Task UpdateTransformationAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var transformation = new TransformationDto();
            this.processor.Setup(m => m.UpdateTransformationAsync(transformation));

            var result = await this.controller.UpdateTransformationAsync(transformation).ConfigureAwait(false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.processor.Verify(c => c.UpdateTransformationAsync(transformation), Times.Once());
        }

        /// <summary>
        /// Queries the transformation asynchronous should invoke processor to return200 success asynchronous.
        /// </summary>
        /// <returns>The query transformation result.</returns>
        [TestMethod]
        public async Task QueryTransformationAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var elements = new[] { new Transformation() }.AsQueryable();
            this.processor.Setup(m => m.QueryAllAsync<Transformation>(null)).ReturnsAsync(elements);

            var result = await this.controller.QueryTransformationAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, elements);

            this.processor.Verify(m => m.QueryAllAsync<Transformation>(null), Times.Once());
        }

        /// <summary>
        /// Deletes the transformation asynchronous should invoke processor to delete transformation success asynchronous.
        /// </summary>
        /// <returns>The deletion.</returns>
        [TestMethod]
        public async Task DeleteTransformationAsync_ShouldInvokeProcessor_ToDeleteTransformationSuccessAsync()
        {
            var deleteTransformation = new DeleteTransformation()
            {
                TransformationId = It.IsAny<int>(),
                RowVersion = It.IsAny<byte[]>(),
            };

            this.processor.Setup(x => x.DeleteTransformationAsync(deleteTransformation));
            var result = await this.controller.DeleteTransformationAsync(deleteTransformation).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Messagekey;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.IsNotNull(returnMessage, "Delete succussfull message should not be null");
            Assert.AreEqual(returnMessage, Entities.Constants.TransformationDeletedSuccessfully);
            this.processor.Verify(x => x.DeleteTransformationAsync(deleteTransformation), Times.Once);
        }

        /// <summary>
        /// Exist transformation asynchronous should invoke processor to return validation result asynchronous.
        /// </summary>
        /// <returns>The validation result.</returns>
        [TestMethod]
        public async Task ExistTransformationAsync_ShouldInvokeProcessor_ToReturnValidationResultAsync()
        {
            var validationResult = new Mock<IEnumerable<Transformation>>();
            var transformationDto = new TransformationDto();
            this.processor.Setup(m => m.ExistsTransformationAsync(It.IsAny<TransformationDto>())).Returns(Task.FromResult(validationResult.Object));
            var result = await this.controller.ExistsTransformationAsync(transformationDto).ConfigureAwait(false);

            this.processor.Verify(c => c.ExistsTransformationAsync(It.IsAny<TransformationDto>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Get transformation info asynchronous should invoke processor to return transformation info result asynchronous.
        /// </summary>
        /// <returns>The validation result.</returns>
        [TestMethod]
        public async Task GetTransformationInfoAsync_ShouldInvokeProcessor_ToReturnGetTransformationInfoAsync()
        {
            this.processor.Setup(x => x.GetTransformationInfoAsync(It.IsAny<int>()));

            var result = await this.controller.GetTransformationInfoAsync(It.IsAny<int>()).ConfigureAwait(false);
            var objectResult = result as EntityResult;
            var returnMessage = objectResult.Messagekey;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(objectResult, "Object result should not be null");
            Assert.AreEqual(returnMessage, Entities.Constants.TransformationDoesNotExists);
            this.processor.Verify(x => x.GetTransformationInfoAsync(It.IsAny<int>()), Times.Once);
        }
    }
}