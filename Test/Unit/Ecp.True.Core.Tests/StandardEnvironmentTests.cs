// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardEnvironmentTests.cs" company="Microsoft">
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
    using Ecp.True.Core.Environment;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The standard environment tests.
    /// </summary>
    [TestClass]
    public class StandardEnvironmentTests
    {
        /// <summary>
        /// Environent names should return valid names when invoked.
        /// </summary>
        [TestMethod]
        public void EnvironentNames_ShouldReturnValidNames_WhenInvoked()
        {
            Assert.AreEqual("Development", StandardEnvironment.Development);
            Assert.AreEqual("Test", StandardEnvironment.Test);
            Assert.AreEqual("Staging", StandardEnvironment.Staging);
            Assert.AreEqual("Performance", StandardEnvironment.Performance);
            Assert.AreEqual("Production", StandardEnvironment.Production);

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", StandardEnvironment.Development);
            Assert.AreEqual(true, StandardEnvironment.IsDevelopment);
            Assert.AreEqual(true, StandardEnvironment.IsDevOrTest);

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", StandardEnvironment.Test);
            Assert.AreEqual(true, StandardEnvironment.IsTest);
            Assert.AreEqual(true, StandardEnvironment.IsDevOrTest);

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", StandardEnvironment.Performance);
            Assert.AreEqual(true, StandardEnvironment.IsPerformance);

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", StandardEnvironment.Production);
            Assert.AreEqual(true, StandardEnvironment.IsProduction);

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", StandardEnvironment.Staging);
            Assert.AreEqual(true, StandardEnvironment.IsStaging);

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", string.Empty);
            Assert.AreNotEqual(true, StandardEnvironment.IsStaging);
        }
    }
}
