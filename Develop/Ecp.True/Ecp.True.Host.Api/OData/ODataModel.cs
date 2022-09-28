// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ODataModel.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.OData
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Analytics;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.OData.Edm;

    /// <summary>
    /// The OData Model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ODataModel
    {
        /// <summary>
        /// The maximum top value.
        /// </summary>
        public static readonly int MaxTopValue = 25000;

        /// <summary>
        /// The default page size value.
        /// </summary>
        public static readonly int DefaultPageSizeValue = 25000;

        /// <summary>
        /// Get EDM model.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>EDM model.</returns>
        public static IEdmModel GetEdmModel(IServiceProvider serviceProvider)
        {
            var builder = new ODataConventionModelBuilder(serviceProvider, true).EnableLowerCamelCase();

            // Build Edm models
            BuildEdmModels(builder);
            return builder.GetEdmModel();
        }

        private static EntitySetConfiguration<TEntity> BuildPluralizedModel<TEntity>(ODataModelBuilder builder, string name)
            where TEntity : class
        {
            var entitySet = builder.EntitySet<TEntity>(name.Pluralize());
            entitySet.EntityType.Count().Filter().OrderBy().Expand(3).Select().Page(MaxTopValue, DefaultPageSizeValue);
            return entitySet;
        }

        private static EntitySetConfiguration<TEntity> BuildPluralizedQueryModel<TEntity, TKey>(ODataModelBuilder builder, string name, Expression<Func<TEntity, TKey>> keyDefinitionExpression)
            where TEntity : class
        {
            var entitySet = builder.EntitySet<TEntity>(name.Pluralize());
            entitySet.EntityType.HasKey(keyDefinitionExpression).Count().Filter().OrderBy().Expand(3).Select().Page(MaxTopValue, DefaultPageSizeValue);
            return entitySet;
        }

        private static void BuildQueryModel<TEntity, TKey>(ODataModelBuilder builder, string name, Expression<Func<TEntity, TKey>> keyDefinitionExpression)
            where TEntity : class
        {
            var entitySet = builder.EntitySet<TEntity>(name);
            entitySet.EntityType.HasKey(keyDefinitionExpression).Count().Filter().OrderBy().Expand().Select().Page(MaxTopValue, DefaultPageSizeValue);
        }

        private static void BuildEdmModels(ODataModelBuilder builder)
        {
            // Admin Entities
            AdminModel.Build(builder);

            // Ticket Model.
            BuildTicketModel(builder);

            // Ownership Node Model.
            BuildOwnershipNodeModel(builder);

            // Pending Transaction Error model
            BuildPendingTransactionErrorModel(builder);

            // Transformation Model.
            BuildTransformationModel(builder);

            // Homologation Model
            BuildHomologationModel(builder);

            // Annulation Model
            BuildPluralizedModel<Annulation>(builder, nameof(Annulation));

            // Analytics Entities
            BuildPluralizedModel<OperativeNodeRelationship>(builder, "operative");
            BuildPluralizedModel<OperativeNodeRelationshipWithOwnership>(builder, "logistic");

            // System Model.
            BuildSystemOwnershipCalculationModel(builder);
            BuildSystemUnbalancesModel(builder);

            // Deadletter messages Model.
            BuildPluralizedModel<DeadletteredMessage>(builder, "failure");

            // Master entities
            BuildPluralizedModel<LogisticCenter>(builder, nameof(LogisticCenter));
            BuildPluralizedModel<StorageLocation>(builder, nameof(StorageLocation));
            BuildPluralizedModel<Product>(builder, nameof(Product));
            BuildPluralizedModel<SystemTypeEntity>(builder, "systemType");
            BuildPluralizedModel<User>(builder, nameof(User));
            BuildPluralizedModel<Contract>(builder, "contract");

            // Query Entities
            BuildQueryModel<TicketEntity, int>(builder, "TicketEntities", k => k.TicketId);
            BuildQueryModel<SapLogisticMovementDetail, string>(builder, "LogisticMovement", k => k.MovementId);
            BuildQueryModel<SapLogisticMovementDetail, string>(builder, "failedlogisticmovement", k => k.MovementId);
            BuildPluralizedQueryModel<FileRegistrationInfo, string>(builder, nameof(FileRegistration), k => k.UploadId);
            BuildPluralizedQueryModel<FileRegistration, int>(builder, "integrationmanagement", x => x.FileRegistrationId);
            BuildPluralizedQueryModel<OwnershipError, int>(builder, nameof(OwnershipError), k => k.OperationId);
            BuildPluralizedQueryModel<OwnershipNodeData, int>(builder, "viewownershipnode", k => k.OwnershipNodeId);
            BuildPluralizedQueryModel<ExceptionInfo, int>(builder, "pendingtransactionerror", k => k.Id);
            BuildPluralizedQueryModel<DeltaNodeInfo, int>(builder, "deltanode", k => k.TicketId);
            BuildPluralizedQueryModel<ReportExecutionEntity, int>(builder, "reportentities", k => k.ExecutionId);

            var nodeModel = BuildPluralizedQueryModel<NodeRuleEntity, int>(builder, "nodeownershiprule", k => k.NodeId);
            nodeModel.EntityType.Ignore(x => x.RowVersion);

            var nodeConnectionProductModel = BuildPluralizedQueryModel<NodeConnectionProductRuleEntity, int>(builder, "nodeconnectionproductrule", k => k.NodeConnectionProductId);
            nodeConnectionProductModel.EntityType.Ignore(x => x.RowVersion);

            var nodeProductModel = BuildPluralizedQueryModel<NodeProductRuleEntity, int>(builder, "nodeproductrule", k => k.StorageLocationProductId);
            nodeProductModel.EntityType.Ignore(x => x.RowVersion);

            BuildPluralizedModel<ReportExecution>(builder, nameof(ReportExecution));
        }

        private static void BuildTicketModel(ODataModelBuilder builder)
        {
            // Ticket
            var ticket = BuildPluralizedModel<Ticket>(builder, nameof(Ticket));
            ticket.HasRequiredBinding(e => e.CategoryElement, nameof(CategoryElement));
        }

        private static void BuildOwnershipNodeModel(ODataModelBuilder builder)
        {
            // Ownership Node
            var ownershipNode = BuildPluralizedModel<OwnershipNode>(builder, nameof(OwnershipNode));
            ownershipNode.HasRequiredBinding(e => e.Ticket, nameof(Ticket));
            ownershipNode.HasRequiredBinding(e => e.Node, nameof(Node));
        }

        private static void BuildPendingTransactionErrorModel(ODataModelBuilder builder)
        {
            // Pending Transaction Error
            var pendingTransactionError = BuildPluralizedModel<PendingTransactionError>(builder, nameof(PendingTransactionError));
            pendingTransactionError.HasRequiredBinding(e => e.PendingTransaction, nameof(PendingTransaction));
        }

        private static void BuildSystemOwnershipCalculationModel(ODataModelBuilder builder)
        {
            var systemOwnershipCalculation = BuildPluralizedModel<SystemOwnershipCalculation>(builder, nameof(SystemOwnershipCalculation));
            systemOwnershipCalculation.HasRequiredBinding(e => e.OwnershipTicket, nameof(Ticket));
        }

        private static void BuildSystemUnbalancesModel(ODataModelBuilder builder)
        {
            var systemUnbalance = BuildPluralizedModel<SystemUnbalance>(builder, nameof(SystemUnbalance));
            systemUnbalance.HasRequiredBinding(e => e.UnbalanceTicket, nameof(Ticket));
        }

        private static void BuildTransformationModel(ODataModelBuilder builder)
        {
            // Transformation
            var transformation = BuildPluralizedModel<Transformation>(builder, nameof(Transformation));
            transformation.HasRequiredBinding(e => e.OriginSourceNode, "OriginSourceNode");
            transformation.HasRequiredBinding(e => e.OriginDestinationNode, "OriginDestinationNode");
            transformation.HasRequiredBinding(e => e.OriginSourceProduct, "OriginSourceProduct");
            transformation.HasRequiredBinding(e => e.OriginDestinationProduct, "OriginDestinationProduct");
            transformation.HasRequiredBinding(e => e.OriginMeasurement, "OriginMeasurement");
            transformation.HasRequiredBinding(e => e.DestinationSourceNode, "DestinationSourceNode");
            transformation.HasRequiredBinding(e => e.DestinationDestinationNode, "DestinationDestinationNode");
            transformation.HasRequiredBinding(e => e.DestinationSourceProduct, "DestinationSourceProduct");
            transformation.HasRequiredBinding(e => e.DestinationDestinationProduct, "DestinationDestinationProduct");
            transformation.HasRequiredBinding(e => e.DestinationMeasurement, "DestinationMeasurement");
        }

        private static void BuildHomologationModel(ODataModelBuilder builder)
        {
            var homologation = BuildPluralizedModel<Homologation>(builder, nameof(Homologation));
            homologation.HasManyBinding(c => c.HomologationGroups, $"{nameof(HomologationGroup)}".Pluralize());

            var homologationGroup = BuildPluralizedModel<HomologationGroup>(builder, nameof(HomologationGroup));
            homologationGroup.HasRequiredBinding(e => e.Homologation, nameof(Homologation));
            homologationGroup.HasManyBinding(c => c.HomologationDataMapping, $"{nameof(HomologationDataMapping)}".Pluralize());

            BuildPluralizedModel<HomologationObjectType>(builder, nameof(HomologationObjectType));
        }
    }
}