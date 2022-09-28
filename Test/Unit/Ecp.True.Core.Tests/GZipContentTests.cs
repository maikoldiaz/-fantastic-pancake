// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GZipContentTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The legacy http client provider tests.
    /// </summary>
    [TestClass]
    public class GZipContentTests
    {
        /// <summary>
        /// the content of the zip content should be encoded.
        /// </summary>
        [TestMethod]
        public void GZipContent_ShouldEncodeTheContent()
        {
            using (var content = new StringContent("Test Content", Encoding.UTF8, "application/json"))
            {
                using (var gzipContent = new GzipContent(content))
                {
                    Assert.IsNotNull(gzipContent);
                    Assert.IsTrue(gzipContent.Headers.ContentEncoding.Any(x => x == Constants.GzipContent));
                }
            }
        }
    }
}