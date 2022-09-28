using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class MovementsInformationByOwner
    {
        public string Category { get; set; }
        public string Element { get; set; }
        public string NodeName { get; set; }
        public DateTime? CalculationDate { get; set; }
        public decimal InitialInventory { get; set; }
        public decimal Inputs { get; set; }
        public decimal Outputs { get; set; }
        public decimal IdentifiedLosses { get; set; }
        public decimal? UnidentifiedLosses { get; set; }
        public decimal? Tolerance { get; set; }
        public decimal Interface { get; set; }
        public decimal FinalInvnetory { get; set; }
        public decimal? Control { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public int TicketId { get; set; }
        public int? TicketTypeId { get; set; }
        public int CategoryId { get; set; }
        public string OwnerName { get; set; }
        public long? Rnum { get; set; }
    }
}
