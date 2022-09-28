// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapLogisticMovementDetail.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap
{
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Sap Logistic Movement Detail class.
    /// </summary>
    public class SapLogisticMovementDetail : Entity
    {
        ///
        /// <summary>
        /// Gets or sets movement transaction identifier.
        /// </summary>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets movement type.
        /// </summary>
        public string MovementType { get; set; }

        /// <summary>
        /// Gets or sets source center.
        /// </summary>
        public string SourceCenter { get; set; }

        /// <summary>
        /// Gets or sets source storage.
        /// </summary>
        public string SourceStorage { get; set; }

        /// <summary>
        /// Gets or sets source product.
        /// </summary>
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets destination center.
        /// </summary>
        public string DestinationCenter { get; set; }

        /// <summary>
        /// Gets or sets destination storage.
        /// </summary>
        public string DestinationStorage { get; set; }

        /// <summary>
        /// Gets or sets destination product.
        /// </summary>
        public string DestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets ownership volume.
        /// </summary>
        public decimal? OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets units.
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        /// Gets or sets operational date.
        /// </summary>
        public DateTime? OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets movement id.
        /// </summary>
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets cost center.
        /// </summary>
        public string CostCenter { get; set; }

        /// <summary>
        /// Gets or sets gm code.
        /// </summary>
        public string GmCode { get; set; }

        /// <summary>
        /// Gets or sets document number.
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets position.
        /// </summary>
        public int? Position { get; set; }

        /// <summary>
        /// Gets or sets order.
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Gets or sets accounting date.
        /// </summary>
        public DateTime? AccountingDate { get; set; }

        /// <summary>
        /// Gets or sets segment.
        /// </summary>
        public string Segment { get; set; }

        /// <summary>
        /// Gets or sets scenario.
        /// </summary>
        public string Scenario { get; set; }

        /// <summary>
        /// Gets or sets owner.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets logistic movement id.
        /// </summary>
        public int? LogisticMovementId { get; set; }
    }
}
