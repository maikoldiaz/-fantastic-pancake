// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterceptionTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Ioc.Tests
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Ioc.Tests.Types.Core;
    using Ecp.True.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The interceptor tests.
    /// </summary>
    [TestClass]
    [CLSCompliant(false)]
    public class InterceptionTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private static Mock<ITelemetry> telemetryMock;

        /// <summary>
        /// The container.
        /// </summary>
        private static IContainer container;

        /// <summary>
        /// Cleanups the specified test context.
        /// </summary>
        [ClassCleanup]
        public static void Cleanup()
        {
            container.Dispose();
        }

        /// <summary>
        /// Initializes the specified test context.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            telemetryMock = new Mock<ITelemetry>();
            var mapping = new Dictionary<Type, object>
            {
                { typeof(ITelemetry), telemetryMock.Object },
            };

            if (testContext != null)
            {
                container = IoCManager.RegisterByConvention(null, null, mapping);
            }
        }

        /// <summary>
        /// Resolve should build new instance when resolving transient type.
        /// </summary>
        [TestMethod]
        [Ignore("To Be Fixed Later")]
        public void Should_Log_Method_Entry_And_Exit_Only_If_Instrumentation_Is_Enabled()
        {
            // Added both scenarios in same unit test as only mock logger can only be registered as singleton.
            container.Resolve<IClassWithoutInterception>().Test();

            telemetryMock.Verify(
                c => c.TrackMetric(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<double>(),
                    It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()), Times.Never());

            container.Resolve<IClassWithInterception>().Test();

            telemetryMock.Verify(
                c => c.TrackMetric(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<double>(),
                    It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()), Times.Exactly(2));
        }
    }
}