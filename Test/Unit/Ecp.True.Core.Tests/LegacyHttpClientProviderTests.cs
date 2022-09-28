// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LegacyHttpClientProviderTests.cs" company="Microsoft">
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
    /// The legacy http client provider tests.
    /// </summary>
    [TestClass]
    public class LegacyHttpClientProviderTests
    {
        /// <summary>
        /// Legacies the HTTP client provider should use cached HTTP client when HTTP client is accessed.
        /// </summary>
        [TestMethod]
        public void LegacyHttpClientProvider_ShouldUseCachedHttpClient_WhenHttpClientIsAccessed()
        {
            var address = new Uri("http://www.microsoft.com");
            var httpClient = LegacyHttpClientProvider.Current;
            httpClient.BaseAddress = address;

            Assert.AreEqual(address, LegacyHttpClientProvider.Current.BaseAddress);
        }
    }
}