// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageConnectionBetweenNodesSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Api
{
    using System.Linq;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class ManageConnectionBetweenNodesSteps : EcpApiStepDefinitionBase
    {
        public ManageConnectionBetweenNodesSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [When(@"I provide a ""(.*)"" do not exist")]
        public async Task WhenIProvideAThatdoNotExistAsync(string field)
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateSourceAndDestinationNodesAsync(content).ConfigureAwait(false) : content;
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(content).ConfigureAwait(false) : content;
            content = content.JsonChangePropertyValue(field, "0");
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide ""(.*)"" and ""(.*)"" already exist")]
        public async Task WhenIProvideAndThatAlreadyExistAsync(string field1, string field2)
        {
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count() + 1);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateSourceAndDestinationNodesAsync(content).ConfigureAwait(false) : content;
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(content).ConfigureAwait(false) : content;
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false);
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide ""(.*)"" contains characters other than expected")]
        public async Task WhenIProvideContainsCharactersOtherThanExpectedAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            content = content.JsonChangePropertyValue(field, "AA");
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide same ""(.*)"" and ""(.*)""")]
        public async Task WhenIProvideSameAndAsync(string field1, string field2)
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count() + 1);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            content = await this.CreateSourceAndDestinationNodesAsync(content).ConfigureAwait(false);
            content = await this.CreateOwnersAsync(content).ConfigureAwait(false);
            var fieldValue = content.JsonGetValue(field1);
            content = content.JsonChangePropertyValue(field2, fieldValue);
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide inactive connection details")]
        public async Task WhenIProvideInactiveConnectionDetailsAsync()
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count() + 1);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            content = await this.CreateSourceAndDestinationNodesAsync(content).ConfigureAwait(false);
            content = await this.CreateOwnersAsync(content).ConfigureAwait(false);
            content = content.JsonChangePropertyValue("isActive", "False");
            this.LogToReport(JToken.Parse(content));
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false);
            content = content.JsonChangePropertyValue("isActive", "True");
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get record with ""(.*)"" and ""(.*)""")]
        public async Task WhenIGetRecordWithAndAsync(string field1, string field2)
        {
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(entity).ConfigureAwait(false);
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], this.ScenarioContext[ConstantValues.SourceNode], this.ScenarioContext[ConstantValues.DestinationNode])).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get record with invalid ""(.*)""")]
        public async Task WhenIGetRecordWithInvalidAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(entity).ConfigureAwait(false);
            this.ScenarioContext[field] = "0";
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], this.ScenarioContext[ConstantValues.SourceNode], this.ScenarioContext[ConstantValues.DestinationNode])).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get record with ""(.*)"" and ""(.*)"" do not exist")]
        public async Task WhenIGetRecordWithAndThatdoNotExistAsync(string field1, string field2)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            content = await this.CreateSourceAndDestinationNodesAsync(content).ConfigureAwait(false);
            this.ScenarioContext[field1] = content.JsonGetValue(field1);
            this.ScenarioContext[field2] = content.JsonGetValue(field2);
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], this.ScenarioContext[field1], this.ScenarioContext[field2])).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get record with same ""(.*)"" and ""(.*)""")]
        public async Task WhenIGetRecordWithSameAndAsync(string field1, string field2)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(entity).ConfigureAwait(false);
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
            await this.CreateConnectionWithSameNodeIdsAsync(entity).ConfigureAwait(false);
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], this.ScenarioContext[field1], this.ScenarioContext[field2])).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update a record with ""(.*)"" contains characters other than expected")]
        public async Task WhenIUpdateARecordWithThatContainsCharactersOtherThanExpectedAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Updates[entity];
            content = content.JsonChangePropertyValue(field, "AA");
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update a record with a ""(.*)"" do not exist")]
        public async Task WhenIUpdateARecordWithAdoNotExistAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(entity).ConfigureAwait(false);
            var updateContent = ApiContent.Updates[entity];
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.SourceNode, this.ScenarioContext[ConstantValues.SourceNode].ToString());
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.DestinationNode, this.ScenarioContext[ConstantValues.DestinationNode].ToString());
            updateContent = updateContent.JsonChangeValue("description", "_Mod_");
            updateContent = field.EqualsIgnoreCase(ConstantValues.SourceNode) ? updateContent.JsonChangePropertyValue(field, this.ScenarioContext[ConstantValues.DestinationNode].ToString()) : updateContent.JsonChangePropertyValue(field, this.ScenarioContext[ConstantValues.SourceNode].ToString());
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update a record where ""(.*)"" and ""(.*)"" are same")]
        public async Task WhenIUpdateARecordWhereAndAreSameAsync(string field1, string field2)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(entity).ConfigureAwait(false);
            await this.CreateConnectionWithSameNodeIdsAsync(entity).ConfigureAwait(false);
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
            var updateContent = ApiContent.Updates[entity];
            updateContent = updateContent.JsonChangeValue("description", "_Mod_");
            updateContent = updateContent.JsonChangePropertyValue(field1, this.ScenarioContext[ConstantValues.DestinationNode].ToString());
            updateContent = updateContent.JsonChangePropertyValue(field2, this.ScenarioContext[ConstantValues.DestinationNode].ToString());
            ////updateContent = updateContent.JsonChangePropertyValue("Product_1-Owner_1 ownerId", this.ScenarioContext["Product_1-OwnerId_1"].ToString());
            ////updateContent = updateContent.JsonChangePropertyValue("Product_1-Owner_2 ownerId", this.ScenarioContext["Product_1-OwnerId_2"].ToString());
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I delete a record with ""(.*)""")]
        public async Task WhenIDeleteARecordWithAsync(string movement)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(entity).ConfigureAwait(false);
            if (movement.EqualsIgnoreCase("Movements"))
            {
                await this.ReadAllSqlAsync(input: ApiContent.InsertRow[ConstantValues.Movement], args: new { MovementId = new Faker().Random.Number(4) }).ConfigureAwait(false);
                var lastCreatedMovement = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.Movement]).ConfigureAwait(false);
                string movementTransactionID = lastCreatedMovement["MovementTransactionId"];
                var storageLocation = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNodeStorageLocationByNodeId, args: new { nodeId = this.ScenarioContext[ConstantValues.SourceNode] }).ConfigureAwait(false);
                string sourceStorageLocationID = storageLocation["NodeStorageLocationId"];
                await this.ReadAllSqlAsync(input: ApiContent.InsertRow[ConstantValues.MovementPeriod], args: new { MovementTransactionID = movementTransactionID }).ConfigureAwait(false);
                await this.ReadAllSqlAsync(input: ApiContent.InsertRow[ConstantValues.MovementSource], args: new { MovementTransactionID = movementTransactionID, SourceNodeId = this.ScenarioContext[ConstantValues.SourceNode].ToString(), SourceStorageLocationID = sourceStorageLocationID }).ConfigureAwait(false);
                await this.ReadAllSqlAsync(input: ApiContent.InsertRow[ConstantValues.MovementDestination], args: new { MovementTransactionID = movementTransactionID, DestinationNodeID = this.ScenarioContext[ConstantValues.DestinationNode].ToString(), SourceStorageLocationID = sourceStorageLocationID }).ConfigureAwait(false);
            }

            await this.SetResultAsync(async () => await this.DeleteWithResponseAsync(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], this.ScenarioContext[ConstantValues.SourceNode], this.ScenarioContext[ConstantValues.DestinationNode])).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I delete a record do not exist")]
        public async Task WhenIDeleteARecordThatdoNotExistAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(entity).ConfigureAwait(false);
            this.ScenarioContext[ConstantValues.DestinationNode] = this.ScenarioContext[ConstantValues.SourceNode];
            await this.SetResultAsync(async () => await this.DeleteWithResponseAsync(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], this.ScenarioContext[ConstantValues.SourceNode], this.ScenarioContext[ConstantValues.DestinationNode])).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I delete a record where ""(.*)"" and ""(.*)"" are same")]
        public async Task WhenIDeleteARecordWhereAndAreSameAsync(string field1, string field2)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(entity).ConfigureAwait(false);
            await this.CreateConnectionWithSameNodeIdsAsync(entity).ConfigureAwait(false);
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
            this.ScenarioContext[field1] = this.ScenarioContext[field2];
            await this.SetResultAsync(async () => await this.DeleteWithResponseAsync(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], this.ScenarioContext[field1], this.ScenarioContext[field2])).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"the response should fail with messages")]
        public async Task ThenTheResponseShouldFailWithMessagesAsync(Table table)
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.VerifyThat(() => Assert.AreEqual(dbResults.Count(), this.GetValue<dynamic>(Entities.Keys.InitialRowCount)));
            var errorCodes = JObject.Parse(await this.GetValue<dynamic>(Entities.Keys.Error).ReadAsStringAsync().ConfigureAwait(false))["errorCodes"];
            Assert.AreEqual(table?.Rows?.Count, errorCodes?.Count);
            foreach (var row in table?.Rows.Select((message, i) => (msg: message["Message"], index: i)))
            {
                this.VerifyThat(() => Assert.AreEqual(row.msg, errorCodes[row.index]["message"].ToString()));
            }
        }

        [Then(@"the response should fail with ""(.*)""")]
        public void ThenTheResponseShouldFailWith(string requestType)
        {
            this.VerifyThat(() => Assert.AreEqual(requestType, this.GetValue<string>(Entities.Keys.ErrorStatus)));
        }

        [Given(@"I have inactive ""(.*)"" in the system")]
        public async Task GivenIHaveInactiveInTheSystemAsync(string entity)
        {
            this.SetValue(Keys.EntityType, entity);
            this.SetValue(Keys.Route, ApiContent.Routes[entity]);
            var createContent = ApiContent.Creates[entity];
            createContent = createContent.JsonChangeValue();
            createContent = await this.CreateSourceAndDestinationNodesAsync(createContent).ConfigureAwait(false);
            createContent = await this.CreateOwnersAsync(createContent).ConfigureAwait(false);
            createContent.JsonChangePropertyValue("isActive", "False");
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(createContent)).ConfigureAwait(false);
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
        }

        [When(@"I update a connection with ""(.*)"" as ""(.*)""")]
        public async Task WhenIUpdateAConnectionWithAsAsync(string field, string fieldValue)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(entity).ConfigureAwait(false);
            var updateContent = ApiContent.Updates[entity];
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.SourceNode, this.ScenarioContext[ConstantValues.SourceNode].ToString());
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.DestinationNode, this.ScenarioContext[ConstantValues.DestinationNode].ToString());
            ////updateContent = updateContent.JsonChangePropertyValue("Product_1-Owner_1 ownerId", this.ScenarioContext["Product_1-OwnerId_1"].ToString());
            ////updateContent = updateContent.JsonChangePropertyValue("Product_1-Owner_2 ownerId", this.ScenarioContext["Product_1-OwnerId_2"].ToString());
            updateContent = updateContent.JsonChangePropertyValue(field, fieldValue);
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I delete a record with invalid ""(.*)""")]
        public async Task WhenIDeleteARecordWithInvalidAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(entity).ConfigureAwait(false);
            this.ScenarioContext[field] = "0";
            await this.SetResultAsync(async () => await this.DeleteWithResponseAsync(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], this.ScenarioContext[ConstantValues.SourceNode], this.ScenarioContext[ConstantValues.DestinationNode])).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"""(.*)"" should be registered in Audit-log")]
        public async Task ThenShouldBeRegisteredInAuditLogAsync(string logType)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            var auditDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.AuditDetails], args: new { nodeId = fieldValue }).ConfigureAwait(false);
            Assert.AreEqual(ApiContent.LogType[logType], auditDetails["LogType"]);
        }

        public async Task GetLastCreatedSourceNodeIdAndDestinationNodeIdAsync(string entity)
        {
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            this.ScenarioContext[ConstantValues.SourceNode] = lastCreatedRow["SourceNodeId"];
            this.ScenarioContext[ConstantValues.DestinationNode] = lastCreatedRow["DestinationNodeId"];
        }

        public async Task CreateConnectionWithSameNodeIdsAsync(string entity)
        {
            var content = ApiContent.Creates[entity];
            this.ScenarioContext[ConstantValues.SourceNode] = this.ScenarioContext[ConstantValues.DestinationNode];
            content = content.JsonChangePropertyValue(ConstantValues.SourceNode, this.ScenarioContext[ConstantValues.SourceNode].ToString());
            content = content.JsonChangePropertyValue(ConstantValues.DestinationNode, this.ScenarioContext[ConstantValues.DestinationNode].ToString());
            content = await this.CreateOwnersAsync(content).ConfigureAwait(false);
            await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false);
        }
    }
}
