// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminModel.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.OData
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using EfCore.Models;
    using Microsoft.AspNet.OData.Builder;

    /// <summary>
    /// The node model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AdminModel
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
        /// Builds the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Build(ODataModelBuilder builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            BuildCategoryModel(builder);
            BuildNodeModel(builder);
            BuildNodeConnectionModel(builder);
            BuildNodeCostCenterModel(builder);
            BuildNodeTagModel(builder);
            BuildMovementModel(builder);
            BuildStorageLocationProductMappingModel(builder);
        }

        /// <summary>
        /// Build Edm Model.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="builder">Model builder.</param>
        /// <param name="name">Model name.</param>
        /// <param name="expand">The level of expansion.</param>
        private static EntitySetConfiguration<TEntity> BuildPluralizedModel<TEntity>(ODataModelBuilder builder, string name, int expand = 2)
            where TEntity : class
        {
            var entitySet = builder.EntitySet<TEntity>(name.Pluralize());
            entitySet.EntityType.Count().Filter().OrderBy().Expand(expand).Select().Page(MaxTopValue, DefaultPageSizeValue);

            return entitySet;
        }

        /// <summary>
        /// Build Edm Model.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="builder">Model builder.</param>
        /// <param name="name">Model name.</param>
        private static EntitySetConfiguration<TEntity> BuildModel<TEntity>(ODataModelBuilder builder, string name)
            where TEntity : class
        {
            var entitySet = builder.EntitySet<TEntity>(name);
            entitySet.EntityType.Count().Filter().OrderBy().Expand().Select().Page(MaxTopValue, DefaultPageSizeValue);

            return entitySet;
        }

        private static void BuildCategoryModel(ODataModelBuilder builder)
        {
            var category = BuildModel<Category>(builder, "Categories");
            var icon = BuildPluralizedModel<Icon>(builder, nameof(Icon));

            var element = BuildPluralizedModel<CategoryElement>(builder, nameof(CategoryElement));
            element.EntityType.HasKey(x => x.ElementId);

            element.HasRequiredBinding(e => e.Category, nameof(Category));
            element.HasOptionalBinding(e => e.Icon, nameof(Icon));

            category.HasManyBinding(c => c.Elements, "Elements");
            icon.HasManyBinding(c => c.CategoryElements, "Elements");
        }

        private static void BuildNodeModel(ODataModelBuilder builder)
        {
            // Node
            var node = BuildPluralizedModel<Node>(builder, nameof(Node), 3);
            builder.EntityType<Node>().Property(n => n.Name);
            builder.EntityType<Node>().Ignore(n => n.NodeTypeId);
            builder.EntityType<Node>().Ignore(n => n.OperatorId);
            builder.EntityType<Node>().Ignore(n => n.SegmentId);
            builder.EntityType<Node>().Ignore(n => n.OffchainNodes);

            // Node Storage Location
            var storageLocation = BuildPluralizedModel<NodeStorageLocation>(builder, nameof(NodeStorageLocation));
            storageLocation.HasRequiredBinding(e => e.Node, nameof(Node));
            node.HasManyBinding(n => n.NodeStorageLocations, $"{nameof(NodeStorageLocation)}".Pluralize());
            node.HasManyBinding(n => n.NodeTags, $"{nameof(NodeTag)}".Pluralize());

            // Node Storage Location Products
            var product = BuildPluralizedModel<StorageLocationProduct>(builder, nameof(StorageLocationProduct));
            product.HasRequiredBinding(p => p.NodeStorageLocation, nameof(NodeStorageLocation));
            storageLocation.HasManyBinding(s => s.Products, $"{nameof(StorageLocationProduct)}".Pluralize());

            // Storage Location Product Rules
            product.HasOptionalBinding(n => n.OwnershipRule, "OwnershipRule");
        }

        private static void BuildNodeConnectionModel(ODataModelBuilder builder)
        {
            // Node Connection
            var nodeConnection = BuildPluralizedModel<NodeConnection>(builder, nameof(NodeConnection));

            builder.EntityType<NodeConnection>().Ignore(n => n.OffchainNodeConnections);

            nodeConnection.HasRequiredBinding(e => e.SourceNode, "SourceNode");
            nodeConnection.HasRequiredBinding(e => e.DestinationNode, "DestinationNode");
            nodeConnection.HasOptionalBinding(e => e.Algorithm, "Algorithm");

            // Node Connection Product
            var nodeConnectionProduct = BuildPluralizedModel<NodeConnectionProduct>(builder, nameof(NodeConnectionProduct));
            nodeConnectionProduct.HasRequiredBinding(n => n.NodeConnection, nameof(NodeConnection));
            nodeConnection.HasManyBinding(c => c.Products, $"{nameof(NodeConnectionProduct)}".Pluralize());

            // Node Connection Product Rules
            nodeConnectionProduct.HasOptionalBinding(n => n.NodeConnectionProductRule, nameof(NodeConnectionProductRule));
        }

        private static void BuildNodeCostCenterModel(ODataModelBuilder builder)
        {
            // Node cost center
            var nodeCostCenter = BuildPluralizedModel<NodeCostCenter>(builder, nameof(NodeCostCenter));

            nodeCostCenter.HasRequiredBinding(e => e.SourceNode, "SourceNode");
            nodeCostCenter.HasRequiredBinding(e => e.DestinationNode, "DestinationNode");
            nodeCostCenter.HasOptionalBinding(e => e.CostCenterCategoryElement, "CostCenter");
            nodeCostCenter.HasOptionalBinding(e => e.MovementTypeCategoryElement, "MovementType");
        }

        private static void BuildNodeTagModel(ODataModelBuilder builder)
        {
            // Node Tag
            var nodeTag = BuildPluralizedModel<NodeTag>(builder, nameof(NodeTag));
            nodeTag.HasRequiredBinding(e => e.CategoryElement, nameof(CategoryElement));
            nodeTag.HasRequiredBinding(e => e.Node, nameof(Node));
        }

        private static void BuildMovementModel(ODataModelBuilder builder)
        {
            var movementModel = BuildPluralizedModel<Movement>(builder, nameof(Movement), 3);

            movementModel.HasRequiredBinding(e => e.MovementSource, nameof(MovementSource));
            movementModel.HasRequiredBinding(e => e.MovementDestination, nameof(MovementDestination));
            movementModel.HasRequiredBinding(e => e.Period, nameof(MovementPeriod));
        }

        private static void BuildStorageLocationProductMappingModel(ODataModelBuilder builder)
        {
            var movementModel = BuildPluralizedModel<StorageLocationProductMapping>(builder, nameof(StorageLocationProductMapping));

            movementModel.HasRequiredBinding(e => e.Product, nameof(Product));
            movementModel.HasRequiredBinding(e => e.StorageLocation, nameof(StorageLocation));
        }
    }
}
