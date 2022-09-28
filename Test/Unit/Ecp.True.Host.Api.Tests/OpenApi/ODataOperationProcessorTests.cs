// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ODataOperationProcessorTests.cs" company="Microsoft">
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
    public class ODataOperationProcessorTests
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private ODataOperationProcessor processor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.processor = new ODataOperationProcessor();
        }

        /// <summary>
        /// Process should not update operation when operation does not begin with query.
        /// </summary>
        [TestMethod]
        public void Process_ShouldNotUpdateOperation_WhenOperationDoesNotBeginWithQuery()
        {
            var context = this.BuildContext(typeof(NodeController), "CreateNodeAsync", "Node_CreateNode");
            var result = this.processor.Process(context);

            Assert.IsTrue(result);
            Assert.AreEqual("Summary", context.OperationDescription.Operation.Summary);
            Assert.AreEqual("Description", context.OperationDescription.Operation.Description);
        }

        /// <summary>
        /// Process should update operation when operation begin with query.
        /// </summary>
        [TestMethod]
        public void Process_ShouldUpdateOperation_WhenOperationBeginWithQuery()
        {
            var context = this.BuildContext(typeof(NodeController), "QueryNodesAsync", "Node_QueryNodes");
            var response = new NSwag.OpenApiResponse();
            context.OperationDescription.Operation.Responses.Add("200", response);

            var result = this.processor.Process(context);

            Assert.IsTrue(result);
            Assert.AreEqual("Gets all the nodes, the method supports ODATA", context.OperationDescription.Operation.Summary);
            Assert.AreEqual("The ODATA query response", response.Description);
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
