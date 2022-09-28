// <copyright file="EcpApiStepDefinitionBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace Ecp.True.Bdd.Tests.StepDefinitions.Api
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Executors;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;
    using Flurl.Http;

    using global::Bdd.Core.Api.Executors;
    using global::Bdd.Core.Api.StepDefinitions;
    using global::Bdd.Core.Entities;
    using global::Bdd.Core.Utils;

    using Microsoft.WindowsAzure.Storage;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using Ocaramba;

    using OfficeOpenXml;

    using TechTalk.SpecFlow;

    [Binding]
    public class EcpApiStepDefinitionBase : ApiStepDefinitionBase
    {
        public EcpApiStepDefinitionBase(FeatureContext featureContext)
        {
            var route = featureContext?.FeatureInfo?.Tags?.FirstOrDefault(x => x.ContainsIgnoreCase("route"))?.Split('=')?.LastOrDefault();
            var endpoint = featureContext?.FeatureInfo?.Tags?.FirstOrDefault(x => x.ContainsIgnoreCase("endpoint"))?.Split('=')?.LastOrDefault();
            this.Endpoint = new Uri(new Uri(endpoint ?? ConfigurationManager.AppSettings["ApiEndpoint"]), route).ToString();
            this.FlowEndpoint = this.Endpoint.Replace("true", "flow");
            this.SapEndpoint = new Uri(new Uri(endpoint ?? ConfigurationManager.AppSettings["SapEndpoint"]), route).ToString();
        }

        public string Endpoint { get; set; }

        public string SapEndpoint { get; set; }

        public string FlowEndpoint { get; set; }

        protected override ApiExecutor ApiExecutor
        {
            get
            {
                if (!this.FeatureContext.ContainsKey(nameof(this.ApiExecutor)))
                {
                    this.FeatureContext.Set(new ApiBase(), nameof(this.ApiExecutor));
                }

                return this.FeatureContext.Get<ApiBase>(nameof(this.ApiExecutor));
            }
        }

        public async Task<T> SapPostAsync<T>(string url, object content)
        {
            return await this.ApiExecutor.PostAsync<T>(url, content, new Credentials { SecretKey = "sappoapi" }).ConfigureAwait(false);
        }

        public async Task<T> FlowGetAsync<T>(string url)
        {
            return await this.ApiExecutor.GetAsync<T>(url, new Credentials { SecretKey = "sappoapi" }).ConfigureAwait(false);
        }

        public async Task<T> FlowPostAsync<T>(string url, object content)
        {
            return await this.ApiExecutor.PostAsync<T>(url, content, new Credentials { SecretKey = "sappoapi" }).ConfigureAwait(false);
        }

        public async Task SetResultAsync<T>(Func<Task<T>> task)
        {
            if (task != null)
            {
                try
                {
                    T value = await task.Invoke().ConfigureAwait(false);
                    this.SetValue(Entities.Keys.Result, value);
                }
                catch (FlurlHttpException ex)
                {
                    this.SetValue(Entities.Keys.Error, ex.Call.Response.Content);
                    this.SetValue(Entities.Keys.ErrorStatus, ex.Call.Response.StatusCode.ToString());
                }
            }
        }

        public async Task SetResultsAsync<T>(Func<Task<T>> task)
        {
            if (task != null)
            {
                try
                {
                    T value = await task.Invoke().ConfigureAwait(false);
                    this.SetValue(Entities.Keys.Results, value);
                }
                catch (FlurlHttpException ex)
                {
                    this.SetValue(Entities.Keys.Error, ex.Call.Response.Content);
                    this.SetValue(Entities.Keys.ErrorStatus, ex.Call.Response.StatusCode.ToString());
                }
            }
        }

        public async Task<string> NodeContentCreationWithOwnershipAsync(string content)
        {
            var temp = this.UserDetails;
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            }

            IDictionary<string, string> categoryElementRow = null;
            if (int.Parse(this.ScenarioContext["Count"].ToString(), CultureInfo.InvariantCulture) == 0 || this.GetValue("TransferPointMovement") == "True")
            {
                await this.CreateCatergoryElementAsync("2").ConfigureAwait(false);
                this.ScenarioContext["SegmentId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
                categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
                this.ScenarioContext["SegmentName"] = categoryElementRow[ConstantValues.Name];
                content = content.JsonChangePropertyValue("segmentId", this.ScenarioContext["SegmentId"].ToString());
                await this.CreateCatergoryElementAsync("3").ConfigureAwait(false);
                this.ScenarioContext["OperatorId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
                categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["OperatorId"] }).ConfigureAwait(false);
                this.ScenarioContext["OperatorName"] = categoryElementRow[ConstantValues.Name];
                content = content.JsonChangePropertyValue("operatorId", this.ScenarioContext["OperatorId"].ToString());
                this.ScenarioContext["Count"] = "1";
            }

            content = content.JsonChangePropertyValue("segmentId", this.ScenarioContext["SegmentId"].ToString());
            content = content.JsonChangePropertyValue("operatorId", this.ScenarioContext["OperatorId"].ToString());
            content = content.JsonChangePropertyValue("order", new Faker().Random.Number(9999, 999999));
            await this.CreateCatergoryElementAsync("1").ConfigureAwait(false);
            this.ScenarioContext["NodeTypeId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["NodeTypeId"] }).ConfigureAwait(false);
            this.ScenarioContext["NodeTypeName"] = categoryElementRow[ConstantValues.Name];
            content = content.JsonChangePropertyValue("nodeTypeId", this.ScenarioContext["NodeTypeId"].ToString());

            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.NodeWithTwoStorageLocations)))
            {
                await this.CreateCatergoryElementAsync("4").ConfigureAwait(false);
                this.ScenarioContext["StorageLocationTypeId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
                content = content.JsonChangePropertyValue("NewStorageLocation StorageLocationTypeId", this.ScenarioContext["StorageLocationTypeId"].ToString());
                this.SetValue(ConstantValues.NodeWithTwoStorageLocations, string.Empty);
            }

            await this.CreateCatergoryElementAsync("4").ConfigureAwait(false);
            this.ScenarioContext["StorageLocationTypeId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            content = content.JsonChangePropertyValue("StorageLocation StorageLocationTypeId", this.ScenarioContext["StorageLocationTypeId"].ToString());
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"" + temp.Role + "\"");
                await this.GivenIAmAuthenticatedForUserAsync(temp.Role).ConfigureAwait(false);
            }

            return content;
        }

        public void IProvidedRequiredDetailsForPendingTransactionsGrid()
        {
            this.ProvidedRequiredDetailsForPendingTransactionsGrid();
        }

        public void IProvidedRequiredDetailsForUnbalancesGrid()
        {
            this.ProvidedRequiredDetailsForUnbalancesGrid();
        }

        public async Task<string> GetLastCreatedRowIdAsync(string entity)
        {
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            return fieldValue;
        }

        public async Task CreateNodeConnectionForOfficialBalanceFileAsync(string sourceNodeId, string destinationNodeId, string uncertainityPercentage = "1.0")
        {
            var connectionContent = ApiContent.Creates[ConstantValues.CreateConnectionForOfficialBalanceFile];
            connectionContent = connectionContent.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.SourceNode], sourceNodeId);
            connectionContent = connectionContent.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.DestinationNode], destinationNodeId);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.ManageConnection]), JObject.Parse(connectionContent)).ConfigureAwait(false);
            this.SetValue(ConstantValues.NodeConnectionId, await this.GetLastCreatedRowIdAsync(ConstantValues.ManageConnection).ConfigureAwait(false));
            var nodeConnectionProductRow = await this.ReadAllSqlAsync(input: SqlQueries.GetNodeConnectionProductByNodeConnnectionID, args: new { nodeConnectionId = this.GetValue(ConstantValues.NodeConnectionId) }).ConfigureAwait(false);
            var nodeConnectionProduct = nodeConnectionProductRow.ToDictionaryList();
            for (int i = 0; i < nodeConnectionProduct.Count; i++)
            {
                var connectionProductContent = ApiContent.Creates[ConstantValues.UpdateNodeConnectionProduct];
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.NodeConnectionId, this.GetValue(ConstantValues.NodeConnectionId));
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.NodeConnectionProductId, nodeConnectionProduct[i][ConstantValues.NodeConnectionProductId].ToString());
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.RowVersion, nodeConnectionProduct[i][ConstantValues.RowVersion].ToString());
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.UpdateNodeConnectionProductId, nodeConnectionProduct[i][ConstantValues.ProductId].ToString());
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.UncertaintyPercentage, uncertainityPercentage);
                await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.NodeConnectionProduct]), JObject.Parse(connectionProductContent)).ConfigureAwait(false);
            }
        }

        public async Task CreateNodeConnectionAsync(string sourceNodeId, string destinationNodeId, int numberOfProducts, string uncertainityPercentage1 = "1.0", string uncertainityPercentage2 = "1.0", string productId = "Empty")
        {
            var connectionContent = numberOfProducts.Equals(1) ? ApiContent.Creates[ConstantValues.ManageConnectionWithOneProduct] : ApiContent.Creates[ConstantValues.ManageConnectionWithTwoProducts];
            connectionContent = numberOfProducts.Equals(1) ? connectionContent.JsonChangePropertyValue(ConstantValues.ManageConnectionProduct1ProductId, productId) : connectionContent;
            connectionContent = connectionContent.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.SourceNode], sourceNodeId);
            connectionContent = connectionContent.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.DestinationNode], destinationNodeId);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.ManageConnection]), JObject.Parse(connectionContent)).ConfigureAwait(false);
            this.SetValue(ConstantValues.NodeConnectionId, await this.GetLastCreatedRowIdAsync(ConstantValues.ManageConnection).ConfigureAwait(false));
            var nodeConnectionProductRow = await this.ReadAllSqlAsync(input: SqlQueries.GetNodeConnectionProductByNodeConnnectionID, args: new { nodeConnectionId = this.GetValue(ConstantValues.NodeConnectionId) }).ConfigureAwait(false);
            var nodeConnectionProduct = nodeConnectionProductRow.ToDictionaryList();
            var connectionProductContent = ApiContent.Creates[ConstantValues.UpdateNodeConnectionProduct];
            connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.NodeConnectionId, this.GetValue(ConstantValues.NodeConnectionId));
            connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.NodeConnectionProductId, nodeConnectionProduct[0][ConstantValues.NodeConnectionProductId].ToString());
            connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.RowVersion, nodeConnectionProduct[0][ConstantValues.RowVersion].ToString());
            connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.UpdateNodeConnectionProductId, nodeConnectionProduct[0][ConstantValues.ProductId].ToString());
            connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.UncertaintyPercentage, uncertainityPercentage1);
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.NodeConnectionProduct]), JObject.Parse(connectionProductContent)).ConfigureAwait(false);
            if (numberOfProducts == 2)
            {
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.NodeConnectionId, this.GetValue(ConstantValues.NodeConnectionId));
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.NodeConnectionProductId, nodeConnectionProduct[1][ConstantValues.NodeConnectionProductId].ToString());
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.RowVersion, nodeConnectionProduct[1][ConstantValues.RowVersion].ToString());
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.UpdateNodeConnectionProductId, nodeConnectionProduct[1][ConstantValues.ProductId].ToString());
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.UncertaintyPercentage, uncertainityPercentage2);
                connectionProductContent = connectionProductContent.JsonChangePropertyValue(ConstantValues.Priority, new Faker().Random.Number(2, 10));
                await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.NodeConnectionProduct]), JObject.Parse(connectionProductContent)).ConfigureAwait(false);
            }
        }

        public async Task CreateOwnershipRulesForNodeConnectionsAsync(string nodeConnectionId, int productCount = 2)
        {
            await this.UpdateOwnersForNodeConnectionAsync(nodeConnectionId, productCount).ConfigureAwait(false);
            var ownerContent = ApiContent.Creates[ConstantValues.CreateOwnership];
            var nodeConnectionProductRow = await this.ReadAllSqlAsync(input: SqlQueries.GetNodeConnectionProductByNodeConnnectionID, args: new { nodeConnectionId = nodeConnectionId }).ConfigureAwait(false);
            var nodeConnectionProduct = nodeConnectionProductRow.ToDictionaryList();
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRuleType, "NodeConnectionProduct");
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesId, nodeConnectionProduct[0][ConstantValues.NodeConnectionProductId].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesRowVersion, nodeConnectionProduct[0][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateOwnershipRule]), JObject.Parse(ownerContent)).ConfigureAwait(false);
            if (productCount == 2)
            {
                ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesId, nodeConnectionProduct[1][ConstantValues.NodeConnectionProductId].ToString());
                ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesRowVersion, nodeConnectionProduct[1][ConstantValues.RowVersion].ToString());
                await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateOwnershipRule]), JObject.Parse(ownerContent)).ConfigureAwait(false);
            }
        }

        public async Task CreateOwnershipRulesForNodeConnectionsForOfficialBalanceFileAsync(string nodeConnectionId)
        {
            await this.UpdateOwnersForNodeConnectionForOfficialBalanceFileAsync(nodeConnectionId).ConfigureAwait(false);
            var ownerContent = ApiContent.Creates[ConstantValues.CreateOwnership];
            var nodeConnectionProductRow = await this.ReadAllSqlAsync(input: SqlQueries.GetNodeConnectionProductByNodeConnnectionID, args: new { nodeConnectionId }).ConfigureAwait(false);
            var nodeConnectionProduct = nodeConnectionProductRow.ToDictionaryList();
            for (int i = 0; i < nodeConnectionProduct.Count; i++)
            {
                ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRuleType, "NodeConnectionProduct");
                ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesId, nodeConnectionProduct[i][ConstantValues.NodeConnectionProductId].ToString());
                ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesRowVersion, nodeConnectionProduct[i][ConstantValues.RowVersion].ToString());
                await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateOwnershipRule]), JObject.Parse(ownerContent)).ConfigureAwait(false);
            }
        }

        public async Task CreateOwnershipRulesAsync(string nodeId)
        {
            ////await this.UpdateNodeOwnersAsync(nodeId).ConfigureAwait(false);
            await this.UpdateNodeProductOwnersAsync(nodeId).ConfigureAwait(false);
            var ownerContent = ApiContent.Creates[ConstantValues.CreateOwnership];
            var storageLocationOwnerRow = await this.ReadAllSqlAsync(input: SqlQueries.GetStorageLocationOwner, args: new { nodeId }).ConfigureAwait(false);
            var storageLocation = storageLocationOwnerRow.ToDictionaryList();
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRuleType, "StorageLocationProduct");
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesId, storageLocation[0][ConstantValues.StorageLocationProductId].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesRowVersion, storageLocation[0][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateOwnershipRule]), JObject.Parse(ownerContent)).ConfigureAwait(false);
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesId, storageLocation[1][ConstantValues.StorageLocationProductId].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesRowVersion, storageLocation[1][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateOwnershipRule]), JObject.Parse(ownerContent)).ConfigureAwait(false);
        }

        public async Task CreateOwnershipRulesAnalyticalModelAsync(string nodeId)
        {
            ////await this.UpdateNodeOwnersAsync(nodeId).ConfigureAwait(false);
            await this.UpdateNodeProductOwnersAnalyticalModelAsync(nodeId).ConfigureAwait(false);
            var ownerContent = ApiContent.Creates[ConstantValues.CreateOwnership];
            var storageLocationOwnerRow = await this.ReadAllSqlAsync(input: SqlQueries.GetStorageLocationOwner, args: new { nodeId = nodeId }).ConfigureAwait(false);
            var storageLocation = storageLocationOwnerRow.ToDictionaryList();
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRuleType, "StorageLocationProduct");
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesId, storageLocation[0][ConstantValues.StorageLocationProductId].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesRowVersion, storageLocation[0][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateOwnershipRule]), JObject.Parse(ownerContent)).ConfigureAwait(false);
        }

        public async Task CreateNodesForTestDataAsync()
        {
            // Create Nodes
            await this.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
            this.SetValue("NodeId_1", this.GetValue("NodeId"));
            this.SetValue("NodeName_1", this.GetValue("NodeName"));
            this.SetValue("CategorySegment", this.GetValue("SegmentName"));
            this.ScenarioContext[ConstantValues.Segment] = this.GetValue("CategorySegment");
            if (string.IsNullOrEmpty(this.GetValue(ConstantValues.NoSONSegment)))
            {
                await this.ReadSqlAsDictionaryAsync(ApiContent.UpdateRow["UpdateSegmentToSONSegment"], args: new { elementId = this.ScenarioContext["SegmentId"].ToString() }).ConfigureAwait(false);
            }

            await this.CreateOwnershipRulesAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
            this.SetValue("NodeId_2", this.GetValue("NodeId"));
            this.SetValue("NodeName_2", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
            this.SetValue("NodeId_3", this.GetValue("NodeId"));
            this.SetValue("NodeName_3", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
            this.SetValue("NodeId_4", this.GetValue("NodeId"));
            this.SetValue("NodeName_4", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesAsync(this.GetValue("NodeId")).ConfigureAwait(false);
        }

        public async Task CreateNodesForAnalyticalModelAsync()
        {
            // Create Nodes
            await this.CreateNodesWithOwnershipAsync("10000002318").ConfigureAwait(false);
            this.SetValue("NodeId_1", this.GetValue("NodeId"));
            this.SetValue("NodeName_1", this.GetValue("NodeName"));
            this.SetValue("CategorySegment", this.GetValue("SegmentName"));
            this.ScenarioContext[ConstantValues.Segment] = this.GetValue("CategorySegment");
            await this.ReadSqlAsDictionaryAsync(ApiContent.UpdateRow["UpdateSegmentToSONSegment"], args: new { elementId = this.ScenarioContext["SegmentId"].ToString() }).ConfigureAwait(false);
            await this.CreateOwnershipRulesAnalyticalModelAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipAsync("10000002372").ConfigureAwait(false);
            this.SetValue("NodeId_2", this.GetValue("NodeId"));
            this.SetValue("NodeName_2", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesAnalyticalModelAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipAsync("10000002049").ConfigureAwait(false);
            this.SetValue("NodeId_3", this.GetValue("NodeId"));
            this.SetValue("NodeName_3", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesAnalyticalModelAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipAsync("10000003085").ConfigureAwait(false);
            this.SetValue("NodeId_4", this.GetValue("NodeId"));
            this.SetValue("NodeName_4", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesAnalyticalModelAsync(this.GetValue("NodeId")).ConfigureAwait(false);
        }

        public async Task CreateNodesForTransferPointTestDataAsync()
        {
            // Create Nodes
            await this.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
            this.SetValue("NodeId_1", this.GetValue("NodeId"));
            this.SetValue("NodeName_1", this.GetValue("NodeName"));
            this.SetValue("CategorySegment", this.GetValue("SegmentName"));
            this.ScenarioContext[ConstantValues.Segment] = this.GetValue("CategorySegment");
            await this.CreateOwnershipRulesAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            this.SetValue("TransferPointMovement", "True");
            await this.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
            this.SetValue("NodeId_2", this.GetValue("NodeId"));
            this.SetValue("NodeName_2", this.GetValue("NodeName"));
            this.SetValue("CategorySegment", this.GetValue("SegmentName"));
            this.ScenarioContext[ConstantValues.Segment] = this.GetValue("CategorySegment");
            await this.CreateOwnershipRulesAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            this.SetValue("TransferPointMovement", "False");
            await this.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
            this.SetValue("NodeId_3", this.GetValue("NodeId"));
            this.SetValue("NodeName_3", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
            this.SetValue("NodeId_4", this.GetValue("NodeId"));
            this.SetValue("NodeName_4", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateSegmentAsSon, args: new { Segment = this.GetValue("CategorySegment") }).ConfigureAwait(false);
        }

        public async Task CreateCatergoryElementAsync(string categoryId, string elementName = "Empty")
        {
            var temp = this.UserDetails;
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            }

            var categoryElementContent = ApiContent.Creates["Category Element"];
            categoryElementContent = elementName.EqualsIgnoreCase("Empty") ? categoryElementContent.JsonChangeValue() : categoryElementContent.JsonChangePropertyValue("name", elementName);
            categoryElementContent = categoryElementContent.JsonChangePropertyValue("categoryId", categoryId);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes["Category Element"]), JObject.Parse(categoryElementContent)).ConfigureAwait(false)).ConfigureAwait(false);
            var lastCategoryElement = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastCategoryElement).ConfigureAwait(false);
            this.ScenarioContext["CategoryElement"] = lastCategoryElement["ElementId"];
            this.ScenarioContext["CategoryElementName"] = lastCategoryElement["Name"];
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"" + temp.Role + "\"");
                await this.GivenIAmAuthenticatedForUserAsync(temp.Role).ConfigureAwait(false);
            }
        }

        public async Task CreateCategoryElementsForTestDataAsync()
        {
            // create elements
            await this.CreateCatergoryElementAsync("9").ConfigureAwait(false);
            this.SetValue("MovementTypeId", this.ScenarioContext["CategoryElement"]);
            this.SetValue("MovementTypeName", this.ScenarioContext["CategoryElementName"]);
            await this.CreateCatergoryElementAsync("9").ConfigureAwait(false);
            this.SetValue("AnnulationMovementTypeId", this.ScenarioContext["CategoryElement"]);
            this.SetValue("AnnulationMovementName", this.ScenarioContext["CategoryElementName"]);
            await this.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            this.SetValue("ProductTypeId", this.ScenarioContext["CategoryElement"]);
            await this.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            this.SetValue("SourceProductTypeId", this.ScenarioContext["CategoryElement"]);
            await this.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            this.SetValue("DestinationProductTypeId", this.ScenarioContext["CategoryElement"]);
            await this.CreateCatergoryElementAsync("7").ConfigureAwait(false);
            this.SetValue("Owner", this.ScenarioContext["CategoryElement"]);
            await this.CreateCatergoryElementAsync("20").ConfigureAwait(false);
            this.SetValue("AttributeId", this.ScenarioContext["CategoryElement"]);
            await this.CreateCatergoryElementAsync("6").ConfigureAwait(false);
            this.SetValue("ValueAttributeUnitId", this.ScenarioContext["CategoryElement"]);
            await this.CreateCatergoryElementAsync("8", this.ScenarioContext["SegmentName"].ToString() + "_Sistema").ConfigureAwait(false);
            this.ScenarioContext["SystemElementId"] = this.ScenarioContext["CategoryElement"].ToString();
            await this.CreateCatergoryElementAsync("8", this.ScenarioContext["SegmentName"].ToString() + "_Sistema2").ConfigureAwait(false);
            this.SetValue("SystemElementId2", this.ScenarioContext["CategoryElement"]);
            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovements))
                || !string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable))
                || !string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovementsCalcualtions))
                || !string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForInvalidScenarioWithAnnulation)))
            {
                await this.CreateCatergoryElementAsync("9").ConfigureAwait(false);
                this.SetValue("AnnulationMovementTypeIdForProductTransfer", this.ScenarioContext["CategoryElement"]);
                this.SetValue("AnnulationMovementNameForProductTransfer", this.ScenarioContext["CategoryElementName"]);

                await this.CreateCatergoryElementAsync("9").ConfigureAwait(false);
                this.SetValue("AnnulationMovementTypeIdForTolerance", this.ScenarioContext["CategoryElement"]);
                this.SetValue("AnnulationMovementNameForTolerance", this.ScenarioContext["CategoryElementName"]);

                await this.CreateCatergoryElementAsync("9").ConfigureAwait(false);
                this.SetValue("AnnulationMovementTypeIdForUnIdentifiedLosses", this.ScenarioContext["CategoryElement"]);
                this.SetValue("AnnulationMovementNameForUnIdentifiedLosses", this.ScenarioContext["CategoryElementName"]);
            }

            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable)))
            {
                for (int i = 1; i < 63; i++)
                {
                    await this.CreateCatergoryElementAsync("9").ConfigureAwait(false);
                    this.SetValue($"MovementTypeId{i}", this.ScenarioContext["CategoryElement"]);
                    this.SetValue($"MovementTypeName{i}", this.ScenarioContext["CategoryElementName"]);

                    await this.CreateCatergoryElementAsync("9").ConfigureAwait(false);
                    this.SetValue($"AnnulationMovementTypeId{i}", this.ScenarioContext["CategoryElement"]);
                    this.SetValue($"AnnulationMovementName{i}", this.ScenarioContext["CategoryElementName"]);
                }
            }
        }

        public async Task CreateNodeConnectionsForTestDataAsync()
        {
            // Create Connections between created nodes
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_2"), this.GetValue("NodeId_1"), 2, "0.07", "0.06").ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_1"), this.GetValue("NodeId_4"), 2, "0.05", "0.04").ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_3"), this.GetValue("NodeId_1"), 2, "0.02", "0.06").ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_3"), this.GetValue("NodeId_4"), 2, "0.02", "0.06").ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);
        }

        public async Task CreateNodeConnectionsForTestDataAnalyticalModelAsync()
        {
            // Create Connections between created nodes
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_2"), this.GetValue("NodeId_1"), 1, "0.07", "0.06").ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsAsync(this.GetValue("NodeConnectionId"), 1).ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_1"), this.GetValue("NodeId_4"), 1, "0.05", "0.04").ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsAsync(this.GetValue("NodeConnectionId"), 1).ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_3"), this.GetValue("NodeId_1"), 1, "0.02", "0.06").ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsAsync(this.GetValue("NodeConnectionId"), 1).ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_3"), this.GetValue("NodeId_4"), 1, "0.02", "0.06").ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsAsync(this.GetValue("NodeConnectionId"), 1).ConfigureAwait(false);
        }

        public async Task CreateNodeConnectionsForOfficialBalanceFileAsync()
        {
            // Create Connections between created nodes
            await this.CreateNodeConnectionForOfficialBalanceFileAsync(this.GetValue("NodeId_2"), this.GetValue("NodeId_1")).ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsForOfficialBalanceFileAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);
            await this.CreateNodeConnectionForOfficialBalanceFileAsync(this.GetValue("NodeId_1"), this.GetValue("NodeId_5")).ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsForOfficialBalanceFileAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);
            await this.CreateNodeConnectionForOfficialBalanceFileAsync(this.GetValue("NodeId_3"), this.GetValue("NodeId_4")).ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsForOfficialBalanceFileAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);
            await this.CreateNodeConnectionForOfficialBalanceFileAsync(this.GetValue("NodeId_1"), this.GetValue("NodeId_3")).ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsForOfficialBalanceFileAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);
            await this.CreateNodeConnectionForOfficialBalanceFileAsync(this.GetValue("NodeId_5"), this.GetValue("NodeId_3")).ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsForOfficialBalanceFileAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);
            await this.CreateNodeConnectionForOfficialBalanceFileAsync(this.GetValue("NodeId_1"), this.GetValue("NodeId_1")).ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsForOfficialBalanceFileAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);
        }

        public async Task CreateHomologationForExcelAsync()
        {
            try
            {
                var numberOfHomologations = await this.ReadAllSqlAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForXL]).ConfigureAwait(false);
                var homologationRow = numberOfHomologations.ToDictionaryList();
                for (int i = 0; i < homologationRow.Count; i++)
                {
                    this.ScenarioContext[ConstantValues.HomologationId] = homologationRow[i][ConstantValues.HomologationId];
                    var homologationGroup = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                    foreach (var homologationGroupRow in homologationGroup)
                    {
                        var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                        this.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                        await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = this.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                    }

                    await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                }
            }
            catch (NullReferenceException ex)
            {
                Logger.Info("Homologation for Excel does not exists");
                Assert.IsNotNull(ex);
            }
            catch (ArgumentNullException ex)
            {
                Logger.Info("Homologation for Excel does not exists");
                Assert.IsNotNull(ex);
            }

            // Create Homologation between 3 to 1
            var setupHomologationRequestForNodes = ApiContent.Creates[ConstantValues.HomologationForNodes];
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(this.GetValue("NodeId_1"), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(this.GetValue("NodeId_2"), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node3 destinationValue", int.Parse(this.GetValue("NodeId_3"), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node4 destinationValue", int.Parse(this.GetValue("NodeId_4"), CultureInfo.InvariantCulture));
            if (!string.IsNullOrEmpty(this.GetValue("NodeId_5")))
            {
                var homologationDataMappingForNodes = ApiContent.Creates[ConstantValues.HomologationDataMapping];
                homologationDataMappingForNodes = homologationDataMappingForNodes.JsonChangePropertyValue("sourceValue", "JAGUAR");
                homologationDataMappingForNodes = homologationDataMappingForNodes.JsonChangePropertyValue("destinationValue", int.Parse(this.GetValue("NodeId_5"), CultureInfo.InvariantCulture));
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMappingForNodes);
            }

            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForNodes)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 3 to 1 for Products
            string setupHomologationRequestForProducts;
            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForLogisticOfficialBalance)))
            {
                setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForLogisticProducts];
            }
            else
            {
                setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForProduct];
            }

            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProducts)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 3 to 1 for Unit
            var setupHomologationRequestForUnit = ApiContent.Creates[ConstantValues.HomologationForUnit];
            var homologationDataMappingForAttributeUnit = ApiContent.Creates[ConstantValues.HomologationDataMapping];
            homologationDataMappingForAttributeUnit = homologationDataMappingForAttributeUnit.JsonChangePropertyValue("sourceValue", "Automation_ValueAttributeUnit");
            homologationDataMappingForAttributeUnit = homologationDataMappingForAttributeUnit.JsonChangePropertyValue("destinationValue", this.GetValue("ValueAttributeUnitId"));
            setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMappingForAttributeUnit);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForUnit)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 3 to 1 for MovementTypeId
            var setupHomologationRequestForMovementType = ApiContent.Creates[ConstantValues.HomologationForMovementTypeId];
            setupHomologationRequestForMovementType = setupHomologationRequestForMovementType.JsonChangePropertyValue("HomologationDataMapping_MovementTypeId destinationValue", int.Parse(this.GetValue("MovementTypeId"), CultureInfo.InvariantCulture));
            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable)))
            {
                for (int i = 1; i < 63; i++)
                {
                    var homologationDataMappingForMovementType = ApiContent.Creates[ConstantValues.HomologationDataMapping];
                    homologationDataMappingForMovementType = homologationDataMappingForMovementType.JsonChangePropertyValue("sourceValue", $"DESPA{i}");
                    homologationDataMappingForMovementType = homologationDataMappingForMovementType.JsonChangePropertyValue("destinationValue", int.Parse(this.GetValue($"MovementTypeId{i}"), CultureInfo.InvariantCulture));
                    setupHomologationRequestForMovementType = setupHomologationRequestForMovementType.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMappingForMovementType);
                }
            }

            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForMovementType)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 3 to 1 for ProductTypeId
            var setupHomologationRequestForProductType = ApiContent.Creates[ConstantValues.HomologationForProductTypeId];
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_ProductTypeId destinationValue", int.Parse(this.GetValue("ProductTypeId"), CultureInfo.InvariantCulture));
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_SourceProductTypeId destinationValue", int.Parse(this.GetValue("SourceProductTypeId"), CultureInfo.InvariantCulture));
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_DestinationProductTypeId destinationValue", int.Parse(this.GetValue("DestinationProductTypeId"), CultureInfo.InvariantCulture));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProductType)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 3 to 1 for OwnerId
            var setupHomologationRequestForOwner = ApiContent.Creates[ConstantValues.HomologationForOwner];
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOwner)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 3 to 1 for Operator
            var setupHomologationRequestForOperator = ApiContent.Creates[ConstantValues.HomologationForOperator];
            setupHomologationRequestForOperator = setupHomologationRequestForOperator.JsonChangePropertyValue("sourceSystemId", 3);
            setupHomologationRequestForOperator = setupHomologationRequestForOperator.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", this.GetValue("OperatorId"));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOperator)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 3 to 1 for AttributeId
            var setupHomologationRequestForAttributeId = ApiContent.Creates[ConstantValues.HomologationForAttributeId];
            setupHomologationRequestForAttributeId = setupHomologationRequestForAttributeId.JsonChangePropertyValue("sourceSystemId", 3);
            setupHomologationRequestForAttributeId = setupHomologationRequestForAttributeId.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", this.GetValue("AttributeId"));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForAttributeId)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 3 to 1 for ValueAttributeUnit
            //// var setupHomologationRequestForValueAttributeUnit = ApiContent.Creates[ConstantValues.HomologationForValueAttributeUnit];
            //// setupHomologationRequestForValueAttributeUnit = setupHomologationRequestForValueAttributeUnit.JsonChangePropertyValue("sourceSystemId", 3);
            //// setupHomologationRequestForValueAttributeUnit = setupHomologationRequestForValueAttributeUnit.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", this.GetValue("ValueAttributeUnitId"));
            //// await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForValueAttributeUnit)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 3 to 1 for System
            var setupHomologationRequestForSystem = ApiContent.Creates[ConstantValues.HomologationForSystem];
            setupHomologationRequestForSystem = setupHomologationRequestForSystem.JsonChangePropertyValue("sourceSystemId", 3);
            setupHomologationRequestForSystem = setupHomologationRequestForSystem.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", this.GetValue("SystemElementId"));
            var homologationDataMappingForSystem = ApiContent.Creates[ConstantValues.HomologationDataMapping];
            homologationDataMappingForSystem = homologationDataMappingForSystem.JsonChangePropertyValue("sourceValue", "Automation_System2");
            homologationDataMappingForSystem = homologationDataMappingForSystem.JsonChangePropertyValue("destinationValue", this.GetValue("SystemElementId2"));
            setupHomologationRequestForSystem = setupHomologationRequestForSystem.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMappingForSystem);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForSystem)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 3 to 1 for SourceSystem
            string setupHomologationRequestForSourceSystem;
            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForOfficialBalanceFileWithoutCancellationMovements))
                || !string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable))
                || !string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovements))
                || !string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForInvalidScenarioWithAnnulation))
                || !string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForInvalidScenarioWithoutAnnulation)))
            {
                setupHomologationRequestForSourceSystem = ApiContent.Creates[ConstantValues.HomologationForExcelWithOfficialSourceSystem];
            }
            else
            {
                setupHomologationRequestForSourceSystem = ApiContent.Creates[ConstantValues.HomologationForSourceSystemExcel];
            }

            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForSourceSystem)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public async Task IHaveEventHomologationInTheSystemAsync()
        {
            // Delete Homologation between 5 to 1
            try
            {
                var homologationRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForEvent]).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                var homologationGroup = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                foreach (var homologationGroupRow in homologationGroup)
                {
                    var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                    this.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                    await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = this.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                }

                await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
            }
            catch (NullReferenceException ex)
            {
                Logger.Info("Homologation for Event does not exists");
                Assert.IsNotNull(ex);
            }
            catch (ArgumentNullException ex)
            {
                Logger.Info("Homologation for Event does not exists");
                Assert.IsNotNull(ex);
            }

            // Create Homologation between 5 to 1 For Nodes
            var setupHomologationRequestForNodes = ApiContent.Creates[ConstantValues.HomologationForEventNodes];
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(this.ScenarioContext["NodeId_1"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(this.ScenarioContext["NodeId_2"].ToString(), CultureInfo.InvariantCulture));
            var homologationDataMapping = ApiContent.Creates[ConstantValues.HomologationDataMapping];
            homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", "DESPACHOS");
            homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", int.Parse(this.ScenarioContext["NodeId_3"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
            homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", "RECIBOS");
            homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", int.Parse(this.ScenarioContext["NodeId_4"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForNodes)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For Product
            var setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForProduct];
            setupHomologationRequestForProducts = setupHomologationRequestForProducts.JsonChangePropertyValue("sourceSystemId", 5);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProducts)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For Owner
            var setupHomologationRequestForOwner = ApiContent.Creates[ConstantValues.HomologationForOwner];
            setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("sourceSystemId", 5);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOwner)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For Unit
            var setupHomologationRequestForUnit = ApiContent.Creates[ConstantValues.HomologationForUnit];
            setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("sourceSystemId", 5);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForUnit)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For EventType
            var setupHomologationRequestForEventType = ApiContent.Creates[ConstantValues.HomologationForEventType];
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForEventType)).ConfigureAwait(false)).ConfigureAwait(false);

            ////this.When("I update data in \"ValidFicoEvents\"");
            this.IUpdateDataInExcel("ValidFicoEvents");
            ////this.When("I navigate to \"FileUploadForPlanningAndProgrammingAndCollaborationAgreements\" page");
            this.UiNavigation("FileUploadForPlanningAndProgrammingAndCollaborationAgreements");
            ////this.When("I click on \"LoadNew\" \"button\"");
            this.IClickOn("LoadNew", "button");
            ////this.Then("I should see upload new file interface");
            this.IShouldSeeUploadNewFileInterface();
            ////this.When("I select \"Planning, Programming and Agreements\" from FileType dropdown");
            this.ISelectFromFileTypeDropdown("Planning, Programming and Agreements");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload in planning, programming and collaboration agreements page");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"ValidFicoEvents\" file from planning, programming and collaboration agreements directory");
            await this.ISelectFileFromExplorerAsync("ValidFicoEvents").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            await Task.Delay(30000).ConfigureAwait(true);
        }

        public async Task CreateHomologationForTrueToSivAsync()
        {
            // Delete Homologation between 1 to 6
            try
            {
                var homologationRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForSIV]).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                var homologationGroup = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                foreach (var homologationGroupRow in homologationGroup)
                {
                    var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                    this.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                    await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = this.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                }

                await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                Logger.Info("Homologation for True to Siv does not exists");
                Assert.IsNotNull(ex);
            }
            catch (NullReferenceException ex)
            {
                Logger.Info("Homologation for True to Siv does not exists");
                Assert.IsNotNull(ex);
            }

            // Deleting Logistic Movement Type from TRUE system
            await this.ReadAllSqlAsync(input: SqlQueries.DeleteOfficialLogisiticMovementTypes).ConfigureAwait(false);

            // Inserting Logistic Movement Type from TRUE system
            await this.ReadAllSqlAsync(input: SqlQueries.InsertOfficialLogisiticMovementTypes).ConfigureAwait(false);

            // Create Homologation between 1 to 5 for MovementTypeId
            var setupHomologationRequestForMovementTypeForSIV = ApiContent.Creates[ConstantValues.HomologationForMovementTypeIdForSIV];
            setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonChangePropertyValue("HomologationDataMapping_MovementTypeId sourceValue", int.Parse(this.GetValue("MovementTypeId"), CultureInfo.InvariantCulture));
            var homologationDataMapping = ApiContent.Creates[ConstantValues.HomologationDataMapping];
            IDictionary<string, object> logisticElementDetails = null;
            if (string.IsNullOrEmpty(this.GetValue(ConstantValues.NoSONSegment)))
            {
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", int.Parse(this.GetValueInternal("AnnulationMovementTypeId"), CultureInfo.InvariantCulture));
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Annul. DESPA");
                setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", 42);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Product Transfer");
                setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", int.Parse(this.GetValueInternal("AnnulationMovementTypeIdForProductTransfer"), CultureInfo.InvariantCulture));
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Annul. Product Transfer");
                setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", 43);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Tolerance");
                setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", int.Parse(this.GetValueInternal("AnnulationMovementTypeIdForTolerance"), CultureInfo.InvariantCulture));
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Annul. Tolerance");
                setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", 44);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "UnIdentified Losses");
                setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", int.Parse(this.GetValueInternal("AnnulationMovementTypeIdForUnIdentifiedLosses"), CultureInfo.InvariantCulture));
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Annul. UnIdentified Losses");
                setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
                logisticElementDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = "Anul. Tr. Material a material" }).ConfigureAwait(false);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", logisticElementDetails[ConstantValues.ElementId]);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Anul. Tr. Material a material");
                setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
                logisticElementDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = "Anul. Tr.trasladar ce a ce" }).ConfigureAwait(false);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", logisticElementDetails[ConstantValues.ElementId]);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Anul. Tr.trasladar ce a ce");
                setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
                logisticElementDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = "Anul. Tr. Almacen a Almacen" }).ConfigureAwait(false);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", logisticElementDetails[ConstantValues.ElementId]);
                homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Anul. Tr. Almacen a Almacen");
                setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
            }

            logisticElementDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = "Tr. Material a material" }).ConfigureAwait(false);
            homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", logisticElementDetails[ConstantValues.ElementId]);
            homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Tr. Material a material");
            setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
            logisticElementDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = "Tr.trasladar ce a ce" }).ConfigureAwait(false);
            homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", logisticElementDetails[ConstantValues.ElementId]);
            homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Tr.trasladar ce a ce");
            setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
            logisticElementDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = "Tr. Almacen a Almacen" }).ConfigureAwait(false);
            homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", logisticElementDetails[ConstantValues.ElementId]);
            homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", "Tr. Almacen a Almacen");
            setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable)))
            {
                for (int i = 1; i < 63; i++)
                {
                    homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", int.Parse(this.GetValue($"MovementTypeId{i}"), CultureInfo.InvariantCulture));
                    homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", $"DESPA{i}");
                    setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);

                    homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("sourceValue", int.Parse(this.GetValue($"AnnulationMovementTypeId{i}"), CultureInfo.InvariantCulture));
                    homologationDataMapping = homologationDataMapping.JsonChangePropertyValue("destinationValue", $"Annul. DESPA{i}");
                    setupHomologationRequestForMovementTypeForSIV = setupHomologationRequestForMovementTypeForSIV.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMapping);
                }
            }

            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForMovementTypeForSIV)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public async Task CreateNodesWithOwnershipAsync()
        {
            var nodeContent = ApiContent.Creates[ConstantValues.NodeWithMutipleProducts];
            nodeContent = nodeContent.JsonChangeValue();
            this.SetValue("NodeName", nodeContent.JsonGetValue("name"));
            nodeContent = await this.NodeContentCreationWithOwnershipAsync(nodeContent).ConfigureAwait(false);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            this.SetValue("NodeId", await this.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false));
        }

        public async Task CreateNodesWithOwnershipAsync(string productId)
        {
            var nodeContent = ApiContent.Creates[ConstantValues.NodeWithOneProduct];
            nodeContent = nodeContent.JsonChangeValue();
            this.SetValue("NodeName", nodeContent.JsonGetValue("name"));
            nodeContent = nodeContent.JsonChangeValueInArray("nodeStorageLocations_products", productId);
            nodeContent = await this.NodeContentCreationWithOwnershipAsync(nodeContent).ConfigureAwait(false);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            this.SetValue("NodeId", await this.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false));
        }

        public async Task GivenIAmAuthenticatedForUserAsync(string role)
        {
            await this.IAmAuthenticatedForUserAsync(this.ApiExecutor, role).ConfigureAwait(false);
        }

        public async Task CreateTestDataForOwnershipCalculationAsync()
        {
            await this.TestDataForOwnershipCalculationAsync(this).ConfigureAwait(false);
        }

        public async Task IHaveCreatedForNodeStatusAsync(string ownershipNodes)
        {
            Assert.IsNotNull(ownershipNodes);
            this.SetValue("NodeStatus", "Yes");
            ////this.Given("I want create TestData for " + ownershipNodes);
            await this.CreateTestDataForOwnershipCalculationAsync().ConfigureAwait(false);
            await this.CreateCatergoryElementAsync("8", this.GetValue("SegmentName") + "_Sistema").ConfigureAwait(false);
            this.SetValue("SystemElementId", this.ScenarioContext["CategoryElement"].ToString());
            this.SetValue("SystemElementName", this.ScenarioContext["CategoryElementName"].ToString());
            await this.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = this.GetValue("NodeId_1"), elementId = this.GetValue("SystemElementId"), date = 10 }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = this.GetValue("NodeId_2"), elementId = this.GetValue("SystemElementId"), date = 10 }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = this.GetValue("NodeId_3"), elementId = this.GetValue("SystemElementId"), date = 10 }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = this.GetValue("NodeId_4"), elementId = this.GetValue("SystemElementId"), date = 10 }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 10, nodeId = this.GetValue("NodeId_2") }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 10, nodeId = this.GetValue("NodeId_3") }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 10, nodeId = this.GetValue("NodeId_4") }).ConfigureAwait(false);
        }

        public async Task GivenIHaveInventoryOrMovementDataToProcessInSystemAsync(string entity)
        {
            await this.IHaveInventoryOrMovementDataToProcessInSystemAsync(entity).ConfigureAwait(false);
        }

        public async Task<string> CreateSourceAndDestinationNodesAsync(string content)
        {
            var temp = this.UserDetails;
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
                temp = this.UserDetails;
            }

            var nodeContent = ApiContent.Creates[ConstantValues.Nodes];
            if (!string.IsNullOrEmpty(this.GetValue("MultipleProduct")))
            {
                nodeContent = ApiContent.Creates[ConstantValues.NodeWithMultipleProduct];
            }

            nodeContent = nodeContent.JsonChangeValue();
            this.ScenarioContext["SourceNodeName"] = nodeContent.JsonGetValue("name");
            nodeContent = await this.NodeContentCreationAsync(nodeContent).ConfigureAwait(false);
            nodeContent = nodeContent.JsonChangePropertyValue("segmentId", 10);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.Nodes]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[ConstantValues.Nodes].ToPascalCase()];
            this.SetValue(Keys.SelectedValue, fieldValue);
            content = content.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.SourceNode], fieldValue);
            nodeContent = nodeContent.JsonChangeValue();
            this.ScenarioContext["DestinationNodeName"] = nodeContent.JsonGetValue("name");
            nodeContent = nodeContent.JsonChangePropertyValue("segmentId", 10);
            if ((this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements") && string.IsNullOrEmpty(this.GetValue(ConstantValues.DestinationProductIsNotSet))) || this.GetValue(ConstantValues.CreationOfBothMovementAndInventory) == "Yes")
            {
                nodeContent = nodeContent.JsonChangePropertyValue("ProductLocation productId", "10000002318");
            }

            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.Nodes]).ConfigureAwait(false);
            fieldValue = lastCreatedRow[ApiContent.Ids[ConstantValues.Nodes].ToPascalCase()];
            content = content.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.DestinationNode], fieldValue);
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"" + temp.Role + "\"");
                await this.GivenIAmAuthenticatedForUserAsync(temp.Role).ConfigureAwait(false);
            }

            return content;
        }

        public async Task UpdateXmlWithAsync(string field, string xmlField, string fieldValue)
        {
            await this.UpdateXmlForSinoperAsync(field, xmlField, fieldValue).ConfigureAwait(false);
        }

        public async Task<string> CreateOwnersAsync(string content, string product = "Product_1")
        {
            var temp = this.UserDetails;
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            }

            var categoryElementContent = ApiContent.Creates[ConstantValues.CategoryElement];
            categoryElementContent = categoryElementContent.JsonChangeValue();
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.CategoryElement]), JObject.Parse(categoryElementContent)).ConfigureAwait(false);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.CategoryElement]).ConfigureAwait(false);
            this.ScenarioContext[product + "-OwnerId_1"] = lastCreatedRow[ApiContent.Ids[ConstantValues.CategoryElement].ToPascalCase()];
            content = content.JsonChangePropertyValue(product + "-Owner_1 ownerId", this.ScenarioContext[product + "-OwnerId_1"].ToString());
            categoryElementContent = categoryElementContent.JsonChangeValue();
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.CategoryElement]), JObject.Parse(categoryElementContent)).ConfigureAwait(false);
            lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.CategoryElement]).ConfigureAwait(false);
            this.ScenarioContext[product + "-OwnerId_2"] = lastCreatedRow[ApiContent.Ids[ConstantValues.CategoryElement].ToPascalCase()];
            content = content.JsonChangePropertyValue(product + "-Owner_2 ownerId", this.ScenarioContext[product + "-OwnerId_2"].ToString());
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"" + temp.Role + "\"");
                await this.GivenIAmAuthenticatedForUserAsync(temp.Role).ConfigureAwait(false);
            }

            return content;
        }

        public async Task CreateNodesStepsAsync()
        {
            var nodeContent = ApiContent.Creates[ConstantValues.NodeWithMutipleProducts];
            nodeContent = nodeContent.JsonChangeValue();
            nodeContent = await this.NodeContentCreationWithSameSegementAsync(nodeContent).ConfigureAwait(false);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            this.SetValue("NodeId", await this.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false));
        }

        public async Task CreateNodeAsync()
        {
            var temp = this.UserDetails;
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            }

            var nodeContent = ApiContent.Creates[ConstantValues.Nodes];
            nodeContent = nodeContent.JsonChangeValue();
            nodeContent = await this.NodeContentCreationAsync(nodeContent).ConfigureAwait(false);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"" + temp.Role + "\"");
                await this.GivenIAmAuthenticatedForUserAsync(temp.Role).ConfigureAwait(false);
            }
        }

        public async Task IDonTReceiveInXMLAsync(string field)
        {
            if (!field.EqualsIgnoreCase("MovementId"))
            {
                this.UpdateXmlDefaultValue(field);
            }

            await this.PerformHomologationAsync(field).ConfigureAwait(false);
        }

        public async Task IReceiveTheDataWithThatExceedsCharactersAsync(string field1, int field2)
        {
            await this.ReceiveTheDataWithThatExceedsCharactersAsync(field1, field2).ConfigureAwait(false);
        }

        public void UpdateXmlDefaultValue(string field)
        {
            var xmlFieldValue = this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements") ? new Faker().Random.Number(100000, 1000000).ToString(CultureInfo.InvariantCulture) : DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var xmlField = this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements") ? ConstantValues.MovementId : "DATE";
#pragma warning disable S1121 // Assignments should not be made from within sub-expressions
            _ = this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements") ? this.ScenarioContext["MOVEMENTID"] = xmlFieldValue : this.ScenarioContext["DATE"] = xmlFieldValue;
#pragma warning restore S1121 // Assignments should not be made from within sub-expressions
            if (this.GetValue(Keys.EntityType).EqualsIgnoreCase("Inventory"))
            {
                this.SetValue(ConstantValues.InventoryId, this.GetValue("DATE").Replace("-", string.Empty));
            }

            if (string.IsNullOrEmpty(this.GetValue(ConstantValues.DestinationProductIsNotSet)) && this.GetValue(ConstantValues.InventoryWithMultipleProducts) != "Yes")
            {
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue);
            }
            else if (this.GetValue(ConstantValues.DestinationProductIsNotSet) == "Yes")
            {
                field = ConstantValues.MovementsWithoutDestination;
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue);
            }

            if (this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements"))
            {
                xmlFieldValue = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                xmlField = "MOVEMENT DATE";
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue);
                this.ScenarioContext["DATE"] = xmlFieldValue;
                xmlFieldValue = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                xmlField = "StartTime";
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue);
                xmlFieldValue = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                xmlField = "EndTime";
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue);
            }
            else if (this.GetValue(Keys.EntityType).EqualsIgnoreCase("Inventory") && this.GetValue(ConstantValues.InventoryWithMultipleProducts) != "Yes")
            {
                xmlField = ConstantValues.BATCHID + "_1";
                xmlFieldValue = string.IsNullOrEmpty(this.GetValue(ConstantValues.NumberOfBatchIdentifierCharacters)) ? new Faker().Random.AlphaNumeric(25).ToString(CultureInfo.InvariantCulture) : new Faker().Random.AlphaNumeric(26).ToString(CultureInfo.InvariantCulture);
                this.SetValue(ConstantValues.BATCHID + "_1", xmlFieldValue);
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue);
            }

            if (this.GetValue(ConstantValues.InventoryWithMultipleProducts) == "Yes")
            {
                field = ConstantValues.InventoryWithMultipleProducts;
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue);
                xmlField = ConstantValues.BATCHID + "_1";
                xmlFieldValue = new Faker().Random.AlphaNumeric(25).ToString(CultureInfo.InvariantCulture);
                this.SetValue(ConstantValues.BATCHID + "_1", xmlFieldValue);
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue);
                xmlField = ConstantValues.BATCHID + "_2";
                xmlFieldValue = string.IsNullOrEmpty(this.GetValue(ConstantValues.InventoryWithSameBatchIdentifier)) ? new Faker().Random.AlphaNumeric(25).ToString(CultureInfo.InvariantCulture) : this.GetValue(ConstantValues.BATCHID + "_1");
                this.SetValue(ConstantValues.BATCHID + "_2", xmlFieldValue);
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue);
            }
        }

        public async Task TestDataForOperationCutOffAsync(string days)
        {
            ////this.Given("I am authenticated as \"admin\"");
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            this.ScenarioContext["Count"] = "0";

            // Create Nodes
            await this.CreateNodesForTestDataAsync().ConfigureAwait(false);

            // create elements
            await this.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

            // Create Connections between created nodes
            await this.CreateNodeConnectionsForTestDataAsync().ConfigureAwait(false);

            // Homologation between 3 to 1
            await this.CreateHomologationForExcelAsync().ConfigureAwait(false);
            var elementRow = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            var elementId = elementRow["ElementId"];
            var otherElementId = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOtherElementIds, args: new { elementId, nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            List<dynamic> nodeTagIds = otherElementId.SelectMany(d => d.Values).ToList();
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            ////await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId1, args: new { nodeTagId = nodeTagIds[0] }).ConfigureAwait(false);
            ////await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId2, args: new { nodeTagId = nodeTagIds[9] }).ConfigureAwait(false);

            if (days == "SingleDay")
            {
                ////this.When("I update the excel with \"SingleDay\" data");
                this.WhenIUpdateTheExcelFileWithDaywiseData("SingleDay");
            }
            else if (days == "Testdata_20215")
            {
                ////this.When("I update the excel with \"Testdata_20215\" data");
                this.WhenIUpdateTheExcelFileWithDaywiseData("Testdata_20215");
            }
            else if (days == "Testdata_20215_0Volume")
            {
                ////this.When("I update the excel with \"Testdata_20215_0Volume\" data");
                this.WhenIUpdateTheExcelFileWithDaywiseData("Testdata_20215_0Volume");
            }
            else
            {
                ////this.When("I update the excel with \"TestDataCutOff_Daywise\" data");
                this.WhenIUpdateTheExcelFileWithDaywiseData("TestDataCutOff_Daywise");
            }

            ////this.When("I am logged in as \"admin\"");
            this.LoggedInAsUser("admin");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");

            if (days == "SingleDay")
            {
                ////this.When("I select \"SingleDay\" file from directory");
                await this.ISelectFileFromDirectoryAsync("SingleDay").ConfigureAwait(false);
            }
            else if (days == "Testdata_20215")
            {
                ////this.When("I select \"Testdata_20215\" file from directory");
                await this.ISelectFileFromDirectoryAsync("Testdata_20215").ConfigureAwait(false);
            }
            else if (days == "Testdata_20215_0Volume")
            {
                ////this.When("I select \"Testdata_20215_0Volume\" file from directory");
                await this.ISelectFileFromDirectoryAsync("Testdata_20215_0Volume").ConfigureAwait(false);
            }
            else
            {
                ////this.When("I select \"TestDataCutOff_Daywise\" file from directory");
                await this.ISelectFileFromDirectoryAsync("TestDataCutOff_Daywise").ConfigureAwait(false);
            }

            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        public async Task CreateInActiveNodesStepsAsync()
        {
            var nodeContent = ApiContent.Creates[ConstantValues.NodeWithMutipleProducts];
            nodeContent = nodeContent.JsonChangeValue();
            nodeContent = await this.NodeContentCreationWithSameSegementAsync(nodeContent).ConfigureAwait(false);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false)).ConfigureAwait(false);
            var entity = ConstantValues.Node;
            var updateContent = ApiContent.Updates[entity].Replace("^^placeholder^^", string.Empty);
            updateContent = await this.UpdateRequestJsonIdsAsync(updateContent, entity).ConfigureAwait(false);
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.IsActive, "false");
            updateContent = updateContent.JsonChangeValue(ConstantValues.Name, "_");
            updateContent = entity.EqualsIgnoreCase(ConstantValues.Node) ? await this.UpdateContentForNodeAsync(updateContent).ConfigureAwait(false) : updateContent;
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
            this.SetValue(ConstantValues.NodeId, await this.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false));
        }

        public async Task NodeOperativeOwnershipAsync()
        {
            this.ScenarioContext["Count"] = "0";
            ////this.Given("I am authenticated as \"admin\"");
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            await this.CreateNodesWithSendToSAPAsync().ConfigureAwait(false);
            this.SetValue("NodeId_1", this.GetValue(ConstantValues.NodeId));
            var nodeRow = await this.ReadSqlAsDictionaryAsync(ApiContent.LastCreated[ConstantValues.Node]).ConfigureAwait(false);
            this.FeatureContext["sourceNode"] = nodeRow[ConstantValues.Name];
            this.SetValue("CategorySegment", this.GetValue("SegmentName"));
            this.ScenarioContext[ConstantValues.Segment] = this.GetValue("CategorySegment");
            await this.CreateOwnershipRulesForSIVAsync(this.GetValue(ConstantValues.NodeId)).ConfigureAwait(false);
            await this.CreateNodesWithSendToSAPAsync().ConfigureAwait(false);
            this.SetValue("NodeId_2", this.GetValue(ConstantValues.NodeId));
            nodeRow = await this.ReadSqlAsDictionaryAsync(ApiContent.LastCreated[ConstantValues.Node]).ConfigureAwait(false);
            this.FeatureContext["destinationNode"] = nodeRow[ConstantValues.Name];
            await this.CreateOwnershipRulesForSIVAsync(this.GetValue(ConstantValues.NodeId)).ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_1"), this.GetValue("NodeId_2"), 1, "0.02", "0.06", "10000003001").ConfigureAwait(false);
            var logisticSourceCenter = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.BuildLogisticCenter, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            var logisticDestinationCenter = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.BuildLogisticCenter, args: new { nodeId = this.GetValue("NodeId_2") }).ConfigureAwait(false);
            this.FeatureContext["destinationStorageLocation"] = logisticDestinationCenter["LogisticCenter"].Split(':')[1];
            this.FeatureContext["sourceStorageLocation"] = logisticSourceCenter["LogisticCenter"].Split(':')[1];
            this.FeatureContext["logisticSourceCenter"] = logisticSourceCenter["LogisticCenter"];
            this.FeatureContext["logisticDestinationCenter"] = logisticDestinationCenter["LogisticCenter"];
            this.FeatureContext[ConstantValues.SourceProductId] = logisticSourceCenter["productName"];
            this.FeatureContext["destinationProduct"] = logisticDestinationCenter["productName"];
        }

        public async Task IHaveCalculatedOwnershipForSegmentAndTicketGeneratedAsync(string eventsCondition)
        {
            if (eventsCondition == "with")
            {
                ////this.Given("I have \"ownershipnodes\" created");
                await this.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
                this.SetValue(ConstantValues.OwnershipCalculatedForSegemnt, ConstantValues.True);
                ////this.Given("I have \"Event\" information in the system");
                await this.IHaveInformationInTheSystemAsync("Event").ConfigureAwait(false);
                ////this.When("I navigate to \"Operational Cutoff\" page");
                this.UiNavigation("Operational Cutoff");
                ////this.When("I choose CategoryElement from \"InitTicket\" \"Segment\" \"combobox\"");
                this.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
                ////this.When("I select the FinalDate lessthan \"4\" days from CurrentDate on \"Cutoff\" DatePicker");
                await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(4, "Cutoff").ConfigureAwait(false);
                ////this.When("I click on \"InitTicket\" \"next\" \"button\"");
                this.IClickOn("InitTicket\" \"Submit", "button");
                ////this.Then("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
                this.ValidateThatAsEnabled("validateInitialInventory\" \"submit", "button");
                ////this.When("I click on \"checkInventories\" \"Next\" \"button\"");
                this.IClickOn("validateInitialInventory\" \"submit", "button");
                this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
                ////this.When("I select all unbalances in the grid");
                this.ISelectAllPendingRepositroriesFromGrid();
                ////this.When("I click on \"consistencyCheck\" \"AddNote\" \"button\"");
                this.IClickOn("consistencyCheck\" \"AddNote", "button");
                ////this.When("I navigate to \"ownershipcalculation\" page");
                this.UiNavigation("ownershipcalculation");
                ////this.When("I select a value from \"Segment\"");
                this.SelectValueFromDropDown(this.ScenarioContext[ConstantValues.Segment].ToString(), "Segment");
                ////this.When("I select the FinalDate lessthan \"1\" days from CurrentDate on \"Ownership\" DatePicker");
                await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "Ownership").ConfigureAwait(false);
                ////this.When("I click on \"ownershipCalCriteria\" \"Next\" \"button\"");
                this.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
                ////this.When("I verify all validations passed");
                this.IVerifyValidationsPassed();
                ////this.When("I click on \"ownershipCalValidations\" \"Next\" \"button\"");
                this.IClickOn("ownershipCalulationValidations\" \"submit", "button");
                ////this.Then("verify the ownership is calculated successfully");
                await this.VerifyTheOwnershipIsCalculatedSuccessfullyAsync().ConfigureAwait(false);
            }
            else
            {
                ////this.Given("I have \"ownershipnodes\" created");
                await this.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
                ////this.When("I navigate to \"Operational Cutoff\" page");
                this.UiNavigation("Operational Cutoff");
                ////this.When("I choose CategoryElement from \"InitTicket\" \"Segment\" \"combobox\"");
                this.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
                ////this.When("I select the FinalDate lessthan \"4\" days from CurrentDate on \"Cutoff\" DatePicker");
                await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(4, "Cutoff").ConfigureAwait(false);
                ////this.When("I click on \"InitTicket\" \"next\" \"button\"");
                this.IClickOn("InitTicket\" \"Submit", "button");
                ////this.Then("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
                this.ValidateThatAsEnabled("validateInitialInventory\" \"submit", "button");
                ////this.When("I click on \"checkInventories\" \"Next\" \"button\"");
                this.IClickOn("validateInitialInventory\" \"submit", "button");
                this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
                ////this.When("I select all unbalances in the grid");
                this.ISelectAllPendingRepositroriesFromGrid();
                ////this.When("I click on \"consistencyCheck\" \"AddNote\" \"button\"");
                this.IClickOn("consistencyCheck\" \"AddNote", "button");
                ////this.When("I navigate to \"ownershipcalculation\" page");
                this.UiNavigation("ownershipcalculation");
                ////this.When("I select a value from \"Segment\"");
                this.SelectValueFromDropDown(this.ScenarioContext[ConstantValues.Segment].ToString(), "Segment");
                ////this.When("I select the FinalDate lessthan \"1\" days from CurrentDate on \"Ownership\" DatePicker");
                await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "Ownership").ConfigureAwait(false);
                ////this.When("I click on \"ownershipCalCriteria\" \"Next\" \"button\"");
                this.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
                ////this.When("I verify all validations passed");
                this.IVerifyValidationsPassed();
                ////this.When("I click on \"ownershipCalValidations\" \"Next\" \"button\"");
                this.IClickOn("ownershipCalulationValidations\" \"submit", "button");
                ////this.Then("verify the ownership is calculated successfully");
                await this.VerifyTheOwnershipIsCalculatedSuccessfullyAsync().ConfigureAwait(false);
            }
        }

        public async Task EventsUnderSameSegmentAsync()
        {
            var uploadFileName = @"TestData\Input\Planning, Programming and Collaboration agreements\EventsforMovements.xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            // For Events
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            // Updating Date Range of Events
            worksheet.Cells[2, 9].Value = DateTime.UtcNow.AddDays(-2).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[2, 10].Value = DateTime.UtcNow.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[3, 9].Value = DateTime.UtcNow.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[3, 10].Value = DateTime.UtcNow.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            package.Save();
            package.Dispose();

            ////this.Given("I am authenticated as \"admin\"");
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            var nodeDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            this.ScenarioContext.Add("NodeName_1", nodeDetails[ConstantValues.Name]);
            nodeDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = this.GetValue("NodeId_2") }).ConfigureAwait(false);
            this.ScenarioContext.Add("NodeName_2", nodeDetails[ConstantValues.Name]);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_1"), this.GetValue("NodeId_2"), 2, "0.07", "0.06").ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);

            // Delete Homologation between 5 to 1
            try
            {
                var homologationRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForEvent]).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                var homologationGroup = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                foreach (var homologationGroupRow in homologationGroup)
                {
                    var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                    this.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                    await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = this.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                }

                await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }

            // Create Homologation between 5 to 1 For Nodes
            var setupHomologationRequestForNodes = ApiContent.Creates[ConstantValues.HomologationForEventNodes];
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(this.GetValue("NodeId_1"), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(this.GetValue("NodeId_2"), CultureInfo.InvariantCulture));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForNodes)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For Product
            var setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForProduct];
            setupHomologationRequestForProducts = setupHomologationRequestForProducts.JsonChangePropertyValue("sourceSystemId", 5);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProducts)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For Owner
            var setupHomologationRequestForOwner = ApiContent.Creates[ConstantValues.HomologationForOwner];
            setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("sourceSystemId", 5);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOwner)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For Unit
            var setupHomologationRequestForUnit = ApiContent.Creates[ConstantValues.HomologationForUnit];
            setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("sourceSystemId", 5);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForUnit)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For EventType
            var setupHomologationRequestForEventType = ApiContent.Creates[ConstantValues.HomologationForEventType];
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForEventType)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public void CommonMethodForInventoryRegistration(int inventoriesCount = 1, int lengthOfField = 1, string attribute = null)
        {
            JArray inventoryArray = new JArray();
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            for (int i = 0; i < inventoriesCount; i++)
            {
                switch (this.GetValue(ConstantValues.TestData))
                {
                    case "InventoryIdentifierIsSet":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.AlphaNumeric(lengthOfField).ToString(CultureInfo.InvariantCulture));
                        break;
                    case "WithoutOptionalFields":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonRemoveObject("ProductId batchId");
                        content = content.JsonRemoveObject("version");
                        content = content.JsonRemoveObject("ProductId grossStandardQuantity");
                        content = content.JsonRemoveObject("ProductId productType");
                        break;
                    case "WithoutMandatoryFields":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        if (attribute == "nodeId" || attribute == "eventType" || attribute == "segmentId" || attribute == "inventoryId" || attribute == "scenarioId" || attribute == "sourceSystem" || attribute == "inventoryDate" || attribute == "destinationSystem")
                        {
                            if (attribute == "inventoryId" || attribute == "inventoryDate")
                            {
                                this.SetValue(ConstantValues.FieldToCheckMandatory, ConstantValues.Yes);
                            }
                            else
                            {
                                this.SetValue(ConstantValues.FieldToCheckMandatory, string.Empty);
                            }

                            content = content.JsonRemoveObject(attribute);
                        }
                        else if (attribute == "productId" || attribute == "netStandardQuantity" || attribute == "measurementUnit")
                        {
                            content = content.JsonRemoveObject("ProductId " + attribute);
                        }
                        else if (attribute == "ownerId" || attribute == "ownershipValue" || attribute == "ownerShipValueUnit")
                        {
                            content = content.JsonRemoveObject("OwnerId " + attribute);
                        }
                        else if (attribute == "attributeId" || attribute == "attributeValue" || attribute == "valueAttributeUnit")
                        {
                            content = content.JsonRemoveObject("Attribute " + attribute);
                        }

                        break;
                    case "FieldWithMoreThanLengthThatAccepts":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        if (attribute == "nodeId" || attribute == "version" || attribute == "system" || attribute == "eventType" || attribute == "segmentId" || attribute == "scenarioId" || attribute == "operatorId" || attribute == "observations" || attribute == "sourceSystem" || attribute == "destinationSystem")
                        {
                            content = content.JsonChangePropertyValue(attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                        }
                        else if (attribute == "batchId" || attribute == "productId" || attribute == "productType" || attribute == "measurementUnit")
                        {
                            content = content.JsonChangePropertyValue("ProductId " + attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                        }
                        else if (attribute == "ownerId" || attribute == "ownerShipValueUnit")
                        {
                            content = content.JsonChangePropertyValue("OwnerId " + attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                        }
                        else if (attribute == "attributeId" || attribute == "attributeType" || attribute == "attributeValue" || attribute == "valueAttributeUnit" || attribute == "attributeDescription")
                        {
                            content = content.JsonChangePropertyValue("Attribute " + attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                        }

                        break;
                    case "WithScenarioId":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("scenarioId", 3);
                        content = content.JsonChangePropertyValue("ProductId batchId", this.GetValue(ConstantValues.BATCHID));
                        break;
                    case "WithOutScenarioId":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonRemoveObject("scenarioId");
                        break;
                    case "WithValidFormatForFields":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("ProductId batchId", this.GetValue(ConstantValues.BATCHID));
                        content = content.JsonChangePropertyValue("ProductId grossStandardQuantity", 1000);
                        content = content.JsonChangePropertyValue("system", this.GetValue(ConstantValues.System));
                        content = content.JsonChangePropertyValue("version", this.GetValue(ConstantValues.Version));
                        content = content.JsonChangePropertyValue("scenarioId", 3);
                        break;
                    case "InvalidFormatFor_batchId":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("ProductId batchId", this.GetValue(ConstantValues.BATCHID));
                        break;
                    case "InvalidFormatFor_version":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("version", this.GetValue(ConstantValues.Version));
                        break;
                    case "InvalidFormatFor_grossStandardQuantity":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("ProductId grossStandardQuantity", this.GetValue(ConstantValues.GrossStandardQuantity));
                        break;
                    case "InvalidFormatFor_system":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("ProductId grossStandardQuantity", this.GetValue(ConstantValues.System));
                        break;
                    case "InvalidFormatFor_scenarioId":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("scenarioId", this.GetValue(ConstantValues.ScenarioId));
                        break;
                    case "RenamedAttributes":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("scenarioId", 3);
                        content = content.JsonChangePropertyValue("uncertainty", 0.01);
                        content = content.JsonChangePropertyValue("ProductId netStandardQuantity", 2343);

                        // segmentId and operatorId will be taken care in previous Homologation step so not updating here
                        break;
                    case "SendingOldAttributesInsteadOfRenamedAttributes_scenario":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonAddField(attribute, 1);
                        break;
                    case "SendingOldAttributesInsteadOfRenamedAttributes_tolerance":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonAddField(attribute, 1);
                        break;
                    case "SendingOldAttributesInsteadOfRenamedAttributes_segment":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonAddField(attribute, "Automation_Segment");
                        break;
                    case "SendingOldAttributesInsteadOfRenamedAttributes_operator":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonAddField(attribute, "ODC");
                        break;
                    case "SendingOldAttributesInsteadOfRenamedAttributes_productVolume":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonAddField(attribute, 1000);
                        break;
                    case "BasedOnEvent_INSERT":
                        content = content.JsonChangePropertyValue("eventType", "INSERT");
                        break;
                    case "BasedOnEvent_UPDATE":
                        content = content.JsonChangePropertyValue("eventType", "UPDATE");
                        break;
                    case "BasedOnEvent_DELETE":
                        content = content.JsonChangePropertyValue("eventType", "DELETE");
                        break;
                    case "WithoutBatchId":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("scenarioId", 3);
                        content = content.JsonChangePropertyValue("ProductId batchId", string.Empty);
                        break;
                    case "WithBatchId":
                        this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("scenarioId", 3);
                        content = content.JsonChangePropertyValue("ProductId batchId", this.GetValue("BatchId"));
                        break;
                    default:
                        break;
                }

                if (string.IsNullOrEmpty(this.GetValue(ConstantValues.FieldToCheckMandatory)))
                {
                    if (string.IsNullOrEmpty(this.GetValue(ConstantValues.FieldToCheckErrorMessage)))
                    {
                        content = content.JsonChangePropertyValue("inventoryId", this.GetValue(ConstantValues.InventoryId));
                    }
                    else
                    {
                        content = content.JsonChangePropertyValue("inventoryId", new Faker().Random.AlphaNumeric(21).ToString(CultureInfo.InvariantCulture));
                    }

                    content = content.JsonChangePropertyValue(ConstantValues.InventoryDate, DateTime.Now.AddDays(-2).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                }

                if (this.GetValue(Entities.Keys.EntityType).ContainsIgnoreCase("Homologated"))
                {
                    content = content.JsonChangePropertyValue("sourceSystem", "162");
                    content = content.JsonChangePropertyValue("nodeId", this.GetValue("NodeId_1"));
                    content = content.JsonChangePropertyValue("system", this.GetValue("SystemElementId"));
                    content = content.JsonChangePropertyValue("segmentId", this.GetValue("SegmentId"));
                    content = content.JsonChangePropertyValue("operatorId", this.GetValue("OperatorId"));
                    content = content.JsonChangePropertyValue("ProductId productId", "10000002318");
                    content = content.JsonChangePropertyValue("ProductId productType", this.GetValue("ProductTypeId"));
                    content = content.JsonChangePropertyValue("ProductId measurementUnit", 31);
                    content = content.JsonChangePropertyValue("Attribute attributeId", this.GetValue("AttributeId"));
                    content = content.JsonChangePropertyValue("Attribute valueAttributeUnit", this.GetValue("ValueAttributeUnitId"));
                    content = content.JsonChangePropertyValue("OwnerId ownerId", this.GetValue("Owner"));
                }

                inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        public async Task GivenIHaveCalculatedOwnershipForSegmentwithEventsofSameSegmentAndTicketGeneratedAsync(string eventsCondition)
        {
            if (eventsCondition == "with")
            {
                ////this.Given("I have \"ownershipnodes\" created");
                await this.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
                this.SetValue(ConstantValues.OwnershipCalculatedForSegemnt, true);
                ////this.Given("I have \"Contract\" information in the system");
                await this.IHaveInformationInTheSystemAsync("Contract").ConfigureAwait(false);
                ////this.When("I create test data for events to generate movements");
                await this.EventsUnderSameSegmentAsync().ConfigureAwait(false);
                ////this.When("I navigate to \"FileUploadForPlanningAndProgrammingAndCollaborationAgreements\" page");
                this.UiNavigation("FileUploadForPlanningAndProgrammingAndCollaborationAgreements");
                ////this.When("I click on \"LoadNew\" \"button\"");
                this.IClickOn("LoadNew", "button");
                ////this.Then("I should see upload new file interface");
                this.IShouldSeeUploadNewFileInterface();
                ////this.When("I select \"Planning, Programming and Agreements\" from FileType dropdown");
                this.ISelectFromFileTypeDropdown("Planning, Programming and Agreements");
                ////this.When("I select \"Insert\" from FileUpload dropdown");
                this.ISelectFileFromFileUploadDropdown("Insert");
                ////this.When("I click on \"Browse\" to upload in planning, programming and collaboration agreements page");
                this.IClickOnUploadButton("Browse");
                ////this.When("I select \"EventsforMovements\" file from planning, programming and collaboration agreements directory");
                await this.ISelectFileFromExplorerAsync("EventsforMovements").ConfigureAwait(false);
                ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
                this.IClickOn("uploadFile\" \"Submit", "button");
                await Task.Delay(60000).ConfigureAwait(true);
                ////this.Given("Ownership is Calculated for Segment and Ticket is Generated");
                await this.CalculatedOwnershipForSegmentAndTicketGeneratedAsync().ConfigureAwait(false);
            }
            else
            {
                ////this.Given("I have \"ownershipnodes\" created");
                await this.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
                ////this.Given("Ownership is Calculated for Segment and Ticket is Generated");
                await this.CalculatedOwnershipForSegmentAndTicketGeneratedAsync().ConfigureAwait(false);
            }
        }

        public async Task CalculatedOwnershipForSegmentAndTicketGeneratedAsync()
        {
            ////this.When("I have ownership strategy for node");
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeWithOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
            ////this.When("I have ownership strategy for node products");
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeProductsWithOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
            ////this.When("I have ownership strategy for node connections");
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeConnectionsWithOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
            ////this.When("I navigate to \"Operational Cutoff\" page");
            this.UiNavigation("Operational Cutoff");
            ////this.When("I click on \"NewCut\" \"button\"");
            this.IClickOn("NewCut", "button");
            ////this.When("I choose CategoryElement from \"InitTicket\" \"Segment\" \"combobox\"");
            this.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
            ////this.When("I select the FinalDate lessthan \"4\" days from CurrentDate on \"Cutoff\" DatePicker");
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(4, "Cutoff").ConfigureAwait(false);
            ////this.When("I click on \"InitTicket\" \"next\" \"button\"");
            this.IClickOn("InitTicket\" \"Submit", "button");
            ////this.Then("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("validateInitialInventory\" \"submit", "button");
            ////this.When("I click on \"checkInventories\" \"Next\" \"button\"");
            this.IClickOn("validateInitialInventory\" \"submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I click on \"ErrorsGrid\" \"AddNote\" \"button\"");
            this.IClickOn("ErrorsGrid\" \"AddNote", "button");
            ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
            this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
            ////this.When("I click on \"AddComment\" \"Submit\" \"button\"");
            this.IClickOn("AddComment\" \"Submit", "button");
            ////this.When("validate that \"ErrorsGrid\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("ErrorsGrid\" \"Submit", "button");
            ////this.When("I click on \"ErrorsGrid\" \"Next\" \"button\"");
            this.IClickOn("ErrorsGrid\" \"Submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all unbalances in the grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            ////this.When("I click on \"consistencyCheck\" \"AddNote\" \"button\"");
            this.IClickOn("consistencyCheck\" \"AddNote", "button");
            ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
            this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
            ////this.When("I click on \"AddComment\" \"submit\" \"button\"");
            this.IClickOn("AddComment\" \"submit", "button");
            ////this.When("I click on \"ConsistencyCheck\" \"Next\" \"button\"");
            this.IClickOn("unbalancesGrid\" \"submit", "button");
            ////this.When("I click on \"Confirm\" \"Cutoff\" \"Submit\" \"button\"");
            this.IClickOn("ConfirmCutoff\" \"Submit", "button");
            ////this.When("I wait till cutoff ticket processing to complete");
            await this.WaitForTicketToCompleteProcessingAsync().ConfigureAwait(false);
            ////this.When("I navigate to \"ownershipcalculation\" page");
            this.UiNavigation("ownershipcalculation");
            ////this.When("I click on \"NewBalance\" \"button\"");
            this.IClickOn("NewBalance", "button");
            ////this.When("I select the FinalDate lessthan \"1\" days from CurrentDate on \"Ownership\" DatePicker");
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "Ownership").ConfigureAwait(false);
            ////this.When("I click on \"ownershipCalCriteria\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
            ////this.When("I verify all \"9\" validations passed");
            this.IVerifyAllValidationsPassed(9);
            ////this.When("I click on \"ownershipCalValidations\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalulationValidations\" \"submit", "button");
            ////this.When("I click on \"OwnershipCalConfirmation\" \"Execute\" \"button\"");
            this.IClickOn("ownershipCalculationConfirmation\" \"submit", "button");
            ////this.Then("I verify if the ownership calculation started");
            ////this.Then("verify the ownership is calculated successfully");
            await this.VerifyTheOwnershipIsCalculatedSuccessfullyAsync().ConfigureAwait(false);
        }

        public async Task IHaveInformationInTheSystemAsync(string type)
        {
            if (type.ContainsIgnoreCase(ConstantValues.Event))
            {
                await this.IHaveHomologationDataInTheSystemAsync(systemType: "Event", this).ConfigureAwait(false);
                if (type.EqualsIgnoreCase("ValidEvent"))
                {
                    ////this.When("I update data in \"InsertEventExcel\"");
                    this.IUpdateDataInExcel("InsertEventExcel");
                }
                else
                {
                    ////this.When("I update date range in \"ValidExcel\" of Events");
                    this.IUpdateTheEventsExcel("ValidExcel");
                }

                ////this.When("I navigate to \"FileUploadForPlanningAndProgrammingAndCollaborationAgreements\" page");
                this.UiNavigation("FileUploadForPlanningAndProgrammingAndCollaborationAgreements");
                ////this.When("I click on \"LoadNew\" \"button\"");
                this.IClickOn("LoadNew", "button");
                ////this.Then("I should see upload new file interface");
                this.IShouldSeeUploadNewFileInterface();
                ////this.When("I select \"Planning, Programming and Agreements\" from FileType dropdown");
                this.ISelectFromFileTypeDropdown("Planning, Programming and Agreements");
                ////this.When("I select \"Insert\" from FileUpload dropdown");
                this.ISelectFileFromFileUploadDropdown("Insert");
                ////this.When("I click on \"Browse\" to upload in planning, programming and collaboration agreements page");
                this.IClickOnUploadButton("Browse");
                if (type.EqualsIgnoreCase("ValidEvent"))
                {
                    ////this.When("I select \"InsertEventExcel\" file from planning, programming and collaboration agreements directory");
                    await this.ISelectFileFromExplorerAsync("InsertEventExcel").ConfigureAwait(false);
                }
                else
                {
                    ////this.When("I select \"ValidExcel\" file from planning, programming and collaboration agreements directory");
                    await this.ISelectFileFromExplorerAsync("ValidExcel").ConfigureAwait(false);
                }

                ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
                this.IClickOn("uploadFile\" \"Submit", "button");
                await Task.Delay(10000).ConfigureAwait(true);

                //// var eventIdForEvent = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetEventId, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
                //// await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateSourceAndDestionNodeIdForEvents, args: new { nodeId1 = this.GetValue("NodeId_1"), nodeId2 = this.GetValue("NodeId_4"), eventId = eventIdForEvent["EventId"] }).ConfigureAwait(false);
                //// Replacing Segment created during Events Testdata creation process with Ownership calculating segment
                this.ScenarioContext["SegmentName"] = this.GetValue("Segment");
            }
            else if (type == ConstantValues.Contract)
            {
                ////this.Given("I create test data for contracts to generate movements");
                await this.SameSegmentContractsAsync().ConfigureAwait(false);
                ////this.When("I navigate to \"FileUploadForSalesAndPurchases\" page");
                this.UiNavigation("FileUploadForSalesAndPurchases");
                ////this.When("I click on \"loadNew\" \"button\"");
                this.IClickOn("loadNew", "button");
                ////this.When("I select in \"Contracts\" from movement type dropdown");
                this.WhenISelectInContractsFromMovementTypeDropdown("Contracts");
                ////this.When("I select \"Insert\" from FileUpload dropdown");
                this.ISelectFileFromFileUploadDropdown("Insert");
                ////this.When("I click on \"Browse\" to upload contracts");
                this.IClickOnUploadButton("Browse");
                ////this.When("I select \"ContractsforMovements\" file from purchase sales");
                this.WhenISelectFileFromGrid("ContractsforMovements");
                ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
                this.IClickOn("uploadFile\" \"Submit", "button");
                ////this.Then("I verify if i have registered the contracts");
                await this.IVerifyIfIHaveRegisteredTheContractsAsync().ConfigureAwait(true);
                this.ScenarioContext["SegmentName"] = this.GetValue("Segment");
            }
        }

        public async Task CreateInActiveNodeConnectionAsync(string sourceNodeId, string destinationNodeId, int numberOfProducts, string connection, string uncertainityPercentage1 = "1.0", string uncertainityPercentage2 = "1.0", string productId = "Empty")
        {
            var connectionContent = numberOfProducts.Equals(1) ? ApiContent.Creates[ConstantValues.ManageConnectionWithOneProduct] : ApiContent.Creates[ConstantValues.ManageConnectionWithTwoProducts];
            connectionContent = numberOfProducts.Equals(1) ? connectionContent.JsonChangePropertyValue(ConstantValues.ManageConnectionProduct1ProductId, productId) : connectionContent;
            connectionContent = connectionContent.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.SourceNode], sourceNodeId);
            connectionContent = connectionContent.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.DestinationNode], destinationNodeId);
            connectionContent = connectionContent.JsonChangePropertyValue(ConstantValues.ManageConnectionProduct1UncertainityPercentage, uncertainityPercentage1);
            connectionContent = connectionContent.JsonChangePropertyValue(connection == ConstantValues.InactiveConnection ? ConstantValues.IsActive : ConstantValues.IsTransfer, "false");
            connectionContent = connection == ConstantValues.WithoutTransferpoint ? connectionContent.JsonChangePropertyValue(ConstantValues.AlgorithmId) : connectionContent;
            connectionContent = numberOfProducts.Equals(2) ? connectionContent.JsonChangePropertyValue(ConstantValues.ManageConnectionProduct2UncertainityPercentage, uncertainityPercentage2) : connectionContent;
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.ManageConnection]), JObject.Parse(connectionContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public async Task SameSegmentContractsAsync()
        {
            var temp = 0;
            var uploadFileName = @"TestData\Input\PurchaseAndSales\ContractsforMovements.xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            ////updating the worksheet
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                temp = new Faker().Random.Number(1000000, 9999999);
                worksheet.Cells[i, 1].Value = temp;
            }

#pragma warning disable CA1305 // Specify IFormatProvider
            this.SetValue<string>(ConstantValues.Contract, temp.ToString());
#pragma warning restore CA1305 // Specify IFormatProvider
            package.Save();
            package.Dispose();

            // Delete Homologation between 4 to 1
            try
            {
                var homologationRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForContracts]).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                var homologationGroup = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                foreach (var homologationGroupRow in homologationGroup)
                {
                    var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                    this.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                    await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = this.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                }

                await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }

            // Create Homologation between 4 to 1 For Nodes
            var setupHomologationRequestForNodes = ApiContent.Creates[ConstantValues.HomologationForContractNodes];
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(this.GetValue("NodeId_1"), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(this.GetValue("NodeId_2"), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node3 destinationValue", int.Parse(this.GetValue("NodeId_3"), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node4 destinationValue", int.Parse(this.GetValue("NodeId_4"), CultureInfo.InvariantCulture));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForNodes)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 4 to 1 For Product
            var setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForContractProduct];
            setupHomologationRequestForProducts = setupHomologationRequestForProducts.JsonChangePropertyValue("sourceSystemId", 4);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProducts)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 4 to 1 For Owners
            var setupHomologationRequestForOwner = ApiContent.Creates[ConstantValues.HomologationForOwner];
            setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("sourceSystemId", 4);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOwner)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 4 to 1 For Unit
            var setupHomologationRequestForUnit = ApiContent.Creates[ConstantValues.HomologationForContractUnit];
            setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("sourceSystemId", 4);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForUnit)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 4 to 1 For MovementTypeId
            var setupHomologationRequestForEventType = ApiContent.Creates[ConstantValues.HomologationForContractMovementTypeId];
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForEventType)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public async Task PerformHomologationAsync(string field)
        {
            var filename = Guid.NewGuid().ToString();
            var uniqueFileName = $@"{filename}.xml";
            if (string.IsNullOrEmpty(this.GetValue(ConstantValues.DestinationProductIsNotSet)) && this.GetValue(ConstantValues.InventoryWithMultipleProducts) != "Yes")
            {
                await uniqueFileName.UploadXmlAsync(ApiContent.InputFolder[this.GetValue(Keys.EntityType)], this.GetValue(Keys.EntityType) + "\\" + field).ConfigureAwait(false);
            }
            else if (this.GetValue(ConstantValues.InventoryWithMultipleProducts) == "Yes")
            {
                await uniqueFileName.UploadXmlAsync(ApiContent.InputFolder[this.GetValue(Keys.EntityType)], this.GetValue(Keys.EntityType) + "\\" + ConstantValues.InventoryWithMultipleProducts).ConfigureAwait(false);
            }
            else
            {
                await uniqueFileName.UploadXmlAsync(ApiContent.InputFolder[this.GetValue(Keys.EntityType)], this.GetValue(Keys.EntityType) + "\\" + ConstantValues.MovementsWithoutDestination).ConfigureAwait(false);
            }

            await Task.Delay(35000).ConfigureAwait(false);
            try
            {
                string mqMessageId = filename.GetMessageId(ApiContent.OutFolder[this.GetValue(Keys.EntityType)]).ToLower(CultureInfo.CurrentCulture);
                this.ScenarioContext[ConstantValues.MessageId] = mqMessageId;
                await Task.Delay(TimeSpan.FromMilliseconds(BaseConfiguration.LongTimeout)).ConfigureAwait(false);
                var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastFileRegistrationId).ConfigureAwait(false);
                var fileRegistrationId = lastRow["FileRegistrationId"];
                if (this.GetValue(Keys.EntityType) == "Inventory")
                {
                    this.SetValue(ApiContent.Ids[this.GetValue(Keys.EntityType)], fileRegistrationId);
                }
                else
                {
                    this.SetValue("Movement" + ApiContent.Ids[this.GetValue(Keys.EntityType)], fileRegistrationId);
                }

                var inventoryJsonContainer = this.InventoryJsonContanierName(this.GetValue(ConstantValues.BATCHID + "_1"));
                var folderPath = this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements") ? ApiContent.OutputFolder[this.GetValue(Keys.EntityType)] + "/" + mqMessageId.Replace("+", "==") + "/" + this.ScenarioContext["MOVEMENTID"] : ApiContent.OutputFolder[this.GetValue(Keys.EntityType)] + "/" + mqMessageId.Replace("+", "==") + "/" + inventoryJsonContainer;
                var fileName = this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements") ? this.ScenarioContext["MOVEMENTID"] + "_" + fileRegistrationId + "_" + "1" : inventoryJsonContainer + "_" + fileRegistrationId + "_" + "1";
                await fileName.DownloadBlobDataAsync(folderPath).ConfigureAwait(false);
                if (this.GetValue(ConstantValues.InventoryWithMultipleProducts) == "Yes" && string.IsNullOrEmpty(this.GetValue(ConstantValues.InventoryWithSameBatchIdentifier)))
                {
                    inventoryJsonContainer = this.InventoryJsonContanierName(this.GetValue(ConstantValues.BATCHID + "_2"));
                    folderPath = this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements") ? ApiContent.OutputFolder[this.GetValue(Keys.EntityType)] + "/" + mqMessageId.Replace("+", "==") + "/" + this.ScenarioContext["MOVEMENTID"] : ApiContent.OutputFolder[this.GetValue(Keys.EntityType)] + "/" + mqMessageId.Replace("+", "==") + "/" + inventoryJsonContainer;
                    fileName = this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements") ? this.ScenarioContext["MOVEMENTID"] + "_" + fileRegistrationId + "_" + "1" : inventoryJsonContainer + "_" + fileRegistrationId + "_" + "1";
                    await fileName.DownloadBlobDataAsync(folderPath).ConfigureAwait(false);
                }
            }
            catch (NullReferenceException)
            {
                Logger.Info("Uploaded xml is true-test container is not processed");
                Logger.Info("Inserting record manually through service bus");
                await this.GivenIHaveProcessedSinoperQueueInTheSystemAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
            }
            catch (StorageException)
            {
                Logger.Info("Blob does not exists");
                Logger.Info("Inserting record manually through service bus");
                await this.GivenIHaveProcessedSinoperQueueInTheSystemAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
            }
        }

        public async Task GivenIAmAuthenticatedForServiceAsync(string service)
        {
            await this.TokenForServiceAsync(this.ApiExecutor, service).ConfigureAwait(false);
        }

        public async Task CreateNodesForOfficialBalanceFileAsync()
        {
            // Create Nodes
            await this.CreateNodesWithOwnershipForOfficialBalanceFileAsync().ConfigureAwait(false);
            this.SetValue("NodeId_1", this.GetValue("NodeId"));
            this.SetValue("NodeName_1", this.GetValue("NodeName"));
            this.SetValue("CategorySegment", this.GetValue("SegmentName"));
            this.ScenarioContext[ConstantValues.Segment] = this.GetValue("CategorySegment");
            if (string.IsNullOrEmpty(this.GetValue(ConstantValues.NoSONSegment)))
            {
                await this.ReadSqlAsDictionaryAsync(ApiContent.UpdateRow["UpdateSegmentToSONSegment"], args: new { elementId = this.ScenarioContext["SegmentId"].ToString() }).ConfigureAwait(false);
            }

            await this.CreateOwnershipRulesForOfficialBalanceFileAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipForOfficialBalanceFileAsync().ConfigureAwait(false);
            this.SetValue("NodeId_2", this.GetValue("NodeId"));
            this.SetValue("NodeName_2", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesForOfficialBalanceFileAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipForOfficialBalanceFileAsync().ConfigureAwait(false);
            this.SetValue("NodeId_3", this.GetValue("NodeId"));
            this.SetValue("NodeName_3", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesForOfficialBalanceFileAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipForOfficialBalanceFileAsync().ConfigureAwait(false);
            this.SetValue("NodeId_4", this.GetValue("NodeId"));
            this.SetValue("NodeName_4", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesForOfficialBalanceFileAsync(this.GetValue("NodeId")).ConfigureAwait(false);
            await this.CreateNodesWithOwnershipForOfficialBalanceFileAsync().ConfigureAwait(false);
            this.SetValue("NodeId_5", this.GetValue("NodeId"));
            this.SetValue("NodeName_5", this.GetValue("NodeName"));
            await this.CreateOwnershipRulesForOfficialBalanceFileAsync(this.GetValue("NodeId")).ConfigureAwait(false);
        }

        public async Task CreateNodesWithOwnershipForOfficialBalanceFileAsync()
        {
            string nodeContent;
            if (string.IsNullOrEmpty(this.GetValue("NodeId_1")))
            {
                this.SetValue(ConstantValues.NodeWithTwoStorageLocations, "Yes");
                nodeContent = ApiContent.Creates[ConstantValues.CreateNodeWithMutipleProductsForOfficialBalanceFile];
            }
            else
            {
                nodeContent = ApiContent.Creates[ConstantValues.CreateNodeWithOneProductForOfficialBalanceFile];
            }

            nodeContent = nodeContent.JsonChangeValue();
            this.SetValue("NodeName", nodeContent.JsonGetValue("name"));
            nodeContent = await this.NodeContentCreationWithOwnershipAsync(nodeContent).ConfigureAwait(false);

            if (string.IsNullOrEmpty(this.GetValue("NodeId_1")) || string.IsNullOrEmpty(this.GetValue("NodeId_2")))
            {
                await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            }
            else if (string.IsNullOrEmpty(this.GetValue("NodeId_3")))
            {
                nodeContent = nodeContent.JsonChangePropertyValue("logisticCenterId", "1088");
                nodeContent = nodeContent.JsonChangePropertyValue("StorageLocation storageLocationId", "1088:M001");
                nodeContent = nodeContent.JsonChangePropertyValue("ProductLocation productId", "40000003009");
                await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            }
            else if (string.IsNullOrEmpty(this.GetValue("NodeId_4")))
            {
                nodeContent = nodeContent.JsonChangePropertyValue("logisticCenterId", "1059");
                nodeContent = nodeContent.JsonChangePropertyValue("StorageLocation storageLocationId", "1059:C001");
                nodeContent = nodeContent.JsonChangePropertyValue("ProductLocation productId", "40000003009");
                await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            }
            else if (string.IsNullOrEmpty(this.GetValue("NodeId_5")))
            {
                nodeContent = nodeContent.JsonChangePropertyValue("logisticCenterId", "1088");
                nodeContent = nodeContent.JsonChangePropertyValue("StorageLocation storageLocationId", "1088:C001");
                nodeContent = nodeContent.JsonChangePropertyValue("ProductLocation productId", "40000003009");
                await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            }

            this.SetValue("NodeId", await this.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false));
        }

        public async Task CreateOwnershipRulesForOfficialBalanceFileAsync(string nodeId)
        {
            await this.UpdateNodeProductOwnersOfficialBalanceFileAsync(nodeId).ConfigureAwait(false);
            var ownerContent = ApiContent.Creates[ConstantValues.CreateOwnership];
            var storageLocationOwnerRow = await this.ReadAllSqlAsync(input: SqlQueries.GetStorageLocationOwner, args: new { nodeId }).ConfigureAwait(false);
            var storageLocation = storageLocationOwnerRow.ToDictionaryList();
            for (int i = 0; i < storageLocation.Count; i++)
            {
                ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRuleType, "StorageLocationProduct");
                ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesId, storageLocation[i][ConstantValues.StorageLocationProductId].ToString());
                ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.OwnershipRulesRowVersion, storageLocation[i][ConstantValues.RowVersion].ToString());
                await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateOwnershipRule]), JObject.Parse(ownerContent)).ConfigureAwait(false);
            }
        }

        public async Task UpdateNodeProductOwnersOfficialBalanceFileAsync(string nodeId)
        {
            var ownerContent = ApiContent.Creates[ConstantValues.UpdateNodeProductOwnersForOfficialBalanceFile];
            var storageLocationOwnerRow = await this.ReadAllSqlAsync(input: SqlQueries.GetStorageLocationOwner, args: new { nodeId }).ConfigureAwait(false);
            var storageLocation = storageLocationOwnerRow.ToDictionaryList();
            for (int i = 0; i < storageLocation.Count; i++)
            {
                ownerContent = ownerContent.JsonChangePropertyValue("productId", storageLocation[i][ConstantValues.StorageLocationProductId]);
                ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.RowVersion, storageLocation[i][ConstantValues.RowVersion].ToString());
                await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateNodeProductOwners]), JObject.Parse(ownerContent)).ConfigureAwait(false);
            }
        }

        public async Task TheTRUESystemIsProcessingTheOperativeMovementsAsync()
        {
            var officialDeltaDetails = await this.ReadAllSqlAsync(SqlQueries.GetProcessingOfficialDeltaTickets, args: new { segmentName = this.GetValue("SegmentName") }).ConfigureAwait(false);
            var numberOfOfficialDeltaProcessingTickets = officialDeltaDetails.ToDictionaryList();
            if (numberOfOfficialDeltaProcessingTickets.Count > 0)
            {
                for (int i = 0; i < numberOfOfficialDeltaProcessingTickets.Count; i++)
                {
                    await this.ReadAllSqlAsync(SqlQueries.UpdateOfficialDeltaTicketsToFailed, args: new { ticketId = numberOfOfficialDeltaProcessingTickets[i]["TicketId"] }).ConfigureAwait(false);
                }
            }

            this.SetValue("DeltaCategorySegment", this.GetValue("SegmentName"));
            ////When I navigate to "Calculation of deltas by official adjust" page
            this.UiNavigation("Calculation of deltas by official adjustment");
            ////And I click on "newDeltasCalculation" "button"
            this.IClickOn("newDeltasCalculation", "button");
            ////And I select delta segment from "initOfficialDeltaTicket" "segment" "dropdown"
            this.ISelectOfficialDeltaSegmentFrom("initOfficialDeltaTicket\" \"segment", "dropdown");
            ////And I select "2020" year from drop down
            this.ISelectYearFromDropDown(this.GetValue("YearForPeriod"));
            ////And I select "Jun" period from drop down
            this.ISelectAPeriodFromDropDown(this.GetValue("PreviousMonthName").ToPascalCase());
            ////And I click on "initOfficialDeltaTicket" "submit" "button"
            this.IClickOn("initOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "validateOfficialDeltaTicket" "submit" "button"
            this.IClickOn("validateOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "confirmOfficialDeltaTicket" "submit" "button"
            this.IClickOn("confirmOfficialDeltaTicket\" \"submit", "button");
            //// wait till ticket processing complete
            await this.VerifyThatOfficialDeltaCalculationShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        protected async Task<string> CreateInactiveSourceAndDestinationNodesAsync(string content)
        {
            var temp = this.UserDetails;
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
                temp = this.UserDetails;
            }

            var nodeContent = ApiContent.Creates[ConstantValues.Nodes];
            nodeContent = nodeContent.JsonChangeValue();
            this.ScenarioContext["SourceNodeName"] = nodeContent.JsonGetValue("name");
            nodeContent = await this.NodeContentCreationAsync(nodeContent).ConfigureAwait(false);
            nodeContent = nodeContent.JsonChangePropertyValue("segmentId", 10);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.Nodes]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[ConstantValues.Nodes].ToPascalCase()];
            var updateNode = "{ }";
            updateNode = updateNode.JsonAddField("name", lastCreatedRow["Name"]);
            updateNode = updateNode.JsonAddField("nodeId", fieldValue);
            updateNode = updateNode.JsonAddField("sendToSap", "false");
            updateNode = updateNode.JsonAddField("isActive", "false");
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(updateNode)).ConfigureAwait(false);

            this.SetValue(Keys.SelectedValue, fieldValue);
            content = content.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.SourceNode], fieldValue);
            nodeContent = nodeContent.JsonChangeValue();
            this.ScenarioContext["DestinationNodeName"] = nodeContent.JsonGetValue("name");
            nodeContent = nodeContent.JsonChangePropertyValue("segmentId", 10);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.Nodes]).ConfigureAwait(false);
            fieldValue = lastCreatedRow[ApiContent.Ids[ConstantValues.Nodes].ToPascalCase()];
            content = content.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.DestinationNode], fieldValue);
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"" + temp.Role + "\"");
                await this.GivenIAmAuthenticatedForUserAsync(temp.Role).ConfigureAwait(false);
            }

            return content;
        }

        protected async Task<string> NodeContentCreationAsync(string content)
        {
            var temp = this.UserDetails;
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            }

            // Range of order between 1 and 2147483647 (int.MaxValue())
            content = content.JsonChangePropertyValue("order", new Faker().Random.Number(1, 2147483647));
            await this.CreateCatergoryElementAsync("2").ConfigureAwait(false);
            this.ScenarioContext["SegmentId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            var categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            this.ScenarioContext["SegmentName"] = categoryElementRow[ConstantValues.Name];
            content = content.JsonChangePropertyValue("segmentId", this.ScenarioContext["SegmentId"].ToString());
            await this.CreateCatergoryElementAsync("3").ConfigureAwait(false);
            this.ScenarioContext["OperatorId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["OperatorId"] }).ConfigureAwait(false);
            this.ScenarioContext["OperatorName"] = categoryElementRow[ConstantValues.Name];
            content = content.JsonChangePropertyValue("operatorId", this.ScenarioContext["OperatorId"].ToString());
            await this.CreateCatergoryElementAsync("1").ConfigureAwait(false);
            this.ScenarioContext["NodeTypeId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["NodeTypeId"] }).ConfigureAwait(false);
            this.ScenarioContext["NodeTypeName"] = categoryElementRow[ConstantValues.Name];
            content = content.JsonChangePropertyValue("nodeTypeId", this.ScenarioContext["NodeTypeId"].ToString());
            await this.CreateCatergoryElementAsync("4").ConfigureAwait(false);
            this.ScenarioContext["StorageLocationTypeId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            content = content.JsonChangePropertyValue("StorageLocation StorageLocationTypeId", this.ScenarioContext["StorageLocationTypeId"].ToString());
            await this.CreateCatergoryElementAsync("10").ConfigureAwait(false);
            this.ScenarioContext["OwnerShipRuleId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["OwnerShipRuleId"] }).ConfigureAwait(false);
            this.ScenarioContext["OwnerShipRuleName"] = categoryElementRow[ConstantValues.Name];
            content = content.JsonChangePropertyValue("ownershipRule", this.ScenarioContext["OwnerShipRuleId"].ToString());
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"" + temp.Role + "\"");
                await this.GivenIAmAuthenticatedForUserAsync(temp.Role).ConfigureAwait(false);
            }

            return content;
        }

        protected async Task<string> NodeContentCreationWithSameSegementAsync(string content)
        {
            var temp = this.UserDetails;
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            }

            IDictionary<string, string> categoryElementRow = null;
            var key = int.TryParse(this.ScenarioContext["Count"].ToString(), out int result);
            if (key && result == 0)
            {
                await this.CreateCatergoryElementAsync("2").ConfigureAwait(false);
                this.ScenarioContext["SegmentId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
                categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
                this.ScenarioContext["SegmentName"] = categoryElementRow[ConstantValues.Name];
                content = content.JsonChangePropertyValue("segmentId", this.ScenarioContext["SegmentId"].ToString());
                this.ScenarioContext["Count"] = 1;
            }

            content = content.JsonChangePropertyValue("segmentId", this.ScenarioContext["SegmentId"].ToString());
            await this.CreateCatergoryElementAsync("3").ConfigureAwait(false);
            this.ScenarioContext["OperatorId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["OperatorId"] }).ConfigureAwait(false);
            this.ScenarioContext["OperatorName"] = categoryElementRow[ConstantValues.Name];
            content = content.JsonChangePropertyValue("operatorId", this.ScenarioContext["OperatorId"].ToString());
            await this.CreateCatergoryElementAsync("1").ConfigureAwait(false);
            this.ScenarioContext["NodeTypeId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["NodeTypeId"] }).ConfigureAwait(false);
            this.ScenarioContext["NodeTypeName"] = categoryElementRow[ConstantValues.Name];
            content = content.JsonChangePropertyValue("nodeTypeId", this.ScenarioContext["NodeTypeId"].ToString());
            await this.CreateCatergoryElementAsync("4").ConfigureAwait(false);
            this.ScenarioContext["StorageLocationTypeId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            content = content.JsonChangePropertyValue("StorageLocation StorageLocationTypeId", this.ScenarioContext["StorageLocationTypeId"].ToString());
            ////await this.CreateCatergoryElementAsync("10").ConfigureAwait(false);
            ////this.ScenarioContext["OwnerShipRuleId"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            ////categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["OwnerShipRuleId"] }).ConfigureAwait(false);
            ////this.ScenarioContext["OwnerShipRuleName"] = categoryElementRow[ConstantValues.Name];
            ////content = content.JsonChangePropertyValue("ownershipRule", this.ScenarioContext["OwnerShipRuleId"].ToString());
            ////if (temp.Role != "admin" && temp.Role != "administrador")
            ////{
            ////this.Given("I am authenticated as \"" + temp.Role + "\"");
            await this.GivenIAmAuthenticatedForUserAsync(temp.Role).ConfigureAwait(false);
            ////}

            return content;
        }

        protected async Task<string> UpdateRequestJsonIdsAsync(string updateContent, string entity)
        {
            var lastCreatedRow = entity.EqualsIgnoreCase("UpdateNodeConnectionProductOwners") ? null : await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue;
            if (entity.EqualsIgnoreCase(ConstantValues.ManageConnection))
            {
                fieldValue = lastCreatedRow[ConstantValues.SourceNode];
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.SourceNode, fieldValue);
                fieldValue = lastCreatedRow[ConstantValues.DestinationNode];
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.DestinationNode, fieldValue);
                ////updateContent = updateContent.JsonChangePropertyValue("Product_1-Owner_1 ownerId", this.ScenarioContext["Product_1-OwnerId_1"].ToString());
                ////updateContent = updateContent.JsonChangePropertyValue("Product_1-Owner_2 ownerId", this.ScenarioContext["Product_1-OwnerId_2"].ToString());
            }
            else if (entity.EqualsIgnoreCase("UpdateNodeConnectionProductOwners"))
            {
                updateContent = updateContent.JArrayChangePropertyValue("Owner_1 ownerId", this.ScenarioContext["Product_1-OwnerId_1"].ToString());
                updateContent = updateContent.JArrayChangePropertyValue("Owner_2 ownerId", this.ScenarioContext["Product_1-OwnerId_2"].ToString());
            }
            else
            {
                this.ScenarioContext["EntityId"] = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
                updateContent = updateContent.JsonChangePropertyValue(ApiContent.Ids[entity], this.ScenarioContext["EntityId"].ToString());
            }

            return updateContent;
        }

        protected string UpdateProductOwners(string updateContent, string product)
        {
            updateContent = updateContent.JsonChangePropertyValue(product + "-Owner_1 ownerId", this.ScenarioContext[product + "-OwnerId_1"].ToString());
            updateContent = updateContent.JsonChangePropertyValue(product + "-Owner_2 ownerId", this.ScenarioContext[product + "-OwnerId_2"].ToString());
            return updateContent;
        }

        protected async Task<string> UpdateContentForNodeAsync(string updateContent)
        {
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.StorageLocationNodeId, this.ScenarioContext["EntityId"].ToString());
            var lastCreatedStorageLocation = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.StorageLocations], args: new { nodeId = this.ScenarioContext["EntityId"] }).ConfigureAwait(false);
            string nodeStorageLocationId = lastCreatedStorageLocation["NodeStorageLocationId"];
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.NodeStorageLocationId, nodeStorageLocationId);
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.ProductLocationNodeStorageLocationId, nodeStorageLocationId);
            updateContent = updateContent.JsonChangePropertyValue("segmentId", this.ScenarioContext["SegmentId"].ToString());
            updateContent = updateContent.JsonChangePropertyValue("operatorId", this.ScenarioContext["OperatorId"].ToString());
            updateContent = updateContent.JsonChangePropertyValue("nodeTypeId", this.ScenarioContext["NodeTypeId"].ToString());
            updateContent = updateContent.JsonChangePropertyValue("StorageLocation StorageLocationTypeId", this.ScenarioContext["StorageLocationTypeId"].ToString());
            return updateContent;
        }

        protected async Task CreateNodeConnectionContentForOperationalCutOffSummaryAsync()
        {
            var temp = this.UserDetails;
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            }

            var connectionContent = ApiContent.Creates[ConstantValues.ManageConnection];
            var nodeContent = ApiContent.Creates[ConstantValues.Nodes];
            nodeContent = nodeContent.JsonChangeValue();
            this.ScenarioContext["SourceNodeName"] = nodeContent.JsonGetValue("name");
            nodeContent = await this.NodeContentCreationAsync(nodeContent).ConfigureAwait(false);
            this.ScenarioContext["SourceSegmentName"] = this.ScenarioContext["SegmentName"];
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            this.ScenarioContext["SourceNodeId"] = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.Nodes]).ConfigureAwait(false);
            connectionContent = connectionContent.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.SourceNode], this.ScenarioContext["SourceNodeId"].ToString());
            nodeContent = ApiContent.Creates[ConstantValues.Nodes];
            nodeContent = nodeContent.JsonChangeValue();
            this.ScenarioContext["DestinationNodeName"] = nodeContent.JsonGetValue("name");
            nodeContent = await this.NodeContentCreationAsync(nodeContent).ConfigureAwait(false);
            this.ScenarioContext["DestinationSegmentName"] = this.ScenarioContext["SegmentName"];
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            this.ScenarioContext["DestinationNodeId"] = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.Nodes]).ConfigureAwait(false);
            connectionContent = connectionContent.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.DestinationNode], this.ScenarioContext["DestinationNodeId"].ToString());
            connectionContent = await this.CreateOwnersAsync(connectionContent).ConfigureAwait(false);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.ManageConnection]), JObject.Parse(connectionContent)).ConfigureAwait(false);
            connectionContent = connectionContent.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.DestinationNode], this.ScenarioContext["SourceNodeId"].ToString());
            connectionContent = connectionContent.JsonChangePropertyValue(ApiContent.Ids[ConstantValues.SourceNode], this.ScenarioContext["DestinationNodeId"].ToString());
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.ManageConnection]), JObject.Parse(connectionContent)).ConfigureAwait(false);
            await this.ReadAllSqlAsync(input: ApiContent.UpdateRow["UpdateNodeTagDate"], args: new { date = 4, nodeId = this.ScenarioContext["SourceNodeId"] }).ConfigureAwait(false);
            if (temp.Role != "admin" && temp.Role != "administrador")
            {
                ////this.Given("I am authenticated as \"" + temp.Role + "\"");
                await this.GivenIAmAuthenticatedForUserAsync(temp.Role).ConfigureAwait(false);
            }
        }

        protected async Task DeleteAllHomologationsAsync()
        {
            try
            {
                await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.DeleteAllHomologationData).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        protected async Task<string> CreateNodesWithProductAsync(string content)
        {
            var nodeContent = ApiContent.Creates[ConstantValues.NodeWithMutipleProducts];
            nodeContent = nodeContent.JsonChangeValue();
            nodeContent = await this.NodeContentCreationWithSameSegementAsync(nodeContent).ConfigureAwait(false);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            this.SetValue("NodeId", await this.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false));
            return content;
        }

        protected async Task CreateNodesWithSendToSAPAsync()
        {
            var nodeContent = ApiContent.Creates[ConstantValues.NodeWithSendToSAPTrue];
            nodeContent = nodeContent.JsonChangeValue();
            nodeContent = await this.NodeContentCreationWithSameSegementAsync(nodeContent).ConfigureAwait(false);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Nodes]), JObject.Parse(nodeContent)).ConfigureAwait(false);
            this.SetValue("NodeId", await this.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false));
        }

        protected async Task UpdateNodeOwnersAsync(string nodeId)
        {
            var ownerContent = ApiContent.Creates[ConstantValues.UpdateNodeOwners];
            var storageLocationOwnerRow = await this.ReadAllSqlAsync(input: SqlQueries.GetStorageLocationOwner, args: new { nodeId = nodeId }).ConfigureAwait(false);
            var storageLocation = storageLocationOwnerRow.ToDictionaryList();
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.StorageLocationProductId, storageLocation[0][ConstantValues.StorageLocationProductId].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue("NodeStorageLocationId", storageLocation[0]["NodeStorageLocationId"].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.NodeUpdateProductId, storageLocation[0][ConstantValues.ProductId].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.RowVersion, storageLocation[0][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateNodeProducts]), JObject.Parse(ownerContent)).ConfigureAwait(false);
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.StorageLocationProductId, storageLocation[1][ConstantValues.StorageLocationProductId].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue("NodeStorageLocationId", storageLocation[1]["NodeStorageLocationId"].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.NodeUpdateProductId, storageLocation[1][ConstantValues.ProductId].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.RowVersion, storageLocation[1][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateNodeProducts]), JObject.Parse(ownerContent)).ConfigureAwait(false);
        }

        protected async Task UpdateNodeProductOwnersAsync(string nodeId)
        {
            var ownerContent = ApiContent.Creates[ConstantValues.UpdateNodeProductOwners];
            var storageLocationOwnerRow = await this.ReadAllSqlAsync(input: SqlQueries.GetStorageLocationOwner, args: new { nodeId }).ConfigureAwait(false);
            var storageLocation = storageLocationOwnerRow.ToDictionaryList();
            ownerContent = ownerContent.JsonChangePropertyValue("productId", storageLocation[0][ConstantValues.StorageLocationProductId]);
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.RowVersion, storageLocation[0][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateNodeProductOwners]), JObject.Parse(ownerContent)).ConfigureAwait(false);
            ownerContent = ownerContent.JsonChangePropertyValue("productId", storageLocation[1][ConstantValues.StorageLocationProductId]);
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.RowVersion, storageLocation[1][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateNodeProductOwners]), JObject.Parse(ownerContent)).ConfigureAwait(false);
        }

        protected async Task UpdateNodeProductOwnersAnalyticalModelAsync(string nodeId)
        {
            var ownerContent = ApiContent.Creates[ConstantValues.UpdateNodeProductOwners];
            var storageLocationOwnerRow = await this.ReadAllSqlAsync(input: SqlQueries.GetStorageLocationOwner, args: new { nodeId }).ConfigureAwait(false);
            var storageLocation = storageLocationOwnerRow.ToDictionaryList();
            ownerContent = ownerContent.JsonChangePropertyValue("productId", storageLocation[0][ConstantValues.StorageLocationProductId]);
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.RowVersion, storageLocation[0][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateNodeProductOwners]), JObject.Parse(ownerContent)).ConfigureAwait(false);
        }

        protected async Task CreateOwnershipRulesForSIVAsync(string nodeId)
        {
            var ownerContent = ApiContent.Creates[ConstantValues.UpdateNodeOwners];
            var storageLocationOwnerRow = await this.ReadAllSqlAsync(input: SqlQueries.GetStorageLocationOwner, args: new { nodeId = nodeId }).ConfigureAwait(false);
            var storageLocation = storageLocationOwnerRow.ToDictionaryList();
            var ownershipRule = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastOwnershipRule).ConfigureAwait(false);
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.StorageLocationProductId, storageLocation[0][ConstantValues.StorageLocationProductId].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue("Owner productId", storageLocation[0]["ProductId"].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue("nodeStorageLocationId", storageLocation[0]["NodeStorageLocationId"].ToString());
            ownerContent = ownerContent.JsonChangePropertyValue("OwnershipRuleId", ownershipRule["ElementId"]);
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.RowVersion, storageLocation[0][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateNodeProducts]), JObject.Parse(ownerContent)).ConfigureAwait(false);
            ownerContent = ownerContent.JsonChangePropertyValue("Owner productId", "10000003001");
            storageLocationOwnerRow = await this.ReadAllSqlAsync(input: SqlQueries.GetStorageLocationOwner, args: new { nodeId = nodeId }).ConfigureAwait(false);
            storageLocation = storageLocationOwnerRow.ToDictionaryList();
            ownerContent = ownerContent.JsonChangePropertyValue(ConstantValues.RowVersion, storageLocation[0][ConstantValues.RowVersion].ToString());
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.UpdateNodeProducts]), JObject.Parse(ownerContent)).ConfigureAwait(false);
        }

        protected async Task UploadSinoperXmlAsync(string entity)
        {
            var filename = Guid.NewGuid().ToString();
            var uniqueFileName = $@"{filename}.xml";
            await uniqueFileName.UploadXmlAsync(ApiContent.InputFolder[this.GetValue(Keys.EntityType)], this.GetValue(Keys.EntityType) + "\\" + entity).ConfigureAwait(false);
            await Task.Delay(25000).ConfigureAwait(false);
            string mqMessageId = filename.GetMessageId(ApiContent.OutFolder[this.GetValue(Keys.EntityType)]).ToLower(CultureInfo.CurrentCulture);
            this.ScenarioContext[ConstantValues.MessageId] = mqMessageId;
            await Task.Delay(TimeSpan.FromMilliseconds(BaseConfiguration.LongTimeout)).ConfigureAwait(false);
        }

        protected async Task UpdateOwnersForNodeConnectionAsync(string nodeConnection, int productCount = 2)
        {
            var updateNodeConnectionProductOwners = ApiContent.Updates["UpdateNodeConnectionProductOwners"];
            var lastCreatedNodeConnectionProduct = await this.ReadAllSqlAsync(input: ApiContent.GetRow["NodeConnectionProductByNodeConnnectionID"], args: new { nodeConnectionId = nodeConnection }).ConfigureAwait(false);
            var lastCreatedNodeConnectionProductRow = lastCreatedNodeConnectionProduct.ToDictionaryList();
            var nodeConnectionProductId = lastCreatedNodeConnectionProductRow[0]["NodeConnectionProductId"];
            var rowVersion = lastCreatedNodeConnectionProductRow[0][ConstantValues.RowVersion];
            updateNodeConnectionProductOwners = updateNodeConnectionProductOwners.JsonChangePropertyValue("ConnectionUpdate ProductId", nodeConnectionProductId.ToString());
            updateNodeConnectionProductOwners = updateNodeConnectionProductOwners.JsonChangePropertyValue("RowVersion", rowVersion.ToString());
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.ManageConnection], "/products/owners"), JObject.Parse(updateNodeConnectionProductOwners)).ConfigureAwait(false)).ConfigureAwait(false);
            if (productCount == 2)
            {
                nodeConnectionProductId = lastCreatedNodeConnectionProductRow[1]["NodeConnectionProductId"];
                rowVersion = lastCreatedNodeConnectionProductRow[1][ConstantValues.RowVersion];
                updateNodeConnectionProductOwners = updateNodeConnectionProductOwners.JsonChangePropertyValue("ConnectionUpdate ProductId", nodeConnectionProductId.ToString());
                updateNodeConnectionProductOwners = updateNodeConnectionProductOwners.JsonChangePropertyValue("RowVersion", rowVersion.ToString());
                await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.ManageConnection], "/products/owners"), JObject.Parse(updateNodeConnectionProductOwners)).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        protected async Task UpdateOwnersForNodeConnectionForOfficialBalanceFileAsync(string nodeConnectionId)
        {
            var updateNodeConnectionProductOwners = ApiContent.Updates[ConstantValues.UpdateNodeConnectionProductOwnersForOfficialBalanceFile];
            var lastCreatedNodeConnectionProduct = await this.ReadAllSqlAsync(input: ApiContent.GetRow["NodeConnectionProductByNodeConnnectionID"], args: new { nodeConnectionId }).ConfigureAwait(false);
            var lastCreatedNodeConnectionProductRow = lastCreatedNodeConnectionProduct.ToDictionaryList();
            for (int i = 0; i < lastCreatedNodeConnectionProductRow.Count; i++)
            {
                var nodeConnectionProductId = lastCreatedNodeConnectionProductRow[i]["NodeConnectionProductId"];
                var rowVersion = lastCreatedNodeConnectionProductRow[i][ConstantValues.RowVersion];
                updateNodeConnectionProductOwners = updateNodeConnectionProductOwners.JsonChangePropertyValue("ConnectionUpdate ProductId", nodeConnectionProductId.ToString());
                updateNodeConnectionProductOwners = updateNodeConnectionProductOwners.JsonChangePropertyValue("RowVersion", rowVersion.ToString());
                await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.ManageConnection], "/products/owners"), JObject.Parse(updateNodeConnectionProductOwners)).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        protected string InventoryJsonContanierName(string batchId)
        {
            this.SetValue(ConstantValues.InventoryId, this.GetValue("DATE").Replace("-", string.Empty));
            string inventoryId = this.GetValue(ConstantValues.InventoryId) != null ? this.GetValue(ConstantValues.InventoryId) : "NA";
            string nodeId = "AYACUCHO";
            string productId = "CRUDOS IMPORTADOS";
            string inventoryDate = this.GetValue("DATE") != null ? this.GetValue("DATE").ToDateTime().ToString("MM/dd/yyyy 00:00:00", CultureInfo.InvariantCulture) : "NA";
            batchId = batchId != null ? batchId : "NA";
            string tankName = this.GetValue(ConstantValues.TankName) != null ? this.GetValue(ConstantValues.TankName) : "NA";

            string plainTextId = string.Join("_", new[] { inventoryId, nodeId, productId, inventoryDate, batchId, tankName });
            return GenerateEncodedString(plainTextId);
        }

        protected void SapAcknowledgementMovementRegistration()
        {
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();

            if (this.GetValue("SapPoAcknowledgement").EqualsIgnoreCase("UPDATE"))
            {
                content = content.JArrayModifyPropertyValue("Array_1 eventType", "UPDATE");
                content = content.JArrayModifyPropertyValue("Array_2 eventType", "UPDATE");
            }
            else
            {
                content = content.JArrayModifyPropertyValue("Array_1 eventType", "DELETE");
                content = content.JArrayModifyPropertyValue("Array_2 eventType", "DELETE");
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = content;
        }

        protected void CommonMethodForMovementRegistration(int movementsCount = 1, int lengthOfField = 1, string attribute = null)
        {
            JArray movementArray = new JArray();
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            for (int i = 0; i < movementsCount; i++)
            {
                switch (this.GetValue(ConstantValues.TestData))
                {
                    case "AttributesOfBackUpMovement":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        break;
                    case "OptionalAttributes":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        if (attribute == "sourceProductTypeId")
                        {
                            content = content.JsonRemoveObject("MovementSource " + attribute);
                        }
                        else if (attribute == "destinationProductId" || attribute == "destinationProductTypeid")
                        {
                            content = content.JsonRemoveObject("MovementDestination " + attribute);
                        }
                        else if (attribute == "batchId")
                        {
                            content = content.JsonRemoveObject("Movement " + attribute);
                        }
                        else
                        {
                            content = content.JsonRemoveObject(attribute);
                        }

                        break;
                    case "WithScenarioId":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        if (string.IsNullOrEmpty(this.GetValue("TransferPointNodes")))
                        {
                            content = content.JsonChangePropertyValue("scenarioId", 3);
                        }
                        else if (this.GetValue("TransferPointNodes").EqualsIgnoreCase("True"))
                        {
                            content = content.JsonChangePropertyValue("scenarioId", 1);
                            this.SetValue("TransferPointNodes", "false");
                        }
                        else if (!this.GetValue("TransferPointNodes").EqualsIgnoreCase("True"))
                        {
                            content = content.JsonChangePropertyValue("scenarioId", 2);
                            this.SetValue("TransferPointNodes", "True");
                            this.SetValue("OfficialMovement", "True");
                        }

                        content = content.JsonChangePropertyValue("Movement batchId", this.GetValue(ConstantValues.BATCHID));
                        break;
                    case "MovementWithSourceAndDestination":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        break;
                    case "MovementWithoutSourceAndDestination":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonRemoveObject("movementSource");
                        content = content.JsonRemoveObject("movementDestination");
                        break;
                    case "MovementWithSource":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonRemoveObject("movementDestination");
                        break;
                    case "MovementWithDestination":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonRemoveObject("movementSource");
                        break;
                    case "WithOfficialInformation":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("OfficialInformation backupMovementId", new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("OfficialInformation globalMovementId", new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("OfficialInformation isOfficial", true);
                        break;
                    case "WithoutOfficialInformation":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonRemoveObject("officialInformation");
                        break;
                    case "MovementWithOutOptionalAttributes":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonRemoveObject("Movement batchId");
                        content = content.JsonRemoveObject("system");
                        content = content.JsonRemoveObject("grossStandardQuantity");
                        content = content.JsonRemoveObject("uncertainty");
                        content = content.JsonRemoveObject("operatorId");
                        content = content.JsonRemoveObject("attributesOfSapPo");
                        content = content.JsonRemoveObject("ownersOfSapPo");
                        content = content.JsonRemoveObject("version");
                        content = content.JsonRemoveObject("observations");
                        content = content.JsonRemoveObject("officialInformation");
                        content = content.JsonRemoveObject("sapProcessStatus");
                        break;
                    case "MovementWithOutDestinationProductIdAndMovementSourceHasValue":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("MovementDestination destinationProductId", null);
                        break;
                    case "MovementWithOutDestinationProductIdAndMovementSourceHasNoValue":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        content = content.JsonChangePropertyValue("MovementDestination destinationProductId", null);
                        content = content.JsonRemoveObject("movementSource");
                        break;
                    case "MovementWithoutMandatoryFields":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        if (attribute == "sourceSystem" || attribute == "eventType" || attribute == "movementId" || attribute == "movementTypeId" || attribute == "operationalDate" || attribute == "period" || attribute == "netStandardQuantity" || attribute == "measurementUnit" || attribute == "segmentId" || attribute == "scenarioId" || attribute == "classification")
                        {
                            if (attribute == "movementId" || attribute == "operationalDate")
                            {
                                this.SetValue(ConstantValues.FieldToCheckMandatory, ConstantValues.Yes);
                            }
                            else
                            {
                                this.SetValue(ConstantValues.FieldToCheckMandatory, string.Empty);
                            }

                            content = content.JsonRemoveObject(attribute);
                        }
                        else if (attribute == "startTime" || attribute == "endTime")
                        {
                            this.SetValue(ConstantValues.FieldToCheckMandatory, ConstantValues.Yes);
                            content = content.JsonRemoveObject("Period " + attribute);
                        }
                        else if (attribute == "sourceProductId" || attribute == "sourceNodeId")
                        {
                            content = content.JsonRemoveObject("MovementSource " + attribute);
                        }
                        else if (attribute == "destinationNodeId")
                        {
                            content = content.JsonRemoveObject("MovementDestination " + attribute);
                        }
                        else if (attribute == "ownerId" || attribute == "ownershipValue" || attribute == "ownerShipValueUnit")
                        {
                            content = content.JsonRemoveObject("SAPMovementOwners " + attribute);
                        }
                        else if (attribute == "attributeId" || attribute == "attributeValue" || attribute == "valueAttributeUnit")
                        {
                            content = content.JsonRemoveObject("SAPMovementAttribute " + attribute);
                        }
                        else if (attribute == "isOfficial" || attribute == "globalMovementId")
                        {
                            content = content.JsonRemoveObject("OfficialInformation " + attribute);
                        }

                        break;
                    case "FieldWithMoreThanLengthThatAccepts":
                        this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                        if (attribute == "sourceSystem" || attribute == "eventType" || attribute == "movementId" || attribute == "batchId" || attribute == "movementTypeId" || attribute == "system" || attribute == "measurementUnit" || attribute == "segmentId" || attribute == "operatorId" || attribute == "version" || attribute == "observations" || attribute == "classification" || attribute == "sapProcessStatus")
                        {
                            if (attribute == "movementId")
                            {
                                this.SetValue(ConstantValues.MovementId, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                            }

                            if (attribute == "batchId")
                            {
                                content = content.JsonChangePropertyValue("Movement " + attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                content = content.JsonChangePropertyValue(attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                            }
                        }
                        else if (attribute == "sourceNodeId" || attribute == "sourceStorageLocationId" || attribute == "sourceProductId" || attribute == "sourceProductTypeId")
                        {
                            content = content.JsonChangePropertyValue("MovementSource " + attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                        }
                        else if (attribute == "destinationNodeId" || attribute == "destinationStorageLocationId" || attribute == "destinationProductId" || attribute == "destinationProductTypeid")
                        {
                            content = content.JsonChangePropertyValue("MovementDestination " + attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                        }
                        else if (attribute == "ownerId" || attribute == "ownerShipValueUnit")
                        {
                            content = content.JsonChangePropertyValue("SAPMovementOwners " + attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                        }
                        else if (attribute == "attributeId" || attribute == "attributeType" || attribute == "attributeValue" || attribute == "valueAttributeUnit" || attribute == "attributeDescription")
                        {
                            content = content.JsonChangePropertyValue("SAPMovementAttribute " + attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                        }
                        else if (attribute == "backupMovementId" || attribute == "globalMovementId")
                        {
                            content = content.JsonChangePropertyValue("OfficialInformation " + attribute, new Faker().Random.AlphaNumeric(lengthOfField + 1).ToString(CultureInfo.InvariantCulture));
                        }

                        break;
                    default:
                        break;
                }

                if (string.IsNullOrEmpty(this.GetValue(ConstantValues.FieldToCheckMandatory)))
                {
                    content = content.JsonChangePropertyValue("movementId", this.GetValue(ConstantValues.MovementId));
                    if (string.IsNullOrEmpty(this.GetValue("OfficialMovement")))
                    {
                        content = content.JsonChangePropertyValue(ConstantValues.OperationalDate, DateTime.Now.AddDays(-2).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                        if (content.ContainsIgnoreCase("period"))
                        {
                            content = content.JsonChangePropertyValue(ConstantValues.PeriodStartDate, DateTime.Now.AddDays(-4).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                            content = content.JsonChangePropertyValue(ConstantValues.PeriodEndDate, DateTime.Now.AddDays(-3).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                        }
                    }
                    else
                    {
                        DateTime date = DateTime.Now.AddMonths(-1);
                        var firstDayOfPreviousMonth = new DateTime(date.Year, date.Month, 1);
                        var lastDayOfPreviousMonth = firstDayOfPreviousMonth.AddMonths(1).AddDays(-1);
                        content = content.JsonChangePropertyValue(ConstantValues.OperationalDate, firstDayOfPreviousMonth.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                        if (content.ContainsIgnoreCase("period"))
                        {
                            content = content.JsonChangePropertyValue(ConstantValues.PeriodStartDate, firstDayOfPreviousMonth.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                            content = content.JsonChangePropertyValue(ConstantValues.PeriodEndDate, lastDayOfPreviousMonth.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                        }
                    }
                }

                if (this.GetValue(Entities.Keys.EntityType).ContainsIgnoreCase("Homologated"))
                {
                    content = content.JsonChangePropertyValue("sourceSystem", "162");
                    content = content.JsonChangePropertyValue("movementTypeId", this.GetValue("MovementTypeId"));
                    content = content.JsonChangePropertyValue("system", this.GetValue("SystemElementId"));
                    content = content.JsonChangePropertyValue("measurementUnit", "31");
                    content = content.JsonChangePropertyValue("segmentId", this.GetValue("SegmentId"));
                    content = content.JsonChangePropertyValue("operatorId", this.GetValue("OperatorId"));
                    content = content.JsonChangePropertyValue(ConstantValues.SourceNodeId, this.GetValue("NodeId_2"));
                    content = content.JsonChangePropertyValue(ConstantValues.SourceProductTypeId, this.GetValue("SourceProductTypeId"));
                    content = content.JsonChangePropertyValue("MovementSource sourceStorageLocationId", this.GetValue("SourceStorageLocationId"));
                    content = content.JsonChangePropertyValue("MovementSource sourceProductId", "10000002318");
                    content = content.JsonChangePropertyValue(ConstantValues.DestinationNodeId, this.GetValue("NodeId_1"));
                    content = content.JsonChangePropertyValue(ConstantValues.DestinationProductTypeid, this.GetValue("DestinationProductTypeId"));
                    content = content.JsonChangePropertyValue("MovementDestination destinationProductId", "10000002318");
                    content = content.JsonChangePropertyValue("MovementDestination destinationStorageLocationId", this.GetValue("DestinationStorageLocationId"));
                    content = content.JsonChangePropertyValue("SAPMovementAttribute attributeId", this.GetValue("AttributeId"));
                    content = content.JsonChangePropertyValue("SAPMovementAttribute valueAttributeUnit", this.GetValue("ValueAttributeUnitId"));
                    content = content.JsonChangePropertyValue("SAPMovementOwners ownerId", this.GetValue("Owner"));
                }

                movementArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = movementArray;
        }

        protected async Task GivenIHaveDataToRegisterInSystemWithoutHomologationAsync(string entity)
        {
            // Not creating homologation as I am verifying only error messages
            var content = ApiContent.Creates[entity];
            this.SetValue(Entities.Keys.EntityType, entity);
            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = content;

            // Delete Homologation between 7 to 1
            try
            {
                var homologationRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForSAP]).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                var homologationGroup = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                foreach (var homologationGroupRow in homologationGroup)
                {
                    var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                    this.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                    await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = this.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                }

                await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
            }
            catch (NullReferenceException ex)
            {
                Logger.Info("Homologation for SAP does not exists");
                Assert.IsNotNull(ex);
            }
            catch (ArgumentNullException ex)
            {
                Logger.Info("Homologation for SAP does not exists");
                Assert.IsNotNull(ex);
            }
        }

        protected async Task WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync(string entity)
        {
            await this.IRegisterInventoriesOrMovementsInSystemThroughSappoAsync(entity).ConfigureAwait(false);
        }

        protected async Task ValidateOfficialPointAsync(string validation)
        {
            await Task.Delay(27000).ConfigureAwait(false);
            var content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            switch (validation)
            {
                case "MalformedJson":
                    content = content.Remove(1, 1);
                    break;
                case "DataTypeError":
                    content = content.JArrayModifyPropertyValue("Array_1 operationalDate", ConstantValues.Active);
                    break;
                case "GlobalIdentifierMissing":
                    content = content = content.JArrayModifyPropertyValue("OfficialInformation_1 globalMovementId", string.Empty);
                    break;
                case "BackupMovementIdentifierMissing":
                    content = content.JArrayModifyPropertyValue("OfficialInformation_1 isOfficial", "false");
                    content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", string.Empty);
                    break;
                case "IncorrectBackupMovementIdentifier":
                    content = content.JArrayModifyPropertyValue("OfficialInformation_1 isOfficial", "false");
                    content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", "test");
                    break;
                case "BackupMovementIdentifierMismatch":
                    content = content.JArrayModifyPropertyValue("OfficialInformation_1 isOfficial", "false");
                    var movementId = content.JarrayGetValue("Array_1 movementId");
                    content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", movementId);
                    content = content.JArrayModifyPropertyValue("OfficialInformation_1 globalMovementId", movementId);
                    break;
                case "OneMovementNotMarkedOfficial":
                    content = content.JArrayModifyPropertyValue("OfficialInformation_1 isOfficial", "false");
                    break;
                case "OneMovementNotRegistered":
                    content = content.JArrayModifyPropertyValue("Array_1 movementId", "test");
                    break;
                case "TwoMovementsNotRegistered":
                    content = content.JArrayModifyPropertyValue("OfficialInformation_1 isOfficial", "false");
                    content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", "test");
                    content = content.JArrayModifyPropertyValue("Array_1 movementId", "test");
                    content = content.JArrayModifyPropertyValue("Array_2 movementId", "test");
                    break;
                case "TwoMovementsDifferentData":
                    content = content.JArrayModifyPropertyValue("OfficialInformation_1 isOfficial", "false");
                    var movementId1 = content.JarrayGetValue("Array_1 movementId");
                    content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", movementId1);
                    content = content.JArrayModifyPropertyValue("MovementSource_1 sourceNodeId", "test");
                    content = content.JArrayModifyPropertyValue("MovementSource_1 sourceProductId", "test");
                    content = content.JArrayModifyPropertyValue("movementDestination_2 destinationNodeId", "test");
                    content = content.JArrayModifyPropertyValue("movementDestination_2 destinationProductId", "test");
                    break;
                case "TwoMovementsNotReportedToSAP":
                    content = content.JArrayModifyPropertyValue("OfficialInformation_1 isOfficial", "false");
                    var movementId2 = content.JarrayGetValue("Array_1 movementId");
                    content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", movementId2);
                    break;
                case "OneMovementIncorrectData":
                    var movementId3 = content.JarrayGetValue("Array_1 movementId");
                    var movementTransactionIdValue = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId3 }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue["MovementTransactionId"] }).ConfigureAwait(false);
                    content = content.JArrayModifyPropertyValue("MovementSource_1 sourceNodeId", "test");
                    break;
                case "TwoMovementsIncorrectData":
                    var movementId4 = content.JarrayGetValue("Array_2 movementId");
                    this.ScenarioContext["movementId"] = movementId4;
                    var movementTransactionIdValue1 = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId4 }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue1["MovementTransactionId"] }).ConfigureAwait(false);
                    content = content.JArrayModifyPropertyValue("MovementDestination_1 destinationProductId", "test");
                    content = content.JArrayModifyPropertyValue("MovementDestination_2 destinationProductId", "test");
                    content = content.JArrayModifyPropertyValue("OfficialInformation_1 isOfficial", "false");
                    var backupMovementId = content.JarrayGetValue("Array_1 movementId");
                    content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", backupMovementId);
                    break;
                case "AllValidationsMet":
                    var movementId5 = content.JarrayGetValue("Array_2 movementId");
                    var movementTransactionIdValue2 = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId5 }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue2["MovementTransactionId"] }).ConfigureAwait(false);
                    content = content.JArrayModifyPropertyValue("OfficialInformation_1 isOfficial", "false");
                    var backupMovementId1 = content.JarrayGetValue("Array_1 movementId");
                    content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", backupMovementId1);
                    content = content.JArrayModifyPropertyValue("MovementSource_1 sourceProductId", ConstantValues.OriginSourceProductId);
                    content = content.JArrayModifyPropertyValue("MovementDestination_1 destinationProductId", ConstantValues.DestinationSourceProductId);
                    content = content.JArrayModifyPropertyValue("MovementSource_2 sourceProductId", ConstantValues.OriginSourceProductId);
                    content = content.JArrayModifyPropertyValue("MovementDestination_2 destinationProductId", ConstantValues.DestinationSourceProductId);
                    var sourceDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNewMovementSource, args: new { MovementTransactionId = movementTransactionIdValue2["MovementTransactionId"] }).ConfigureAwait(false);
                    var destinationDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNewMovementDestination, args: new { MovementTransactionId = movementTransactionIdValue2["MovementTransactionId"] }).ConfigureAwait(false);
                    content = content.JArrayModifyPropertyValue("MovementSource_1 sourceNodeId", sourceDetails["SourceNodeId"]);
                    content = content.JArrayModifyPropertyValue("MovementDestination_1 destinationNodeId", destinationDetails["DestinationNodeId"]);
                    content = content.JArrayModifyPropertyValue("MovementSource_2 sourceNodeId", sourceDetails["SourceNodeId"]);
                    content = content.JArrayModifyPropertyValue("MovementDestination_2 destinationNodeId", destinationDetails["DestinationNodeId"]);
                    break;
                default:
                    break;
            }

            await this.GivenIAmAuthenticatedForServiceAsync("sappoapi").ConfigureAwait(false);
            await this.SetResultsAsync(async () => await this.SapPostAsync<dynamic>(this.SapEndpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Official]), JArray.Parse(content.ToString())).ConfigureAwait(false)).ConfigureAwait(false);
        }

        protected async Task RegisterOfficialPointAsync(string type)
        {
            await Task.Delay(27000).ConfigureAwait(false);
            var content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            content = content.JArrayModifyPropertyValue("OfficialInformation_1 isOfficial", "false");
            var mainBackupMovementId = content.JarrayGetValue("Array_1 movementId");
            content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", mainBackupMovementId);
            this.SetValue("GlobalMovementId", new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
            for (int i = 1; i <= 2; i++)
            {
                content = content.JArrayModifyPropertyValue("Array_" + i + " sourceSystem", "162");
                content = content.JArrayModifyPropertyValue("Array_" + i + " movementTypeId", this.GetValue("MovementTypeId"));
                content = content.JArrayModifyPropertyValue("Array_" + i + " measurementUnit", "31");
                content = content.JArrayModifyPropertyValue("Array_" + i + " segmentId", this.GetValue("SegmentId"));
                content = content.JArrayModifyPropertyValue("Array_" + i + " operatorId", this.GetValue("OperatorId"));
                content = content.JArrayModifyPropertyValue("MovementSource_" + i + " sourceNodeId", this.GetValue("NodeId_2"));
                content = content.JArrayModifyPropertyValue("MovementSource_" + i + " sourceProductTypeId", string.Empty);
                content = content.JArrayModifyPropertyValue("MovementSource_" + i + " sourceStorageLocationId", string.Empty);
                content = content.JArrayModifyPropertyValue("MovementSource_" + i + " sourceProductId", "10000002318");
                content = content.JArrayModifyPropertyValue("MovementDestination_" + i + " destinationNodeId", this.GetValue("NodeId_1"));
                content = content.JArrayModifyPropertyValue("MovementDestination_" + i + " destinationProductTypeid", string.Empty);
                content = content.JArrayModifyPropertyValue("MovementDestination_" + i + " destinationProductId", "10000002318");
                content = content.JArrayModifyPropertyValue("MovementDestination_" + i + " destinationStorageLocationId", string.Empty);
                content = content.JArrayModifyPropertyValue("Attributes_" + i + " attributeId", this.GetValue("AttributeId"));
                content = content.JArrayModifyPropertyValue("Attributes_" + i + " valueAttributeUnit", this.GetValue("ValueAttributeUnitId"));
                content = content.JArrayModifyPropertyValue("MovementOwner_" + i + " ownerId", this.GetValue("Owner"));
                content = content.JArrayModifyPropertyValue("MovementOwner_" + i + " ownershipValue", "100");
                content = content.JArrayModifyPropertyValue("MovementOwner_" + i + " ownerShipValueUnit", " %");
                content = content.JArrayModifyPropertyValue("OfficialInformation_" + i + " globalMovementId", this.GetValue("GlobalMovementId"));
            }

            switch (type)
            {
                case "BackupMovementNotRegistered":
                    var movementId = content.JarrayGetValue("Array_2 movementId");
                    var movementTransactionIdValue2 = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue2["MovementTransactionId"] }).ConfigureAwait(false);
                    this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                    content = content.JArrayModifyPropertyValue("Array_1 movementId", this.GetValue(ConstantValues.MovementId));
                    content = content.JArrayModifyPropertyValue("OfficialInformation_2 backupMovementId", this.GetValue(ConstantValues.MovementId));
                    break;
                case "BackupMovementAndLastEventTypeDelete":
                    var movementId1 = content.JarrayGetValue("Array_2 movementId");
                    var movementTransactionIdValue3 = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId1 }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue3["MovementTransactionId"] }).ConfigureAwait(false);
                    var backupMovementId = content.JarrayGetValue("Array_1 movementId");
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateMovementEventType, args: new { MovementId = backupMovementId }).ConfigureAwait(false);
                    break;
                case "BackupMovementIncorrectNetQuantity":
                    var movementId2 = content.JarrayGetValue("Array_2 movementId");
                    var movementTransactionIdValue4 = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId2 }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue4["MovementTransactionId"] }).ConfigureAwait(false);
                    content = content.JArrayModifyPropertyValue("Array_1 netStandardQuantity", "2460.00");
                    break;
                case "BackupMovementWithEventsReportedToSAP":
                    var movementId3 = content.JarrayGetValue("Array_1 movementId");
                    var movementTransactionIdValue5 = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId3 }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue5["MovementTransactionId"] }).ConfigureAwait(false);
                    break;
                case "OfficialMovementNotRegistered":
                    var movementId4 = content.JarrayGetValue("Array_1 movementId");
                    var movementTransactionIdValue6 = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId4 }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue6["MovementTransactionId"] }).ConfigureAwait(false);
                    this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                    content = content.JArrayModifyPropertyValue("Array_2 movementId", this.GetValue(ConstantValues.MovementId));
                    break;
                case "OfficialMovementAndLastEventTypeDelete":
                    var movementId5 = content.JarrayGetValue("Array_1 movementId");
                    var movementTransactionIdValue7 = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId5 }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue7["MovementTransactionId"] }).ConfigureAwait(false);
                    var officialMovementId = content.JarrayGetValue("Array_2 movementId");
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateMovementEventType, args: new { MovementId = officialMovementId }).ConfigureAwait(false);
                    break;
                case "OfficialMovementIncorrectNetQuantity":
                    var movementId6 = content.JarrayGetValue("Array_1 movementId");
                    var movementTransactionIdValue8 = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId6 }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue8["MovementTransactionId"] }).ConfigureAwait(false);
                    content = content.JArrayModifyPropertyValue("Array_2 netStandardQuantity", "2460.00");
                    break;
                case "OfficialMovementWithEventsReportedToSAP":
                    var movementId7 = content.JarrayGetValue("Array_2 movementId");
                    var movementTransactionIdValue9 = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementTransactionId, args: new { MovementId = movementId7 }).ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertIntoSapTracking, args: new { movementTransactionId = movementTransactionIdValue9["MovementTransactionId"] }).ConfigureAwait(false);
                    break;
                default:
                    break;
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = content;
            this.SetValue("BackupMovementId", content.JarrayGetValue("Array_1 movementId"));
            this.SetValue("OfficialMovementId", content.JarrayGetValue("Array_2 movementId"));
            this.SetValue("GlobalMovementId", content.JarrayGetValue("OfficialInformation_1 globalMovementId"));
            await this.GivenIAmAuthenticatedForServiceAsync("sappoapi").ConfigureAwait(false);
            await this.SetResultsAsync(async () => await this.SapPostAsync<dynamic>(this.SapEndpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Official]), JArray.Parse(content.ToString())).ConfigureAwait(false)).ConfigureAwait(false);
            await Task.Delay(15000).ConfigureAwait(false);
        }

        private static string GenerateEncodedString(string plainTextId)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainTextId);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private async Task GivenIHaveProcessedSinoperQueueInTheSystemAsync(string entityType)
        {
            var uploadFileId = new Faker().Random.AlphaNumeric(35);
            this.ScenarioContext[ConstantValues.MessageId] = uploadFileId;
            var serviceBusMessageForSinoper = ApiContent.Creates[ConstantValues.ServiceBusMessageForSinoper];
            serviceBusMessageForSinoper = serviceBusMessageForSinoper.JsonChangePropertyValue("UploadFileId", uploadFileId);
            string blobPath;
            if (entityType.ContainsIgnoreCase(ConstantValues.InventoryQueueName))
            {
                blobPath = "/true/sinoper/xml/inventory/" + uploadFileId;
                if (string.IsNullOrEmpty(this.GetValue(ConstantValues.DestinationProductIsNotSet)) && this.GetValue(ConstantValues.InventoryWithMultipleProducts) != "Yes")
                {
                    await BlobExtensions.UploadXmlFileAsync("true/sinoper/xml/inventory", uploadFileId, ConstantValues.InventoryQueueName).ConfigureAwait(false);
                }
                else if (this.GetValue(ConstantValues.InventoryWithMultipleProducts) == "Yes")
                {
                    await BlobExtensions.UploadXmlFileAsync("true/sinoper/xml/inventory", uploadFileId, ConstantValues.InventoryWithMultipleProducts).ConfigureAwait(false);
                }

                await this.ReadSqlAsync(SqlQueries.InsertFileRegistrationForSinoper, args: new { uploadFileId, blobPath }).ConfigureAwait(false);

                ////sending request with chaos label
                await ServiceBusHelper.PutAsync(ConstantValues.InventoryQueueName, serviceBusMessageForSinoper).ConfigureAwait(false);
            }
            else if (entityType.ContainsIgnoreCase(ConstantValues.MovementQueueName))
            {
                blobPath = "/true/sinoper/xml/movements/" + uploadFileId;
                if (string.IsNullOrEmpty(this.GetValue(ConstantValues.DestinationProductIsNotSet)) && this.GetValue(ConstantValues.InventoryWithMultipleProducts) != "Yes")
                {
                    await BlobExtensions.UploadXmlFileAsync("true/sinoper/xml/movements", uploadFileId, ConstantValues.MovementQueueName).ConfigureAwait(false);
                }
                else if (this.GetValue(ConstantValues.DestinationProductIsNotSet) == "Yes")
                {
                    await BlobExtensions.UploadXmlFileAsync("true/sinoper/xml/movements", uploadFileId, ConstantValues.MovementsWithoutDestination).ConfigureAwait(false);
                }

                await this.ReadSqlAsync(SqlQueries.InsertFileRegistrationForSinoper, args: new { uploadFileId, blobPath }).ConfigureAwait(false);

                ////sending request with chaos label
                await ServiceBusHelper.PutAsync(ConstantValues.MovementQueueName, serviceBusMessageForSinoper).ConfigureAwait(false);
            }

            ////waiting As I observed when processed an invenotry or movement through service bus it is taking 5-10 seconds to register into database
            await Task.Delay(10000).ConfigureAwait(true);
        }
    }
}