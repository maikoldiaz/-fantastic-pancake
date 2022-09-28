// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateUpdateOperationProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Tests.OpenApi
{
    using System;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Shared.OpenApi;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSwag.Generation.Processors.Contexts;

    /// <summary>
    /// The ODATA operation processor tests.
    /// </summary>
    [TestClass]
    public class CreateUpdateOperationProcessorTests
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private ModifyOperationProcessor processor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.processor = new ModifyOperationProcessor();
        }

        /// <summary>
        /// Process should not update operation when operation does not begin with query.
        /// </summary>
        [TestMethod]
        public void Process_ShouldNotUpdateOperation_WhenOperationDoesNotBeginWithQuery()
        {
            var context = this.BuildContext(typeof(NodeController), "GetNodeByIdAsync", "Node_GetNodeById");
            var result = this.processor.Process(context);

            Assert.IsTrue(result);
            Assert.AreEqual("Summary", context.OperationDescription.Operation.Summary);
        }

        /// <summary>
        /// Process should update operation when operation begin with create.
        /// </summary>
        [TestMethod]
        public void Process_ShouldUpdateOperation_WhenOperationBeginWithCreate()
        {
            var context = this.BuildContext(typeof(NodeController), "CreateNodeAsync", "Node_CreateNode");

            var response200 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("200", response200);

            var response400 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("400", response400);

            var response500 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("500", response500);

            var result = this.processor.Process(context);

            Assert.IsTrue(result);
            Assert.AreEqual("Creates a new node", context.OperationDescription.Operation.Summary);
            Assert.AreEqual("The node was created successfully.", response200.Description);
            Assert.AreEqual("The node has missing/invalid values.", response400.Description);
            Assert.AreEqual("Unknown error while creating node", response500.Description);
        }

        /// <summary>
        /// Process should update operation when operation begin with update.
        /// </summary>
        [TestMethod]
        public void Process_ShouldUpdateOperation_WhenOperationBeginWithUpdate()
        {
            var context = this.BuildContext(typeof(NodeController), "UpdateNodeAsync", "Node_UpdateNode");

            var response200 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("200", response200);

            var response400 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("400", response400);

            var response500 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("500", response500);

            var result = this.processor.Process(context);

            Assert.IsTrue(result);
            Assert.AreEqual("Updates an existing node", context.OperationDescription.Operation.Summary);
            Assert.AreEqual("The node was updated successfully.", response200.Description);
            Assert.AreEqual("The node has missing/invalid values.", response400.Description);
            Assert.AreEqual("Unknown error while updating node", response500.Description);
        }

        /// <summary>
        /// Process should update operation when operation begin with update.
        /// </summary>
        [TestMethod]
        public void Process_ShouldUpdateOperation_WhenOperationBeginWithDelete()
        {
            var context = this.BuildContext(typeof(NodeConnectionController), "DeleteNodeConnectionAsync", "Node_DeleteNodeConnection");

            var response200 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("200", response200);

            var response400 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("400", response400);

            var response500 = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("500", response500);

            var result = this.processor.Process(context);

            Assert.IsTrue(result);
            Assert.AreEqual("Deletes an existing node connection", context.OperationDescription.Operation.Summary);
            Assert.AreEqual("The node connection was deleted successfully.", response200.Description);
            Assert.AreEqual("The node connection has missing/invalid values.", response400.Description);
            Assert.AreEqual("Unknown error while deleting node connection", response500.Description);
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
