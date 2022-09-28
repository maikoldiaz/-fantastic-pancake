using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewKpipreviousDateDataByCategoryElementNode
    {
        public string FilterType { get; set; }
        public int OrderToDisplay { get; set; }
        public string CategoryPrev { get; set; }
        public string ElementPrev { get; set; }
        public string NodeNamePrev { get; set; }
        public int? NodeId { get; set; }
        public DateTime? CalculationDatePrev { get; set; }
        public string Product { get; set; }
        public string Indicator { get; set; }
        public decimal? CurrentValuePrev { get; set; }
        public int? TicketId { get; set; }
    }
}
