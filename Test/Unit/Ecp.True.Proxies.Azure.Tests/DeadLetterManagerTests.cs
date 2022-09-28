// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeadLetterManagerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The deadletter manager tests.
    /// </summary>
    [TestClass]
    public class DeadLetterManagerTests
    {
        /// <summary>
        /// The manager.
        /// </summary>
        private DeadLetterManager manager;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.manager = new DeadLetterManager();
        }

        /// <summary>
        /// Determines whether [has chaos should return true when chaos is expected].
        /// </summary>
        [TestMethod]
        public void IsDeadLettered_ShouldReturnTrue_WhenInitialized()
        {
            this.manager.Initialize(true);
            Assert.IsTrue(this.manager.IsDeadLettered);
        }
    }
}