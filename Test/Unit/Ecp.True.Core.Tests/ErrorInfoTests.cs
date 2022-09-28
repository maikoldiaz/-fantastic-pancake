// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorInfoTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Tests
{
    using System;
    using Ecp.True.Core.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The error info tests.
    /// </summary>
    [TestClass]
    public class ErrorInfoTests
    {
        /// <summary>
        /// Errors the information should throw argument null exception when error string is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorInfo_ShouldThrowArgumentNullException_WhenErrorStringIsNull()
        {
            string error = null;
            var errorInfo = new ErrorInfo(error);

            Assert.IsNull(errorInfo);
        }

        /// <summary>
        /// Errors the information should throw argument null exception when error string is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorInfo_ShouldThrowArgumentNullException_WhenErrorStringIsEmpty()
        {
            var error = string.Empty;
            var errorInfo = new ErrorInfo(error);

            Assert.IsNull(errorInfo);
        }

        /// <summary>
        /// Errors the information should throw argument null exception when error string is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorInfo_ShouldThrowArgumentNullException_WhenErrorStringIsWhiteSpace()
        {
            string error = " ";
            var errorInfo = new ErrorInfo(error);

            Assert.IsNull(errorInfo);
        }

        /// <summary>
        /// Errors the information should return unknown error code when error string does not have code.
        /// </summary>
        [TestMethod]
        public void ErrorInfo_ShouldReturnUnknownErrorCode_WhenErrorStringDoesNotHaveCode()
        {
            string error = "Some Error Without Code";
            var errorInfo = new ErrorInfo(error);

            Assert.IsNotNull(errorInfo);
            Assert.AreEqual("unknown", errorInfo.Code);
            Assert.AreEqual(error, errorInfo.Message);
        }

        /// <summary>
        /// Errors the information should return error code when error string has code.
        /// </summary>
        [TestMethod]
        public void ErrorInfo_ShouldReturnErrorCode_WhenErrorStringHasCode()
        {
            string error = "5009-Some Error With Code";
            var errorInfo = new ErrorInfo(error);

            Assert.IsNotNull(errorInfo);
            Assert.AreEqual("5009", errorInfo.Code);
            Assert.AreEqual("Some Error With Code", errorInfo.Message);
        }

        /// <summary>
        /// Errors the information should return error code when code is set.
        /// </summary>
        [TestMethod]
        public void ErrorInfo_ShouldReturnErrorCode_WhenCodeIsSet()
        {
            string error = "5009-Some Error Without Code";
            var info = new ErrorInfo(error);

            var errorInfo = new ErrorInfo(info.Code, info.Message);

            Assert.IsNotNull(errorInfo);
            Assert.AreEqual(info.Code, errorInfo.Code);
            Assert.AreEqual(info.Message, errorInfo.Message);
        }
    }
}
