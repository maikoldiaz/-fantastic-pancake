// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorResponseTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Entity
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The error response tests.
    /// </summary>
    [TestClass]
    public sealed class ErrorResponseTests
    {
        /// <summary>
        /// Verifies the contructor with parameter of list of error information to create valid request identifier with correlation manager and error codes.
        /// </summary>
        [TestMethod]
        public void VerifyContructorWithParameterOfListOfErrorInfo_ToCreateValidRequestIdWithCorrelationManagerAndErrorCodes()
        {
            // Arrage
            var expectedErrorCode = "1001";
            var expectedErrorMessage = "Test Error Message";
            var errors = new List<ErrorInfo>() { new ErrorInfo(string.Concat(expectedErrorCode, "-", expectedErrorMessage)) };
            var resposne = new ErrorResponse(errors);

            // Assert
            Assert.IsNotNull(resposne, "Response should not be null");
            Assert.IsNotNull(resposne.ErrorCodes, "Error codes should not be null");
            Assert.IsTrue(resposne.ErrorCodes.First().Code == expectedErrorCode, "Error codes are not as expected.");
            Assert.IsTrue(resposne.ErrorCodes.First().Message == expectedErrorMessage, "Error message are not as expected.");
        }

        /// <summary>
        /// Verifies the contructor with parameter of error as string to create valid request identifier with correlation manager and error codes.
        /// </summary>
        [TestMethod]
        public void VerifyContructorWithParameterOfErrorAsString_ToCreateValidRequestIdWithCorrelationManagerAndErrorCodes()
        {
            // Arrage
            var expectedErrorCode = "1001";
            var expectedErrorMessage = "Test Error Message";
            var error = string.Concat(expectedErrorCode, "-", expectedErrorMessage);
            var resposne = new ErrorResponse(error);

            // Assert
            Assert.IsNotNull(resposne, "Response should not be null");
            Assert.IsNotNull(resposne.ErrorCodes, "Error codes should not be null");
            Assert.IsTrue(resposne.ErrorCodes.First().Code == expectedErrorCode, "Error codes are not as expected.");
            Assert.IsTrue(resposne.ErrorCodes.First().Message == expectedErrorMessage, "Error message are not as expected.");
        }

        /// <summary>
        /// Verifies the contructor with parameter of error information to create valid request identifier with correlation manager and error codes.
        /// </summary>
        [TestMethod]
        public void VerifyContructorWithParameterOfErrorInfo_ToCreateValidRequestIdWithCorrelationManagerAndErrorCodes()
        {
            // Arrage
            var expectedErrorCode = "1001";
            var expectedErrorMessage = "Test Error Message";
            var errorInfo = new ErrorInfo(string.Concat(expectedErrorCode, "-", expectedErrorMessage));
            var resposne = new ErrorResponse(errorInfo);

            // Assert
            Assert.IsNotNull(resposne, "Response should not be null");
            Assert.IsNotNull(resposne.ErrorCodes, "Error codes should not be null");
            Assert.IsTrue(resposne.ErrorCodes.First().Code == expectedErrorCode, "Error codes are not as expected.");
            Assert.IsTrue(resposne.ErrorCodes.First().Message == expectedErrorMessage, "Error message are not as expected.");
        }

        /// <summary>
        /// Verifies the contructor with parameter of invalid error string to create valid request identifier with correlation manager and error codes.
        /// </summary>
        [TestMethod]
        public void VerifyContructorWithParameterOfInvalidErrorString_ToCreateValidRequestIdWithCorrelationManagerAndErrorCodes()
        {
            // Arrage
            var expectedCode = "unknown";
            var expectedErrorMessage = "Test Error Message";
            var errorInfo = new ErrorInfo(expectedErrorMessage);
            var resposne = new ErrorResponse(errorInfo);

            // Assert
            Assert.IsNotNull(resposne, "Response should not be null");
            Assert.IsNotNull(resposne.ErrorCodes, "Error codes should not be null");
            Assert.IsTrue(resposne.ErrorCodes.First().Code == expectedCode, "Error codes are not as expected.");
            Assert.IsTrue(resposne.ErrorCodes.First().Message == expectedErrorMessage, "Error message are not as expected.");
        }
    }
}
