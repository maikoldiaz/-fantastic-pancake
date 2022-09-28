// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChaosManagerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Chaos.Tests
{
    using System.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The chaos manager tests.
    /// </summary>
    [TestClass]
    public class ChaosManagerTests
    {
        /// <summary>
        /// The manager.
        /// </summary>
        private ChaosManager manager;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.manager = new ChaosManager();
        }

        /// <summary>
        /// Determines whether [has chaos should return true when chaos is expected].
        /// </summary>
        [TestMethod]
        public void HasChaos_ShouldReturnTrue_WhenChaosIsExpected()
        {
            this.manager.Initialize(ChaosType.Api.ToString());
            Assert.IsTrue(this.manager.HasChaos);
        }

        /// <summary>
        /// Determines whether [has chaos should return false when chaos is not expected].
        /// </summary>
        [TestMethod]
        public void HasChaos_ShouldReturnFalse_WhenChaosIsNotExpected()
        {
            Assert.IsFalse(this.manager.HasChaos);
        }

        /// <summary>
        /// Chaoses the value should not return value when chaos is not expected.
        /// </summary>
        [TestMethod]
        public void ChaosValue_ShouldNotReturnValue_WhenChaosIsNotExpected()
        {
            Assert.IsNull(this.manager.ChaosValue);
        }

        /// <summary>
        /// Chaoses the value should return value when chaos is expected.
        /// </summary>
        [TestMethod]
        public void ChaosValue_ShouldReturnValue_WhenChaosIsExpected()
        {
            this.manager.Initialize(ChaosType.Api.ToString());
            Assert.AreEqual(ChaosType.Api.ToString(), this.manager.ChaosValue);
        }

        /// <summary>
        /// Tries the trigger chaos should create chaos in API when chaos is API.
        /// </summary>
        [TestMethod]
        public void TryTriggerChaos_ShouldCreateChaosInApi_WhenChaosIsApi()
        {
            this.manager.Initialize(ChaosType.Api.ToString());
            var message = this.manager.TryTriggerChaos(ChaosType.Api.ToString());

            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, Constants.ErrorMessage, ChaosType.Api.ToString()), message);
        }

        /// <summary>
        /// Tries the trigger chaos should create chaos in web when chaos is web.
        /// </summary>
        [TestMethod]
        public void TryTriggerChaos_ShouldCreateChaosInWeb_WhenChaosIsWeb()
        {
            this.manager.Initialize(ChaosType.Web.ToString());
            var message = this.manager.TryTriggerChaos(ChaosType.Web.ToString());

            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, Constants.ErrorMessage, ChaosType.Web.ToString()), message);
        }

        /// <summary>
        /// Tries the trigger chaos should create chaos in services when chaos is in services.
        /// </summary>
        [TestMethod]
        public void TryTriggerChaos_ShouldCreateChaosInServices_WhenChaosIsInServices()
        {
            this.manager.Initialize("approvals");
            var message = this.manager.TryTriggerChaos("approvals");

            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, Constants.ErrorMessage, "approvals"), message);
        }

        /// <summary>
        /// Tries the trigger chaos should not create chaos when chaos caller is unknown.
        /// </summary>
        [TestMethod]
        public void TryTriggerChaos_ShouldNotCreateChaos_WhenChaosCallerIsUnknown()
        {
            this.manager.Initialize("approvals");
            var message = this.manager.TryTriggerChaos("test");

            Assert.IsNull(message);
        }

        /// <summary>
        /// Tries the trigger chaos should not create chaos when chaos is unknown.
        /// </summary>
        [TestMethod]
        public void TryTriggerChaos_ShouldNotCreateChaos_WhenChaosIsUnknown()
        {
            var message = this.manager.TryTriggerChaos("approvals");
            Assert.IsNull(message);
        }
    }
}
