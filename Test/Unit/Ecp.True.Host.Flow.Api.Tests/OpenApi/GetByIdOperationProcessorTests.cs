// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetByIdOperationProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Flow.Api.Tests.OpenApi
{
    using System;
    using Ecp.True.Host.Flow.Api.Controllers;
    using Ecp.True.Host.Shared.OpenApi;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSwag.Generation.Processors.Contexts;

    /// <summary>
    /// The ODATA operation processor tests.
    /// </summary>
    [TestClass]
    public class GetByIdOperationProcessorTests
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private GetByIdOperationProcessor processor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.processor = new GetByIdOperationProcessor();
        }

        /// <summary>
        /// Process should update operation when operation contains get by id.
        /// </summary>
        [TestMethod]
        public void Process_ShouldUpdateOperation_WhenOperationContainGetById()
        {
            var context = this.BuildContext(typeof(OwnershipController), "GetNodeOwnershipByIdAsync", "Ownership_GetNodeOwnershipById");

            var response200 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("200", response200);

            var response404 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("404", response404);

            var response500 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("500", response500);

            var result = this.processor.Process(context);

            Assert.IsTrue(result);
            Assert.AreEqual("Gets the node ownership by node ownership id", context.OperationDescription.Operation.Summary);
            Assert.AreEqual("The node ownership is found.", response200.Description);
            Assert.AreEqual("The node ownership is not found.", response404.Description);
            Assert.AreEqual("Unknown error while getting node ownership.", response500.Description);
        }

        private OperationProcessorContext BuildContext(Type controller, string methodName, string operationId)
        {
            var methodInfo = controller.GetMethod(methodName);
            var document = new NSwag.OpenApiDocument();
            var description = new NSwag.OpenApiOperationDescription();
            description.Operation = new NSwag.OpenApiOperation
            {
                Summary = "Summary",
                Description = "Description",
                OperationId = operationId,
            };

            return new OperationProcessorContext(
                document,
                description,
                controller,
                methodInfo,
                null,
                null,
                null,
                null,
                null);
        }
    }
}
