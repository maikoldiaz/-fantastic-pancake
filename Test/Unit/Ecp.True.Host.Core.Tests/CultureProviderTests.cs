// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CultureProviderTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Tests
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The custom validation attribute adapter provider tests.
    /// </summary>
    [TestClass]
    public class CultureProviderTests
    {
        /// <summary>
        /// The english culture.
        /// </summary>
        private const string EnglishCulture = "en-US";

        /// <summary>
        /// The spanish culture.
        /// </summary>
        private const string SpanishCulture = "es-SO";

        /// <summary>
        /// The culture provider.
        /// </summary>
        private CultureProvider cultureProvider;

        /// <summary>
        /// The context.
        /// </summary>
        private HttpContext context;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.context = new DefaultHttpContext();
            this.cultureProvider = new CultureProvider(EnglishCulture, SpanishCulture);
        }

        /// <summary>
        /// Should return english culture when culture header is specified in request headers.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task DetermineProviderCultureResult_ShouldReturnEnglishProvider_WhenHeadersHasEnglishCultureAsync()
        {
            // Arrange
            this.context.Request.Headers.Add("culture", EnglishCulture);

            // Act
            var result = await this.cultureProvider.DetermineProviderCultureResult(this.context).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Cultures.Count);
            Assert.AreEqual(1, result.UICultures.Count);
            Assert.AreEqual(EnglishCulture, result.Cultures[0].Value);
            Assert.AreEqual(EnglishCulture, result.UICultures[0].Value);
        }

        /// <summary>
        /// Should return spanish culture when no culture header is specified in request headers.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task DetermineProviderCultureResult_ShouldReturnSpanishProvider_WhenNoCultureHeaderInRequestAsync()
        {
            // Act
            var result = await this.cultureProvider.DetermineProviderCultureResult(this.context).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Cultures.Count);
            Assert.AreEqual(1, result.UICultures.Count);
            Assert.AreEqual(SpanishCulture, result.Cultures[0].Value);
            Assert.AreEqual(SpanishCulture, result.UICultures[0].Value);
        }
    }
}
