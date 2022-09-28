using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewUnbalanceInput
    {
        public string Movimiento { get; set; }
        public string Classification { get; set; }
        public string SourceNode { get; set; }
        public string DestiationNode { get; set; }
        public string SourceProduct { get; set; }
        public string DestinationProduct { get; set; }
        public decimal NetStandardVolume { get; set; }
        public string MeasurementUnit { get; set; }
        public int? TicketId { get; set; }
        public int MovementSourceId { get; set; }
        public int MovementTransactionId { get; set; }
        public int? SourceNodeId { get; set; }
        public int? SourceStorageLocationId { get; set; }
        public string SourceProductId { get; set; }
        public string SourceProductTypeId { get; set; }
        public string MovementTypeId { get; set; }
        public string MovementId { get; set; }
        public DateTime OperationalDate { get; set; }
        public string Scenario { get; set; }
        public int? DestinationNodeId { get; set; }
        public string UnidadDeMedida { get; set; }
    }
}
