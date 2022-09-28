// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllClassesTests.cs" company="Microsoft">
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
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The IOC tests.
    /// </summary>
    [TestClass]
    [CLSCompliant(false)]
    public class AllClassesTests
    {
        /// <summary>
        /// Get all assemblies.
        /// </summary>
        [TestMethod]
        public void GetAllAssemblies()
        {
            var allClasses = AllClasses.FromAssembliesInBasePath();
            Assert.IsNotNull(allClasses);
        }

        /// <summary>
        /// Get from assemblies.
        /// </summary>
        [TestMethod]
        public void GetFromAssemblies()
        {
            var basePathAssemblies = new List<Assembly>();
            var callingAssembly = Assembly.GetEntryAssembly();
            basePathAssemblies.Add(callingAssembly);
            var allClasses = AllClasses.FromAssemblies(basePathAssemblies);
            Assert.IsNotNull(allClasses);
        }

        /// <summary>
        /// Get exception from assemblies.
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetExceptionFromAssemblies()
        {
            AllClasses.FromAssemblies(null);
        }
    }
}