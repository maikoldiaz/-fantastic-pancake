using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OwnershipCalculation
    {
        public OwnershipCalculation()
        {
            OwnershipCalculationResult = new HashSet<OwnershipCalculationResult>();
        }

        public int OwnershipCalculationId { get; set; }
        public int NodeId { get; set; }
        public string ProductId { get; set; }
        public int? OwnershipTicketId { get; set; }
        public int? OwnerId { get; set; }
        public decimal? InitialInventoryVolume { get; set; }
        public decimal? InitialInventoryPercentage { get; set; }
        public decimal? FinalInventoryVolume { get; set; }
        public decimal? FinalInventoryPercentage { get; set; }
        public decimal? InputVolume { get; set; }
        public decimal? InputPercentage { get; set; }
        public decimal? OutputVolume { get; set; }
        public decimal? OutputPercentage { get; set; }
        public decimal? IdentifiedLossesVolume { get; set; }
        public decimal? IdentifiedLossesPercentage { get; set; }
        public decimal? UnbalanceVolume { get; set; }
        public decimal? UnbalancePercentage { get; set; }
        public decimal? InterfaceVolume { get; set; }
        public decimal? InterfacePercentage { get; set; }
        public decimal? ToleranceVolume { get; set; }
        public decimal? TolerancePercentage { get; set; }
        public decimal? UnidentifiedLossesVolume { get; set; }
        public decimal? UnidentifiedLossesPercentage { get; set; }
        public decimal? InterfaceUnbalanceVolume { get; set; }
        public decimal? InterfaceUnbalancePercentage { get; set; }
        public decimal? ToleranceUnbalanceVolume { get; set; }
        public decimal? ToleranceUnbalancePercentage { get; set; }
        public decimal? IdentifiedLossesUnbalanceVolume { get; set; }
        public decimal? IdentifiedLossesUnbalancePercentage { get; set; }
        public DateTime? Date { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Node Node { get; set; }
        public virtual CategoryElement Owner { get; set; }
        public virtual Ticket OwnershipTicket { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<OwnershipCalculationResult> OwnershipCalculationResult { get; set; }
    }
}
