// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonDateConverterTests.cs" company="Microsoft">
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
    using Ecp.True.Core.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The date converter tests.
    /// </summary>
    [TestClass]
    public class JsonDateConverterTests
    {
        /// <summary>
        /// Jsons the date converter tests should use default date format when format is not specified.
        /// </summary>
        [TestMethod]
        public void JsonDateConverterTests_ShouldUseDefaultDateFormat_WhenFormatIsNotSpecified()
        {
            var converter = new JsonDateConverter();
            Assert.AreEqual("yyyy-MM-ddTHH:mm:ssZ", converter.DateTimeFormat);
        }

        /// <summary>
        /// Jsons the date converter tests should use specified date format when format is specified.
        /// </summary>
        [TestMethod]
        public void JsonDateConverterTests_ShouldUseSpecifiedDateFormat_WhenFormatIsSpecified()
        {
            var converter = new JsonDateConverter("dd-MM-yyyyTHH:mm:ssZ");
            Assert.AreEqual("dd-MM-yyyyTHH:mm:ssZ", converter.DateTimeFormat);
        }
    }
}