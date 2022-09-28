// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementWithTicketDatesSpecificationTests.cs" company="Microsoft">
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
    using System;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Delta.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The consolidatedMovementWithTicketDatesTestsTests test class.
    /// </summary>
    [TestClass]
    public class ConsolidatedMovementWithTicketDatesSpecificationTests
    {
        /// <summary>
        /// ShouldReturnFalse_WhenStartDateIsDifferentFromTicket.
        /// </summary>
        [TestMethod]
        public void ShouldReturnFalse_WhenStartDateIsDifferentFromTicket()
        {
            // Arrange
            var ticket = GetTicket();

            var movement = GetConsolidatedMovement(ticket.StartDate.AddMonths(-1));
            var spec = new ConsolidatedMovementWithTicketDatesSpecification(ticket);

            // Act
            var isSatisfiedBy = spec.IsSatisfiedBy(movement);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
        }

        /// <summary>
        /// ShouldReturnFalse_WhenStartDateIsDifferentFromTicket.
        /// </summary>
        [TestMethod]
        public void ShouldReturnFalse_WhenEndDateIsDifferentFromTicket()
        {
            // Arrange
            var ticket = GetTicket();

            var movement = GetConsolidatedMovement(ticket.EndDate.AddMonths(1));
            var spec = new ConsolidatedMovementWithTicketDatesSpecification(ticket);

            // Act
            var isSatisfiedBy = spec.IsSatisfiedBy(movement);

            // Assert
            Assert.IsFalse(isSatisfiedBy);
        }

        private static Ticket GetTicket()
        {
            return new Ticket
            {
                StartDate = new DateTime(2020, 8, 31),
                EndDate = new DateTime(2020, 8, 01),
            };
        }

        private static ConsolidatedMovement GetConsolidatedMovement(DateTime date)
        {
            return new ConsolidatedMovement
            {
                StartDate = date,
            };
        }
    }
}