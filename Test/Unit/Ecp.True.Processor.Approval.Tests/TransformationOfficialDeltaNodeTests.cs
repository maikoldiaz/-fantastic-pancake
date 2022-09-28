// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformationOfficialDeltaNodeTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Approval.Tests
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Approval.Transformation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Transformation Official Delta Node tests.
    /// </summary>
    [TestClass]
    public class TransformationOfficialDeltaNodeTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private TransformationOfficialDeltaNode controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<ITrueLogger<TransformationOfficialDeltaNode>> mockTrueLogger;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockTrueLogger = new Mock<ITrueLogger<TransformationOfficialDeltaNode>>();
            this.controller = new TransformationOfficialDeltaNode(this.mockTrueLogger.Object);
        }

        /// <summary>
        /// Apply Transformation Official Delta.
        /// </summary>
        [TestMethod]
        public void ApplyTransformationOfficialDelta_ShouldInitializationTransformedMovementDeltaInventory()
        {
            // Arrange
            IEnumerable<OfficialDeltaNodeMovement> listOfficialDeltaNodeMovements = new List<OfficialDeltaNodeMovement>() { new OfficialDeltaNodeMovement() { MovementTypeId = 187 } };

            // Act
            var valueReturn = this.controller.ApplyTransformationOfficialDelta(listOfficialDeltaNodeMovements, DateTime.Now);

            // Assert
            Assert.IsNotNull(valueReturn);
        }

        /// <summary>
        /// Apply Transformation Official Delta.
        /// </summary>
        [TestMethod]
        public void ApplyTransformationOfficialDelta_ShouldInitializationTransformedMovementInputEvacuation()
        {
            // Arrange
            IEnumerable<OfficialDeltaNodeMovement> listOfficialDeltaNodeMovements = new List<OfficialDeltaNodeMovement>() { new OfficialDeltaNodeMovement() { MovementTypeId = 153 } };

            // Act
            var valueReturn = this.controller.ApplyTransformationOfficialDelta(listOfficialDeltaNodeMovements, DateTime.Now);

            // Assert
            Assert.IsNotNull(valueReturn);
        }

        /// <summary>
        /// Apply Transformation Official Delta.
        /// </summary>
        [TestMethod]
        public void ApplyTransformationOfficialDelta_ShouldInitializationTransformedMovementOutputEvacuation()
        {
            // Arrange
            IEnumerable<OfficialDeltaNodeMovement> listOfficialDeltaNodeMovements = new List<OfficialDeltaNodeMovement>() { new OfficialDeltaNodeMovement() { MovementTypeId = 154 } };

            // Act
            var valueReturn = this.controller.ApplyTransformationOfficialDelta(listOfficialDeltaNodeMovements, DateTime.Now);

            // Assert
            Assert.IsNotNull(valueReturn);
        }

        /// <summary>
        /// Apply Transformation Official Delta.
        /// </summary>
        [TestMethod]
        public void ApplyTransformationOfficialDelta_ShouldInitializationTransformedMovementNotCompleted()
        {
            // Arrange
            IEnumerable<OfficialDeltaNodeMovement> listOfficialDeltaNodeMovements = new List<OfficialDeltaNodeMovement>() { new OfficialDeltaNodeMovement() { MovementTypeId = 23 } };

            // Act
            var valueReturn = this.controller.ApplyTransformationOfficialDelta(listOfficialDeltaNodeMovements, DateTime.Now);

            // Assert
            Assert.IsNotNull(valueReturn);
        }
    }
}
