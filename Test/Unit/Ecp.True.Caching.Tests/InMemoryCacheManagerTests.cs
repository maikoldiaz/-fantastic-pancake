// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryCacheManagerTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Caching.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// In Memory cache manager tests.
    /// </summary>
    [TestClass]
    public class InMemoryCacheManagerTests
    {
        /// <summary>
        /// The manager should add data in cache.
        /// </summary>
        [TestMethod]
        public void Manager_ShouldAddDataInCache_WhenInvoked()
        {
            // Arrange
            var testData = "TestData";
            var testDataKey = "TestDataKey";

            // Act
            InMemoryCacheManager.Add(testDataKey, testData);

            // Assert
            var result = InMemoryCacheManager.Get<string>(testDataKey);
            Assert.AreEqual(testData, result);
        }

        /// <summary>
        /// The manager should add data in cache when invoked with absolute expiry.
        /// </summary>
        [TestMethod]
        public void Manager_ShouldAddDataInCache_WhenInvokedWithAbsoltueExpiry()
        {
            // Arrange
            var testData = "TestData";
            var testDataKey = "TestDataKey";

            // Act
            InMemoryCacheManager.Add(testDataKey, testData, 30);

            // Assert
            var result = InMemoryCacheManager.Get<string>(testDataKey);
            Assert.AreEqual(testData, result);
        }

        /// <summary>
        /// The manager should add data in cache when invoked with sliding expiry.
        /// </summary>
        [TestMethod]
        public void Manager_ShouldAddDataInCache_WhenInvokedWithSlidingExpiry()
        {
            // Arrange
            var testData = "TestData";
            var testDataKey = "TestDataKey";

            // Act
            InMemoryCacheManager.Add(testDataKey, testData, 30, true);

            // Assert
            var result = InMemoryCacheManager.Get<string>(testDataKey);
            Assert.AreEqual(testData, result);
        }

        /// <summary>
        /// The manager should remove data from cache.
        /// </summary>
        [TestMethod]
        public void Manager_ShouldRemoveDataFromCache_WhenInvoked()
        {
            // Arrange
            var testData = "TestData";
            var testDataKey = "TestDataKey";
            InMemoryCacheManager.Add(testDataKey, testData);

            // Act
            InMemoryCacheManager.Remove(testDataKey);

            // Assert
            var result = InMemoryCacheManager.Get<string>(testDataKey);
            Assert.IsNull(result);
        }
    }
}