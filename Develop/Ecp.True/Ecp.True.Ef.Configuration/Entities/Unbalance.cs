using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Unbalance
    {
        public int UnbalanceId { get; set; }
        public int TicketId { get; set; }
        public int NodeId { get; set; }
        public string ProductId { get; set; }
        public decimal InitialInventory { get; set; }
        public decimal Inputs { get; set; }
        public decimal Outputs { get; set; }
        public decimal FinalInvnetory { get; set; }
        public decimal IdentifiedLosses { get; set; }
        public decimal? Unbalance1 { get; set; }
        public decimal Interface { get; set; }
        public decimal? Tolerance { get; set; }
        public decimal? UnidentifiedLosses { get; set; }
        public DateTime CalculationDate { get; set; }
        public decimal? InterfaceUnbalance { get; set; }
        public decimal? ToleranceUnbalance { get; set; }
        public decimal? UnidentifiedLossesUnbalance { get; set; }
        public decimal? StandardUncertainty { get; set; }
        public decimal? AverageUncertainty { get; set; }
        public decimal? AverageUncertaintyUnbalancePercentage { get; set; }
        public decimal? Warning { get; set; }
        public decimal? Action { get; set; }
        public decimal? ControlTolerance { get; set; }
        public decimal? ToleranceIdentifiedLosses { get; set; }
        public decimal? ToleranceInputs { get; set; }
        public decimal? ToleranceOutputs { get; set; }
        public decimal? ToleranceInitialInventory { get; set; }
        public decimal? ToleranceFinalInventory { get; set; }
        public bool? BlockchainStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Node Node { get; set; }
        public virtual Product Product { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
