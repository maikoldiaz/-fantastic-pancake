// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericLogisticsMovement.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The GenericLogisticsMovement.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    public class GenericLogisticsMovement : QueryEntity
    {
        /// <summary>
        /// Gets or sets the concatenated moviment identifier.
        /// </summary>
        /// <value>
        /// The concatenated movement identifier.
        /// </value>
        public string ConcatMovementId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public string DestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets the source storage location identifier.
        /// </summary>
        /// <value>
        /// The source storage location identifier.
        /// </value>
        public string SourceStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the source storage location.
        /// </summary>
        /// <value>
        /// The source storage location.
        /// </value>
        public string SourceStorageLocation { get; set; }

        /// <summary>
        /// Gets or sets the destination storage location identifier.
        /// </summary>
        /// <value>
        /// The destination storage location identifier.
        /// </value>
        public string DestinationStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the destination storage location.
        /// </summary>
        /// <value>
        /// The destination storage location.
        /// </value>
        public string DestinationStorageLocation { get; set; }

        /// <summary>
        /// Gets or sets the source logistic center identifier.
        /// </summary>
        /// <value>
        /// The source logistic center identifier.
        /// </value>
        public string SourceLogisticCenterId { get; set; }

        /// <summary>
        /// Gets or sets the source logistic center.
        /// </summary>
        /// <value>
        /// The source logistic center.
        /// </value>
        public string SourceLogisticCenter { get; set; }

        /// <summary>
        /// Gets or sets the destination logistic center identifier.
        /// </summary>
        /// <value>
        /// The destination logistic center identifier.
        /// </value>
        public string DestinationLogisticCenterId { get; set; }

        /// <summary>
        /// Gets or sets the destination logistic center.
        /// </summary>
        /// <value>
        /// The destination logistic center.
        /// </value>
        public string DestinationLogisticCenter { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        public int MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has annulation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has annulation; otherwise, <c>false</c>.
        /// </value>
        [NotMapped]
        public bool HasAnnulation { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        [NotMapped]
        public string MeasurementUnitName { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        public decimal NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public int? MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the ownership value.
        /// </summary>
        /// <value>
        /// The ownership value.
        /// </value>
        public decimal? OwnershipValue { get; set; }

        /// <summary>
        /// Gets or sets the ownership value unit.
        /// </summary>
        /// <value>
        /// The ownership value unit.
        /// </value>
        public string OwnershipValueUnit { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public string Order { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        public DateTime OperationDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the logistics movement.
        /// </summary>
        /// <value>
        /// The type of the logistics movement.
        /// </value>
        public string LogisticsMovementType { get; set; }

        /// <summary>
        /// Gets or sets the OrderPurchase.
        /// </summary>
        /// <value>
        /// The type of the OrderPurchase movement.
        /// </value>
        public int? OrderPurchase { get; set; }

        /// <summary>
        /// Gets or sets the PosPurchase.
        /// </summary>
        /// <value>
        /// The type of the PosPurchase .
        /// </value>
        public int? PosPurchase { get; set; }

        /// <summary>
        /// Gets or sets the CostCenter.
        /// </summary>
        /// <value>
        /// the CostCenter .
        /// </value>
        public int? CostCenter { get; set; }

        /// <summary>
        /// Gets or sets the MovementId.
        /// </summary>
        /// <value>
        /// the MovementId .
        /// </value>
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the DestinationNodeOrder.
        /// </summary>
        /// <value>
        /// the DestinationNodeOrder .
        /// </value>
        public int? DestinationNodeOrder { get; set; }

        /// <summary>
        /// Gets or sets the SourceNodeOrder.
        /// </summary>
        /// <value>
        /// the SourceNodeOrder .
        /// </value>
        public int? SourceNodeOrder { get; set; }

        /// <summary>
        /// Gets or sets the MoveTypeName.
        /// </summary>
        /// <value>
        /// the MoveTypeName .
        /// </value>
        public string MovementTypeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the DestinationNodeExportation.
        /// </summary>
        /// <value>
        /// the DestinationNodeExportation .
        /// </value>
        public bool? DestinationNodeExportation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the SourceNodeExportation.
        /// </summary>
        /// <value>
        /// the SourceNodeExportation .
        /// </value>
        public bool? SourceNodeExportation { get; set; }

        /// <summary>
        /// Gets or sets the Classification.
        /// </summary>
        /// <value>
        /// the Classification .
        /// </value>
        public string Classification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the SourceNodeSendToSap.
        /// </summary>
        /// <value>
        /// the SourceNodeSendToSap .
        /// </value>
        public bool? SourceNodeSendToSap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the DestnationNodeSendToSap.
        /// </summary>
        /// <value>
        /// the DestnationNodeSendToSap .
        /// </value>
        public bool? DestinationNodeSendToSap { get; set; }

        /// <summary>
        /// Gets or sets the MovementTransactionId.
        /// </summary>
        /// <value>
        /// the MovementTransactionId .
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the Cost Center Name.
        /// </summary>
        /// <value>
        /// the Classification .
        /// </value>
        public string CostCenterName { get; set; }

        /// <summary>
        /// Gets or sets the GmCode.
        /// </summary>
        /// <value>
        /// the MoveTypeName .
        /// </value>
        [NotMapped]
        public string GmCode { get; set; }

        /// <summary>
        /// Gets or sets the MoveTypeName.
        /// </summary>
        /// <value>
        /// the MoveTypeName .
        /// </value>
        [NotMapped]
        public StatusType Status { get; set; }

        /// <summary>
        /// Gets or sets the ErrorMessage.
        /// </summary>
        /// <value>
        /// the ErrorMessage .
        /// </value>
        [NotMapped]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the type of the homologated movement.
        /// </summary>
        /// <value>
        /// The type of the homologated movement.
        /// </value>
        [NotMapped]
        public string HomologatedMovementType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets a value indicating NodeApproved .
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is NodeApproved valid; otherwise, <c>false</c>.
        /// </value>
        [NotMapped]
        public bool NodeApproved { get; set; }

        /// <summary>
        /// Gets the logistics movement.
        /// </summary>
        /// <value>
        /// The logistics movement.
        /// </value>
        [NotMapped]
        public string LogisticsTautologyKey
        {
            get
            {
                if (this.HasTautology)
                {
                    var sameProduct = this.SourceProductId == this.DestinationProductId ? "1" : "0";
                    var sameLogisticCenter = this.SourceLogisticCenterId == this.DestinationLogisticCenterId ? "1" : "0";
                    var sameStorageLocation = this.SourceStorageLocationId == this.DestinationStorageLocationId ? "1" : "0";
                    var annulationExists = this.HasAnnulation ? "1" : "0";

                    return sameProduct + sameLogisticCenter + sameStorageLocation + annulationExists;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the LogisticsSpatialCases.
        /// </summary>
        /// <value>
        /// The LogisticsSpatialCases.
        /// </value>
        [NotMapped]
        public int LogisticsSpatialCase => this.MovementTypeId switch
        {
            (int)MovementType.Tolerance => !string.IsNullOrEmpty(this.DestinationNode) ? Constants.EmToleranceBce : Constants.SmToleranceBce,
            (int)MovementType.UnidentifiedLoss => !string.IsNullOrEmpty(this.DestinationNode) ? Constants.EmAjteBce : Constants.SmAjteBce,
            (int)MovementType.InputEvacuation => this.DestinationNodeExportation.GetValueOrDefault() ? Constants.EmExportLoans : this.MovementTypeId,
            (int)MovementType.OutputEvacuation => this.SourceNodeExportation.GetValueOrDefault() ? Constants.SmExportLoans : this.MovementTypeId,
            (int)MovementType.InputCancellation => this.SourceNodeExportation.GetValueOrDefault() ? Constants.AnulEmLoansExport : this.MovementTypeId,
            (int)MovementType.OutputCancellation => this.DestinationNodeExportation.GetValueOrDefault() ? Constants.AnulSmLoansExport : this.MovementTypeId,
            _ => this.MovementTypeId,
        };

        /// <summary>
        /// Gets a value indicating whether this instance is tautology valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is tautology valid; otherwise, <c>false</c>.
        /// </value>
        private bool HasTautology
        {
            get
            {
                return this.SourceNodeId != null && this.DestinationNodeId != null && this.SourceProductId != null && this.DestinationProductId != null;
            }
        }

        /// <summary>
        /// Annulates the specified annulation.
        /// </summary>
        /// <param name="annulation">The annulation.</param>
        public void Annulate(Annulation annulation)
        {
            ArgumentValidators.ThrowIfNull(annulation, nameof(annulation));
            var sourceNode = this.SourceNode;
            var sourceProduct = this.SourceProduct;
            var sourceStorageLocation = this.SourceStorageLocation;
            this.SourceNode = DoAnnulate(annulation.SourceNodeId, this.SourceNode, this.DestinationNode);
            this.DestinationNode = DoAnnulate(annulation.DestinationNodeId, sourceNode, this.DestinationNode);
            this.SourceStorageLocation = DoAnnulate(annulation.SourceNodeId, this.SourceStorageLocation, this.DestinationStorageLocation);
            this.DestinationStorageLocation = DoAnnulate(annulation.DestinationNodeId, sourceStorageLocation, this.DestinationStorageLocation);
            this.SourceProduct = DoAnnulate(annulation.SourceProductId, this.SourceProduct, this.DestinationProduct);
            this.DestinationProduct = DoAnnulate(annulation.DestinationProductId, sourceProduct, this.DestinationProduct);

            var sourceNodeId = this.SourceNodeId;
            var sourceProductId = this.SourceProductId;
            var sourceStorageLocationId = this.SourceStorageLocationId;
            var sourceLogisticCenterId = this.SourceLogisticCenterId;
            this.SourceNodeId = DoAnnulate(annulation.SourceNodeId, this.SourceNodeId.GetValueOrDefault(), this.DestinationNodeId.GetValueOrDefault());
            this.DestinationNodeId = DoAnnulate(annulation.DestinationNodeId, sourceNodeId.GetValueOrDefault(), this.DestinationNodeId.GetValueOrDefault());
            this.SourceStorageLocationId = DoAnnulate(annulation.SourceNodeId, this.SourceStorageLocationId, this.DestinationStorageLocationId);
            this.DestinationStorageLocationId = DoAnnulate(annulation.DestinationNodeId, sourceStorageLocationId, this.DestinationStorageLocationId);
            this.SourceProductId = DoAnnulate(annulation.SourceProductId, this.SourceProductId, this.DestinationProductId);
            this.DestinationProductId = DoAnnulate(annulation.DestinationProductId, sourceProductId, this.DestinationProductId);
            this.SourceLogisticCenterId = DoAnnulate(annulation.SourceNodeId, this.SourceLogisticCenterId, this.DestinationLogisticCenterId);
            this.DestinationLogisticCenterId = DoAnnulate(annulation.DestinationNodeId, sourceLogisticCenterId, this.DestinationLogisticCenterId);
        }

        /// <summary>
        /// Check nodes of Source and destination.
        /// </summary>
        /// <param name="errorMessage">error message.</param>
        /// <param name="nodesForSegments">nodesForSegments.</param>
        public void CheckNodes(string errorMessage, IEnumerable<NodesForSegmentResult> nodesForSegments)
        {
            ArgumentValidators.ThrowIfNull(nodesForSegments, nameof(nodesForSegments));

            if (this.DestinationNodeId != null && nodesForSegments.Any(x => x.NodeId == this.DestinationNodeId) && !this.DestinationNodeSendToSap.GetValueOrDefault())
            {
                this.Status = StatusType.EMPTY;
                this.ErrorMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    errorMessage,
                    this.DestinationNode);
            }

            if (this.SourceNodeId != null && nodesForSegments.Any(x => x.NodeId == this.SourceNodeId) && !this.SourceNodeSendToSap.GetValueOrDefault())
            {
                this.Status = StatusType.EMPTY;
                this.ErrorMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    errorMessage,
                    this.SourceNode);
            }

            if (!this.DestinationNodeSendToSap.GetValueOrDefault())
            {
                this.DestinationNodeId = null;
                this.DestinationStorageLocation = null;
            }

            if (!this.SourceNodeSendToSap.GetValueOrDefault())
            {
                this.SourceNodeId = null;
                this.SourceStorageLocation = null;
            }
        }

        /// <summary>
        /// Check nodes of Source and destination approved.
        /// </summary>
        /// <param name="nodesForSegments">nodesForSegments.</param>
        public void CheckNodesApproved(IEnumerable<NodesForSegmentResult> nodesForSegments)
        {
            this.NodeApproved = false;
            if ((this.DestinationNodeId != null && this.CheckNodeApproved(this.DestinationNodeId.GetValueOrDefault(), nodesForSegments)) ||
                (this.SourceNodeId != null && this.CheckNodeApproved(this.SourceNodeId.GetValueOrDefault(), nodesForSegments)))
            {
                this.NodeApproved = true;
            }
        }

        /// <summary>
        /// Check cost center.
        /// </summary>
        /// <param name="errorMessage">message. </param>
        public void CheckCostCenter(string errorMessage)
        {
            if (this.CostCenter == null && Constants.LossClassification.EqualsIgnoreCase(this.Classification))
            {
                this.Status = StatusType.EMPTY;
                this.ErrorMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    errorMessage,
                    this.SourceNode,
                    this.DestinationNode,
                    this.MovementTypeName);
            }
        }

        private static string DoAnnulate(int type, string source, string destination)
        {
            if (type == 3)
            {
                return string.Empty;
            }

            return type == 2 ? destination : source;
        }

        private static int? DoAnnulate(int type, int source, int destination)
        {
            if (type == 3)
            {
                return null;
            }

            return type == 2 ? destination : source;
        }

        private bool CheckNodeApproved(int node, IEnumerable<NodesForSegmentResult> nodesForSegments)
        {
            return nodesForSegments.Any(x => x.NodeId == node &&
                (x.OperationDate >= this.StartDate && x.OperationDate <= this.EndDate) &&
                x.IsApproved);
        }
    }
}