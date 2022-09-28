// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeToAddManualMovementsTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Specifications
{
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Builders;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The deltaNodeToAddManualMovementsTests test class.
    /// </summary>
    [TestClass]
    public class DeltaNodeToAddManualMovementsTests
    {
        /// <summary>
        /// The specification.
        /// </summary>
        private DeltaNodeToAddManualMovementsSpec spec;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.spec = new DeltaNodeToAddManualMovementsSpec(MovementBuilder.DeltaNodeId);
        }

        /// <summary>
        /// Should not be satisfied with sent to approval state.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="expectedSatisfied">The expected satisfaction.</param>
        [DataTestMethod]
        [DataRow(OwnershipNodeStatusType.SUBMITFORAPPROVAL, false)]
        [DataRow(OwnershipNodeStatusType.APPROVED, false)]
        [DataRow(OwnershipNodeStatusType.DELTAS, true)]
        [DataRow(OwnershipNodeStatusType.REJECTED, true)]
        [DataRow(OwnershipNodeStatusType.REOPENED, true)]
        public void ShouldNotBeSatisfiedWithSentToApprovalState(OwnershipNodeStatusType status, bool expectedSatisfied)
        {
            // Prepare
            var deltaNode = new DeltaNode
            {
                DeltaNodeId = MovementBuilder.DeltaNodeId,
                Status = status,
            };

            // Execute
            var satisfied = this.spec.IsSatisfiedBy(deltaNode);

            // Assert
            Assert.AreEqual(expectedSatisfied, satisfied);
        }
    }
}