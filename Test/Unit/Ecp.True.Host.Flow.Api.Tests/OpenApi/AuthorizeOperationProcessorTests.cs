// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorizeOperationProcessorTests.cs" company="Microsoft">
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
    using System.Linq;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Flow.Api.Controllers;
    using Ecp.True.Host.Shared.OpenApi;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSwag.Generation.Processors.Contexts;

    /// <summary>
    /// The AuthorizeOperationProcessorTests.
    /// </summary>
    [TestClass]
    public class AuthorizeOperationProcessorTests
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private AuthorizeOperationProcessor processor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.processor = new AuthorizeOperationProcessor();
        }

        /// <summary>
        /// Processes the should update security roles when operation contains true authorize attribute.
        /// </summary>
        [TestMethod]
        public void Process_ShouldUpdateSecurityRoles_WhenOperationContainsTrueAuthorizeAttribute()
        {
            var context = this.BuildContext(typeof(OwnershipController), "UpdateNodeOwnershipAsync", "Ownership_UpdateNodeOwnership");
            var result = this.processor.Process(context);

            Assert.IsTrue(result);
            Assert.AreEqual("Summary", context.OperationDescription.Operation.Summary);
            Assert.IsNotNull(context.OperationDescription.Operation.Security);
            Assert.IsNotNull(context.OperationDescription.Operation.Security.FirstOrDefault());
            Assert.AreEqual(HostConstants.FlowRoleClaimType, context.OperationDescription.Operation.Security.FirstOrDefault().FirstOrDefault().Value.FirstOrDefault());
        }

        /// <summary>
        /// Builds the context.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="operationId">The operation identifier.</param>
        /// <returns>The OperationProcessorContext.</returns>
        private OperationProcessorContext BuildContext(Type controller, string methodName, string operationId)
        {
            var methodInfo = controller.GetMethod(methodName);
            var document = new NSwag.OpenApiDocument();
            var description = new NSwag.OpenApiOperationDescription
            {
                Operation = new NSwag.OpenApiOperation
                {
                    Summary = "Summary",
                    Description = "Description",
                    OperationId = operationId,
                },
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
