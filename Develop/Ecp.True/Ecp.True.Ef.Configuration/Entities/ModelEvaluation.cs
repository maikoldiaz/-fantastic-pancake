using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ModelEvaluation
    {
        public int ModelEvaluationId { get; set; }
        public DateTime OperationalDate { get; set; }
        public string TransferPoint { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public int AlgorithmId { get; set; }
        public string AlgorithmType { get; set; }
        public decimal? MeanAbsoluteError { get; set; }
        public DateTime LoadDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
