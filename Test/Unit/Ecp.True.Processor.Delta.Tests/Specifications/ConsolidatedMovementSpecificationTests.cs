// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementSpecificationTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Delta.Tests.Specifications
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Delta.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The consolidatedMovementSpecificationTestsTests test class.
    /// </summary>
    [TestClass]
    public class ConsolidatedMovementSpecificationTests
    {
        /// <summary>
        /// ShouldInclude_ConsolidatedOwners.
        /// </summary>
        [TestMethod]
        public void ShouldInclude_ConsolidatedOwners()
        {
            // Arrange
            var spec = new ConsolidatedMovementSpecification(new Ticket(), new List<int>());

            // Act
            var includes = spec.IncludeProperties;

            // Assert
            Assert.IsTrue(includes.Contains($"{nameof(ConsolidatedMovement.ConsolidatedOwners)}"));
        }
    }
}