// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentValidatorTests.cs" company="Microsoft">
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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The argument validator tests.
    /// </summary>
    [TestClass]
    public class ArgumentValidatorTests
    {
        /// <summary>
        /// Throws if null should throw exception when parameter is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfNull_ShouldThrowException_WhenParameterIsNull()
        {
            var parameter = default(Exception);
            ArgumentValidators.ThrowIfNull(parameter, nameof(parameter));
        }

        /// <summary>
        /// Throws if null or empty should throw exception when string is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfNullOrEmpty_ShouldThrowException_WhenStringIsNull()
        {
            string parameter = null;
            ArgumentValidators.ThrowIfNullOrEmpty(parameter, nameof(parameter));
        }

        /// <summary>
        /// Throws if null or empty should throw exception when string is empty.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfNullOrEmpty_ShouldThrowException_WhenStringIsEmpty()
        {
            var parameter = string.Empty;
            ArgumentValidators.ThrowIfNullOrEmpty(parameter, nameof(parameter));
        }

        /// <summary>
        /// Throws if null or empty should throw exception when string is white space.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfNullOrEmpty_ShouldThrowException_WhenStringIsWhiteSpace()
        {
            string parameter = " ";
            ArgumentValidators.ThrowIfNullOrEmpty(parameter, nameof(parameter));
        }
    }
}
