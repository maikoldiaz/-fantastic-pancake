using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Ecp.True.Ef.Configuration.Entities;

namespace Ecp.True.Ef.Configuration.Context
{
    public partial class AppContext : DbContext
    {
        public AppContext()
        {
        }

        public AppContext(DbContextOptions<AppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Algorithm> Algorithm { get; set; }
        public virtual DbSet<ApprovalRule> ApprovalRule { get; set; }
        public virtual DbSet<Ecp.True.Ef.Configuration.Entities.Attribute> Attribute { get; set; }
        public virtual DbSet<AttributeDetailsWithOwner> AttributeDetailsWithOwner { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<AuditStatus> AuditStatus { get; set; }
        public virtual DbSet<AuditStatus1> AuditStatus1 { get; set; }
        public virtual DbSet<BalanceControl> BalanceControl { get; set; }
        public virtual DbSet<BalanceControlChart> BalanceControlChart { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategoryElement> CategoryElement { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<ContractInformation> ContractInformation { get; set; }
        public virtual DbSet<ControlScript> ControlScript { get; set; }
        public virtual DbSet<ControlType> ControlType { get; set; }
        public virtual DbSet<DeadletteredMessage> DeadletteredMessage { get; set; }
        public virtual DbSet<DimDate> DimDate { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<ErrorLog1> ErrorLog1 { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventInformation> EventInformation { get; set; }
        public virtual DbSet<Feature> Feature { get; set; }
        public virtual DbSet<FeatureRole> FeatureRole { get; set; }
        public virtual DbSet<FileRegistration> FileRegistration { get; set; }
        public virtual DbSet<FileRegistrationError> FileRegistrationError { get; set; }
        public virtual DbSet<FileRegistrationTransaction> FileRegistrationTransaction { get; set; }
        public virtual DbSet<FileUploadState> FileUploadState { get; set; }
        public virtual DbSet<Homologation> Homologation { get; set; }
        public virtual DbSet<HomologationDataMapping> HomologationDataMapping { get; set; }
        public virtual DbSet<HomologationGroup> HomologationGroup { get; set; }
        public virtual DbSet<HomologationObject> HomologationObject { get; set; }
        public virtual DbSet<HomologationObjectType> HomologationObjectType { get; set; }
        public virtual DbSet<Icon> Icon { get; set; }
        public virtual DbSet<InventoryDetailsBeforeCutoff> InventoryDetailsBeforeCutoff { get; set; }
        public virtual DbSet<InventoryDetailsWithOwner> InventoryDetailsWithOwner { get; set; }
        public virtual DbSet<InventoryDetailsWithoutOwner> InventoryDetailsWithoutOwner { get; set; }
        public virtual DbSet<InventoryProduct> InventoryProduct { get; set; }
        public virtual DbSet<InventoryQualityDetailsBeforeCutoff> InventoryQualityDetailsBeforeCutoff { get; set; }
        public virtual DbSet<KpidataByCategoryElementNodeWithOwnership> KpidataByCategoryElementNodeWithOwnership { get; set; }
        public virtual DbSet<KpipreviousDateDataByCategoryElementNodeWithOwner> KpipreviousDateDataByCategoryElementNodeWithOwner { get; set; }
        public virtual DbSet<LogisticCenter> LogisticCenter { get; set; }
        public virtual DbSet<MessageType> MessageType { get; set; }
        public virtual DbSet<ModelEvaluation> ModelEvaluation { get; set; }
        public virtual DbSet<Movement> Movement { get; set; }
        public virtual DbSet<MovementContract> MovementContract { get; set; }
        public virtual DbSet<MovementDestination> MovementDestination { get; set; }
        public virtual DbSet<MovementDetailsBeforeCutoff> MovementDetailsBeforeCutoff { get; set; }
        public virtual DbSet<MovementDetailsWithOwner> MovementDetailsWithOwner { get; set; }
        public virtual DbSet<MovementEvent> MovementEvent { get; set; }
        public virtual DbSet<MovementPeriod> MovementPeriod { get; set; }
        public virtual DbSet<MovementQualityDetailsBeforeCutoff> MovementQualityDetailsBeforeCutoff { get; set; }
        public virtual DbSet<MovementSource> MovementSource { get; set; }
        public virtual DbSet<MovementsByProductWithOwner> MovementsByProductWithOwner { get; set; }
        public virtual DbSet<MovementsInformationByOwner> MovementsInformationByOwner { get; set; }
        public virtual DbSet<Node> Node { get; set; }
        public virtual DbSet<NodeConnection> NodeConnection { get; set; }
        public virtual DbSet<NodeConnectionInformation> NodeConnectionInformation { get; set; }
        public virtual DbSet<NodeConnectionProduct> NodeConnectionProduct { get; set; }
        public virtual DbSet<NodeConnectionProductOwner> NodeConnectionProductOwner { get; set; }
        public virtual DbSet<NodeConnectionProductRule> NodeConnectionProductRule { get; set; }
        public virtual DbSet<NodeGeneralInformation> NodeGeneralInformation { get; set; }
        public virtual DbSet<NodeOwnershipRule> NodeOwnershipRule { get; set; }
        public virtual DbSet<NodeProductInformation> NodeProductInformation { get; set; }
        public virtual DbSet<NodeProductRule> NodeProductRule { get; set; }
        public virtual DbSet<NodeStatusIconUrl> NodeStatusIconUrl { get; set; }
        public virtual DbSet<NodeStorageLocation> NodeStorageLocation { get; set; }
        public virtual DbSet<NodeTag> NodeTag { get; set; }
        public virtual DbSet<Operational> Operational { get; set; }
        public virtual DbSet<OperationalInventory> OperationalInventory { get; set; }
        public virtual DbSet<OperationalInventoryQuality> OperationalInventoryQuality { get; set; }
        public virtual DbSet<OperationalMovement> OperationalMovement { get; set; }
        public virtual DbSet<OperationalMovementQuality> OperationalMovementQuality { get; set; }
        public virtual DbSet<OperativeMovements> OperativeMovements { get; set; }
        public virtual DbSet<OperativeMovementsWithOwnership> OperativeMovementsWithOwnership { get; set; }
        public virtual DbSet<OperativeNodeRelationship> OperativeNodeRelationship { get; set; }
        public virtual DbSet<OperativeNodeRelationshipWithOwnership> OperativeNodeRelationshipWithOwnership { get; set; }
        public virtual DbSet<OriginType> OriginType { get; set; }
        public virtual DbSet<Owner> Owner { get; set; }
        public virtual DbSet<Ownership> Ownership { get; set; }
        public virtual DbSet<OwnershipCalculation> OwnershipCalculation { get; set; }
        public virtual DbSet<OwnershipCalculationResult> OwnershipCalculationResult { get; set; }
        public virtual DbSet<OwnershipNode> OwnershipNode { get; set; }
        public virtual DbSet<OwnershipNodeError> OwnershipNodeError { get; set; }
        public virtual DbSet<OwnershipNodeStatusType> OwnershipNodeStatusType { get; set; }
        public virtual DbSet<OwnershipPercentageValues> OwnershipPercentageValues { get; set; }
        public virtual DbSet<OwnershipResult> OwnershipResult { get; set; }
        public virtual DbSet<OwnershipRuleRefreshHistory> OwnershipRuleRefreshHistory { get; set; }
        public virtual DbSet<PendingTransaction> PendingTransaction { get; set; }
        public virtual DbSet<PendingTransactionError> PendingTransactionError { get; set; }
        public virtual DbSet<PipelineLog> PipelineLog { get; set; }
        public virtual DbSet<PipelineLog1> PipelineLog1 { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<QualityDetailsWithOwner> QualityDetailsWithOwner { get; set; }
        public virtual DbSet<QualityDetailsWithoutOwner> QualityDetailsWithoutOwner { get; set; }
        public virtual DbSet<RegisterFileActionType> RegisterFileActionType { get; set; }
        public virtual DbSet<ReportConfiguration> ReportConfiguration { get; set; }
        public virtual DbSet<ReportExecutionDate> ReportExecutionDate { get; set; }
        public virtual DbSet<ReportHeaderDetails> ReportHeaderDetails { get; set; }
        public virtual DbSet<ReportTemplateConfiguration> ReportTemplateConfiguration { get; set; }
        public virtual DbSet<ReportTemplateDetails> ReportTemplateDetails { get; set; }
        public virtual DbSet<Reversal> Reversal { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Rules> Rules { get; set; }
        public virtual DbSet<Scenario> Scenario { get; set; }
        public virtual DbSet<ScenarioType> ScenarioType { get; set; }
        public virtual DbSet<SegmentOwnershipCalculation> SegmentOwnershipCalculation { get; set; }
        public virtual DbSet<SegmentUnbalance> SegmentUnbalance { get; set; }
        public virtual DbSet<StageOperativeMovements> StageOperativeMovements { get; set; }
        public virtual DbSet<StageOperativeMovementsWithOwnership> StageOperativeMovementsWithOwnership { get; set; }
        public virtual DbSet<StageOwnershipPercentageValues> StageOwnershipPercentageValues { get; set; }
        public virtual DbSet<StatusType> StatusType { get; set; }
        public virtual DbSet<StorageLocation> StorageLocation { get; set; }
        public virtual DbSet<StorageLocationProduct> StorageLocationProduct { get; set; }
        public virtual DbSet<StorageLocationProductMapping> StorageLocationProductMapping { get; set; }
        public virtual DbSet<StorageLocationProductOwner> StorageLocationProductOwner { get; set; }
        public virtual DbSet<StorageLocationProductVariable> StorageLocationProductVariable { get; set; }
        public virtual DbSet<SystemOwnershipCalculation> SystemOwnershipCalculation { get; set; }
        public virtual DbSet<SystemType> SystemType { get; set; }
        public virtual DbSet<SystemUnbalance> SystemUnbalance { get; set; }
        public virtual DbSet<TempCategoryElementMapping> TempCategoryElementMapping { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketNodeStatus> TicketNodeStatus { get; set; }
        public virtual DbSet<TicketType> TicketType { get; set; }
        public virtual DbSet<Transformation> Transformation { get; set; }
        public virtual DbSet<Unbalance> Unbalance { get; set; }
        public virtual DbSet<UnbalanceComment> UnbalanceComment { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<VariableType> VariableType { get; set; }
        public virtual DbSet<ViewAttributeDetails> ViewAttributeDetails { get; set; }
        public virtual DbSet<ViewCalculationErrors> ViewCalculationErrors { get; set; }
        public virtual DbSet<ViewFileRegistrationStatus> ViewFileRegistrationStatus { get; set; }
        public virtual DbSet<ViewFinalInventory> ViewFinalInventory { get; set; }
        public virtual DbSet<ViewGetParsingErrors> ViewGetParsingErrors { get; set; }
        public virtual DbSet<ViewInventoryInformation> ViewInventoryInformation { get; set; }
        public virtual DbSet<ViewInventoryProductWithProductName> ViewInventoryProductWithProductName { get; set; }
        public virtual DbSet<ViewKpidataByCategoryElementNode> ViewKpidataByCategoryElementNode { get; set; }
        public virtual DbSet<ViewKpipreviousDateDataByCategoryElementNode> ViewKpipreviousDateDataByCategoryElementNode { get; set; }
        public virtual DbSet<ViewMovementDestinationWithNodeAndProductName> ViewMovementDestinationWithNodeAndProductName { get; set; }
        public virtual DbSet<ViewMovementDetails> ViewMovementDetails { get; set; }
        public virtual DbSet<ViewMovementInformation> ViewMovementInformation { get; set; }
        public virtual DbSet<ViewMovementSourceWithNodeAndProductName> ViewMovementSourceWithNodeAndProductName { get; set; }
        public virtual DbSet<ViewMovementsByProduct> ViewMovementsByProduct { get; set; }
        public virtual DbSet<ViewNodeConnectionProductRule> ViewNodeConnectionProductRule { get; set; }
        public virtual DbSet<ViewNodeProductRule> ViewNodeProductRule { get; set; }
        public virtual DbSet<ViewNodeRule> ViewNodeRule { get; set; }
        public virtual DbSet<ViewNodeTagWithCategoryId> ViewNodeTagWithCategoryId { get; set; }
        public virtual DbSet<ViewOperativeMovementsPeriodic> ViewOperativeMovementsPeriodic { get; set; }
        public virtual DbSet<ViewOperativeMovementswithOwnerShipPeriodic> ViewOperativeMovementswithOwnerShipPeriodic { get; set; }
        public virtual DbSet<ViewOwnerShipNode> ViewOwnerShipNode { get; set; }
        public virtual DbSet<ViewRelKpi> ViewRelKpi { get; set; }
        public virtual DbSet<ViewRelationShipView> ViewRelationShipView { get; set; }
        public virtual DbSet<ViewStorageLocationProductWithProductName> ViewStorageLocationProductWithProductName { get; set; }
        public virtual DbSet<ViewTicket> ViewTicket { get; set; }
        public virtual DbSet<ViewUnBalanceOutput> ViewUnBalanceOutput { get; set; }
        public virtual DbSet<ViewUnBalanceTicket> ViewUnBalanceTicket { get; set; }
        public virtual DbSet<ViewUnbalance> ViewUnbalance { get; set; }
        public virtual DbSet<ViewUnbalanceInput> ViewUnbalanceInput { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Algorithm>(entity =>
            {
                entity.ToTable("Algorithm", "Admin");

                entity.HasComment("This table holds the data for Algorithms based on Analytical Model. This is a master table and contains seeded data.");

                entity.Property(e => e.AlgorithmId).HasComment("The identifier of the Algorithm");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.ModelName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the analytical model");

                entity.Property(e => e.PeriodsToForecast).HasComment("The value of the periods to forecast");
            });

            modelBuilder.Entity<ApprovalRule>(entity =>
            {
                entity.ToTable("ApprovalRule", "Admin");

                entity.HasComment("This table holds the rules for ticket node approval. This is a master table and has seeded data.");

                entity.Property(e => e.ApprovalRuleId).HasComment("The identifier of the approval rule");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Rule)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("The expression of the rule (like PI/E < 0.2)");
            });

            modelBuilder.Entity<Ecp.True.Ef.Configuration.Entities.Attribute>(entity =>
            {
                entity.ToTable("Attribute", "Offchain");

                entity.HasComment("This table holds the details for the Atributes.");

                entity.Property(e => e.Id).HasComment("The identifier of the record");

                entity.Property(e => e.AttributeDescription)
                    .HasMaxLength(150)
                    .HasComment("The description of the attribute");

                entity.Property(e => e.AttributeId)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the attribute");

                entity.Property(e => e.AttributeType)
                    .HasMaxLength(150)
                    .HasComment("The type of the attribute (like General)");

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The value of the attribute in a unit");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.InventoryProductId).HasComment("The identifier of the inventory product");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MovementTransactionId).HasComment("The identifier of the movement transaction");

                entity.Property(e => e.ValueAttributeUnit)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name or identifier of the unit (category element of unit category, like Bbl)");

                entity.HasOne(d => d.InventoryProduct)
                    .WithMany(p => p.Attribute)
                    .HasForeignKey(d => d.InventoryProductId)
                    .HasConstraintName("FK_Attribute_InventoryProduct");

                entity.HasOne(d => d.MovementTransaction)
                    .WithMany(p => p.Attribute)
                    .HasForeignKey(d => d.MovementTransactionId)
                    .HasConstraintName("FK_Attribute_Movement");
            });

            modelBuilder.Entity<AttributeDetailsWithOwner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("AttributeDetailsWithOwner", "Admin");

                entity.HasComment("This View is to Fetch AttributeDetailsWithOwner Data For PowerBi Report From Tables(Unbalance, Product, Ticket, Node, NodeTag, CategoryElement,  Category, Ownership)");

                entity.Property(e => e.AttributeDescription).HasMaxLength(150);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DestinationNodeName).HasMaxLength(150);

                entity.Property(e => e.DestinationProductName).HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.GrossStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(309);

                entity.Property(e => e.Operacion)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OperationalDate).HasColumnType("date");

                entity.Property(e => e.OwnerName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OwnershipProcessDate).HasColumnType("datetime");

                entity.Property(e => e.OwnershipVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SourceNodeName).HasMaxLength(150);

                entity.Property(e => e.SourceProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SourceProductName).HasMaxLength(150);

                entity.Property(e => e.SystemName).HasMaxLength(50);

                entity.Property(e => e.Uncertainty).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.ValueAttributeUnit)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLog", "Audit");

                entity.HasComment("This table holds the data for the Audit Log.");

                entity.Property(e => e.AuditLogId).HasComment("The identifier of the log record");

                entity.Property(e => e.Entity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of the entity that has been modified");

                entity.Property(e => e.Field)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of the attribute that has been modified");

                entity.Property(e => e.LogDate)
                    .HasColumnType("datetime")
                    .HasComment("The date of the log record");

                entity.Property(e => e.LogType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("The type of operation of the log record ");

                entity.Property(e => e.NewValue).HasComment("The value after the change");

                entity.Property(e => e.OldValue).HasComment("The value before the change");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record");
            });

            modelBuilder.Entity<AuditStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK_Analytics_AuditStatus");

                entity.ToTable("AuditStatus", "Analytics");

                entity.HasComment("This table holds the data for the Audit Status. This is a seeded table.");

                entity.Property(e => e.StatusId).HasComment("The identifier to the status");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("The name of the status (InProgress, Successful, Failed)");
            });

            modelBuilder.Entity<AuditStatus1>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("AuditStatus", "Audit");

                entity.HasComment("This table holds the data for the Audit Status.");

                entity.Property(e => e.StatusId).HasComment("The identifier to the status");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("The name of the status");
            });

            modelBuilder.Entity<BalanceControl>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("BalanceControl", "Admin");

                entity.HasComment("This View is to Fetch Data [Admin].[BalanceControl] For PowerBi Report From Tables (Unbalance, Product, Node, NodeTag,  CategoryElement,Category)");

                entity.Property(e => e.Action).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Action1)
                    .HasColumnName("Action(-)")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.AverageUncertainty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AverageUncertaintyUnbalance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ControlTolerance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ControlTolerance1)
                    .HasColumnName("ControlTolerance(-)")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Input).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.StandardUncertainty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Unbalance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Warning).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Warning1)
                    .HasColumnName("Warning(-)")
                    .HasColumnType("decimal(20, 2)");
            });

            modelBuilder.Entity<BalanceControlChart>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("BalanceControlChart", "Admin");

                entity.Property(e => e.Action)
                    .HasColumnName("Action(+)")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Action1)
                    .HasColumnName("Action(-)")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AverageUncertainty)
                    .HasColumnName("AverageUncertainty%")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ControlTolerance)
                    .HasColumnName("ControlTolerance(+)")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ControlTolerance1)
                    .HasColumnName("ControlTolerance(-)")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Unbalance)
                    .HasColumnName("Unbalance%")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Warning)
                    .HasColumnName("Warning(+)")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Warning1)
                    .HasColumnName("Warning(-)")
                    .HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category", "Admin");

                entity.HasComment("This table holds the data for different categories in the system. This is a master table and contains seeded data.");

                entity.HasIndex(e => e.Name)
                    .HasName("UC_Category")
                    .IsUnique();

                entity.Property(e => e.CategoryId).HasComment("The identifier of the category");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasComment("The description of the category");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if the category is active or not, 1 means active");

                entity.Property(e => e.IsGrouper).HasComment("Defines a category as a grouper of nodes");

                entity.Property(e => e.IsHomologation).HasComment("The flag indicating if the category is homologated, 1 for yes");

                entity.Property(e => e.IsReadOnly)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if the category is readonly or can be modified");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the category");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version column used for consistency")
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<CategoryElement>(entity =>
            {
                entity.HasKey(e => e.ElementId)
                    .HasName("PK_CategoryElements");

                entity.ToTable("CategoryElement", "Admin");

                entity.HasComment("This table holds the data for elements and related category. This is a master table and contains seeded data.");

                entity.HasIndex(e => new { e.Name, e.CategoryId })
                    .HasName("UQ_CategoryElement_Name_CategoryId")
                    .IsUnique();

                entity.Property(e => e.ElementId).HasComment("The identifier of the element");

                entity.Property(e => e.CategoryId).HasComment("The identifier of the category to which the element belongs (segment, system, type of node, etc)");

                entity.Property(e => e.Color)
                    .HasMaxLength(20)
                    .HasComment("The color representing the element in hex");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasComment("The description of the element");

                entity.Property(e => e.IconId).HasComment("The icon representing the element, relates to the Icon table");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if the element is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the element");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version column used for consistency")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryElement)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryElement_Category");

                entity.HasOne(d => d.Icon)
                    .WithMany(p => p.CategoryElement)
                    .HasForeignKey(d => d.IconId)
                    .HasConstraintName("FK_CategoryElement_Icon");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract", "Admin");

                entity.HasComment("This table holds the data for purchases and sales contracts registrered in the system.");

                entity.Property(e => e.ContractId).HasComment("The identifier of the contract");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNodeId).HasComment("The identifier of the destination node");

                entity.Property(e => e.DocumentNumber).HasComment("The document number");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasComment("The end date of the contract");

                entity.Property(e => e.IsDeleted).HasComment("The flag indicating if the contract is deleted or not, 1 means delete");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MeasurementUnit).HasComment("The identifier of the measurement unit (category element of unit category, like Bbl)");

                entity.Property(e => e.MovementTypeId).HasComment("The identifier of the movement type");

                entity.Property(e => e.Owner1Id)
                    .HasDefaultValueSql("((124))")
                    .HasComment("The identifier of the first owner (category element of owner category, like Ecopetrol)");

                entity.Property(e => e.Owner2Id)
                    .HasDefaultValueSql("((124))")
                    .HasComment("The identifier of the second owner (category element of owner category, like Ecopetrol)");

                entity.Property(e => e.Position).HasComment("The position");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product ");

                entity.Property(e => e.SourceNodeId).HasComment("The identifier of the source node");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasComment("The start date of the contract");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The volume");

                entity.HasOne(d => d.DestinationNode)
                    .WithMany(p => p.ContractDestinationNode)
                    .HasForeignKey(d => d.DestinationNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MeasurementUnitNavigation)
                    .WithMany(p => p.ContractMeasurementUnitNavigation)
                    .HasForeignKey(d => d.MeasurementUnit)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MovementType)
                    .WithMany(p => p.ContractMovementType)
                    .HasForeignKey(d => d.MovementTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Owner1)
                    .WithMany(p => p.ContractOwner1)
                    .HasForeignKey(d => d.Owner1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Owner2)
                    .WithMany(p => p.ContractOwner2)
                    .HasForeignKey(d => d.Owner2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Contract)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_Product");

                entity.HasOne(d => d.SourceNode)
                    .WithMany(p => p.ContractSourceNode)
                    .HasForeignKey(d => d.SourceNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ContractInformation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ContractInformation", "Admin");

                entity.HasComment("This View is to Fetch Data for Contract Details for Segment and show it on PowerBi reports Tables (Contact, NodeTag, Node, CategoryElement).");

                entity.Property(e => e.DestinationNode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(309);

                entity.Property(e => e.Owner1Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Owner2Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SourceNode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TypeOfMovement)
                    .IsRequired()
                    .HasColumnName("Type of Movement")
                    .HasMaxLength(150);

                entity.Property(e => e.Volume).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ControlScript>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ControlScript", "Admin");

                entity.HasComment("This table holds the data for ControlScript (deployment scripts) used for making one time or everytime changes before or after deployment");

                entity.Property(e => e.DeploymentType)
                    .HasMaxLength(10)
                    .HasComment("The type of the controlscript, post for postdeployment, null for predeployment");

                entity.Property(e => e.ExecutedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the controlscript was executed");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("(newid())")
                    .HasComment("The GUID of controlscript(deployment script)");

                entity.Property(e => e.Status).HasComment("The flag indicating if the controlscript ran successfully or not, 1 means success");
            });

            modelBuilder.Entity<ControlType>(entity =>
            {
                entity.ToTable("ControlType", "Admin");

                entity.HasComment("This table holds the data for ControlType (InitialInventory, Unbalance, Tolerance, etc). This is a master table and contains seeded data.");

                entity.Property(e => e.ControlTypeId).HasComment("The identifier of the control type");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of the control type (Unbalance, Interface, InitialInventory, etc.)");
            });

            modelBuilder.Entity<DeadletteredMessage>(entity =>
            {
                entity.ToTable("DeadletteredMessage", "Admin");

                entity.HasComment("This Table is to store messages from the deadletter service bus queue.");

                entity.Property(e => e.DeadletteredMessageId).HasComment("The identifier of the dead letter queue message");

                entity.Property(e => e.BlobPath)
                    .IsRequired()
                    .HasComment("The path of the blob ");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.ErrorMessage).HasComment("The error message");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.ProcessName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the process (like OwnershipProcessing)");

                entity.Property(e => e.QueueName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the service bus queue");

                entity.Property(e => e.Status).HasComment("The flag indicating if the message is reprocessed or not");
            });

            modelBuilder.Entity<DimDate>(entity =>
            {
                entity.HasKey(e => e.DateKey)
                    .HasName("PK__DimDate__40DF45E38D500CE0");

                entity.ToTable("DimDate", "Admin");

                entity.HasComment("This table holds the dates (all consecutive dates of a specific hardcoded range) to show on report header in Power Bi reports.");

                entity.Property(e => e.DateKey)
                    .HasComment("The date in form of YYYYDD (20190131)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasComment("The date (eg: 01/31/2019 12:00:00 AM)");

                entity.Property(e => e.Day).HasComment("The day position in the monthly calendar (31 for 31st december)");

                entity.Property(e => e.DayOfYear).HasComment("The day position in the yearly calendar (31st december 2019 will be 365)");

                entity.Property(e => e.DaySuffix)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("The suffix for \"Day\" column (st for 1st, nd for 2nd, etc.)");

                entity.Property(e => e.DowinMonth)
                    .HasColumnName("DOWInMonth")
                    .HasComment("The day position in the monthly calendar (31 for 31st day in month of december)");

                entity.Property(e => e.IsHoliday).HasComment("The flag indicating if it is a holiday, 1 for yes");

                entity.Property(e => e.IsWeekend).HasComment("The flag indicating if it is a weekend, 1 for yes");

                entity.Property(e => e.Mmyyyy)
                    .IsRequired()
                    .HasColumnName("MMYYYY")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("The date in the format of 012019 for january 2019");

                entity.Property(e => e.Month).HasComment("The month number in the year (12 for december)");

                entity.Property(e => e.MonthName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("The month name");

                entity.Property(e => e.MonthNameFirstLetter)
                    .IsRequired()
                    .HasColumnName("MonthName_FirstLetter")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("The firstletter of the month name (J for January)");

                entity.Property(e => e.MonthNameShort)
                    .IsRequired()
                    .HasColumnName("MonthName_Short")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("The short of the month name (Jan for January)");

                entity.Property(e => e.MonthYear)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("The date in the format of 2019JAN ");

                entity.Property(e => e.Quarter).HasComment("The quarter number in the year (1 for 1st quarter)");

                entity.Property(e => e.QuarterName)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasComment("The quarter by name (First for 1st quarter)");

                entity.Property(e => e.WeekDayName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("The name of the weekday (Sunday)");

                entity.Property(e => e.WeekDayNameFirstLetter)
                    .IsRequired()
                    .HasColumnName("WeekDayName_FirstLetter")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("The firstletter of the weekday name (S for Sunday)");

                entity.Property(e => e.WeekDayNameShort)
                    .IsRequired()
                    .HasColumnName("WeekDayName_Short")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("The short of the weekday name (Sun for Sunday)");

                entity.Property(e => e.WeekOfMonth).HasComment("The week position in the monthly calendar (1 for 1st week of the month)");

                entity.Property(e => e.WeekOfYear).HasComment("The week position in the yearly calendar (12 for 12th week of the year)");

                entity.Property(e => e.Weekday).HasComment("The day position in the weekly calendar (if Sunday, then 1)");

                entity.Property(e => e.Year).HasComment("The year (2019 for the year of 2019)");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.HasKey(e => e.ErrorId)
                    .HasName("PK_Analytics_ErrorLog");

                entity.ToTable("ErrorLog", "Analytics");

                entity.HasComment("This table holds the details for the Error Log (ADF).");

                entity.Property(e => e.ErrorId).HasComment("The identifier of the error");

                entity.Property(e => e.ErrorDate)
                    .HasColumnType("datetime")
                    .HasComment("The date when the error occurs");

                entity.Property(e => e.ErrorMsg)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("The message of the error ");

                entity.Property(e => e.PipelineId).HasComment("The identifier of the pipeline");

                entity.HasOne(d => d.Pipeline)
                    .WithMany(p => p.ErrorLog)
                    .HasForeignKey(d => d.PipelineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Analytics_ErrorLog_Pipeline");
            });

            modelBuilder.Entity<ErrorLog1>(entity =>
            {
                entity.HasKey(e => e.ErrorId);

                entity.ToTable("ErrorLog", "Audit");

                entity.HasComment("This table holds the details for the Error Log (ADF).");

                entity.Property(e => e.ErrorId).HasComment("The identifier of the error");

                entity.Property(e => e.ErrorDate)
                    .HasColumnType("datetime")
                    .HasComment("The date when the error occurs");

                entity.Property(e => e.ErrorMsg)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("The message of the error ");

                entity.Property(e => e.PipelineId).HasComment("The identifier of the pipeline");

                entity.HasOne(d => d.Pipeline)
                    .WithMany(p => p.ErrorLog1)
                    .HasForeignKey(d => d.PipelineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ErrorLog_Pipeline");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event", "Admin");

                entity.HasComment("This table holds the data for events of planning, programming and collaboration agreements.");

                entity.Property(e => e.EventId).HasComment("The identifier of the event");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNodeId).HasComment("The identifier of the destination node");

                entity.Property(e => e.DestinationProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the destination product");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasComment("The end date of the event ");

                entity.Property(e => e.EventTypeId).HasComment("The identifier of the category element of event category");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValueSql("((0))")
                    .HasComment("The flag indicating if the element is deleted or not, 1 means delete");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The identifier or name of the measurement unit (category element of unit category) like for Bbl(Barriles)");

                entity.Property(e => e.Owner1Id)
                    .HasDefaultValueSql("((124))")
                    .HasComment("The identifier of the first owner (category element of owner category like for Ecopetrol)");

                entity.Property(e => e.Owner2Id)
                    .HasDefaultValueSql("((124))")
                    .HasComment("The identifier of the second owner (category element of owner category like for Ecopetrol)");

                entity.Property(e => e.SourceNodeId).HasComment("The identifier of the source node");

                entity.Property(e => e.SourceProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the source product");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasComment("The start date of the event");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The volume");

                entity.HasOne(d => d.DestinationNode)
                    .WithMany(p => p.EventDestinationNode)
                    .HasForeignKey(d => d.DestinationNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_Node2");

                entity.HasOne(d => d.DestinationProduct)
                    .WithMany(p => p.EventDestinationProduct)
                    .HasForeignKey(d => d.DestinationProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_Product2");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.EventEventType)
                    .HasForeignKey(d => d.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_EventType");

                entity.HasOne(d => d.Owner1)
                    .WithMany(p => p.EventOwner1)
                    .HasForeignKey(d => d.Owner1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Owner2)
                    .WithMany(p => p.EventOwner2)
                    .HasForeignKey(d => d.Owner2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SourceNode)
                    .WithMany(p => p.EventSourceNode)
                    .HasForeignKey(d => d.SourceNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_Node1");

                entity.HasOne(d => d.SourceProduct)
                    .WithMany(p => p.EventSourceProduct)
                    .HasForeignKey(d => d.SourceProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_Product1");
            });

            modelBuilder.Entity<EventInformation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("EventInformation", "Admin");

                entity.HasComment("This View is to Fetch Data for Event Details for Segment and show it on PowerBi reports Tables (Event, NodeTag, Node, CategoryElement).");

                entity.Property(e => e.DestinationNode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DestinationProduct)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(309);

                entity.Property(e => e.Owner1Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Owner2Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PropertyEvent)
                    .IsRequired()
                    .HasColumnName("Property Event")
                    .HasMaxLength(150);

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SourceNode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.SourceProduct)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Volume).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.ToTable("Feature", "Admin");

                entity.HasComment("This table holds the data for Feature (sub menu in the nav bar under scenario/main menu). This is a master table and contains seeded data.");

                entity.Property(e => e.FeatureId).HasComment("The identifier of the feature (sub menu in the nav bar under scenario/main menu)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of the feature(sub menu)");

                entity.Property(e => e.ScenarioId).HasComment("The identifier of the scenario (main menu like for administration(Administración))");

                entity.Property(e => e.Sequence).HasComment("The number signifying the sequence of the submenu under it's parent menu");

                entity.HasOne(d => d.Scenario)
                    .WithMany(p => p.Feature)
                    .HasForeignKey(d => d.ScenarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FeatureRole>(entity =>
            {
                entity.ToTable("FeatureRole", "Admin");

                entity.HasComment("This table holds the data for FeatureRole (submenu and the roles for which it is accessible). This is a master table and contains seeded data.");

                entity.Property(e => e.FeatureRoleId).HasComment("The identifier of the featurerole");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.FeatureId).HasComment("The identifier of the feature (submenu)");

                entity.Property(e => e.RoleId).HasComment("The identifier of the role to which the submenu is accessible");

                entity.HasOne(d => d.Feature)
                    .WithMany(p => p.FeatureRole)
                    .HasForeignKey(d => d.FeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.FeatureRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FileRegistration>(entity =>
            {
                entity.ToTable("FileRegistration", "Admin");

                entity.HasComment("This table is to capture file registration details for uploaded, updated, removed, etc files.");

                entity.HasIndex(e => e.UploadId)
                    .HasName("UC_FileRegistration")
                    .IsUnique();

                entity.Property(e => e.FileRegistrationId).HasComment("The identifier of the fileregistration");

                entity.Property(e => e.Action).HasComment("The identifier for the file action type (Insert, Update, Remove, etc)");

                entity.Property(e => e.BlobPath)
                    .IsRequired()
                    .HasComment("The blob path for this file");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.HomologationInventoryBlobPath).HasComment("The blob path if the file registration is of homologated inventory");

                entity.Property(e => e.HomologationMovementBlobPath).HasComment("The blob path if the file registration is of homologated movement");

                entity.Property(e => e.IsParsed).HasComment("Flag to signify whether canonical transformation is successful or not");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("The name of the file uploaded");

                entity.Property(e => e.PreviousUploadId).HasComment("The previous upload id of the same file registration");

                entity.Property(e => e.SegmentId).HasComment("The identifier of the category element of segment category");

                entity.Property(e => e.Status).HasComment("The identifier for the file upload state (Processing, failed, etc.)");

                entity.Property(e => e.SystemTypeId).HasComment("The identifier of the system (excel, etc.) for the file");

                entity.Property(e => e.UploadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The date of the upload of file");

                entity.Property(e => e.UploadId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The GUID of upload");

                entity.HasOne(d => d.ActionNavigation)
                    .WithMany(p => p.FileRegistration)
                    .HasForeignKey(d => d.Action)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileRegistrationError_RegisterFileActionType");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.FileRegistration)
                    .HasForeignKey(d => d.SegmentId)
                    .HasConstraintName("FK_FileRegistrationError_CategoryElement");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.FileRegistration)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileRegistrationError_FileUploadState");

                entity.HasOne(d => d.SystemType)
                    .WithMany(p => p.FileRegistration)
                    .HasForeignKey(d => d.SystemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileRegistrationError_SystemType");
            });

            modelBuilder.Entity<FileRegistrationError>(entity =>
            {
                entity.ToTable("FileRegistrationError", "Admin");

                entity.HasComment("This table is to capture errors encountered during file registration process.");

                entity.Property(e => e.FileRegistrationErrorId).HasComment("The identifier of the file registration error");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.ErrorMessage)
                    .IsRequired()
                    .HasComment("The error message");

                entity.Property(e => e.FileRegistrationId).HasComment("The identifier of the file registration to which this error is for.");

                entity.Property(e => e.MessageId)
                    .HasMaxLength(50)
                    .HasComment("The upload identifier (QU1RIEVBSTAyLkQuUU0gIF5Yh0wg93My for /true/sinoper/xml/inventory/QU1RIEVBSTAyLkQuUU0gIF5Yh0wg93My)");

                entity.HasOne(d => d.FileRegistration)
                    .WithMany(p => p.FileRegistrationError)
                    .HasForeignKey(d => d.FileRegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileRegistrationError_FileRegistration");
            });

            modelBuilder.Entity<FileRegistrationTransaction>(entity =>
            {
                entity.ToTable("FileRegistrationTransaction", "Admin");

                entity.HasComment("This table holds the data for transactions of file registration.");

                entity.Property(e => e.FileRegistrationTransactionId).HasComment("The identifier of the transaction of file registration");

                entity.Property(e => e.BlobPath).HasComment("The path of the blob (like a json/xml for event for a system like sinoper)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.FileRegistrationId).HasComment("The identifier of fileregistrations belonging to this transaction");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.RecordId)
                    .HasColumnName("RecordID")
                    .HasMaxLength(250)
                    .HasComment("The identifier used for pending transaction to link with this transaction");

                entity.Property(e => e.SessionId).HasComment("A movement id or a unique id generated from inventory product id");

                entity.Property(e => e.StatusTypeId).HasComment("The identifier of the status of the transaction (Processing, failed, etc)");

                entity.HasOne(d => d.FileRegistration)
                    .WithMany(p => p.FileRegistrationTransaction)
                    .HasForeignKey(d => d.FileRegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileRegistrationTransaction_FileRegistration");

                entity.HasOne(d => d.StatusType)
                    .WithMany(p => p.FileRegistrationTransaction)
                    .HasForeignKey(d => d.StatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileRegistrationTransaction_StatusType");
            });

            modelBuilder.Entity<FileUploadState>(entity =>
            {
                entity.ToTable("FileUploadState", "Admin");

                entity.HasComment("This table holds the data for File upload state like Processing, Finalized and Failed. This is a master table and contains seeded data.");

                entity.Property(e => e.FileUploadStateId)
                    .HasComment("The identifier for the file upload state")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.FileUploadState1)
                    .IsRequired()
                    .HasColumnName("FileUploadState")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The name for the state (FINALIZED, PROCESSING, FAILED)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");
            });

            modelBuilder.Entity<Homologation>(entity =>
            {
                entity.ToTable("Homologation", "Admin");

                entity.HasComment("This table holds the data for Homologation service for the registration of data mapping between TRUE and other Ecopetrol systems.");

                entity.Property(e => e.HomologationId).HasComment("The identifier of the homologation");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationSystemId).HasComment("The identifier of the destination system (like for Sinoper, Excel, etc)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.SourceSystemId).HasComment("The identifier of the source system (like for Sinoper, Excel, etc)");

                entity.HasOne(d => d.DestinationSystem)
                    .WithMany(p => p.HomologationDestinationSystem)
                    .HasForeignKey(d => d.DestinationSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Homologation_System2");

                entity.HasOne(d => d.SourceSystem)
                    .WithMany(p => p.HomologationSourceSystem)
                    .HasForeignKey(d => d.SourceSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Homologation_System1");
            });

            modelBuilder.Entity<HomologationDataMapping>(entity =>
            {
                entity.ToTable("HomologationDataMapping", "Admin");

                entity.HasComment(" This table holds the data for homologationGroupId and their source, destination values.");

                entity.Property(e => e.HomologationDataMappingId).HasComment("The identifier of homologation data mapping");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationValue)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the destination");

                entity.Property(e => e.HomologationGroupId).HasComment("The identifier of the homologation group");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.SourceValue)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the source  ");

                entity.HasOne(d => d.HomologationGroup)
                    .WithMany(p => p.HomologationDataMapping)
                    .HasForeignKey(d => d.HomologationGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HomologationDataMapping_HomologationGroup");
            });

            modelBuilder.Entity<HomologationGroup>(entity =>
            {
                entity.ToTable("HomologationGroup", "Admin");

                entity.HasComment("This table holds the data for Homologation and categories associated with it.");

                entity.Property(e => e.HomologationGroupId).HasComment("The identifier of the homologation group");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.GroupTypeId).HasComment("The identifier of the group type (CategoryId)");

                entity.Property(e => e.HomologationId).HasComment("The identifier of the homologation ");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version of the record")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.GroupType)
                    .WithMany(p => p.HomologationGroup)
                    .HasForeignKey(d => d.GroupTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HomologationGroup_Category");

                entity.HasOne(d => d.Homologation)
                    .WithMany(p => p.HomologationGroup)
                    .HasForeignKey(d => d.HomologationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HomologationGroup_Homologation");
            });

            modelBuilder.Entity<HomologationObject>(entity =>
            {
                entity.ToTable("HomologationObject", "Admin");

                entity.HasComment("This table holds the data about Homologation Object, its type, its group and if it requires mapping.");

                entity.HasIndex(e => new { e.HomologationObjectTypeId, e.HomologationGroupId })
                    .HasName("UC_HomologationObjectTypeId_HomologationGroupId")
                    .IsUnique();

                entity.Property(e => e.HomologationObjectId).HasComment("The identifier of homologation object");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.HomologationGroupId).HasComment("The identifier of the homologation group");

                entity.Property(e => e.HomologationObjectTypeId).HasComment("The identifier of homologation object type");

                entity.Property(e => e.IsRequiredMapping)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if the mapping is required or not, 1 means require");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.HasOne(d => d.HomologationGroup)
                    .WithMany(p => p.HomologationObject)
                    .HasForeignKey(d => d.HomologationGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HomologationObject_HomologationGroup");

                entity.HasOne(d => d.HomologationObjectType)
                    .WithMany(p => p.HomologationObject)
                    .HasForeignKey(d => d.HomologationObjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HomologationObject_HomologationObjectType");
            });

            modelBuilder.Entity<HomologationObjectType>(entity =>
            {
                entity.ToTable("HomologationObjectType", "Admin");

                entity.HasComment("This table holds the data for type of Homologation Objects (OwnerId, NodeId, ProductId, etc.). This is a master table and contains seeded data.");

                entity.HasIndex(e => e.Name)
                    .HasName("UC_HomologationObjectType")
                    .IsUnique();

                entity.Property(e => e.HomologationObjectTypeId).HasComment("The identifier of the homologation object type");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("The name of the homologation object type (NodeId, ProductId, MeasurementUnit, etc)");
            });

            modelBuilder.Entity<Icon>(entity =>
            {
                entity.ToTable("Icon", "Admin");

                entity.HasComment("This table stores svg data for Icons of an element. This is a master table and contains seeded data.");

                entity.HasIndex(e => e.Name)
                    .HasName("UC_Icon_Name")
                    .IsUnique();

                entity.Property(e => e.IconId).HasComment("The identifier of the icon");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasComment("The svg data of the icon");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of the icon (for accessing)");
            });

            modelBuilder.Entity<InventoryDetailsBeforeCutoff>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("InventoryDetailsBeforeCutoff", "Admin");

                entity.HasComment("This View is to Fetch Data for Inventory Details before Cutoff for both Segment and System for PowerBi Report From Tables (Inventory, InventoryProduct, Node, CategoryElement, Category).");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NetVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.TankName).HasMaxLength(20);

                entity.Property(e => e.UncertaintyPercentage).HasColumnType("decimal(29, 2)");
            });

            modelBuilder.Entity<InventoryDetailsWithOwner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("InventoryDetailsWithOwner", "Admin");

                entity.HasComment("This View is to Fetch Data [Admin].[InventoryDetailsWithOwnership] For PowerBi Report From Tables(Inventory, InventoryProduct, Unbalance, Ownership, Ticket, Product, Node, CategoryElement,Category)");

                entity.Property(e => e.AppliedRule)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BatchId).HasMaxLength(150);

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Incertidumbre).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.InventoryDate).HasColumnType("date");

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetVolume)
                    .HasColumnName("Net Volume")
                    .HasColumnType("decimal(29, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OwnerName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OwnershipPercentage).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.OwnershipVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SystemName).HasMaxLength(50);

                entity.Property(e => e.TankName).HasMaxLength(20);

                entity.Property(e => e.UncertainityPercentage).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.VolumeUnit)
                    .IsRequired()
                    .HasColumnName("Volume Unit")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<InventoryDetailsWithoutOwner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("InventoryDetailsWithoutOwner", "Admin");

                entity.HasComment("This View is to Fetch Data [Admin].[InventoryDetailsWithOutOwner] For PowerBi Report From Tables(Inventory, Product, Node, NodeConnectionProduct, NodeConnection, CategoryElement,Category)");

                entity.Property(e => e.BatchId).HasMaxLength(150);

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Incertidumbre).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MeasurmentUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.ProdutId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SystemName).HasMaxLength(50);

                entity.Property(e => e.TankName).HasMaxLength(20);

                entity.Property(e => e.UncertainityPercentage).HasColumnType("decimal(29, 2)");
            });

            modelBuilder.Entity<InventoryProduct>(entity =>
            {
                entity.ToTable("InventoryProduct", "Offchain");

                entity.HasComment("This table holds the details for association between inventories, products and their blockchain registration.");

                entity.Property(e => e.InventoryProductId).HasComment("The identifier ot the inventory product");

                entity.Property(e => e.BatchId)
                    .HasMaxLength(150)
                    .HasComment("The identifier of the batch");

                entity.Property(e => e.BlockNumber)
                    .HasMaxLength(255)
                    .HasComment("The value of the block number");

                entity.Property(e => e.BlockchainInventoryProductTransactionId).HasComment("The GUID of the blockchain inventory product transaction");

                entity.Property(e => e.BlockchainStatus).HasComment("The flag indicating if present in blockchain register");

                entity.Property(e => e.Comment)
                    .HasMaxLength(200)
                    .HasComment("The comment of the record");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationSystem)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasComment("The name of the destination system");

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("The type of the event (like Insert, Update, etc)");

                entity.Property(e => e.FileRegistrationTransactionId).HasComment("The identifier of the file registration transaction");

                entity.Property(e => e.GrossStandardQuantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InventoryDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the inventory was taken");

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The identifier of inventory (Inventory table is now deprecated)");

                entity.Property(e => e.InventoryProductUniqueId)
                    .HasMaxLength(150)
                    .HasComment("The identifier of the inventory product");

                entity.Property(e => e.IsDeleted).HasComment("The flag indicating if the element is deleted or not, 1 means delete");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MeasurementUnit)
                    .HasMaxLength(50)
                    .HasComment("The identifier of the measurement unit (category element of unit category, like Bbl)");

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.Observations)
                    .HasMaxLength(150)
                    .HasComment("The observations of the movement (like Reporte Operativo Cusiana -Fecha)");

                entity.Property(e => e.Operator)
                    .HasMaxLength(150)
                    .HasComment("The identifier of the operator");

                entity.Property(e => e.OwnershipTicketId).HasComment("The identifier of the ownership ticket");

                entity.Property(e => e.PreviousBlockchainInventoryProductTransactionId).HasComment("The GUID of the previous blockchain inventory product transaction");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.ProductType)
                    .HasMaxLength(150)
                    .HasComment("The identifier of the type of the product");

                entity.Property(e => e.ProductVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The volumen of the product");

                entity.Property(e => e.ReasonId).HasComment("The identifier of the reason (category element of reason category)");

                entity.Property(e => e.RetryCount).HasComment("The value of the retry count");

                entity.Property(e => e.ScenarioId).HasComment("The name of the scenario (like Operativo)");

                entity.Property(e => e.SegmentId).HasComment("The identifier of the segment (category element of segment category)");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasComment("The name of the source system from");

                entity.Property(e => e.SystemName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The name of the system from");

                entity.Property(e => e.SystemTypeId).HasComment("The identifier of the system type (Sinoper, etc)");

                entity.Property(e => e.TankName)
                    .HasMaxLength(20)
                    .HasComment("The name of the tank ");

                entity.Property(e => e.TicketId).HasComment("The identifier of the ticket");

                entity.Property(e => e.TransactionHash)
                    .HasMaxLength(255)
                    .HasComment("The value of the transaction hash");

                entity.Property(e => e.UncertaintyPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the uncertainty percentage");

                entity.Property(e => e.Version).HasMaxLength(50);

                entity.HasOne(d => d.FileRegistrationTransaction)
                    .WithMany(p => p.InventoryProduct)
                    .HasForeignKey(d => d.FileRegistrationTransactionId)
                    .HasConstraintName("FK_InventoryProduct_FileRegistrationTransaction");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.InventoryProduct)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryProduct_Node");

                entity.HasOne(d => d.OwnershipTicket)
                    .WithMany(p => p.InventoryProductOwnershipTicket)
                    .HasForeignKey(d => d.OwnershipTicketId)
                    .HasConstraintName("FK_InventoryProduct_Ticket");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InventoryProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryProduct_Product");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.InventoryProductReason)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK_InventoryProduct_CategoryElement_Reason");

                entity.HasOne(d => d.Scenario)
                    .WithMany(p => p.InventoryProduct)
                    .HasForeignKey(d => d.ScenarioId)
                    .HasConstraintName("FK_InventoryProduct_ScenarioType");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.InventoryProductSegment)
                    .HasForeignKey(d => d.SegmentId);

                entity.HasOne(d => d.SystemType)
                    .WithMany(p => p.InventoryProduct)
                    .HasForeignKey(d => d.SystemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryProduct_SystemType");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.InventoryProductTicket)
                    .HasForeignKey(d => d.TicketId);
            });

            modelBuilder.Entity<InventoryQualityDetailsBeforeCutoff>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("InventoryQualityDetailsBeforeCutoff", "Admin");

                entity.HasComment("This View is to Fetch Data for Inventory Quality Details before Cutoff for both Segment and System for PowerBi Report From Tables (Inventory, InventoryProduct, Attribute, Node, CategoryElement, Category).");

                entity.Property(e => e.AttributeDescription).HasMaxLength(150);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NetVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.TankName).HasMaxLength(20);

                entity.Property(e => e.UncertaintyPercentage).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.ValueAttributeUnit)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<KpidataByCategoryElementNodeWithOwnership>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("KPIDataByCategoryElementNodeWithOwnership", "Admin");

                entity.HasComment("This View is to Fetch KPIDataByCategoryElementNodeWithOwnership Data For PowerBi Report From Tables(Unbalance, Product, Ticket, Node, NodeTag, CategoryElement, Category)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CurrentValue).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.FilterType)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Indicator)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.NodeName).HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<KpipreviousDateDataByCategoryElementNodeWithOwner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("KPIPreviousDateDataByCategoryElementNodeWithOwner", "Admin");

                entity.HasComment("This View is to Fetch KPIPreviousDateDataByCategoryElementNodeWithOwnership Data For PowerBi Report From Tables(Unbalance, Product, Ticket, Node, NodeTag, CategoryElement, Category)");

                entity.Property(e => e.CalculationDatePrev).HasColumnType("date");

                entity.Property(e => e.CategoryPrev)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CurrentValuePrev).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.ElementPrev)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.FilterType)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Indicator)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.NodeNamePrev).HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<LogisticCenter>(entity =>
            {
                entity.ToTable("LogisticCenter", "Admin");

                entity.HasComment("This table holds the data for LogisticCenter.");

                entity.HasIndex(e => e.Name)
                    .HasName("UC_LogisticsCenter")
                    .IsUnique();

                entity.Property(e => e.LogisticCenterId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the logistic center");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if the logistc center is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the logistic center");
            });

            modelBuilder.Entity<MessageType>(entity =>
            {
                entity.ToTable("MessageType", "Admin");

                entity.HasComment("This table holds the data for MessageType. This is a master table and contains seeded data.");

                entity.Property(e => e.MessageTypeId).HasComment("The identifier of the message type");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasComment("The name of the message type (like Movement, Inventory, Event, Contract, etc)");
            });

            modelBuilder.Entity<ModelEvaluation>(entity =>
            {
                entity.ToTable("ModelEvaluation", "Analytics");

                entity.HasComment("This table holds the data for the Model Evaluation.");

                entity.Property(e => e.ModelEvaluationId).HasComment("The identifier of the model evaluation");

                entity.Property(e => e.AlgorithmId).HasComment("The identifier of the algorithm");

                entity.Property(e => e.AlgorithmType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the algorithn");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasDefaultValueSql("('Analytics')")
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.LoadDate)
                    .HasColumnType("datetime")
                    .HasComment("The date of loading the movement");

                entity.Property(e => e.MeanAbsoluteError)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the error absolute");

                entity.Property(e => e.OperationalDate)
                    .HasColumnType("date")
                    .HasComment("The operational date of the record");

                entity.Property(e => e.OwnershipPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of ownership");

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of transfer point");
            });

            modelBuilder.Entity<Movement>(entity =>
            {
                entity.HasKey(e => e.MovementTransactionId);

                entity.ToTable("Movement", "Offchain");

                entity.HasComment("This table holds the details related to Movements.");

                entity.Property(e => e.MovementTransactionId).HasComment("The identifier of the movement transaction");

                entity.Property(e => e.BackupMovementId).HasComment("The identifier of the backup movement");

                entity.Property(e => e.BalanceStatus)
                    .HasMaxLength(50)
                    .HasComment("The value of the balance status");

                entity.Property(e => e.BlockNumber)
                    .HasMaxLength(255)
                    .HasComment("The value of the block number");

                entity.Property(e => e.BlockchainMovementTransactionId).HasComment("The identifier of the blockchain inventory product transaction");

                entity.Property(e => e.BlockchainStatus).HasComment("The flag indicating if present in blockchain register");

                entity.Property(e => e.Classification)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("The classification of the movement (cls)");

                entity.Property(e => e.Comment)
                    .HasMaxLength(200)
                    .HasComment("The comment of the record");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasComment("The type of the event (like Insert, Update, etc.)");

                entity.Property(e => e.FileRegistrationTransactionId).HasComment("The identifier of file registration transaction");

                entity.Property(e => e.GlobalMovementId)
                    .HasMaxLength(50)
                    .HasComment("The identifier of the global movement ");

                entity.Property(e => e.GrossStandardVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the gross standard volume");

                entity.Property(e => e.IsDeleted).HasComment("The flag indicating if the element is deleted or not, 1 means delete");

                entity.Property(e => e.IsOfficial).HasComment("The value of if it is official transfer point");

                entity.Property(e => e.IsSystemGenerated).HasComment("The flag indicating if the system is generated or not, 1 means generate");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MeasurementUnit)
                    .HasMaxLength(50)
                    .HasComment("The value of the measurement unit ");

                entity.Property(e => e.MessageTypeId).HasComment("The identifier of the message type");

                entity.Property(e => e.MovementContractId).HasComment("The identifier of the movement contract");

                entity.Property(e => e.MovementEventId).HasComment("The identifier of the movement event");

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The identifier of the movement");

                entity.Property(e => e.MovementTypeId)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The identifier of the movement type");

                entity.Property(e => e.NetStandardVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the net standard volume");

                entity.Property(e => e.Observations)
                    .HasMaxLength(150)
                    .HasComment("The observations of the movement (like Reporte Operativo Cusiana -Fecha)");

                entity.Property(e => e.OperationalDate)
                    .HasColumnType("datetime")
                    .HasComment("The operational date of the movement");

                entity.Property(e => e.Operator)
                    .HasMaxLength(150)
                    .HasComment("The name of the operator");

                entity.Property(e => e.OwnershipTicketId).HasComment("The identifier of ownership ticket");

                entity.Property(e => e.PreviousBlockchainMovementTransactionId).HasComment("The identifier of the previous blockchain ownership");

                entity.Property(e => e.ReasonId).HasComment("The identifier of the reason (category element of reason category)");

                entity.Property(e => e.RetryCount).HasComment("The value of the retry count");

                entity.Property(e => e.SapprocessStatus)
                    .HasColumnName("SAPProcessStatus")
                    .HasMaxLength(50)
                    .HasComment("The value of the SAP Process Status");

                entity.Property(e => e.ScenarioId).HasComment("The value of the scenario (like Operativo)");

                entity.Property(e => e.SegmentId).HasComment("The identifier of the segment");

                entity.Property(e => e.SourceMovementId).HasComment("The identifier of the source movement");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasComment("The name of the source system (like Sinoper, Excel)");

                entity.Property(e => e.SystemName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The name of the system from (like EXCEL - OCENSA)");

                entity.Property(e => e.SystemTypeId).HasComment("The identifier of the system type");

                entity.Property(e => e.TicketId).HasComment("The identifier of the ticket ");

                entity.Property(e => e.Tolerance)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the tolerance");

                entity.Property(e => e.TransactionHash)
                    .HasMaxLength(255)
                    .HasComment("The value of the transaction hash");

                entity.Property(e => e.UncertaintyPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the uncertainty percentage");

                entity.Property(e => e.VariableTypeId).HasComment("The identifier of variable type (like Interface, Tolerance, Entrada, Salida, etc)");

                entity.Property(e => e.Version)
                    .HasMaxLength(50)
                    .HasComment("The value of the version");

                entity.HasOne(d => d.FileRegistrationTransaction)
                    .WithMany(p => p.Movement)
                    .HasForeignKey(d => d.FileRegistrationTransactionId)
                    .HasConstraintName("FK_Movement_FileRegistrationTransaction");

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.Movement)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Movement_MessageType");

                entity.HasOne(d => d.MovementContract)
                    .WithMany(p => p.Movement)
                    .HasForeignKey(d => d.MovementContractId)
                    .HasConstraintName("FK_Movement_MovemnetContract");

                entity.HasOne(d => d.MovementEvent)
                    .WithMany(p => p.Movement)
                    .HasForeignKey(d => d.MovementEventId)
                    .HasConstraintName("FK_Movement_MovementEvent");

                entity.HasOne(d => d.OwnershipTicket)
                    .WithMany(p => p.MovementOwnershipTicket)
                    .HasForeignKey(d => d.OwnershipTicketId)
                    .HasConstraintName("FK_Movement_Ticket_Ownership");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.MovementReason)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK_Movement_CategoryElement_Reason");

                entity.HasOne(d => d.Scenario)
                    .WithMany(p => p.Movement)
                    .HasForeignKey(d => d.ScenarioId)
                    .HasConstraintName("FK_Movement_ScenarioType");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.MovementSegment)
                    .HasForeignKey(d => d.SegmentId)
                    .HasConstraintName("FK_Movement_CategoryElement");

                entity.HasOne(d => d.SystemType)
                    .WithMany(p => p.Movement)
                    .HasForeignKey(d => d.SystemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Movement_SystemType");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.MovementTicket)
                    .HasForeignKey(d => d.TicketId);

                entity.HasOne(d => d.VariableType)
                    .WithMany(p => p.Movement)
                    .HasForeignKey(d => d.VariableTypeId)
                    .HasConstraintName("FK_Movement_VariableType");
            });

            modelBuilder.Entity<MovementContract>(entity =>
            {
                entity.ToTable("MovementContract", "Admin");

                entity.HasComment("This Table is to store contracts specific to movements");

                entity.Property(e => e.MovementContractId).HasComment("The identifier of the movement contract");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNodeId).HasComment("The identifier of the destination node");

                entity.Property(e => e.DocumentNumber).HasComment("The number of the document ");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the movement contract is ended");

                entity.Property(e => e.IsDeleted).HasComment("The flag indicating if the movement contract is deleted or not, 1 means delete");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MeasurementUnit).HasComment("The name or identifier of the measurement unit ");

                entity.Property(e => e.MovementTypeId).HasComment("The identifier of the movement type");

                entity.Property(e => e.Owner1Id)
                    .HasDefaultValueSql("((124))")
                    .HasComment("The identifier of the first owner (category element of owner category, like Ecopetrol)");

                entity.Property(e => e.Owner2Id)
                    .HasDefaultValueSql("((124))")
                    .HasComment("The identifier of the second owner (category element of owner category, like Ecopetrol)");

                entity.Property(e => e.Position).HasComment("The value of the position");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.SourceNodeId).HasComment("The identifier of the source node");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the movement contract is started");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The volumen");

                entity.HasOne(d => d.DestinationNode)
                    .WithMany(p => p.MovementContractDestinationNode)
                    .HasForeignKey(d => d.DestinationNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MeasurementUnitNavigation)
                    .WithMany(p => p.MovementContractMeasurementUnitNavigation)
                    .HasForeignKey(d => d.MeasurementUnit)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MovementType)
                    .WithMany(p => p.MovementContractMovementType)
                    .HasForeignKey(d => d.MovementTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Owner1)
                    .WithMany(p => p.MovementContractOwner1)
                    .HasForeignKey(d => d.Owner1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Owner2)
                    .WithMany(p => p.MovementContractOwner2)
                    .HasForeignKey(d => d.Owner2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.MovementContract)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovementContract_Product");

                entity.HasOne(d => d.SourceNode)
                    .WithMany(p => p.MovementContractSourceNode)
                    .HasForeignKey(d => d.SourceNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<MovementDestination>(entity =>
            {
                entity.ToTable("MovementDestination", "Offchain");

                entity.HasComment("This table holds the details for the Movement Destination.");

                entity.HasIndex(e => e.MovementTransactionId)
                    .HasName("UC_MovementDestination_Movement")
                    .IsUnique();

                entity.Property(e => e.MovementDestinationId).HasComment("The identifier of the movement destination");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNodeId).HasComment("The identifier of the destination node");

                entity.Property(e => e.DestinationProductId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the destination product");

                entity.Property(e => e.DestinationProductTypeId)
                    .HasMaxLength(150)
                    .HasComment("The identifier of the destination product type");

                entity.Property(e => e.DestinationStorageLocationId).HasComment("The identifier of the destination storage location");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MovementTransactionId).HasComment("The identifier of the movement transaction");

                entity.HasOne(d => d.DestinationNode)
                    .WithMany(p => p.MovementDestination)
                    .HasForeignKey(d => d.DestinationNodeId)
                    .HasConstraintName("FK_MovementDestination_Node");

                entity.HasOne(d => d.DestinationProduct)
                    .WithMany(p => p.MovementDestination)
                    .HasForeignKey(d => d.DestinationProductId)
                    .HasConstraintName("FK_MovementDestination_Product");

                entity.HasOne(d => d.DestinationStorageLocation)
                    .WithMany(p => p.MovementDestination)
                    .HasForeignKey(d => d.DestinationStorageLocationId)
                    .HasConstraintName("FK_MovementDestination_StorageLocation");

                entity.HasOne(d => d.MovementTransaction)
                    .WithOne(p => p.MovementDestination)
                    .HasForeignKey<MovementDestination>(d => d.MovementTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovementDestination_Movement");
            });

            modelBuilder.Entity<MovementDetailsBeforeCutoff>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MovementDetailsBeforeCutoff", "Admin");

                entity.HasComment("This View is to Fetch Data for Movement Details before Cutoff For PowerBi Report From Tables (view_MovementInformation, Movement, Node, NodeTag, NodeConnection, CategoryElement, Category).");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DestinationNode).HasMaxLength(150);

                entity.Property(e => e.DestinationProduct).HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.GrossStandardVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Movement)
                    .HasMaxLength(22)
                    .IsUnicode(false);

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Operacion)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PercentStandardUnCertainty).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SourceNode).HasMaxLength(150);

                entity.Property(e => e.SourceProduct).HasMaxLength(150);

                entity.Property(e => e.SourceProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Uncertainty).HasColumnType("decimal(29, 2)");
            });

            modelBuilder.Entity<MovementDetailsWithOwner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MovementDetailsWithOwner", "Admin");

                entity.HasComment("This View is to Fetch MovementDetailsWithOwner Data For PowerBi Report From Tables (Unbalance, Product, Node, NodeTag, CategoryElement, Category, Ownership)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DestinationNode).HasMaxLength(150);

                entity.Property(e => e.DestinationProduct).HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.GrossStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Movement)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(156);

                entity.Property(e => e.Operacion)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OperationalDate).HasColumnType("date");

                entity.Property(e => e.OwnerName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OwnershipPercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.OwnershipProcessDate).HasColumnType("datetime");

                entity.Property(e => e.OwnershipVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.Rule)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SourceNode).HasMaxLength(150);

                entity.Property(e => e.SourceProduct).HasMaxLength(150);

                entity.Property(e => e.SourceProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.StandardUncertainty)
                    .HasColumnName("% Standard Uncertainty")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.SystemName).HasMaxLength(50);

                entity.Property(e => e.Uncertainty).HasColumnType("decimal(24, 4)");
            });

            modelBuilder.Entity<MovementEvent>(entity =>
            {
                entity.ToTable("MovementEvent", "Admin");

                entity.HasComment("This table holds the data for movement events of planning, programming and collaboration agreements.");

                entity.Property(e => e.MovementEventId).HasComment("The identifier of the movement");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNodeId).HasComment("The identifier of the destination node");

                entity.Property(e => e.DestinationProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the destination product");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the movement event  is ended");

                entity.Property(e => e.EventTypeId).HasComment("The identifier of the event type (category element of event category)");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValueSql("((0))")
                    .HasComment("The flag indicating if the movement event is deleted or not, 1 means delete");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name or the identifier of the measurement unit ");

                entity.Property(e => e.Owner1Id)
                    .HasDefaultValueSql("((124))")
                    .HasComment("The identifier of the first owner (category element of owner category, like Ecopetrol)");

                entity.Property(e => e.Owner2Id)
                    .HasDefaultValueSql("((124))")
                    .HasComment("The identifier of the second owner (category element of owner category, like Ecopetrol)");

                entity.Property(e => e.SourceNodeId).HasComment("The identifier of the source node");

                entity.Property(e => e.SourceProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the source product ");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the movement event  is started");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The volume");

                entity.HasOne(d => d.DestinationNode)
                    .WithMany(p => p.MovementEventDestinationNode)
                    .HasForeignKey(d => d.DestinationNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovementEvent_Node2");

                entity.HasOne(d => d.DestinationProduct)
                    .WithMany(p => p.MovementEventDestinationProduct)
                    .HasForeignKey(d => d.DestinationProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovementEvent_Product2");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.MovementEventEventType)
                    .HasForeignKey(d => d.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovementEvent_EventType");

                entity.HasOne(d => d.Owner1)
                    .WithMany(p => p.MovementEventOwner1)
                    .HasForeignKey(d => d.Owner1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Owner2)
                    .WithMany(p => p.MovementEventOwner2)
                    .HasForeignKey(d => d.Owner2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SourceNode)
                    .WithMany(p => p.MovementEventSourceNode)
                    .HasForeignKey(d => d.SourceNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovementEvent_Node1");

                entity.HasOne(d => d.SourceProduct)
                    .WithMany(p => p.MovementEventSourceProduct)
                    .HasForeignKey(d => d.SourceProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovementEvent_Product1");
            });

            modelBuilder.Entity<MovementPeriod>(entity =>
            {
                entity.ToTable("MovementPeriod", "Offchain");

                entity.HasComment("This table holds the details for the Movement Period.");

                entity.HasIndex(e => e.MovementTransactionId)
                    .HasName("UC_MovementPeriod_Movement")
                    .IsUnique();

                entity.Property(e => e.MovementPeriodId).HasComment("The identifier of the movement period");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the movement is ended");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MovementTransactionId).HasComment("The identifier of the movement transaction ");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the movement is started");

                entity.HasOne(d => d.MovementTransaction)
                    .WithOne(p => p.MovementPeriod)
                    .HasForeignKey<MovementPeriod>(d => d.MovementTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovementPeriod_Movement");
            });

            modelBuilder.Entity<MovementQualityDetailsBeforeCutoff>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MovementQualityDetailsBeforeCutoff", "Admin");

                entity.HasComment("This View is to Fetch Data for Movement Details before Cutoff For PowerBi Report From Tables (view_MovementInformation, Movement, Node, NodeTag, NodeConnection, CategoryElement, Category, Attribute).");

                entity.Property(e => e.AttributeDescription).HasMaxLength(150);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DestinationNode).HasMaxLength(150);

                entity.Property(e => e.DestinationProduct).HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.GrossStandardVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Movement)
                    .HasMaxLength(22)
                    .IsUnicode(false);

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Operacion)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PercentStandardUnCertainty).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SourceNode).HasMaxLength(150);

                entity.Property(e => e.SourceProduct).HasMaxLength(150);

                entity.Property(e => e.SourceProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Uncertainty).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.ValueAttributeUnit)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MovementSource>(entity =>
            {
                entity.ToTable("MovementSource", "Offchain");

                entity.HasComment("This table holds the details for the Movement Source.");

                entity.HasIndex(e => e.MovementTransactionId)
                    .HasName("UC_MovementSource_Movement")
                    .IsUnique();

                entity.Property(e => e.MovementSourceId).HasComment("The identifier of the movement source");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MovementTransactionId).HasComment("The identifier of the movement transaction ");

                entity.Property(e => e.SourceNodeId).HasComment("The identifier of the source node");

                entity.Property(e => e.SourceProductId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the source product");

                entity.Property(e => e.SourceProductTypeId)
                    .HasMaxLength(150)
                    .HasComment("The identifier of the source product type");

                entity.Property(e => e.SourceStorageLocationId).HasComment("The identifier of the source storage location");

                entity.HasOne(d => d.MovementTransaction)
                    .WithOne(p => p.MovementSource)
                    .HasForeignKey<MovementSource>(d => d.MovementTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovementSource_Movement");

                entity.HasOne(d => d.SourceNode)
                    .WithMany(p => p.MovementSource)
                    .HasForeignKey(d => d.SourceNodeId)
                    .HasConstraintName("FK_MovementSource_Node");

                entity.HasOne(d => d.SourceProduct)
                    .WithMany(p => p.MovementSource)
                    .HasForeignKey(d => d.SourceProductId)
                    .HasConstraintName("FK_MovementSource_Product");

                entity.HasOne(d => d.SourceStorageLocation)
                    .WithMany(p => p.MovementSource)
                    .HasForeignKey(d => d.SourceStorageLocationId)
                    .HasConstraintName("FK_MovementSource_StorageLocation");
            });

            modelBuilder.Entity<MovementsByProductWithOwner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MovementsByProductWithOwner", "Admin");

                entity.HasComment("This View is to Fetch MovementsByProductWithOwner Data For PowerBi Report From Tables(OwnershipCalculation, SegmentOwnershipCalculation, SystemOwnershipCalculation, Product, Ticket, Node, NodeTag, CategoryElement,  Category)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Control).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.FilterType)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FinalInventory).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.IdentifiedLosses).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.InitialInventory).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Input).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Interface).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.NodeName).HasMaxLength(150);

                entity.Property(e => e.Output).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.OwnerName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Tolerance).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.UnidentifiedLosses).HasColumnType("decimal(29, 2)");
            });

            modelBuilder.Entity<MovementsInformationByOwner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MovementsInformationByOwner", "Admin");

                entity.HasComment("This View is to Fetch Data [Admin].[MovementsInformationByOwner] For PowerBi Report From Tables(Unbalance,OwnershipCalculation,OwnershipCalculationResult, Ticket, Product, Node,CategoryElement,Category)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Control).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.FinalInvnetory).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IdentifiedLosses).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InitialInventory).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Inputs).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interface).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Outputs).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OwnerName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OwnershipPercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Tolerance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnidentifiedLosses).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Node>(entity =>
            {
                entity.ToTable("Node", "Admin");

                entity.HasComment("This table holds information about the Node created in the system.");

                entity.HasIndex(e => e.Name)
                    .HasName("UC_Node")
                    .IsUnique();

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.AcceptableBalancePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The acceptable balance percentage");

                entity.Property(e => e.Capacity)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The capacity");

                entity.Property(e => e.ControlLimit)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The control limit");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasComment("The description of the node");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if the node is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.LogisticCenterId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the logistic center");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the node");

                entity.Property(e => e.NodeOwnershipRuleId).HasComment("The identifier of Estrategia de propiedad for node");

                entity.Property(e => e.Order).HasComment("The order");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version column used for consistency")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.SendToSap)
                    .HasColumnName("SendToSAP")
                    .HasComment("The flag indicating if the node should be sent to SAP, 1 means yes");

                entity.Property(e => e.UnitId).HasComment("The identifier of the unit (category element of unit category, like Bbl)");

                entity.HasOne(d => d.LogisticCenter)
                    .WithMany(p => p.Node)
                    .HasForeignKey(d => d.LogisticCenterId)
                    .HasConstraintName("FK_Node_LogisticCenter");

                entity.HasOne(d => d.NodeOwnershipRule)
                    .WithMany(p => p.Node)
                    .HasForeignKey(d => d.NodeOwnershipRuleId)
                    .HasConstraintName("FK_Node_NodeOwnershipRule");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Node)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_Node_CategoryElement");
            });

            modelBuilder.Entity<NodeConnection>(entity =>
            {
                entity.ToTable("NodeConnection", "Admin");

                entity.HasComment("This table holds the data for Node Connections between source and destination Nodes.");

                entity.Property(e => e.NodeConnectionId).HasComment("The identifier for connection between source and destination node");

                entity.Property(e => e.AlgorithmId).HasComment("The identifier of algorithm for analytical model");

                entity.Property(e => e.ControlLimit)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The control limit");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.Description)
                    .HasMaxLength(300)
                    .HasComment("The description of the connection");

                entity.Property(e => e.DestinationNodeId).HasComment("The identifier of the destination node");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if the connection is active or not, 1 means active");

                entity.Property(e => e.IsDeleted).HasComment("The flag indicating if the connection is deleted, 1 means deleted");

                entity.Property(e => e.IsTransfer).HasComment("The flag indicating if the connection is a transfer point, 1 means yes. Transfer point ownership calculated through analyticsAPI");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version column used for consistency")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.SourceNodeId).HasComment("The identifier of the source node");

                entity.HasOne(d => d.Algorithm)
                    .WithMany(p => p.NodeConnection)
                    .HasForeignKey(d => d.AlgorithmId)
                    .HasConstraintName("FK_NodeConnection_Algorithm");

                entity.HasOne(d => d.DestinationNode)
                    .WithMany(p => p.NodeConnectionDestinationNode)
                    .HasForeignKey(d => d.DestinationNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SourceNode)
                    .WithMany(p => p.NodeConnectionSourceNode)
                    .HasForeignKey(d => d.SourceNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<NodeConnectionInformation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("NodeConnectionInformation", "Admin");

                entity.HasComment("This View is used to fetch the data for Node Connections related to the Nodes, to be displayed in the Node Configuration Report.");

                entity.Property(e => e.AlgorithmName).HasMaxLength(150);

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ConnectionType)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeConnectionName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OwnerName).HasMaxLength(150);

                entity.Property(e => e.OwnershipPercentage).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.OwnershipStrategy).HasMaxLength(100);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UncertaintyConnectionProduct).HasColumnType("decimal(9, 6)");
            });

            modelBuilder.Entity<NodeConnectionProduct>(entity =>
            {
                entity.ToTable("NodeConnectionProduct", "Admin");

                entity.HasComment(" This table holds the details for associations of products with the connections of nodes.");

                entity.HasIndex(e => new { e.NodeConnectionId, e.ProductId })
                    .HasName("UC_NodeConnectionId_ProductId")
                    .IsUnique();

                entity.Property(e => e.NodeConnectionProductId).HasComment("The identifier for an association of product with a node connection");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.IsDeleted).HasComment("The flag indicating if the connection is deleted, 1 means deleted");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.NodeConnectionId).HasComment("The identifier for a node connection");

                entity.Property(e => e.NodeConnectionProductRuleId).HasComment("The identifier of Estrategia de propiedad for connection");

                entity.Property(e => e.Priority)
                    .HasDefaultValueSql("((10))")
                    .HasComment("The priority of connection or product between 1 and 10");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier for a product");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version column used for consistency")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.UncertaintyPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The uncertainty percentage");

                entity.HasOne(d => d.NodeConnection)
                    .WithMany(p => p.NodeConnectionProduct)
                    .HasForeignKey(d => d.NodeConnectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NodeConnectionProduct_NodeConnection");

                entity.HasOne(d => d.NodeConnectionProductRule)
                    .WithMany(p => p.NodeConnectionProduct)
                    .HasForeignKey(d => d.NodeConnectionProductRuleId)
                    .HasConstraintName("FK_NodeConnectionProduct_NodeConnectionProductRule");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.NodeConnectionProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NodeConnectionProduct_Product");
            });

            modelBuilder.Entity<NodeConnectionProductOwner>(entity =>
            {
                entity.ToTable("NodeConnectionProductOwner", "Admin");

                entity.HasComment("This table holds details for associations of owners with a node connection products.");

                entity.HasIndex(e => new { e.NodeConnectionProductId, e.OwnerId })
                    .HasName("UC_NodeConnectionProductId_OwnerId")
                    .IsUnique();

                entity.Property(e => e.NodeConnectionProductOwnerId).HasComment("The identifier for an association of an owner with a node connection product");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.IsDeleted).HasComment("The flag indicating if the connection is deleted, 1 means deleted");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.NodeConnectionProductId).HasComment("The identifier of node connection product");

                entity.Property(e => e.OwnerId).HasComment("The identifier of the owner (category element of owner category, like Ecopetrol)");

                entity.Property(e => e.OwnershipPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The percentage of ownership");

                entity.HasOne(d => d.NodeConnectionProduct)
                    .WithMany(p => p.NodeConnectionProductOwner)
                    .HasForeignKey(d => d.NodeConnectionProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NodeConnectionProductOwner_NodeConnectionProduct");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.NodeConnectionProductOwner)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NodeConnectionProductOwner_CategoryElement");
            });

            modelBuilder.Entity<NodeConnectionProductRule>(entity =>
            {
                entity.HasKey(e => e.RuleId);

                entity.ToTable("NodeConnectionProductRule", "Admin");

                entity.HasComment("This table contains ownership rules for node connections.");

                entity.Property(e => e.RuleId)
                    .HasComment("The identifier of the rule")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.IsActive).HasComment("The flag indicating if the rule is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.RuleName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the rule");
            });

            modelBuilder.Entity<NodeGeneralInformation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("NodeGeneralInformation", "Admin");

                entity.HasComment("This View is used to fetch the general Node information, to be displayed in the Node Configuration Report.");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeAcceptableBalancePercentage)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.NodeControlLimit)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeOrder)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NodeOwnershipStrategy)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<NodeOwnershipRule>(entity =>
            {
                entity.HasKey(e => e.RuleId);

                entity.ToTable("NodeOwnershipRule", "Admin");

                entity.HasComment("This table contains ownership rules for nodes.");

                entity.Property(e => e.RuleId)
                    .HasComment("The identifier of rule ")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.IsActive).HasComment("The flag indicating if the rule is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.RuleName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the rule");
            });

            modelBuilder.Entity<NodeProductInformation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("NodeProductInformation", "Admin");

                entity.HasComment("This View is used to fetch the data for Products related to the Nodes, to be displayed in the Node Configuration Report.");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OwnerName).HasMaxLength(150);

                entity.Property(e => e.OwnershipPercentage).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.OwnershipStrategy).HasMaxLength(100);

                entity.Property(e => e.Pi).HasColumnName("PI");

                entity.Property(e => e.Pni).HasColumnName("PNI");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductOrder)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.UncertaintyNodeProduct).HasColumnType("decimal(9, 6)");
            });

            modelBuilder.Entity<NodeProductRule>(entity =>
            {
                entity.HasKey(e => e.RuleId);

                entity.ToTable("NodeProductRule", "Admin");

                entity.HasComment("This table contains ownership rules for association between product and node-storagelocation.");

                entity.Property(e => e.RuleId)
                    .HasComment("The identifier of the rule")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.IsActive).HasComment("The flag indicating if the rule is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.RuleName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the rule");
            });

            modelBuilder.Entity<NodeStatusIconUrl>(entity =>
            {
                entity.ToTable("NodeStatusIconUrl", "Admin");

                entity.HasComment("This table is created to store urls for icons, representing node statuses by different colors for a ticket, to display on Power Bi report. This is a seeded table, different for every environment.");

                entity.Property(e => e.NodeStatusIconUrlId).HasComment("The identifier for status icons used in the report");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.NodeStatusIconUrl1)
                    .IsRequired()
                    .HasColumnName("NodeStatusIconUrl")
                    .HasMaxLength(2038)
                    .HasComment("The icon url (deployed in UI) ");
            });

            modelBuilder.Entity<NodeStorageLocation>(entity =>
            {
                entity.ToTable("NodeStorageLocation", "Admin");

                entity.HasComment("This table contains the association between nodes and storage locations.");

                entity.Property(e => e.NodeStorageLocationId).HasComment("The identifier for an association of node and storage location");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasComment("The description");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if the association between node and storage location is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name for the association between node and storage location");

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.SendToSap)
                    .HasColumnName("SendToSAP")
                    .HasComment("The flag indicating if this should be sent to SAP or not , 1 means yes");

                entity.Property(e => e.StorageLocationId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of storage location");

                entity.Property(e => e.StorageLocationTypeId).HasComment("The type of the storage location (category element of storage location category)");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.NodeStorageLocation)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NodeStorageLocation_Node");

                entity.HasOne(d => d.StorageLocation)
                    .WithMany(p => p.NodeStorageLocation)
                    .HasForeignKey(d => d.StorageLocationId)
                    .HasConstraintName("FK_NodeStorageLocation_StorageLocation");

                entity.HasOne(d => d.StorageLocationType)
                    .WithMany(p => p.NodeStorageLocation)
                    .HasForeignKey(d => d.StorageLocationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NodeStorageLocation_CategoryElement");
            });

            modelBuilder.Entity<NodeTag>(entity =>
            {
                entity.ToTable("NodeTag", "Admin");

                entity.HasComment("This table holds the information about Nodes and tagged elements with it.");

                entity.Property(e => e.NodeTagId).HasComment("The identifier of the association between element and node for a date range");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.ElementId).HasComment("The identifier of the category element");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasComment("The enddate of the node validity for the element");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasComment("The startdate of the node validity for the element");

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.NodeTag)
                    .HasForeignKey(d => d.ElementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NodeTag_CategoryElement");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.NodeTag)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NodeTag_Node");
            });

            modelBuilder.Entity<Operational>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Operational", "Admin");

                entity.HasComment("This table holds the data for summary before cutoff. This table is being used in before cutoff report.");

                entity.Property(e => e.CalculationDate)
                    .HasColumnType("date")
                    .HasComment("The datetime when the record is calculated");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.ExecutionId)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("The identifier of the execution ");

                entity.Property(e => e.FinalInventory)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the final inventory");

                entity.Property(e => e.IdentifiedLosses)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the identified losses");

                entity.Property(e => e.InputCategory)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input category");

                entity.Property(e => e.InputElementName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input element");

                entity.Property(e => e.InputEndDate)
                    .HasColumnType("date")
                    .HasComment("The end date of the input");

                entity.Property(e => e.InputNodeName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input node");

                entity.Property(e => e.InputStartDate)
                    .HasColumnType("date")
                    .HasComment("The start date of the input");

                entity.Property(e => e.Inputs)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the inputs");

                entity.Property(e => e.IntialInventory)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the initial inventory");

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.Outputs)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the outputs");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasColumnName("ProductID")
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the product");

                entity.Property(e => e.SegmentId)
                    .HasColumnName("SegmentID")
                    .HasComment("The identifier of the segment ");

                entity.Property(e => e.SegmentName)
                    .HasMaxLength(150)
                    .HasComment("The name of the segment");

                entity.Property(e => e.UnBalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the unbalance");
            });

            modelBuilder.Entity<OperationalInventory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OperationalInventory", "Admin");

                entity.HasComment("This table holds the data for inventories before cutoff. This table is being used in before cutoff report.");

                entity.Property(e => e.BatchId)
                    .HasMaxLength(150)
                    .HasComment("The identifier of the batch");

                entity.Property(e => e.CalculationDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the calculation was done");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.ExecutionId)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("The identifier of the execution of the record");

                entity.Property(e => e.InputCategory)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input category");

                entity.Property(e => e.InputElementName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input element");

                entity.Property(e => e.InputEndDate)
                    .HasColumnType("date")
                    .HasComment("The date when the input is ended");

                entity.Property(e => e.InputNodeName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input node ");

                entity.Property(e => e.InputStartDate)
                    .HasColumnType("date")
                    .HasComment("The date when the input is started");

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The identifier of the inventory");

                entity.Property(e => e.MeasurementUnit)
                    .HasMaxLength(150)
                    .HasComment("The value of the measurement units");

                entity.Property(e => e.NetStandardVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the net standard volume");

                entity.Property(e => e.NodeName)
                    .HasMaxLength(150)
                    .HasComment("The name of the node");

                entity.Property(e => e.PercentStandardUnCertainty)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage standard of the uncertainty");

                entity.Property(e => e.Product)
                    .HasMaxLength(150)
                    .HasComment("The name of the product");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.Rno)
                    .HasColumnName("RNo")
                    .HasComment("The correlative number of the record");

                entity.Property(e => e.SystemName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The name of the system ");

                entity.Property(e => e.TankName)
                    .HasMaxLength(20)
                    .HasComment("The name of the tank");
            });

            modelBuilder.Entity<OperationalInventoryQuality>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OperationalInventoryQuality", "Admin");

                entity.HasComment("This table holds the data for quality attributes of inventories before cutoff. This table is being used in before cutoff report.");

                entity.Property(e => e.AttributeDescription)
                    .HasMaxLength(150)
                    .HasComment("The description of the attribute");

                entity.Property(e => e.AttributeValue)
                    .HasMaxLength(150)
                    .HasComment("The value of the attribute");

                entity.Property(e => e.BatchId)
                    .HasMaxLength(150)
                    .HasComment("The identifier of the batch");

                entity.Property(e => e.CalculationDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the calculation was done");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.ExecutionId)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("The identifier of the execution of the record");

                entity.Property(e => e.InputCategory)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input category");

                entity.Property(e => e.InputElementName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input element");

                entity.Property(e => e.InputEndDate)
                    .HasColumnType("date")
                    .HasComment("The date when the input is ended");

                entity.Property(e => e.InputNodeName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input node");

                entity.Property(e => e.InputStartDate)
                    .HasColumnType("date")
                    .HasComment("The date when the input is started");

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The identifier of the inventory");

                entity.Property(e => e.MeasurementUnit)
                    .HasMaxLength(150)
                    .HasComment("The value of the measurement unit");

                entity.Property(e => e.NetStandardVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the net standard volume");

                entity.Property(e => e.NodeName)
                    .HasMaxLength(150)
                    .HasComment("The name of the node");

                entity.Property(e => e.PercentStandardUnCertainty)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the percentage standard of the uncertainty");

                entity.Property(e => e.Product)
                    .HasMaxLength(150)
                    .HasComment("The name of the product");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product ");

                entity.Property(e => e.Rno)
                    .HasColumnName("RNo")
                    .HasComment("The correlative number of the record");

                entity.Property(e => e.SystemName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The name of the system");

                entity.Property(e => e.TankName)
                    .HasMaxLength(20)
                    .HasComment("The name of the tank");

                entity.Property(e => e.ValueAttributeUnit)
                    .HasMaxLength(50)
                    .HasComment("The value of the attribute unit");
            });

            modelBuilder.Entity<OperationalMovement>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OperationalMovement", "Admin");

                entity.HasComment(" This table holds the data for inventories before cutoff. This table is being used in before cutoff report.");

                entity.Property(e => e.CalculationDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the calculation was done");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNode)
                    .HasMaxLength(150)
                    .HasComment("The name of the destination node");

                entity.Property(e => e.DestinationProduct)
                    .HasMaxLength(150)
                    .HasComment("The name of the destination product");

                entity.Property(e => e.ExecutionId)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("The identifier of the execution of the record");

                entity.Property(e => e.GrossStandardVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the gross standard volume");

                entity.Property(e => e.InputCategory)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input category");

                entity.Property(e => e.InputElementName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input element");

                entity.Property(e => e.InputEndDate)
                    .HasColumnType("date")
                    .HasComment("The date when the input is ended");

                entity.Property(e => e.InputNodeName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input node");

                entity.Property(e => e.InputStartDate)
                    .HasColumnType("date")
                    .HasComment("The date when the input is started");

                entity.Property(e => e.MeasurementUnit)
                    .HasMaxLength(150)
                    .HasComment("The value of the measurement unit");

                entity.Property(e => e.Movement)
                    .HasMaxLength(150)
                    .HasComment("The name of the movement");

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The movement identifier by the process");

                entity.Property(e => e.MovementTypeName)
                    .HasMaxLength(150)
                    .HasComment("The name of the movement type");

                entity.Property(e => e.NetStandardVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the net standard volume");

                entity.Property(e => e.PercentStandardUnCertainty)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage standard of the uncertainty");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasColumnName("ProductID")
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.Rno)
                    .HasColumnName("RNo")
                    .HasComment("The correlative number of the record");

                entity.Property(e => e.SourceNode)
                    .HasMaxLength(150)
                    .HasComment("The name of the source node");

                entity.Property(e => e.SourceProduct)
                    .HasMaxLength(150)
                    .HasComment("The name of the source product");

                entity.Property(e => e.SystemName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The name of the system ");

                entity.Property(e => e.Uncertainty)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the uncertainty");
            });

            modelBuilder.Entity<OperationalMovementQuality>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OperationalMovementQuality", "Admin");

                entity.HasComment("This table holds the data for quality attributes of movements before cutoff. This table is being used in before cutoff report.");

                entity.Property(e => e.AttributeDescription)
                    .HasMaxLength(150)
                    .HasComment("The description of the attribute");

                entity.Property(e => e.AttributeValue)
                    .HasMaxLength(150)
                    .HasComment("The value of the attribute");

                entity.Property(e => e.CalculationDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the calculation was done");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNode)
                    .HasMaxLength(150)
                    .HasComment("The name of the destination node");

                entity.Property(e => e.DestinationProduct)
                    .HasMaxLength(150)
                    .HasComment("The name of the destination product");

                entity.Property(e => e.ExecutionId)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("The identifier of the execution of the record");

                entity.Property(e => e.GrossStandardVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the gross standard volume");

                entity.Property(e => e.InputCategory)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input category");

                entity.Property(e => e.InputElementName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input element");

                entity.Property(e => e.InputEndDate)
                    .HasColumnType("date")
                    .HasComment("The date when the input is ended");

                entity.Property(e => e.InputNodeName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the input node");

                entity.Property(e => e.InputStartDate)
                    .HasColumnType("date")
                    .HasComment("The date when the input is started");

                entity.Property(e => e.MeasurementUnit)
                    .HasMaxLength(150)
                    .HasComment("The value of the measurement unit");

                entity.Property(e => e.Movement)
                    .HasMaxLength(150)
                    .HasComment("The name of the movement");

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The identifier of the movement");

                entity.Property(e => e.MovementTypeName)
                    .HasMaxLength(150)
                    .HasComment("The name of the movement type");

                entity.Property(e => e.NetStandardVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the net standard volume");

                entity.Property(e => e.PercentStandardUnCertainty)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage standard of the uncertainty");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasColumnName("ProductID")
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.Rno)
                    .HasColumnName("RNo")
                    .HasComment("The correlative number of the record");

                entity.Property(e => e.SourceNode)
                    .HasMaxLength(150)
                    .HasComment("The name of the source node");

                entity.Property(e => e.SourceProduct)
                    .HasMaxLength(150)
                    .HasComment("The name of the source product");

                entity.Property(e => e.SystemName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The name of the system");

                entity.Property(e => e.Uncertainty)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the uncertainty");

                entity.Property(e => e.ValueAttributeUnit)
                    .HasMaxLength(50)
                    .HasComment("The value of the attribute unit");
            });

            modelBuilder.Entity<OperativeMovements>(entity =>
            {
                entity.ToTable("OperativeMovements", "Analytics");

                entity.HasComment("This table holds the data for the Operative Movements.");

                entity.HasIndex(e => e.DestinationNode)
                    .HasName("NIX_OperativeMovements_DestinationNode");

                entity.HasIndex(e => e.DestinationNodeType)
                    .HasName("NIX_OperativeMovements_DestinationNodeType");

                entity.HasIndex(e => e.MovementType)
                    .HasName("NIX_OperativeMovements_MovementType");

                entity.HasIndex(e => e.SourceNode)
                    .HasName("NIX_OperativeMovements_SourceNode");

                entity.HasIndex(e => e.SourceNodeType)
                    .HasName("NIX_OperativeMovements_SourceNodeType");

                entity.HasIndex(e => e.SourceProduct)
                    .HasName("NIX_OperativeMovements_SourceProduct");

                entity.HasIndex(e => e.SourceProductType)
                    .HasName("NIX_OperativeMovements_SourceProductType");

                entity.Property(e => e.OperativeMovementsId).HasComment("The identifier of the movement without ownership");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasDefaultValueSql("('ADF')")
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the destination node");

                entity.Property(e => e.DestinationNodeType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the destination node");

                entity.Property(e => e.ExecutionId)
                    .HasColumnName("ExecutionID")
                    .HasComment("The identifier of the execution of pipeline");

                entity.Property(e => e.FieldWaterProduction)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the field of water of production ");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.LoadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([admin].[udf_GetTrueDate]())")
                    .HasComment("The date of loading the movement");

                entity.Property(e => e.MovementType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the movement");

                entity.Property(e => e.NetStandardVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The net volumen of the movement ");

                entity.Property(e => e.OperationalDate)
                    .HasColumnType("date")
                    .HasComment("The operational date of the movement");

                entity.Property(e => e.RelatedSourceField)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source field related");

                entity.Property(e => e.SourceField)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source field");

                entity.Property(e => e.SourceNode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source node");

                entity.Property(e => e.SourceNodeType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the source node");

                entity.Property(e => e.SourceProduct)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source product");

                entity.Property(e => e.SourceProductType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the source product");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('CSV')")
                    .HasComment("The name of the source system which movement come from");

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the transfer point");
            });

            modelBuilder.Entity<OperativeMovementsWithOwnership>(entity =>
            {
                entity.ToTable("OperativeMovementsWithOwnership", "Analytics");

                entity.HasComment("This table holds the data for the Operative Movements With Ownership.");

                entity.Property(e => e.OperativeMovementsWithOwnershipId).HasComment("The identifier of the movement with ownership");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasDefaultValueSql("('ADF')")
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DayOfMonth).HasComment("The day of the month of the operational date");

                entity.Property(e => e.DestinationProduct)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the destination product");

                entity.Property(e => e.DestinationStorageLocation)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the location of the destination storage");

                entity.Property(e => e.ExecutionId)
                    .HasColumnName("ExecutionID")
                    .HasComment("The identifier of the execution of pipeline");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.LoadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The date of loading the movement");

                entity.Property(e => e.Month).HasComment("The number of the month of the operational date");

                entity.Property(e => e.MovementType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the movement");

                entity.Property(e => e.OperationalDate)
                    .HasColumnType("date")
                    .HasComment("The operational date of the movement");

                entity.Property(e => e.OwnershipVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The volume of the movement");

                entity.Property(e => e.SourceProduct)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source product");

                entity.Property(e => e.SourceStorageLocation)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the location of the source storage");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('CSV')")
                    .HasComment("The name of the source system which movement come from");

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the transfer point");

                entity.Property(e => e.Year).HasComment("The year of the operational date");
            });

            modelBuilder.Entity<OperativeNodeRelationship>(entity =>
            {
                entity.ToTable("OperativeNodeRelationship", "Analytics");

                entity.HasComment("This table holds the data for the OperativeNode Relationship.");

                entity.HasIndex(e => e.DestinationNode)
                    .HasName("NIX_OperativeNodeRelationship_DestinationNode");

                entity.HasIndex(e => e.DestinationNodeType)
                    .HasName("NIX_OperativeNodeRelationship_DestinationNodeType");

                entity.HasIndex(e => e.MovementType)
                    .HasName("NIX_OperativeNodeRelationship_MovementType");

                entity.HasIndex(e => e.SourceNode)
                    .HasName("NIX_OperativeNodeRelationship_SourceNode");

                entity.HasIndex(e => e.SourceNodeType)
                    .HasName("NIX_OperativeNodeRelationship_SourceNodeType");

                entity.HasIndex(e => e.SourceProduct)
                    .HasName("NIX_OperativeNodeRelationship_SourceProduct");

                entity.HasIndex(e => e.SourceProductType)
                    .HasName("NIX_OperativeNodeRelationship_SourceProductType");

                entity.Property(e => e.OperativeNodeRelationshipId).HasComment("The identifier of the node relationship without ownership");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasDefaultValueSql("('ADF')")
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the destination node");

                entity.Property(e => e.DestinationNodeType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the destination node");

                entity.Property(e => e.ExecutionId)
                    .HasColumnName("ExecutionID")
                    .HasComment("The identifier of the execution of pipeline");

                entity.Property(e => e.FieldWaterProduction)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the field of water of production ");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValueSql("((0))")
                    .HasComment("The flag indicating if the record is deleted or not, 1 means delete");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.LoadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The date of loading the movement");

                entity.Property(e => e.MovementType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the movement");

                entity.Property(e => e.Notes)
                    .HasMaxLength(200)
                    .HasComment("Additional record information");

                entity.Property(e => e.RelatedSourceField)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source field related");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version column used for consistency")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.SourceField)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source field");

                entity.Property(e => e.SourceNode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source node");

                entity.Property(e => e.SourceNodeType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the source node");

                entity.Property(e => e.SourceProduct)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source product");

                entity.Property(e => e.SourceProductType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the source product");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('CSV')")
                    .HasComment("The name of the source system which record come from");

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the transfer point");
            });

            modelBuilder.Entity<OperativeNodeRelationshipWithOwnership>(entity =>
            {
                entity.ToTable("OperativeNodeRelationshipWithOwnership", "Analytics");

                entity.HasComment("This table holds the data for the OperativeNode Relationship With Ownership.");

                entity.Property(e => e.OperativeNodeRelationshipWithOwnershipId).HasComment("The identifier of the node relationship with ownership");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasDefaultValueSql("('ADF')")
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationProduct)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the destination product");

                entity.Property(e => e.ExecutionId)
                    .HasColumnName("ExecutionID")
                    .HasComment("The identifier of the execution of pipeline");

                entity.Property(e => e.IsDeleted).HasComment("The flag indicating if the record is deleted or not, 1 means delete");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.LoadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The date of loading the record");

                entity.Property(e => e.LogisticDestinationCenter)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the logistic destination center");

                entity.Property(e => e.LogisticSourceCenter)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the logistic source center ");

                entity.Property(e => e.Notes)
                    .HasMaxLength(1000)
                    .HasComment("Additional record information");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version column used for consistency")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.SourceProduct)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source product");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('CSV')")
                    .HasComment("The name of the source system which record come from");

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the transfer point");
            });

            modelBuilder.Entity<OriginType>(entity =>
            {
                entity.ToTable("OriginType", "Admin");

                entity.HasComment("This table holds the data for OriginType. This is a master table and contains seeded data.");

                entity.HasIndex(e => e.Name)
                    .HasName("UC_OriginType")
                    .IsUnique();

                entity.Property(e => e.OriginTypeId).HasComment("The identifier for the scenario type ");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The spanish name of the type (like Origen, Destino)");
            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.ToTable("Owner", "Offchain");

                entity.HasComment("This table holds the data for the Owner with its associated movement, inventory and blockchain status.");

                entity.Property(e => e.Id).HasComment("The identifier of the record");

                entity.Property(e => e.BlockchainInventoryProductTransactionId).HasComment("The identifier of the blockchain inventory product transaction");

                entity.Property(e => e.BlockchainMovementTransactionId).HasComment("The identifier of the blockchain movement transaction ");

                entity.Property(e => e.BlockchainStatus).HasComment("The flag indicating if present in blockchain register");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.InventoryProductId).HasComment("The identifier of the inventory product");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MovementTransactionId).HasComment("The identifier of the movement transaction");

                entity.Property(e => e.OwnerId)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The identifier of the owner");

                entity.Property(e => e.OwnershipValue)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the ownership in the unit");

                entity.Property(e => e.OwnershipValueUnit)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name or identifier of the unit (category element of unit category, like Bbl)");

                entity.HasOne(d => d.InventoryProduct)
                    .WithMany(p => p.Owner)
                    .HasForeignKey(d => d.InventoryProductId)
                    .HasConstraintName("FK_Owner_InventoryProduct");

                entity.HasOne(d => d.MovementTransaction)
                    .WithMany(p => p.Owner)
                    .HasForeignKey(d => d.MovementTransactionId)
                    .HasConstraintName("FK_Owner_Movement");
            });

            modelBuilder.Entity<Ownership>(entity =>
            {
                entity.ToTable("Ownership", "Offchain");

                entity.HasComment("This table holds the data for the Ownership with owner, movement, inventory, blockchain information and volume, percentage.");

                entity.Property(e => e.OwnershipId).HasComment("The identifier of the ownershipid");

                entity.Property(e => e.AppliedRule)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The value of the rule applied (comes from algorithm analytics api)");

                entity.Property(e => e.BlockNumber)
                    .HasMaxLength(255)
                    .HasComment("The block number");

                entity.Property(e => e.BlockchainInventoryProductTransactionId).HasComment("The identifier of the blockchain inventory product transaction");

                entity.Property(e => e.BlockchainMovementTransactionId).HasComment("The identifier of the blockchain movement transaction ");

                entity.Property(e => e.BlockchainOwnershipId).HasComment("The identifier of the blockchain ownership");

                entity.Property(e => e.BlockchainStatus).HasComment("The flag indicating if present in blockchain register");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.ExecutionDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is executed");

                entity.Property(e => e.InventoryProductId).HasComment("The identifier of the inventory product");

                entity.Property(e => e.IsDeleted).HasComment("The flag indicating if the record is delete or not, 1 means delete");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MessageTypeId).HasComment("The identifier of the message type (like Movement, Inventory, Event, etc.)");

                entity.Property(e => e.MovementTransactionId).HasComment("The identifier of the movement transaction");

                entity.Property(e => e.OwnerId).HasComment("The identifier of the owner (category element of owner category, like Ecopetrol)");

                entity.Property(e => e.OwnershipPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the ownership");

                entity.Property(e => e.OwnershipVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the ownership");

                entity.Property(e => e.PreviousBlockchainOwnershipId).HasComment("The identifier of the previous blockchain ownership");

                entity.Property(e => e.RetryCount).HasComment("The number of retries");

                entity.Property(e => e.RuleVersion)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The version of the rule (comes from algorithm analytics api)");

                entity.Property(e => e.TicketId).HasComment("The identifier of the ticket");

                entity.Property(e => e.TransactionHash)
                    .HasMaxLength(255)
                    .HasComment("The transaction hash");

                entity.HasOne(d => d.InventoryProduct)
                    .WithMany(p => p.Ownership)
                    .HasForeignKey(d => d.InventoryProductId)
                    .HasConstraintName("FK_Ownership_InventoryProduct");

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.Ownership)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PK_Ownership_MessageType");

                entity.HasOne(d => d.MovementTransaction)
                    .WithMany(p => p.Ownership)
                    .HasForeignKey(d => d.MovementTransactionId)
                    .HasConstraintName("FK_Ownership_Movement");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Ownership)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ownership_CategoryElement");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Ownership)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ownership_Ticket");
            });

            modelBuilder.Entity<OwnershipCalculation>(entity =>
            {
                entity.ToTable("OwnershipCalculation", "Admin");

                entity.HasComment("This table holds the data for collect and calculate FICO ownership calculation response, used in Balance operativo con propiedad por nodo page.");

                entity.Property(e => e.OwnershipCalculationId).HasComment("The identifier of the ownership calculation");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is calculated");

                entity.Property(e => e.FinalInventoryPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the final inventory");

                entity.Property(e => e.FinalInventoryVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volumen of the final inventory");

                entity.Property(e => e.IdentifiedLossesPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the identified losses");

                entity.Property(e => e.IdentifiedLossesUnbalancePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the identified losses");

                entity.Property(e => e.IdentifiedLossesUnbalanceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the identified losses");

                entity.Property(e => e.IdentifiedLossesVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the identified losses");

                entity.Property(e => e.InitialInventoryPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the initial inventory");

                entity.Property(e => e.InitialInventoryVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volumen of the initial inventory");

                entity.Property(e => e.InputPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the input");

                entity.Property(e => e.InputVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of  the volume of the input");

                entity.Property(e => e.InterfacePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the interface");

                entity.Property(e => e.InterfaceUnbalancePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the interface unbalance");

                entity.Property(e => e.InterfaceUnbalanceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the interface unbalance");

                entity.Property(e => e.InterfaceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the interface");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.OutputPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the output");

                entity.Property(e => e.OutputVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the output");

                entity.Property(e => e.OwnerId).HasComment("The identifier of the owner");

                entity.Property(e => e.OwnershipTicketId).HasComment("The identifier of the ownership ticket");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.TolerancePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the tolerance");

                entity.Property(e => e.ToleranceUnbalancePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the tolerance unbalance");

                entity.Property(e => e.ToleranceUnbalanceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the tolerance unbalance");

                entity.Property(e => e.ToleranceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the tolerance");

                entity.Property(e => e.UnbalancePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the unbalance");

                entity.Property(e => e.UnbalanceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the unbalance");

                entity.Property(e => e.UnidentifiedLossesPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the unidentified losses");

                entity.Property(e => e.UnidentifiedLossesVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the unidentified losses");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.OwnershipCalculation)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OwnershipCalculation_Node");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.OwnershipCalculation)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_OwnershipCalculation_CategoryElement");

                entity.HasOne(d => d.OwnershipTicket)
                    .WithMany(p => p.OwnershipCalculation)
                    .HasForeignKey(d => d.OwnershipTicketId)
                    .HasConstraintName("FK_OwnershipCalculation_Ticket");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OwnershipCalculation)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OwnershipCalculation_Product");
            });

            modelBuilder.Entity<OwnershipCalculationResult>(entity =>
            {
                entity.ToTable("OwnershipCalculationResult", "Admin");

                entity.Property(e => e.OwnershipCalculationResultId).HasComment("The identifier of the ownership calculation result");

                entity.Property(e => e.ControlTypeId).HasComment("The identifier of the control type");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.OwnerId).HasComment("The identifier of the owner ");

                entity.Property(e => e.OwnershipCalculationId).HasComment("The identifier of the ownership calculation");

                entity.Property(e => e.OwnershipPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the ownership");

                entity.Property(e => e.OwnershipVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the ownership");

                entity.HasOne(d => d.ControlType)
                    .WithMany(p => p.OwnershipCalculationResult)
                    .HasForeignKey(d => d.ControlTypeId)
                    .HasConstraintName("FK_OwnershipCalculationResult_Type");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.OwnershipCalculationResult)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_OwnershipCalculationResult_CategoryElement");

                entity.HasOne(d => d.OwnershipCalculation)
                    .WithMany(p => p.OwnershipCalculationResult)
                    .HasForeignKey(d => d.OwnershipCalculationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OwnershipCalculationResult_OwnershipCalculation");
            });

            modelBuilder.Entity<OwnershipNode>(entity =>
            {
                entity.ToTable("OwnershipNode", "Admin");

                entity.HasComment("This table contains current ownership status of that node.");

                entity.Property(e => e.OwnershipNodeId).HasComment("The identifier of ownership node");

                entity.Property(e => e.ApproverAlias)
                    .HasMaxLength(50)
                    .HasComment("The approver alias");

                entity.Property(e => e.Comment)
                    .HasMaxLength(200)
                    .HasComment("The comment provided by the user");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Editor)
                    .HasMaxLength(50)
                    .HasComment("The value of the editor");

                entity.Property(e => e.EditorConnectionId)
                    .HasMaxLength(50)
                    .HasComment("SignalR Hub ConnectionId (used in Balance operativo con propiedad por nodo for ConcurrentEdit)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.NodeId).HasComment("The identifier of node");

                entity.Property(e => e.OwnershipStatusId).HasComment("The identifier of the ownership status");

                entity.Property(e => e.ReasonId).HasComment("The identifier of the reason (category element of reason category)");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is registered");

                entity.Property(e => e.Status).HasComment("The status of ownership node");

                entity.Property(e => e.TicketId).HasComment("The identifier of ticket ");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.OwnershipNode)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OwnershipNode_Node");

                entity.HasOne(d => d.OwnershipStatus)
                    .WithMany(p => p.OwnershipNode)
                    .HasForeignKey(d => d.OwnershipStatusId)
                    .HasConstraintName("FK_OwnershipNode_OwnershipNodeStatusType");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.OwnershipNode)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK_OwnershipNode_CategoryElement_Reason");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.OwnershipNode)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OwnershipNode_StatusType");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.OwnershipNode)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OwnershipNode_Ticket");
            });

            modelBuilder.Entity<OwnershipNodeError>(entity =>
            {
                entity.ToTable("OwnershipNodeError", "Admin");

                entity.HasComment("This table holds the data for error related to the ownership node and maps it to the relevant inventory product or movement and error message.");

                entity.Property(e => e.OwnershipNodeErrorId).HasComment("The identifier of the ownership node error");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.ErrorMessage)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("The error message related with ownership node");

                entity.Property(e => e.ExecutionDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is executed");

                entity.Property(e => e.InventoryProductId).HasComment("The identifier of inventory product");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MovementTransactionId).HasComment("The identifier of the movement transaction");

                entity.Property(e => e.OwnershipNodeId).HasComment("The identifier of the ownership node");

                entity.HasOne(d => d.InventoryProduct)
                    .WithMany(p => p.OwnershipNodeError)
                    .HasForeignKey(d => d.InventoryProductId)
                    .HasConstraintName("FK_OwnershipNodeError_InventoryProduct");

                entity.HasOne(d => d.MovementTransaction)
                    .WithMany(p => p.OwnershipNodeError)
                    .HasForeignKey(d => d.MovementTransactionId)
                    .HasConstraintName("FK_OwnershipNodeError_Movement");

                entity.HasOne(d => d.OwnershipNode)
                    .WithMany(p => p.OwnershipNodeError)
                    .HasForeignKey(d => d.OwnershipNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OwnershipNodeError_OwnershipNode");
            });

            modelBuilder.Entity<OwnershipNodeStatusType>(entity =>
            {
                entity.ToTable("OwnershipNodeStatusType", "Admin");

                entity.HasComment("This table holds the data for OwnershipNodeStatusType. This is a master table and contains seeded data.");

                entity.HasIndex(e => e.Name)
                    .HasName("UC_OwnershipNodeStatusType")
                    .IsUnique();

                entity.Property(e => e.OwnershipNodeStatusTypeId).HasComment("The identifier for the ownership node status ");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The spanish name of the status (like Enviado, Propiedad)");
            });

            modelBuilder.Entity<OwnershipPercentageValues>(entity =>
            {
                entity.ToTable("OwnershipPercentageValues", "Analytics");

                entity.HasComment("This table holds the data for the Ownership Percentage Values. ");

                entity.Property(e => e.OwnershipPercentageValuesId).HasComment("The identifier of the ownership percentage value");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasDefaultValueSql("('Analytics')")
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.ExecutionId)
                    .HasColumnName("ExecutionID")
                    .HasComment("The identifier of the execution of pipeline");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.LoadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([admin].[udf_GetTrueDate]())")
                    .HasComment("The date of loading the record");

                entity.Property(e => e.OperationalDate)
                    .HasColumnType("date")
                    .HasComment("The operational date of the movement");

                entity.Property(e => e.OwnershipPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the ownership ");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('CSV')")
                    .HasComment("The name of the source system which record come from");

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the transfer point ");
            });

            modelBuilder.Entity<OwnershipResult>(entity =>
            {
                entity.ToTable("OwnershipResult", "Admin");

                entity.Property(e => e.OwnershipResultId).HasComment("The identifier of the ownership result");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.ExecutionDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime of the execution ");

                entity.Property(e => e.FinalInventory)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the final inventory");

                entity.Property(e => e.InitialInventory)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the initial inventory");

                entity.Property(e => e.Input)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the input");

                entity.Property(e => e.InventoryProductId).HasComment("The identifier of the inventory product");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MessageTypeId).HasComment("The identifier of the type of the message");

                entity.Property(e => e.MovementTransactionId).HasComment("The identifier of the movement transaction");

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.Output)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the output");

                entity.Property(e => e.OwnerId).HasComment("The identifier of the owner (categoryelement of owner category, like Ecopetrol)");

                entity.Property(e => e.OwnershipPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the ownership");

                entity.Property(e => e.OwnershipVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the ownership");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.HasOne(d => d.InventoryProduct)
                    .WithMany(p => p.OwnershipResult)
                    .HasForeignKey(d => d.InventoryProductId)
                    .HasConstraintName("FK_OwnershipResult_InventoryProduct");

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.OwnershipResult)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PK_OwnershipResult_MessageType");

                entity.HasOne(d => d.MovementTransaction)
                    .WithMany(p => p.OwnershipResult)
                    .HasForeignKey(d => d.MovementTransactionId)
                    .HasConstraintName("FK_OwnershipResult_Movement");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.OwnershipResult)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OwnershipResult_Node");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OwnershipResult)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OwnershipResult_Product");
            });

            modelBuilder.Entity<OwnershipRuleRefreshHistory>(entity =>
            {
                entity.ToTable("OwnershipRuleRefreshHistory", "Admin");

                entity.HasComment("This table holds the previous ownership rule refresh logs.");

                entity.Property(e => e.OwnershipRuleRefreshHistoryId).HasComment("The identifier of ownership rule refresh history");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.RequestedBy)
                    .HasMaxLength(60)
                    .HasComment("The name of the user who requested it");

                entity.Property(e => e.Status).HasComment("The flag indicating if the rule refresh was success, 1 means success");
            });

            modelBuilder.Entity<PendingTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("PendingTransaction", "Admin");

                entity.HasComment("This table holds the details for PendingTransaction.");

                entity.Property(e => e.TransactionId).HasComment("The identifier of the transaction");

                entity.Property(e => e.ActionType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The value of the action type");

                entity.Property(e => e.ActionTypeId).HasComment("The identifier of the action type");

                entity.Property(e => e.BlobName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("The name of the blob ");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNodeId)
                    .HasMaxLength(100)
                    .HasComment("The identifier of the destination node");

                entity.Property(e => e.DestinationProductId)
                    .HasMaxLength(100)
                    .HasComment("The identifier of the destination product");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasComment("The date when the pending transaction is ended");

                entity.Property(e => e.ErrorJson)
                    .IsRequired()
                    .HasComment("The error json message ");

                entity.Property(e => e.Identifier)
                    .HasMaxLength(50)
                    .HasComment("The value of the identifier");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MessageId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The identifier of the message");

                entity.Property(e => e.MessageTypeId).HasComment("The identifier of the message type");

                entity.Property(e => e.Messagetype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The value of the message type");

                entity.Property(e => e.OwnerId).HasComment("The identifier of the owner");

                entity.Property(e => e.SegmentId).HasComment("The identifier of the segment");

                entity.Property(e => e.SourceNodeId)
                    .HasMaxLength(100)
                    .HasComment("The identifier of the source node");

                entity.Property(e => e.SourceProductId)
                    .HasMaxLength(100)
                    .HasComment("The identifier of the source product");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasComment("The date when the pending transaction is started");

                entity.Property(e => e.SystemName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The name of the system");

                entity.Property(e => e.SystemTypeId).HasComment("The identifier of the system type");

                entity.Property(e => e.TicketId).HasComment("The identifier of the ticket");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasComment("The value of the type");

                entity.Property(e => e.TypeId).HasComment("The identifier of the type");

                entity.Property(e => e.Units)
                    .HasMaxLength(50)
                    .HasComment("The value of the units");

                entity.Property(e => e.Volume)
                    .HasMaxLength(50)
                    .HasComment("The value of the volume");

                entity.HasOne(d => d.ActionTypeNavigation)
                    .WithMany(p => p.PendingTransaction)
                    .HasForeignKey(d => d.ActionTypeId)
                    .HasConstraintName("FK_PendingTransaction _CategoryElement_ActionType");

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.PendingTransaction)
                    .HasForeignKey(d => d.MessageTypeId)
                    .HasConstraintName("FK_PendingTransaction_MessageType");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.PendingTransactionOwner)
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.PendingTransactionSegment)
                    .HasForeignKey(d => d.SegmentId);

                entity.HasOne(d => d.SystemType)
                    .WithMany(p => p.PendingTransaction)
                    .HasForeignKey(d => d.SystemTypeId)
                    .HasConstraintName("FK_PendingTransaction_SystemType");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.PendingTransaction)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK_PendingTransaction _CategoryElement_TicketId");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.PendingTransactionTypeNavigation)
                    .HasForeignKey(d => d.TypeId);
            });

            modelBuilder.Entity<PendingTransactionError>(entity =>
            {
                entity.HasKey(e => e.ErrorId)
                    .HasName("PK_PendingTransactionError ");

                entity.ToTable("PendingTransactionError", "Admin");

                entity.HasComment("This table holds the error details for pending transaction.");

                entity.Property(e => e.ErrorId).HasComment("The identifier of the pending transaction error ");

                entity.Property(e => e.Comment)
                    .HasMaxLength(1000)
                    .HasComment("The comment for the error provided by the user");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.ErrorMessage)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("The message of the error ");

                entity.Property(e => e.IsRetrying).HasComment("The flag indicating if the transaction is currently being retried, 1 means yes");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.RecordId)
                    .HasColumnName("RecordID")
                    .HasMaxLength(250)
                    .HasComment("The unique identifier used to map to file regisration transaction");

                entity.Property(e => e.TransactionId).HasComment("The identifier of the transaction ");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.PendingTransactionError)
                    .HasForeignKey(d => d.TransactionId);
            });

            modelBuilder.Entity<PipelineLog>(entity =>
            {
                entity.HasKey(e => e.PipelineId)
                    .HasName("PK_Analytics_PipelineId");

                entity.ToTable("PipelineLog", "Analytics");

                entity.HasComment("This table holds the data for the Pipeline Log (ADF).");

                entity.Property(e => e.PipelineId).HasComment("The identifier of the pipeline");

                entity.Property(e => e.PipelineEndTime)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the pipeline is ended");

                entity.Property(e => e.PipelineName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("The name of the pipeline");

                entity.Property(e => e.PipelineRunId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("The identifier of the execution of pipeline");

                entity.Property(e => e.PipelineStartTime)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the pipeline is started");

                entity.Property(e => e.PipelineStatusId).HasComment("The id of the status of the pipeline");

                entity.HasOne(d => d.PipelineStatus)
                    .WithMany(p => p.PipelineLog)
                    .HasForeignKey(d => d.PipelineStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Analytics_PipelineLog_AuditStatus");
            });

            modelBuilder.Entity<PipelineLog1>(entity =>
            {
                entity.HasKey(e => e.PipelineId)
                    .HasName("PK_PipelineId");

                entity.ToTable("PipelineLog", "Audit");

                entity.HasComment("This table holds the data for the Pipeline Log (ADF).");

                entity.Property(e => e.PipelineId).HasComment("The identifier of the pipeline");

                entity.Property(e => e.PipelineEndTime)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the pipeline is ended");

                entity.Property(e => e.PipelineName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("The name of the pipeline");

                entity.Property(e => e.PipelineRunId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("The identifier of the execution of pipeline");

                entity.Property(e => e.PipelineStartTime)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the pipeline is started");

                entity.Property(e => e.PipelineStatusId).HasComment("The id of the status of the pipeline");

                entity.HasOne(d => d.PipelineStatus)
                    .WithMany(p => p.PipelineLog1)
                    .HasForeignKey(d => d.PipelineStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PipelineLog_AuditStatus");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "Admin");

                entity.HasComment("This table holds the data for different Products.");

                entity.HasIndex(e => e.Name)
                    .HasName("UC_Products")
                    .IsUnique();

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if the product is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the product");
            });

            modelBuilder.Entity<QualityDetailsWithOwner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("QualityDetailsWithOwner", "Admin");

                entity.HasComment("This View is to Fetch Data [Admin].[QualityDetailsWithOwnership] For PowerBi Report From Tables (Inventory, InventoryProduct, Attribute, Unbalance, Ownership, Ticket, Product, Node, CategoryElement,Category)");

                entity.Property(e => e.AttributeDescription).HasMaxLength(150);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.BatchId).HasMaxLength(150);

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.InventoryDate).HasColumnType("date");

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MeasurmentUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ProductVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SystemName).HasMaxLength(50);

                entity.Property(e => e.TankName).HasMaxLength(20);

                entity.Property(e => e.ValueAttributeUnit)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<QualityDetailsWithoutOwner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("QualityDetailsWithoutOwner", "Admin");

                entity.HasComment("This View is to Fetch Data [Admin].[InventoryDetailsWithOutOwner] For PowerBi Report From Tables(Inventory,Unbalance, Ticket, Attribute, ,Product, Node, NodeConnectionProduct, NodeConnection, CategoryElement,Category)");

                entity.Property(e => e.AttributeDescription).HasMaxLength(150);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.BatchId).HasMaxLength(150);

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MeasurmentUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.ProdutId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SystemName).HasMaxLength(50);

                entity.Property(e => e.TankName).HasMaxLength(20);

                entity.Property(e => e.ValueAttributeUnit)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RegisterFileActionType>(entity =>
            {
                entity.HasKey(e => e.ActionTypeId);

                entity.ToTable("RegisterFileActionType", "Admin");

                entity.HasComment("This table contains file action types in Spanish (like Insertar, Eliminar, etc). This is a master table and contains seeded data.");

                entity.Property(e => e.ActionTypeId).HasComment("The identifier of the action type");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.FileActionType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The action name (Insertar, Eliminar, etc)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");
            });

            modelBuilder.Entity<ReportConfiguration>(entity =>
            {
                entity.ToTable("ReportConfiguration", "Admin");

                entity.HasComment("This table contains custom variables and its values to use in the reports.");

                entity.Property(e => e.ReportConfigurationId).HasComment("The identifier for the configuration");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.ReportConfiguartionValue).HasComment("The value of the configuration");

                entity.Property(e => e.ReportConfigurationDescription)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The description of the configuration");

                entity.Property(e => e.ReportConfigurationName)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The configuration name");

                entity.Property(e => e.ReportName)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The name of the report");
            });

            modelBuilder.Entity<ReportExecutionDate>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ReportExecutionDate", "Admin");

                entity.HasComment("This view is to get Current time in Colombian time zone");

                entity.Property(e => e.ReportExecutionDate1)
                    .HasColumnName("ReportExecutionDate")
                    .HasColumnType("datetimeoffset(3)");
            });

            modelBuilder.Entity<ReportHeaderDetails>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ReportHeaderDetails", "Admin");

                entity.HasComment("This View is to Fetch header deatils For PowerBi Report From Tables(NodeTag, Node,CategoryElement,Category)");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName).HasMaxLength(150);

                entity.Property(e => e.PcCategory).HasColumnName("PC_Category");

                entity.Property(e => e.PcElement).HasColumnName("PC_Element");

                entity.Property(e => e.PcNodeName).HasColumnName("PC_NodeName");
            });

            modelBuilder.Entity<ReportTemplateConfiguration>(entity =>
            {
                entity.HasKey(e => e.TemplateConfigurationId)
                    .HasName("PK_TemplateConfiguration");

                entity.ToTable("ReportTemplateConfiguration", "Admin");

                entity.HasComment("This table holds the data to be shown in the hidden pages of the reports (Portada and Bitcora). This is a master table and has seeded data.");

                entity.Property(e => e.TemplateConfigurationId).HasComment("The identifier for the template configuration");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("Nombre del área que genera el reporte");

                entity.Property(e => e.ChangeResponsible)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("Responsable del Cambio");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Datamart)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("The data mart name");

                entity.Property(e => e.Frequency)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("Frecuencia de Actualización:");

                entity.Property(e => e.InformationResponsible)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("Responsable por la información publicada");

                entity.Property(e => e.InformationSource)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("Fuentes Principales de Información");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.ReportIdentifier)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("The name of the PBIX file");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasComment("Fecha Actualización Informe");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("The version of the report");
            });

            modelBuilder.Entity<ReportTemplateDetails>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ReportTemplateDetails", "Admin");

                entity.HasComment("This View is to Fetch Data for the Report Template details from the ReportTemplateConfiguration table");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ChangeResponsible)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Datamart)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Frequency)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.InformationResponsible)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.InformationSource)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ReportIdentifier)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UpdateDate)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Reversal>(entity =>
            {
                entity.ToTable("Reversal", "Admin");

                entity.HasComment("This table holds the data for movement type relationships.");

                entity.HasIndex(e => new { e.SourceMovementTypeId, e.ReversedMovementTypeId })
                    .HasName("UQ_Reversal_SourceMovementTypeId_ReversalMovementTypeId")
                    .IsUnique();

                entity.Property(e => e.ReversalId).HasComment("The identifier of the relationship");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.DestinationNodeId).HasComment("The identifier of the destination node");

                entity.Property(e => e.DestinationProductId).HasComment("The identifier of the destination product");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.ReversedMovementTypeId).HasComment("The identifier of the reversed movement");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version column used for consistency")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.SourceMovementTypeId).HasComment("The identifier of the source movement");

                entity.Property(e => e.SourceNodeId).HasComment("The identifier of the source node");

                entity.Property(e => e.SourceProductId).HasComment("The identifier of the source product");

                entity.HasOne(d => d.DestinationNode)
                    .WithMany(p => p.ReversalDestinationNode)
                    .HasForeignKey(d => d.DestinationNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reversal_Node_DestinationNodeId");

                entity.HasOne(d => d.DestinationProduct)
                    .WithMany(p => p.ReversalDestinationProduct)
                    .HasForeignKey(d => d.DestinationProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reversal_Product_DestinationProductId");

                entity.HasOne(d => d.SourceNode)
                    .WithMany(p => p.ReversalSourceNode)
                    .HasForeignKey(d => d.SourceNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reversal_Node_SourceNodeId");

                entity.HasOne(d => d.SourceProduct)
                    .WithMany(p => p.ReversalSourceProduct)
                    .HasForeignKey(d => d.SourceProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reversal_Product_SourceProductId");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "Admin");

                entity.HasComment("This table holds the data for different Roles in the system like ADMINISTRADOR,APROBADOR. This is a master table and has seeded data.");

                entity.Property(e => e.RoleId).HasComment("The identifier of the role");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The name of the role (Administrador, Consulta, etc)");
            });

            modelBuilder.Entity<Rules>(entity =>
            {
                entity.HasKey(e => e.RuleId);

                entity.ToTable("Rules", "Admin");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Rule)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Scenario>(entity =>
            {
                entity.ToTable("Scenario", "Admin");

                entity.HasComment("This table holds the data for different scenarios like balanceIntegration,reports (main menu). This is a master table and has seeded data.");

                entity.Property(e => e.ScenarioId).HasComment("The identifier of the scenario (main menu in nav bar)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of the scenario (balanceTransportes, etc.)");

                entity.Property(e => e.Sequence).HasComment("The number signifying sequence in which scenarios (main menu) must be ordered in navbar");
            });

            modelBuilder.Entity<ScenarioType>(entity =>
            {
                entity.ToTable("ScenarioType", "Admin");

                entity.HasComment("This table holds the data for OriginType. This is a master table and contains seeded data.");

                entity.HasIndex(e => e.Name)
                    .HasName("UC_ScenarioType")
                    .IsUnique();

                entity.Property(e => e.ScenarioTypeId).HasComment("The identifier for the scenario type ");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The spanish name of the type (like Operativo, Oficial)");
            });

            modelBuilder.Entity<SegmentOwnershipCalculation>(entity =>
            {
                entity.ToTable("SegmentOwnershipCalculation", "Admin");

                entity.HasComment("This table holds the data for segment ownership calculation.");

                entity.Property(e => e.SegmentOwnershipCalculationId).HasComment("The identifier of the segment ownership calculation");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the segment ownership calculation was done");

                entity.Property(e => e.FinalInventoryPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the final inventory");

                entity.Property(e => e.FinalInventoryVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the final inventory");

                entity.Property(e => e.IdentifiedLossesPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the identified losses");

                entity.Property(e => e.IdentifiedLossesVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the identified losses");

                entity.Property(e => e.InitialInventoryPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the initial inventory");

                entity.Property(e => e.InitialInventoryVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the initial inventory");

                entity.Property(e => e.InputPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the input");

                entity.Property(e => e.InputVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the input");

                entity.Property(e => e.InterfacePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the interface");

                entity.Property(e => e.InterfaceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the interface");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.OutputPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the output");

                entity.Property(e => e.OutputVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the output");

                entity.Property(e => e.OwnerId).HasComment("The identifier of the owner");

                entity.Property(e => e.OwnershipTicketId).HasComment("The identifier of the ownership ticket");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.SegmentId).HasComment("The identifier of the segment");

                entity.Property(e => e.TolerancePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the tolerance");

                entity.Property(e => e.ToleranceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of  the tolerance");

                entity.Property(e => e.UnbalancePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the unbalance");

                entity.Property(e => e.UnbalanceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the unbalance");

                entity.Property(e => e.UnidentifiedLossesPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the unidentified losses ");

                entity.Property(e => e.UnidentifiedLossesVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the unidentified losses");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.SegmentOwnershipCalculationOwner)
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.OwnershipTicket)
                    .WithMany(p => p.SegmentOwnershipCalculation)
                    .HasForeignKey(d => d.OwnershipTicketId)
                    .HasConstraintName("FK_SegmentOwnershipCalculation_Ticket");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SegmentOwnershipCalculation)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentOwnershipCalculation_Product");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SegmentOwnershipCalculationSegment)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SegmentUnbalance>(entity =>
            {
                entity.ToTable("SegmentUnbalance", "Admin");

                entity.HasComment("This table holds the data for segment unbalance.");

                entity.Property(e => e.SegmentUnbalanceId).HasComment("The identifier of the segment unbalance");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasComment("The date when the segment unbalance was generated");

                entity.Property(e => e.FinalInventoryVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the final inventory");

                entity.Property(e => e.IdentifiedLossesVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the identified losses volume");

                entity.Property(e => e.InitialInventoryVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the initial inventory");

                entity.Property(e => e.InputVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the input volume");

                entity.Property(e => e.InterfaceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the interface volume");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.OutputVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the output volume");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.SegmentId).HasComment("The identifier of the segment");

                entity.Property(e => e.TicketId).HasComment("The identifier of the ticket");

                entity.Property(e => e.ToleranceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the tolerance volume");

                entity.Property(e => e.UnbalanceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the unbalance volume");

                entity.Property(e => e.UnidentifiedLossesVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the unidentified losses volume");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SegmentUnbalance)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentUnbalance_Product");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SegmentUnbalance)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.SegmentUnbalance)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK_SegmentUnbalance_Ticket");
            });

            modelBuilder.Entity<StageOperativeMovements>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Stage_OperativeMovements", "Analytics");

                entity.HasComment("This table holds the data for the Operative Movements from CSV.");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasDefaultValueSql("('ADF')")
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationNode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the destination node");

                entity.Property(e => e.DestinationNodeType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the destination node");

                entity.Property(e => e.ExecutionId)
                    .HasColumnName("ExecutionID")
                    .HasComment("The identifier of the execution of pipeline");

                entity.Property(e => e.FieldWaterProduction)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the field of water of production ");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.LoadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([admin].[udf_GetTrueDate]())")
                    .HasComment("The date of loading the movement");

                entity.Property(e => e.MovementType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the movement");

                entity.Property(e => e.NetStandardVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The net volumen of the movement ");

                entity.Property(e => e.OperationalDate)
                    .HasColumnType("date")
                    .HasComment("The operational date of the movement");

                entity.Property(e => e.RelatedSourceField)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source field related");

                entity.Property(e => e.SourceField)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source field");

                entity.Property(e => e.SourceNode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source node");

                entity.Property(e => e.SourceNodeType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the source node");

                entity.Property(e => e.SourceProduct)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source product");

                entity.Property(e => e.SourceProductType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the source product");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('CSV')")
                    .HasComment("The name of the source system which movement come from");

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the transfer point");
            });

            modelBuilder.Entity<StageOperativeMovementsWithOwnership>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Stage_OperativeMovementsWithOwnership", "Analytics");

                entity.HasComment("This table holds the data for the Operative Movements With Ownership from CSV.");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasDefaultValueSql("('ADF')")
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DayOfMonth).HasComment("The day of the month of the operational date");

                entity.Property(e => e.DestinationProduct)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the destination product");

                entity.Property(e => e.DestinationStorageLocation)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the location of the destination storage");

                entity.Property(e => e.ExecutionId)
                    .HasColumnName("ExecutionID")
                    .HasComment("The identifier of the execution of pipeline");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.LoadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The date of loading the movement");

                entity.Property(e => e.Month).HasComment("The day of the month of the operational date");

                entity.Property(e => e.MovementType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the type of the movement");

                entity.Property(e => e.OperationalDate)
                    .HasColumnType("date")
                    .HasComment("The operational date of the movement");

                entity.Property(e => e.OwnershipVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The volume of the movement");

                entity.Property(e => e.SourceProduct)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the source product");

                entity.Property(e => e.SourceStorageLocation)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the location of the source storage");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('CSV')")
                    .HasComment("The name of the source system which record come from");

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the transfer point");

                entity.Property(e => e.Year).HasComment("The year of the operational date");
            });

            modelBuilder.Entity<StageOwnershipPercentageValues>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Stage_OwnershipPercentageValues", "Analytics");

                entity.HasComment("This stage table holds the CSV data for the Ownership Percentage Values. ");

                entity.Property(e => e.ExecutionId)
                    .HasColumnName("ExecutionID")
                    .HasComment("The identifier of the execution of pipeline");

                entity.Property(e => e.LoadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([admin].[udf_GetTrueDate]())")
                    .HasComment("The date of loading the record");

                entity.Property(e => e.OperationalDate)
                    .HasColumnType("date")
                    .HasComment("The operational date of the movement");

                entity.Property(e => e.OwnershipPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the ownership ");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('CSV')")
                    .HasComment("The name of the source system which record come from");

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the transfer point ");
            });

            modelBuilder.Entity<StatusType>(entity =>
            {
                entity.ToTable("StatusType", "Admin");

                entity.HasComment(" This table holds the data types of status used in ownership nodes, tickets, fileregistrations. This is a master table and has seeded data.");

                entity.HasIndex(e => e.StatusType1)
                    .HasName("UC_StatusType")
                    .IsUnique();

                entity.Property(e => e.StatusTypeId).HasComment("The identifier of the status type");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.StatusType1)
                    .IsRequired()
                    .HasColumnName("StatusType")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("The type of the status (Finalizado, Fallido, Procesando, etc.)");
            });

            modelBuilder.Entity<StorageLocation>(entity =>
            {
                entity.ToTable("StorageLocation", "Admin");

                entity.HasComment("This table holds the data for StorageLocation.");

                entity.Property(e => e.StorageLocationId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the storage location");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if the storage location is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.LogisticCenterId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the logistic center");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("The name of the storage location");

                entity.HasOne(d => d.LogisticCenter)
                    .WithMany(p => p.StorageLocation)
                    .HasForeignKey(d => d.LogisticCenterId)
                    .HasConstraintName("FK_StorageLocation_LogisticCenter");
            });

            modelBuilder.Entity<StorageLocationProduct>(entity =>
            {
                entity.ToTable("StorageLocationProduct", "Admin");

                entity.HasComment("This table holds the details for association product and node-storage location. Also, Estrategias de propiedad of storage location-product.");

                entity.Property(e => e.StorageLocationProductId).HasComment("The identifier of the association between product and node-storagelocation");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("The flag indicating if this is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.NodeProductRuleId).HasComment("The identifier of Estrategia de propiedad for association between product and node-storagelocation");

                entity.Property(e => e.NodeStorageLocationId).HasComment("The identifier of the association between storage location and node");

                entity.Property(e => e.OwnershipRuleId).HasComment("The identifier of rule (category element of rule category)");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version column used for consistency")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.UncertaintyPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The uncertaintity percentage");

                entity.HasOne(d => d.NodeProductRule)
                    .WithMany(p => p.StorageLocationProduct)
                    .HasForeignKey(d => d.NodeProductRuleId)
                    .HasConstraintName("FK_StorageLocationProduct_NodeProductRule");

                entity.HasOne(d => d.NodeStorageLocation)
                    .WithMany(p => p.StorageLocationProduct)
                    .HasForeignKey(d => d.NodeStorageLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StorageLocationProduct_NodeStorageLocation");

                entity.HasOne(d => d.OwnershipRule)
                    .WithMany(p => p.StorageLocationProduct)
                    .HasForeignKey(d => d.OwnershipRuleId)
                    .HasConstraintName("FK_StorageLocationProduct_OwnershipRuleId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.StorageLocationProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StorageLocationProduct_Product");
            });

            modelBuilder.Entity<StorageLocationProductMapping>(entity =>
            {
                entity.ToTable("StorageLocationProductMapping", "Admin");

                entity.HasComment("This table is to store product and storage location mapping.");

                entity.Property(e => e.StorageLocationProductMappingId).HasComment("The identifier of the relationship between a product and a storage location");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.StorageLocationId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the storage location");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.StorageLocationProductMapping)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StorageLocationProductMapping_Product");

                entity.HasOne(d => d.StorageLocation)
                    .WithMany(p => p.StorageLocationProductMapping)
                    .HasForeignKey(d => d.StorageLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StorageLocationProductMapping_StorageLocation");
            });

            modelBuilder.Entity<StorageLocationProductOwner>(entity =>
            {
                entity.ToTable("StorageLocationProductOwner", "Admin");

                entity.HasComment("This table is to store storage location product and owner mapping.");

                entity.Property(e => e.StorageLocationProductOwnerId).HasComment("The identifier of the relationship between a storage location product and a owner");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.IsDeleted).HasComment("The flag indicating if the storagelocationproduct-owner mapping is active or not, 1 means active");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.OwnerId).HasComment("The identifier of a owner (category element of owner category, like Ecopetrol)");

                entity.Property(e => e.OwnershipPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The ownership percentage");

                entity.Property(e => e.StorageLocationProductId).HasComment("The identifier of a storage location product");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.StorageLocationProductOwner)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StorageLocationProductOwner_CategoryElement");

                entity.HasOne(d => d.StorageLocationProduct)
                    .WithMany(p => p.StorageLocationProductOwner)
                    .HasForeignKey(d => d.StorageLocationProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StorageLocationProductOwner_StorageLocationProduct");
            });

            modelBuilder.Entity<StorageLocationProductVariable>(entity =>
            {
                entity.ToTable("StorageLocationProductVariable", "Admin");

                entity.HasComment("This Table is to store StorageLocationProduct and Variable mapping.");

                entity.HasIndex(e => new { e.StorageLocationProductId, e.VariableTypeId })
                    .HasName("UQ_StorageLocationProductVariable_StorageLocationProduct")
                    .IsUnique();

                entity.Property(e => e.StorageLocationProductVariableId).HasComment("The identifier of the storage location product variable");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version column used for consistency")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.StorageLocationProductId).HasComment("The identifier of the storage location product");

                entity.Property(e => e.VariableTypeId).HasComment("The identifier of the variable type (like for Interfase, Tolerancia, Entrada, etc.)");

                entity.HasOne(d => d.StorageLocationProduct)
                    .WithMany(p => p.StorageLocationProductVariable)
                    .HasForeignKey(d => d.StorageLocationProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StorageLocationProductVariable_StorageLocationProduct");

                entity.HasOne(d => d.VariableType)
                    .WithMany(p => p.StorageLocationProductVariable)
                    .HasForeignKey(d => d.VariableTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StorageLocationProductVariable_VariableType");
            });

            modelBuilder.Entity<SystemOwnershipCalculation>(entity =>
            {
                entity.ToTable("SystemOwnershipCalculation", "Admin");

                entity.HasComment("This table holds the data for ownership calculation of a system.");

                entity.Property(e => e.SystemOwnershipCalculationId).HasComment("The identifier of the system ownership calculation");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasComment("The datetime of the system ownership calculation");

                entity.Property(e => e.FinalInventoryPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the final inventory");

                entity.Property(e => e.FinalInventoryVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the final inventory");

                entity.Property(e => e.IdentifiedLossesPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the identified losses");

                entity.Property(e => e.IdentifiedLossesVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the identified losses");

                entity.Property(e => e.InitialInventoryPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the initial inventory");

                entity.Property(e => e.InitialInventoryVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the initial inventory ");

                entity.Property(e => e.InputPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the input");

                entity.Property(e => e.InputVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the input");

                entity.Property(e => e.InterfacePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the interface");

                entity.Property(e => e.InterfaceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volumen of the interface");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.OutputPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the output");

                entity.Property(e => e.OutputVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the output");

                entity.Property(e => e.OwnerId).HasComment("The identifier of the owner (category element for owner category, like Ecopetrol)");

                entity.Property(e => e.OwnershipTicketId).HasComment("The identifier of the ownership ticket");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product id ");

                entity.Property(e => e.SegmentId).HasComment("The identifier of the segment (category element for segment category)");

                entity.Property(e => e.SystemId).HasComment("The identifier of the system (category element for system category)");

                entity.Property(e => e.TolerancePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the tolerance");

                entity.Property(e => e.ToleranceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volumen of the tolerance");

                entity.Property(e => e.UnbalancePercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the unbalance");

                entity.Property(e => e.UnbalanceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volume of the unbalance");

                entity.Property(e => e.UnidentifiedLossesPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasComment("The value of the percentage of the unidentified losses");

                entity.Property(e => e.UnidentifiedLossesVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the volumen of the unidentified losses");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.SystemOwnershipCalculationOwner)
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.OwnershipTicket)
                    .WithMany(p => p.SystemOwnershipCalculation)
                    .HasForeignKey(d => d.OwnershipTicketId)
                    .HasConstraintName("FK_SystemOwnershipCalculation_Ticket");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SystemOwnershipCalculation)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemOwnershipCalculation_Product");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SystemOwnershipCalculationSegment)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.System)
                    .WithMany(p => p.SystemOwnershipCalculationSystem)
                    .HasForeignKey(d => d.SystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SystemType>(entity =>
            {
                entity.ToTable("SystemType", "Admin");

                entity.HasComment("This table holds the data for SystemType. This is a master table and has seeded data.");

                entity.Property(e => e.SystemTypeId).HasComment("The identifier of the system type");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system  (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated  (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of the type (like Sinoper, Excel, etc)");
            });

            modelBuilder.Entity<SystemUnbalance>(entity =>
            {
                entity.ToTable("SystemUnbalance", "Admin");

                entity.HasComment("This table holds the unbalance details for system.");

                entity.Property(e => e.SystemUnbalanceId).HasComment("The identifier of the system unbalance");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasComment("The date when the system unbalance was generated");

                entity.Property(e => e.FinalInventoryVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The volume of the final inventory");

                entity.Property(e => e.IdentifiedLossesVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The identified losses volume");

                entity.Property(e => e.InitialInventoryVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The volume of the initial inventory ");

                entity.Property(e => e.InputVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The input volume");

                entity.Property(e => e.InterfaceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The interface volume");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.OutputVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The output volume");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.SegmentId).HasComment("The identifier of the segment (category element for segment category)");

                entity.Property(e => e.SystemId).HasComment("The identifier of the system (category element for system category)");

                entity.Property(e => e.TicketId).HasComment("The identifier of the ticket");

                entity.Property(e => e.ToleranceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The tolerance volume");

                entity.Property(e => e.UnbalanceVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The unbalance volume");

                entity.Property(e => e.UnidentifiedLossesVolume)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The unidentified losses volume");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SystemUnbalance)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemUnbalance_Product");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SystemUnbalanceSegment)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.System)
                    .WithMany(p => p.SystemUnbalanceSystem)
                    .HasForeignKey(d => d.SystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.SystemUnbalance)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK_SystemUnbalance_Ticket");
            });

            modelBuilder.Entity<TempCategoryElementMapping>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tempCategoryElementMapping", "Admin");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket", "Admin");

                entity.HasComment("This table holds the data for Tickets generated in the system.");

                entity.Property(e => e.TicketId).HasComment("The identifier of the ticket");

                entity.Property(e => e.AnalyticsErrorMessage).HasComment("The error message when the ticket is processed and failed");

                entity.Property(e => e.AnalyticsStatus).HasComment("The value of the status of the ticket , 0 means success, 1 means failed");

                entity.Property(e => e.BlobPath).HasComment("The blobpath");

                entity.Property(e => e.CategoryElementId).HasComment("The identifier of the category element");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the ticket is ended");

                entity.Property(e => e.ErrorMessage).HasComment("The error message when the ticket is processed and failed");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.OwnerId).HasComment("The identifier of the owner (category element of owner category)");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the ticket is started");

                entity.Property(e => e.Status).HasComment("The value of the status of the ticket , 1 means Ok, 2 means failed");

                entity.Property(e => e.TicketGroupId)
                    .HasMaxLength(255)
                    .HasComment("The identifier of ticket group");

                entity.Property(e => e.TicketTypeId)
                    .HasDefaultValueSql("((1))")
                    .HasComment("The identifier of ticket type (Logistics, Ownership, cutoff)");

                entity.HasOne(d => d.CategoryElement)
                    .WithMany(p => p.TicketCategoryElement)
                    .HasForeignKey(d => d.CategoryElementId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.NodeId);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.TicketOwner)
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_StatusType");

                entity.HasOne(d => d.TicketType)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.TicketTypeId)
                    .HasConstraintName("FK_Ticket_TicketType");
            });

            modelBuilder.Entity<TicketNodeStatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TicketNodeStatus", "Admin");

                entity.HasComment("This table is created to store Node statuses for a ticket and is used to create Power Bi report.");

                entity.Property(e => e.Approver)
                    .HasMaxLength(50)
                    .HasComment("The name of the approver");

                entity.Property(e => e.CalculatedDays)
                    .HasMaxLength(250)
                    .HasComment("The number of the calculate days");

                entity.Property(e => e.Comment)
                    .HasMaxLength(200)
                    .HasComment("The comment provided by the user");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Enddate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the ticket is ended");

                entity.Property(e => e.ExecutionId)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("The execution id unique to session received from UI");

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.NodeName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("The name of the node");

                entity.Property(e => e.NotInApprovedState).HasComment("The flag indicating if the ticket is in approved state or not, 1 means approved state");

                entity.Property(e => e.OwnershipNodeId).HasComment("The identifier of ownership node");

                entity.Property(e => e.OwnershipNodeStatusId).HasComment("The identifier of the ownership node status");

                entity.Property(e => e.ReportConfiguartionValue).HasComment("The number of days used for CASOS SIN GESTIÓN OPORTUNA (ANS) chart");

                entity.Property(e => e.SegmentId).HasComment("the identifier of the segment (category element of segment category)");

                entity.Property(e => e.SegmentName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("The name of the segment");

                entity.Property(e => e.Startdate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the ticket is started");

                entity.Property(e => e.StatusDateChange)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the status changed");

                entity.Property(e => e.StatusNode)
                    .IsRequired()
                    .HasColumnName("statusNode")
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("The value of the status node");

                entity.Property(e => e.SystemId).HasComment("The identifier of the system (category element of system category)");

                entity.Property(e => e.SystemName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("The name of the system");

                entity.Property(e => e.TicketId).HasComment("The identifier of ticket");
            });

            modelBuilder.Entity<TicketType>(entity =>
            {
                entity.ToTable("TicketType", "Admin");

                entity.HasComment("This table holds the data for TicketType values like Cutoff,Ownership,Logistics. This is a master table and has seeded data.");

                entity.Property(e => e.TicketTypeId).HasComment("The identifier for the ticket type");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of the ticket type (Logistics, Cutoff, etc)");
            });

            modelBuilder.Entity<Transformation>(entity =>
            {
                entity.ToTable("Transformation", "Admin");

                entity.HasComment("This table holds the details for Transformation from source to destination over a message type.");

                entity.Property(e => e.TransformationId).HasComment("The identifier of the transformation");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.DestinationDestinationNodeId).HasComment("The identifier of the destination destination node");

                entity.Property(e => e.DestinationDestinationProductId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the destination destination product");

                entity.Property(e => e.DestinationMeasurementId).HasComment("The identifier of the destination measurement");

                entity.Property(e => e.DestinationSourceNodeId).HasComment("The identifier of the destination source node");

                entity.Property(e => e.DestinationSourceProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the destination source product");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValueSql("((0))")
                    .HasComment("The flag indicating if the record is delete or not, 1 means delete");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(260)
                    .HasComment("The modifier of the record, normally system (common column)");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The datetime when the record is updated (common column)");

                entity.Property(e => e.MessageTypeId).HasComment("The identifier of the message type (like Movement, Inventory, Event, etc.)");

                entity.Property(e => e.OriginDestinationNodeId).HasComment("The identifier of the original destination node");

                entity.Property(e => e.OriginDestinationProductId)
                    .HasMaxLength(20)
                    .HasComment("The identifier of the original destination product");

                entity.Property(e => e.OriginMeasurementId).HasComment("The identifier of the original measurement");

                entity.Property(e => e.OriginSourceNodeId).HasComment("The identifier of the original source node ");

                entity.Property(e => e.OriginSourceProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the original source product");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasComment("The version of the record")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.DestinationDestinationNode)
                    .WithMany(p => p.TransformationDestinationDestinationNode)
                    .HasForeignKey(d => d.DestinationDestinationNodeId)
                    .HasConstraintName("FK_Transformation_Node4");

                entity.HasOne(d => d.DestinationDestinationProduct)
                    .WithMany(p => p.TransformationDestinationDestinationProduct)
                    .HasForeignKey(d => d.DestinationDestinationProductId)
                    .HasConstraintName("FK_Transformation_StorageLocationProduct4");

                entity.HasOne(d => d.DestinationMeasurement)
                    .WithMany(p => p.TransformationDestinationMeasurement)
                    .HasForeignKey(d => d.DestinationMeasurementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transformation_CategoryElement2");

                entity.HasOne(d => d.DestinationSourceNode)
                    .WithMany(p => p.TransformationDestinationSourceNode)
                    .HasForeignKey(d => d.DestinationSourceNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transformation_Node3");

                entity.HasOne(d => d.DestinationSourceProduct)
                    .WithMany(p => p.TransformationDestinationSourceProduct)
                    .HasForeignKey(d => d.DestinationSourceProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transformation_StorageLocationProduct3");

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.Transformation)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PK_Transformation_MessageType");

                entity.HasOne(d => d.OriginDestinationNode)
                    .WithMany(p => p.TransformationOriginDestinationNode)
                    .HasForeignKey(d => d.OriginDestinationNodeId)
                    .HasConstraintName("FK_Transformation_Node2");

                entity.HasOne(d => d.OriginDestinationProduct)
                    .WithMany(p => p.TransformationOriginDestinationProduct)
                    .HasForeignKey(d => d.OriginDestinationProductId)
                    .HasConstraintName("FK_Transformation_StorageLocationProduct2");

                entity.HasOne(d => d.OriginMeasurement)
                    .WithMany(p => p.TransformationOriginMeasurement)
                    .HasForeignKey(d => d.OriginMeasurementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transformation_CategoryElement1");

                entity.HasOne(d => d.OriginSourceNode)
                    .WithMany(p => p.TransformationOriginSourceNode)
                    .HasForeignKey(d => d.OriginSourceNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transformation_Node1");

                entity.HasOne(d => d.OriginSourceProduct)
                    .WithMany(p => p.TransformationOriginSourceProduct)
                    .HasForeignKey(d => d.OriginSourceProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transformation_StorageLocationProduct1");
            });

            modelBuilder.Entity<Unbalance>(entity =>
            {
                entity.ToTable("Unbalance", "Admin");

                entity.HasComment("This table holds the details for Unbalance.");

                entity.Property(e => e.UnbalanceId).HasComment("The identifier of the unbalance");

                entity.Property(e => e.Action)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the action");

                entity.Property(e => e.AverageUncertainty)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the average of the uncertainty ");

                entity.Property(e => e.AverageUncertaintyUnbalancePercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the average of uncertainty unbalance");

                entity.Property(e => e.BlockchainStatus).HasComment("The flag indicating if present in blockchain register");

                entity.Property(e => e.CalculationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is calculated");

                entity.Property(e => e.ControlTolerance)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the control tolerance");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.FinalInvnetory)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the final inventory");

                entity.Property(e => e.IdentifiedLosses)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the identified losses");

                entity.Property(e => e.InitialInventory)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the initial inventory");

                entity.Property(e => e.Inputs)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the inputs");

                entity.Property(e => e.Interface)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the interface");

                entity.Property(e => e.InterfaceUnbalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the interface unbalance");

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.Outputs)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the outputs");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.StandardUncertainty)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the standard uncertainty");

                entity.Property(e => e.TicketId).HasComment("The identifier of the ticket");

                entity.Property(e => e.Tolerance)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the tolerance");

                entity.Property(e => e.ToleranceFinalInventory)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the tolerance of final inventory");

                entity.Property(e => e.ToleranceIdentifiedLosses)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the tolerance of identified losses");

                entity.Property(e => e.ToleranceInitialInventory)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the tolerance of initial inventory");

                entity.Property(e => e.ToleranceInputs)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the tolerance of inputs");

                entity.Property(e => e.ToleranceOutputs)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the tolerance of outputs");

                entity.Property(e => e.ToleranceUnbalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the tolerance unbalance");

                entity.Property(e => e.Unbalance1)
                    .HasColumnName("Unbalance")
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the unbalance");

                entity.Property(e => e.UnidentifiedLosses)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the unidentified losses");

                entity.Property(e => e.UnidentifiedLossesUnbalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the unidentified losses unbalance");

                entity.Property(e => e.Warning)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the warning");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.Unbalance)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Unbalance)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Unbalance)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<UnbalanceComment>(entity =>
            {
                entity.HasKey(e => e.UnbalanceId);

                entity.ToTable("UnbalanceComment", "Admin");

                entity.HasComment("This table holds the details for unbalance verification before generating cutoff ticket or before starting cutoff. ");

                entity.Property(e => e.UnbalanceId).HasComment("The identifier of the unbalance comment");

                entity.Property(e => e.CalculationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The date when the unbalance was calculated");

                entity.Property(e => e.Comment)
                    .HasMaxLength(1000)
                    .HasComment("The comment of the result of the record");

                entity.Property(e => e.ControlLimit)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the control limit ");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.NodeId).HasComment("The identifier of the node");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The identifier of the product");

                entity.Property(e => e.Status).HasComment("The status (1 means inprogress , 0 means processed)");

                entity.Property(e => e.TicketId).HasComment("The identifier of the ticket");

                entity.Property(e => e.Unbalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the unbalance");

                entity.Property(e => e.UnbalancePercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("The value of the porcentage of the unbalance");

                entity.Property(e => e.Units)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The value of the units");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.UnbalanceComment)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.UnbalanceComment)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.UnbalanceComment)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "Admin");

                entity.HasComment("This table holds information for Users registered in the system.");

                entity.Property(e => e.UserId).HasComment("The identifier of the user");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created (common column)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(260)
                    .IsUnicode(false)
                    .HasComment("The email of the user ");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(260)
                    .IsUnicode(false)
                    .HasComment("The name of the user");
            });

            modelBuilder.Entity<VariableType>(entity =>
            {
                entity.ToTable("VariableType", "Admin");

                entity.HasComment("This table holds the data for types of variables (Interfase, Tolerancia, etc). This is a master table and has seeded data. ");

                entity.Property(e => e.VariableTypeId).HasComment("The identifier of the variable type");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasComment("The creator of the record, normally system (common column)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("([Admin].[udf_GetTrueDate]())")
                    .HasComment("The datetime when the record is created  (common column)");

                entity.Property(e => e.FicoName)
                    .HasMaxLength(50)
                    .HasComment("The FICO alternative name for the variable type");

                entity.Property(e => e.IsConfigurable).HasComment("The flag indicating if the variable type is configurable or not, 1 means yes");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The spanish name of the variable type (like Tolerancia, Entrada, Salida, etc.)");

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("The short name of the variable type");
            });

            modelBuilder.Entity<ViewAttributeDetails>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_AttributeDetails", "Admin");

                entity.HasComment("This View is to Fetch  View_AttributeDetails  Data For PowerBi Report From Tables(Movement, Product, Ticket, Node, NodeTag, CategoryElement, Category, Attribute)");

                entity.Property(e => e.AttributeDescription).HasMaxLength(150);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DestinationNodeName).HasMaxLength(150);

                entity.Property(e => e.DestinationProductName).HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.GrossStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(309);

                entity.Property(e => e.Operacion)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OperationalDate).HasColumnType("date");

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SourceNodeName).HasMaxLength(150);

                entity.Property(e => e.SourceProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SourceProductName).HasMaxLength(150);

                entity.Property(e => e.SystemName).HasMaxLength(50);

                entity.Property(e => e.ValueAttributeUnit)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ViewCalculationErrors>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_CalculationErrors", "Admin");

                entity.HasComment("This View is to Fetch view_CalculationErrors Data");

                entity.Property(e => e.ErrorMessage)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ExecutionDate).HasColumnType("datetime");

                entity.Property(e => e.NetVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Operation).HasMaxLength(25);

                entity.Property(e => e.OperationDate).HasColumnType("datetime");

                entity.Property(e => e.ProductDestination).HasMaxLength(150);

                entity.Property(e => e.ProductOrigin).HasMaxLength(150);

                entity.Property(e => e.Segment)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ViewFileRegistrationStatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_FileRegistrationStatus", "Admin");

                entity.HasComment("This View is to Fetch Data view_FileRegistrationStatus to primarily display the calculated Status,RecordsProcessed and ErrorCount of Fileregistrations with other default columns.");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileActionType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.SegmentName).HasMaxLength(150);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UploadId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ViewFinalInventory>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_FinalInventory", "Admin");

                entity.HasComment("This View is to Fetch view_ForFinalInventory Data For PowerBi Report From Tables(Unbalance, Product, Node, NodeTag, CategoryElement, Category)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ViewGetParsingErrors>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_GetParsingErrors", "Admin");

                entity.HasComment("This View is to Fetch all the records where parsing was successfull(FileRegTran) and un parsed(PTE with FRTID = null)");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(500);

                entity.Property(e => e.MessageId).HasMaxLength(250);

                entity.Property(e => e.Process).HasMaxLength(25);

                entity.Property(e => e.SystemName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UploadId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ViewInventoryInformation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_InventoryInformation", "Admin");

                entity.HasComment("This View is to Fetch Data [Admin].[view_InventoryInformation] For PowerBi Report From Tables(Inventory, InventoryProduct, Attribute, Unbalance, Ownership, Ticket, Product, Node, CategoryElement,Category)");

                entity.Property(e => e.BatchId).HasMaxLength(150);

                entity.Property(e => e.Comment).HasMaxLength(200);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DestinationSystem)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.InventoryDate).HasColumnType("date");

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedBy).HasMaxLength(260);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MeasurementUnit).HasMaxLength(50);

                entity.Property(e => e.MeasurmentUnit).HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeSendToSap).HasColumnName("NodeSendToSAP");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductType).HasMaxLength(150);

                entity.Property(e => e.ProductVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SegmentName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.SystemName).HasMaxLength(50);

                entity.Property(e => e.TankName).HasMaxLength(20);

                entity.Property(e => e.UncertaintyPercentage).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<ViewInventoryProductWithProductName>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_InventoryProductWithProductName", "Admin");

                entity.HasComment("This View is to Fetch Data [Admin].[[view_InventoryProductWithProductName]] For PowerBi Report From Tables(InventoryProduct,[view_StorageLocationProductWithProductName])");

                entity.Property(e => e.BatchId).HasMaxLength(150);

                entity.Property(e => e.BlockNumber).HasMaxLength(255);

                entity.Property(e => e.Comment).HasMaxLength(200);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DestinationSystem)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.InventoryDate).HasColumnType("datetime");

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InventoryProductUniqueId).HasMaxLength(150);

                entity.Property(e => e.LastModifiedBy).HasMaxLength(260);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MeasurementUnit).HasMaxLength(50);

                entity.Property(e => e.Observations).HasMaxLength(150);

                entity.Property(e => e.Operator).HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ProductName).HasMaxLength(150);

                entity.Property(e => e.ProductType).HasMaxLength(150);

                entity.Property(e => e.ProductVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.SystemName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TankName).HasMaxLength(20);

                entity.Property(e => e.TransactionHash).HasMaxLength(255);

                entity.Property(e => e.UncertaintyPercentage).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<ViewKpidataByCategoryElementNode>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_KPIDataByCategoryElementNode", "Admin");

                entity.HasComment("This View is to Fetch KPIDataByCategoryElementNode Data For PowerBi Report From Tables(Unbalance, Product, Ticket, Node, NodeTag, CategoryElement, Category)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CurrentValue).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.FilterType)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Indicator)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.NodeName).HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ViewKpipreviousDateDataByCategoryElementNode>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_KPIPreviousDateDataByCategoryElementNode", "Admin");

                entity.HasComment("This View is to Fetch view_KPIPreviousDateDataByCategoryElementNode Data For PowerBi Report From Tables(Unbalance, Product, Ticket, Node, NodeTag, CategoryElement, Category)");

                entity.Property(e => e.CalculationDatePrev).HasColumnType("date");

                entity.Property(e => e.CategoryPrev)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CurrentValuePrev).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.ElementPrev)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.FilterType)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Indicator)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.NodeNamePrev).HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ViewMovementDestinationWithNodeAndProductName>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_MovementDestinationWithNodeAndProductName", "Admin");

                entity.HasComment("This View is to Fetch Data related to movementdestination along with node and product information From Tables([MovementDestination],Node,view_StorageLocationProductWithProductName)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DestinationNode).HasMaxLength(150);

                entity.Property(e => e.DestinationProduct).HasMaxLength(150);

                entity.Property(e => e.DestinationProductId).HasMaxLength(20);

                entity.Property(e => e.DestinationProductTypeId).HasMaxLength(150);

                entity.Property(e => e.LastModifiedBy).HasMaxLength(260);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ViewMovementDetails>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_MovementDetails", "Admin");

                entity.HasComment("This View is to Fetch view_MovementDetails Data For PowerBi Report From Tables(Movement, Product, Ticket, Node, NodeTag, CategoryElement, Category, VariableType)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DestinationNode).HasMaxLength(150);

                entity.Property(e => e.DestinationProduct).HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.GrossStandardVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Movement)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(156);

                entity.Property(e => e.Operacion)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OperationalDate).HasColumnType("date");

                entity.Property(e => e.PercentStandardUnCertainty).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Rno).HasColumnName("RNo");

                entity.Property(e => e.SourceNode).HasMaxLength(150);

                entity.Property(e => e.SourceProduct).HasMaxLength(150);

                entity.Property(e => e.SourceProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SystemName).HasMaxLength(50);

                entity.Property(e => e.Uncertainty).HasColumnType("decimal(29, 2)");
            });

            modelBuilder.Entity<ViewMovementInformation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_MovementInformation", "Admin");

                entity.HasComment("This View is to Fetch Data related to movement and related elements From Tables(Movement,MovementDestination,MovementSource,Node,Product,CategoryElemnet)");

                entity.Property(e => e.Comment).HasMaxLength(200);

                entity.Property(e => e.DestinationNodeName).HasMaxLength(150);

                entity.Property(e => e.DestinationProductId).HasMaxLength(20);

                entity.Property(e => e.DestinationProductName).HasMaxLength(150);

                entity.Property(e => e.DestinationProductTypeId).HasMaxLength(150);

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.GrossStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MeasurementUnit).HasMaxLength(50);

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasColumnName("MovementID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MovementTypeId)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.MovementTypeName).HasMaxLength(150);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OperationalDate).HasColumnType("date");

                entity.Property(e => e.SourceNodeName).HasMaxLength(150);

                entity.Property(e => e.SourceProductId).HasMaxLength(20);

                entity.Property(e => e.SourceProductName).HasMaxLength(150);

                entity.Property(e => e.SourceProductTypeId).HasMaxLength(150);

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.SystemName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UncertaintyPercentage).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<ViewMovementSourceWithNodeAndProductName>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_MovementSourceWithNodeAndProductName", "Admin");

                entity.HasComment("This View is to Fetch Data related to movement along with node and product information From Tables(MovementSource,Node,view_StorageLocationProductWithProductName)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy).HasMaxLength(260);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SourceNode).HasMaxLength(150);

                entity.Property(e => e.SourceProduct).HasMaxLength(150);

                entity.Property(e => e.SourceProductId).HasMaxLength(20);

                entity.Property(e => e.SourceProductTypeId).HasMaxLength(150);
            });

            modelBuilder.Entity<ViewMovementsByProduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_MovementsByProduct", "Admin");

                entity.HasComment("This View is to Fetch MovementsByProduct Data For PowerBi Report From Tables(Unbalance, Product, Ticket, Node, NodeTag, CategoryElement, Category)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Control).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.FilterType)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FinalInventory).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.IdentifiedLosses).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.InitialInventory).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Input).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Interface).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.NodeName).HasMaxLength(150);

                entity.Property(e => e.Output).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Tolerance).HasColumnType("decimal(29, 2)");

                entity.Property(e => e.UnidentifiedLosses).HasColumnType("decimal(29, 2)");
            });

            modelBuilder.Entity<ViewNodeConnectionProductRule>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_NodeConnectionProductRule", "Admin");

                entity.HasComment("This View is to Fetch Data for NodeConnectionProducts from NodeConnectionProduct,Node, categoryelement, product, Nodetag");

                entity.Property(e => e.DestinationNode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DestinationOperator)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.RuleName).HasMaxLength(100);

                entity.Property(e => e.SourceNode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.SourceOperator)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ViewNodeProductRule>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_NodeProductRule", "Admin");

                entity.HasComment("This View is to Fetch Data for NodeProductRule from NodeProductRule,NodeStorageLocation,StorageLocationProduct, Node, CategoryElement, product, Nodetag");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeType)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Operator)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.RuleName).HasMaxLength(100);

                entity.Property(e => e.Segment)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.StorageLocation)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.StorageLocationProductId).HasColumnName("StorageLocationProductID");
            });

            modelBuilder.Entity<ViewNodeRule>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_NodeRule", "Admin");

                entity.HasComment("This View is to Fetch Data for NodeRule from NodeOwnershipRule, Node, CategoryElement, product, Nodetag");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeType)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Operator)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.RuleName).HasMaxLength(100);

                entity.Property(e => e.Segment)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ViewNodeTagWithCategoryId>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_NodeTagWithCategoryId", "Admin");

                entity.HasComment("This View is to Fetch Data related to node tags and related elements From Tables(NodeTag,CategoryElement)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy).HasMaxLength(260);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ViewOperativeMovementsPeriodic>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_OperativeMovementsPeriodic", "Admin");

                entity.HasComment("This View is to Fetch Data of Movements for ADF to Load the data into OperativeMovements table.");

                entity.Property(e => e.DestinationNode).HasMaxLength(150);

                entity.Property(e => e.DestinationNodeType)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.MovementType).HasMaxLength(150);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OperationalDate).HasColumnType("date");

                entity.Property(e => e.SourceNode).HasMaxLength(150);

                entity.Property(e => e.SourceNodeType)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.SourceProduct).HasMaxLength(150);

                entity.Property(e => e.SourceProductType)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ViewOperativeMovementswithOwnerShipPeriodic>(entity =>
            {
                entity.HasNoKey();

                entity.ToView(" view_OperativeMovementswithOwnerShipPeriodic", "Admin");

                entity.HasComment("This View is to Fetch Data of Movements for ADF to Load the data.");

                entity.Property(e => e.DestinationNode).HasMaxLength(150);

                entity.Property(e => e.DestinationNodeType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ExecutionId)
                    .IsRequired()
                    .HasColumnName("ExecutionID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FieldWaterProduction)
                    .IsRequired()
                    .HasColumnName("FIeldWaterProduction")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LoadDate).HasColumnType("datetime");

                entity.Property(e => e.MovementType).HasMaxLength(150);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OperationalDate).HasColumnType("date");

                entity.Property(e => e.RelatedSourceField)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SourceField)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SourceNode).HasMaxLength(150);

                entity.Property(e => e.SourceNodeType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SourceProduct).HasMaxLength(150);

                entity.Property(e => e.SourceProductType).HasMaxLength(150);

                entity.Property(e => e.SourceSystem)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.TransferPoint)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ViewOwnerShipNode>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_OwnerShipNode", "Admin");

                entity.HasComment("This View is to Fetch Data from OwnershipNode and Ticket Table.");

                entity.Property(e => e.BlobPath).HasColumnName("blobPath");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("categoryName")
                    .HasMaxLength(150);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("createdBy")
                    .HasMaxLength(260);

                entity.Property(e => e.CutoffExecutionDate)
                    .HasColumnName("cutoffExecutionDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ErrorMessage).HasColumnName("errorMessage");

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasColumnName("nodeName")
                    .HasMaxLength(150);

                entity.Property(e => e.OwnerName)
                    .HasColumnName("ownerName")
                    .HasMaxLength(150);

                entity.Property(e => e.OwnershipNodeId).HasColumnName("ownershipNodeId");

                entity.Property(e => e.Segment)
                    .IsRequired()
                    .HasColumnName("segment")
                    .HasMaxLength(150);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TicketFinalDate)
                    .HasColumnName("ticketFinalDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TicketId).HasColumnName("ticketId");

                entity.Property(e => e.TicketStartDate)
                    .HasColumnName("ticketStartDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeId");
            });

            modelBuilder.Entity<ViewRelKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_RelKPI", "Admin");

                entity.HasComment("This View is to Fetch [view_RelKPI] Data For PowerBi Report");

                entity.Property(e => e.Indicator)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ViewRelationShipView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_RelationShipView", "Admin");

                entity.HasComment("This View is to Fetch View_RelationShipView Data For PowerBi Report From Tables(Unbalance, Product, Ticket, Node, NodeTag, CategoryElement, Category)");

                entity.Property(e => e.CalculationDate).HasColumnType("date");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Element)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ViewStorageLocationProductWithProductName>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_StorageLocationProductWithProductName", "Admin");

                entity.HasComment("This View is to Fetch Data related to StorageLocationProduct From Tables(StorageLocationProduct,Product)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy).HasMaxLength(260);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ProductName).HasMaxLength(150);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.UncertaintyPercentage).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<ViewTicket>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_Ticket", "Admin");

                entity.HasComment("This View is to Fetch Data from Ticket Table");

                entity.Property(e => e.BlobPath).HasColumnName("blobPath");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("categoryName")
                    .HasMaxLength(150);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("createdBy")
                    .HasMaxLength(260);

                entity.Property(e => e.CutoffExecutionDate)
                    .HasColumnName("cutoffExecutionDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ErrorMessage).HasColumnName("errorMessage");

                entity.Property(e => e.NodeName).HasMaxLength(150);

                entity.Property(e => e.OwnerName)
                    .HasColumnName("ownerName")
                    .HasMaxLength(150);

                entity.Property(e => e.Segment)
                    .IsRequired()
                    .HasColumnName("segment")
                    .HasMaxLength(150);

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TicketFinalDate)
                    .HasColumnName("ticketFinalDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TicketId).HasColumnName("ticketId");

                entity.Property(e => e.TicketStartDate)
                    .HasColumnName("ticketStartDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeId");
            });

            modelBuilder.Entity<ViewUnBalanceOutput>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_UnBalanceOutput", "Admin");

                entity.HasComment("This View is to Fetch Data related to UnablanceOutput For PowerBi Report From Tables(Movement, MovementDestination, MovementSource, Node, NodeTag, Product, Ticket)");

                entity.Property(e => e.Classification)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.DestiationNode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DestinationProduct)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.MeasurementUnit).HasMaxLength(50);

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MovementTypeId)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Movimiento)
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OperationalDate).HasColumnType("datetime");

                entity.Property(e => e.Scenario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SourceNode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.SourceProduct)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.SourceProductId).HasMaxLength(20);

                entity.Property(e => e.SourceProductTypeId).HasMaxLength(150);

                entity.Property(e => e.UnidadDeMedida)
                    .IsRequired()
                    .HasColumnName("Unidad de Medida")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ViewUnBalanceTicket>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_UnBalanceTicket", "Admin");

                entity.HasComment("This View is to Fetch Data related to Ticket Data For PowerBi Report From Table(Ticket)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(260);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedBy).HasMaxLength(260);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.TicketId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ViewUnbalance>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_Unbalance", "Admin");

                entity.HasComment("This View is to Fetch Data related to Unbalance  Data For PowerBi Report From Tables(Movement, Product, Unbalance)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FinalInvnetory).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IdentifiedLosses).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InitialInventory).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Inputs).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interface).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MeasurementUnit)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NodeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Outputs).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Tolerance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Unbalance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnidentifiedLosses).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ViewUnbalanceInput>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_UnbalanceInput", "Admin");

                entity.HasComment("This View is to Fetch Data related to UnbalanceInput  Data For PowerBi Report From Tables(Movement, MovementDestination, MovementSource, Node, NodeTag, Product, Ticket)");

                entity.Property(e => e.Classification)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.DestiationNode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DestinationProduct)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.MeasurementUnit).HasMaxLength(50);

                entity.Property(e => e.MovementId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MovementTypeId)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Movimiento)
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.NetStandardVolume).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OperationalDate).HasColumnType("datetime");

                entity.Property(e => e.Scenario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SourceNode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.SourceProduct)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.SourceProductId).HasMaxLength(20);

                entity.Property(e => e.SourceProductTypeId).HasMaxLength(150);

                entity.Property(e => e.UnidadDeMedida)
                    .IsRequired()
                    .HasColumnName("Unidad de Medida")
                    .HasMaxLength(150);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
