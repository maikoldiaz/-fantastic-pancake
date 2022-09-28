// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateEnumAttributeTests.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Enumeration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The validate enum tests.
    /// </summary>
    [TestClass]
    public class ValidateEnumAttributeTests
    {
        /// <summary>
        /// Validates the enum attribute tests validate scenario type when invalid value is specified.
        /// </summary>
        [TestMethod]
        public void ValidateEnumAttributeTests_ValidateScenarioType_WhenInvalidValueIsSpecified()
        {
            var attribute = new ValidateEnumAttribute(typeof(ScenarioType), SapConstants.ScenarioIdValueRangeFailed);
            var result = attribute.GetValidationResult("123", new ValidationContext("123"));
            Assert.IsNotNull(result);
            Assert.AreEqual(SapConstants.ScenarioIdValueRangeFailed, result.ErrorMessage);
        }

        /// <summary>
        /// Validates the enum attribute tests validate scenario type when valid value is specified.
        /// </summary>
        [TestMethod]
        public void ValidateEnumAttributeTests_ValidateScenarioType_WhenValidValueIsSpecified()
        {
            var attribute = new ValidateEnumAttribute(typeof(ScenarioType), SapConstants.ScenarioIdValueRangeFailed);
            var result = attribute.GetValidationResult("1", new ValidationContext("1"));
            Assert.IsNull(result);
        }
    }
}