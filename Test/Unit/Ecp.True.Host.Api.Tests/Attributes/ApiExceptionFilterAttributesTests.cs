// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiExceptionFilterAttributesTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core.Entities;
    using Ecp.True.Host.Core;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The api exception filter attributes tests.
    /// </summary>
    [TestClass]
    public sealed class ApiExceptionFilterAttributesTests : ControllerTestBase
    {
        /// <summary>
        /// The json error message.
        /// </summary>
        private readonly string jsonErrorMessage = "source node is not a valid integer";

        /// <summary>
        /// The API exception filter attribute.
        /// </summary>
        private ApiExceptionFilterAttribute apiExceptionFilterAttribute;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.apiExceptionFilterAttribute = new ApiExceptionFilterAttribute();
            this.SetupHttpContext();
        }

        /// <summary>
        /// APIs the exception filter attribute should handle argument null exception when invoked.
        /// </summary>
        [TestMethod]
        public void ApiExceptionFilterAttribute_ShouldHandleArgumentNullException_WhenInvoked()
        {
            var exceptionContext = this.BuildExceptionContext(new ArgumentNullException());

            this.apiExceptionFilterAttribute.OnException(exceptionContext);

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOfType(exceptionContext.Result, typeof(BadRequestResult));
        }

        /// <summary>
        /// APIs the exception filter attribute should handle unauthorized access exception when invoked.
        /// </summary>
        [TestMethod]
        public void ApiExceptionFilterAttribute_ShouldHandleUnauthorizedAccessException_WhenInvoked()
        {
            var exceptionContext = this.BuildExceptionContext(new UnauthorizedAccessException());

            this.apiExceptionFilterAttribute.OnException(exceptionContext);

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOfType(exceptionContext.Result, typeof(Ecp.True.Host.Core.UnauthorizedResult));
        }

        /// <summary>
        /// APIs the exception filter attribute should handle key not found exception when invoked.
        /// </summary>
        [TestMethod]
        public void ApiExceptionFilterAttribute_ShouldHandleKeyNotFoundException_WhenInvoked()
        {
            var expectedErrorInfo = new ErrorInfo(Ecp.True.Entities.Constants.CategoryNotExist);
            var exceptionContext = this.BuildExceptionContext(new KeyNotFoundException(Entities.Constants.CategoryNotExist));

            this.apiExceptionFilterAttribute.OnException(exceptionContext);
            var badRequestObjectResult = exceptionContext.Result as BadRequestObjectResult;
            var errorResponse = badRequestObjectResult?.Value as ErrorResponse;

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOfType(exceptionContext.Result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(badRequestObjectResult.Value);
            Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(ErrorResponse));
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorResponse.ErrorCodes);
            Assert.AreEqual(expectedErrorInfo.Code, errorResponse.ErrorCodes.First().Code);
        }

        [TestMethod]
        public void ApiExceptionFilterAttribute_ShouldHandleSqlUniqueConstraintsException_WhenInvoked()
        {
            var sqlException = this.CreateSqlExceptionStub(DataAccess.Sql.Constants.UniqueConstraintCode, "'Admin.Category'");

            var exceptionContext = this.BuildExceptionContext(new DbUpdateException(string.Empty, sqlException));

            this.apiExceptionFilterAttribute.OnException(exceptionContext);
            var badRequestObjectResult = exceptionContext.Result as BadRequestObjectResult;
            var errorResponse = badRequestObjectResult?.Value as ErrorResponse;

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOfType(exceptionContext.Result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(badRequestObjectResult.Value);
            Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(ErrorResponse));
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorResponse.ErrorCodes);
        }

        /// <summary>
        /// APIs the exception filter attribute should handle SQL identity insert exception when invoked.
        /// </summary>
        [TestMethod]
        public void ApiExceptionFilterAttribute_ShouldHandleSqlIdentityInsertException_WhenInvoked()
        {
            var errorInfo = new ErrorInfo(EntityConstants.DuplicateEntityInsert);
            var sqlException = this.CreateSqlExceptionStub(DataAccess.Sql.Constants.IdentityInsertCode);

            var exceptionContext = this.BuildExceptionContext(new DbUpdateException(string.Empty, sqlException));

            this.apiExceptionFilterAttribute.OnException(exceptionContext);
            var badRequestObjectResult = exceptionContext.Result as BadRequestObjectResult;
            var errorResponse = badRequestObjectResult?.Value as ErrorResponse;

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOfType(exceptionContext.Result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(badRequestObjectResult.Value);
            Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(ErrorResponse));
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorResponse.ErrorCodes);
            Assert.AreEqual(errorInfo.Code, errorResponse.ErrorCodes.First().Code);
        }

        /// <summary>
        /// APIs the exception filter attribute should handle SQL foreign keyconstraints exception when invoked.
        /// </summary>
        [TestMethod]
        public void ApiExceptionFilterAttribute_ShouldHandleSqlForeignKeyconstraintsException_WhenInvoked()
        {
            var errorInfo = new ErrorInfo(EntityConstants.EntityNotExists);
            var sqlException = this.CreateSqlExceptionStub(DataAccess.Sql.Constants.ForeignKeyConstraintCode);

            var exceptionContext = this.BuildExceptionContext(new DbUpdateException(string.Empty, sqlException));

            this.apiExceptionFilterAttribute.OnException(exceptionContext);
            var badRequestObjectResult = exceptionContext.Result as BadRequestObjectResult;
            var errorResponse = badRequestObjectResult?.Value as ErrorResponse;

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOfType(exceptionContext.Result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(badRequestObjectResult.Value);
            Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(ErrorResponse));
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorResponse.ErrorCodes);
            Assert.AreEqual(errorInfo.Code, errorResponse.ErrorCodes.First().Code);
        }

        /// <summary>
        /// APIs the exception filter attribute should handle SQL un handled exception when invoked.
        /// </summary>
        [TestMethod]
        public void ApiExceptionFilterAttribute_ShouldHandleSqlUnHandledException_WhenInvoked()
        {
            var errorNumber = 123456;
            var errorInfo = new ErrorInfo("unknown", null);
            var sqlException = this.CreateSqlExceptionStub(errorNumber);

            var exceptionContext = this.BuildExceptionContext(new DbUpdateException(string.Empty, sqlException));

            this.apiExceptionFilterAttribute.OnException(exceptionContext);
            var badRequestObjectResult = exceptionContext.Result as BadRequestObjectResult;
            var errorResponse = badRequestObjectResult?.Value as ErrorResponse;

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOfType(exceptionContext.Result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(badRequestObjectResult.Value);
            Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(ErrorResponse));
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorResponse.ErrorCodes);
            Assert.AreEqual(errorInfo.Code, errorResponse.ErrorCodes.First().Code);
        }

        [TestMethod]
        public void ApiExceptionFilterAttribute_ShouldHandleSqlException_WhenInvoked()
        {
            var errorNumber = 515;
            var errorInfo = new ErrorInfo(EntityConstants.NotNullConstraintsError);
            var sqlException = this.CreateSqlExceptionStub(errorNumber);

            var exceptionContext = this.BuildExceptionContext(sqlException);
            this.apiExceptionFilterAttribute.OnException(exceptionContext);
            var badRequestObjectResult = exceptionContext.Result as BadRequestObjectResult;
            var errorResponse = badRequestObjectResult?.Value as ErrorResponse;

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOfType(exceptionContext.Result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(badRequestObjectResult.Value);
            Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(ErrorResponse));
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorResponse.ErrorCodes);
            Assert.AreEqual(errorInfo.Code, errorResponse.ErrorCodes.First().Code);
        }

        [TestMethod]
        public void ApiExceptionFilterAttribute_ShouldHandleSqlException_Custom_WhenInvoked()
        {
            var errorNumber = 51001;
            var sqlException = this.CreateSqlExceptionStub(errorNumber, "HOMOLOGATION_CREATE_SAMESYSTEMVALIDATION");

            var exceptionContext = this.BuildExceptionContext(sqlException);
            this.apiExceptionFilterAttribute.OnException(exceptionContext);
            var badRequestObjectResult = exceptionContext.Result as BadRequestObjectResult;
            var errorResponse = badRequestObjectResult?.Value as ErrorResponse;

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOfType(exceptionContext.Result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(badRequestObjectResult.Value);
            Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(ErrorResponse));
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorResponse.ErrorCodes);
        }

        /// <summary>
        /// APIs the exception filter attribute should handle argument null exception when invoked.
        /// </summary>
        [TestMethod]
        public void ApiExceptionFilterAttribute_ShouldHandleJsonReaderException_WhenInvoked()
        {
            var exceptionContext = this.BuildExceptionContext(new JsonReaderException(this.jsonErrorMessage, "sourceNode", 0, 0, null));

            this.apiExceptionFilterAttribute.OnException(exceptionContext);

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOfType(exceptionContext.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// Builds the exception context.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>Return the exception context.</returns>
        private ExceptionContext BuildExceptionContext(Exception exception)
        {
            var actionContext = new ActionContext
            {
                HttpContext = this.HttpContext,
                RouteData = this.RouteData,
                ActionDescriptor = this.ActionDescriptor,
            };

            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                HttpContext = this.HttpContext,
                Exception = exception,
                RouteData = this.RouteData,
                ActionDescriptor = this.ActionDescriptor,
            };

            return exceptionContext;
        }
    }
}
