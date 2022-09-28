// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateNodesSteps.cs" company="Microsoft">
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
    using System.Collections.Generic;
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
    public class UpdateNodesSteps : EcpApiStepDefinitionBase
    {
        public UpdateNodesSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Given(@"I have one Node in the system with the below Node Name")]
        public void GivenIHaveOneNodeInTheSystemWithTheBelowNodeName()
        {
            this.ScenarioContext.Pending();
        }

        [When(@"I update a record with ""(.*)"" as ""(.*)"" and without ""(.*)""")]
        public async Task WhenIUpdateWithAsAndWithoutAsync(string field1, string field1Value, string field2)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Updates[entity].Replace("^^placeholder^^", string.Empty);
            content = content.JsonChangePropertyValue(field1, field1Value);
            content = content.JsonChangePropertyValue(field2);
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update ""(.*)"" as ""(.*)"" and without ""(.*)""")]
        public async Task WhenIUpdateAsAndWithoutAsync(string field1, string field1Value, string field2)
        {
            Assert.IsNotNull(field2);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Updates[entity].Replace("^^placeholder^^", string.Empty);
            content = content.JsonChangePropertyValue(field1, field1Value);
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"the active field of the ""(.*)"" must be updated to ""(.*)"" for respective ""(.*)""")]
        public async Task ThenTheActiveFieldOfTheMustBeUpdatedToAsync(string field1, string field2, string field3)
        {
            Assert.IsNotNull(field1);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: field3 == ConstantValues.StorageLocations ? ApiContent.LastCreated[entity] : ApiContent.LastCreated[ConstantValues.ProductLocations]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[field3 == ConstantValues.StorageLocations ? ApiContent.Ids[entity].ToPascalCase() : ApiContent.Ids[ConstantValues.NodeStorageLocationId].ToPascalCase()];
            IDictionary<string, string> storageLocationStatus = null;
            if (field3 == ConstantValues.StorageLocations)
            {
                storageLocationStatus = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.StorageLocationStatus], args: new { nodeId = fieldValue }).ConfigureAwait(false);
            }
            else
            {
                storageLocationStatus = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.ProductLocationStatus], args: new { nodeStorageLocationId = fieldValue }).ConfigureAwait(false);
            }

            string nodeStorageLocationStatus = storageLocationStatus["IsActive"];
            Assert.AreEqual(field2, nodeStorageLocationStatus.ToCamelCase());
        }

        [Then(@"""(.*)"" to the field ""(.*)"" as (.*) should be registered in audit log for respective ""(.*)""")]
        public async Task ThenToTheFieldAsShouldBeRegisteredInAuditLogAsync(string logType, string field1, string fieldValue1, string field2)
        {
            Assert.IsNotNull(field2);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            IDictionary<string, string> auditDetails = null;
            if (field2 == ConstantValues.StorageLocations)
            {
                auditDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.AuditDetails], args: new { nodeId = fieldValue }).ConfigureAwait(false);
            }
            else
            {
                auditDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.ProductAuditDetails], args: new { newLogType = logType, field = field1 }).ConfigureAwait(false);
            }

            Assert.AreEqual(field1, auditDetails["Field"]);
            Assert.AreEqual(logType, auditDetails["LogType"]);
            Assert.AreEqual(fieldValue1.ToPascalCase(), auditDetails["NewValue"]);
        }

        [Then(@"the ""(.*)"" data should be registered in the system")]
        public async Task ThenTheDataShouldBeRegisteredInTheSystemAsync(string field)
        {
            Assert.IsNotNull(field);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            var storageLocationName = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.StorageLocationName], args: new { nodeId = fieldValue }).ConfigureAwait(false);
            string nodeStorageLocationName = storageLocationName["Name"];
            Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), nodeStorageLocationName);
        }

        [When(@"I update a record with new ""(.*)""")]
        public async Task WhenIUpdateARecordWithNewAsync(string field)
        {
            Assert.IsNotNull(field);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            var updateContent = ApiContent.Updates[entity].Replace("^^placeholder^^", ApiContent.AddStorageLocation);
            this.ScenarioContext["EntityId"] = fieldValue;
            updateContent = updateContent.JsonChangePropertyValue(ApiContent.Ids[entity], fieldValue);
            updateContent = updateContent.JsonChangeValue("name", "_Mod_");
            if (entity.ContainsIgnoreCase(ConstantValues.Node))
            {
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.StorageLocationNodeId, fieldValue);
                var lastCreatedStorageLocation = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.StorageLocations], args: new { nodeId = fieldValue }).ConfigureAwait(false);
                string nodeStorageLocationId = lastCreatedStorageLocation["NodeStorageLocationId"];
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.NodeStorageLocationId, nodeStorageLocationId);
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.ProductLocationNodeStorageLocationId, nodeStorageLocationId);
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.NewStorageLocationNodeId, fieldValue);
            }

            this.SetValue(Keys.RandomFieldValue, string.Concat($"Automation{"_"}", new Faker().Random.AlphaNumeric(5)));
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.NewStorageLocationName, this.GetValue(Keys.RandomFieldValue));
            updateContent = entity.EqualsIgnoreCase(ConstantValues.Node) || entity.EqualsIgnoreCase(ConstantValues.Nodes) ? await this.UpdateContentForNodeAsync(updateContent).ConfigureAwait(false) : updateContent;
            updateContent = entity.EqualsIgnoreCase(ConstantValues.Node) || entity.EqualsIgnoreCase(ConstantValues.Nodes) ? updateContent.JsonChangePropertyValue(ConstantValues.NewStorageLocationStorageLocationTypeId, this.ScenarioContext["StorageLocationTypeId"].ToString()) : updateContent;
            this.LogToReport(JToken.Parse(updateContent));
            this.SetValue(Entities.Keys.Result, await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false));
        }

        [Then(@"Node data should be updated")]
        public async Task ThenNodeDataShouldBeUpdatedAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            var nodeDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.NodeDetails], args: new { nodeId = fieldValue }).ConfigureAwait(false);
            string nodeName = nodeDetails["Name"];
            Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), nodeName);
        }

        [Then(@"""(.*)"" to the ""(.*)"" should be registered in audit log")]
        public async Task ThenToTheShouldBeRegisteredInAuditLogAsync(string logType, string field1)
        {
            Assert.IsNotNull(field1);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            var auditDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.AuditDetails], args: new { nodeId = fieldValue }).ConfigureAwait(false);
            Assert.AreEqual("Name", auditDetails["Field"]);
            Assert.AreEqual(logType, auditDetails["LogType"]);
            Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), auditDetails["NewValue"]);
        }

        [When(@"I update ""(.*)"" as ""(.*)"" from the list of existing ""(.*)""")]
        public async Task WhenIUpdateAsFromTheListofExistingAsync(string field1, string field1Value, string field2)
        {
            this.SetValue(Entities.Keys.Status, field1Value);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedNodeRow = await this.ReadSqlAsStringDictionaryAsync(ApiContent.LastCreated[entity]).ConfigureAwait(false);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: field2 == ConstantValues.StorageLocations ? ApiContent.LastCreated[entity] : ApiContent.LastCreated[ConstantValues.ProductLocations]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[field2 == ConstantValues.StorageLocations ? ApiContent.Ids[entity].ToPascalCase() : ApiContent.Ids[ConstantValues.NodeStorageLocationId].ToPascalCase()];
            string nodeIdValue = lastCreatedNodeRow[ApiContent.Ids[entity].ToPascalCase()];
            var updateContent = ApiContent.Updates[entity].Replace("^^placeholder^^", string.Empty);
            updateContent = updateContent.JsonChangePropertyValue(ApiContent.Ids[entity], nodeIdValue);
            updateContent = updateContent.JsonChangeValue("name", "_Mod_");
            if (entity.ContainsIgnoreCase(ConstantValues.Node))
            {
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.StorageLocationNodeId, nodeIdValue);
                IDictionary<string, string> lastCreatedStorageLocation = null;
                if (field2 == ConstantValues.StorageLocations)
                {
                    lastCreatedStorageLocation = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.StorageLocations], args: new { nodeId = fieldValue }).ConfigureAwait(false);
                }
                else
                {
                    lastCreatedStorageLocation = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.ProductLocations], args: new { nodeStorageLocationId = fieldValue }).ConfigureAwait(false);
                    string storageLocationProductId = lastCreatedStorageLocation["StorageLocationProductId"];
                    updateContent = updateContent.JsonChangePropertyValue(ConstantValues.ProductLocationStorageLocationProductId, storageLocationProductId);
                }

                string nodeStorageLocationId = lastCreatedStorageLocation["NodeStorageLocationId"];
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.NodeStorageLocationId, nodeStorageLocationId);
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.ProductLocationNodeStorageLocationId, nodeStorageLocationId);
            }

            updateContent = updateContent.JsonChangePropertyValue(field1, this.GetValue(Entities.Keys.Status));
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update ""(.*)"" from the existing ""(.*)""")]
        public async Task WhenIUpdateFromTheListOfExistingAsync(string field1, string field2)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedNodeRow = await this.ReadSqlAsStringDictionaryAsync(ApiContent.LastCreated[entity]).ConfigureAwait(false);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: field2 == ConstantValues.StorageLocations ? ApiContent.LastCreated[entity] : ApiContent.LastCreated[ConstantValues.ProductLocations]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[field2 == ConstantValues.StorageLocations ? ApiContent.Ids[entity].ToPascalCase() : ApiContent.Ids[ConstantValues.NodeStorageLocationId].ToPascalCase()];
            string nodeIdValue = lastCreatedNodeRow[ApiContent.Ids[entity].ToPascalCase()];
            var updateContent = ApiContent.Updates[entity].Replace("^^placeholder^^", string.Empty);
            updateContent = updateContent.JsonChangePropertyValue(ApiContent.Ids[entity], nodeIdValue);
            updateContent = updateContent.JsonChangeValue("name", "_Mod_");
            if (entity.ContainsIgnoreCase(ConstantValues.Node))
            {
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.StorageLocationNodeId, nodeIdValue);
                IDictionary<string, string> lastCreatedStorageLocation = null;
                if (field2 == ConstantValues.StorageLocations)
                {
                    lastCreatedStorageLocation = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.StorageLocations], args: new { nodeId = fieldValue }).ConfigureAwait(false);
                }
                else
                {
                    lastCreatedStorageLocation = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.ProductLocations], args: new { nodeStorageLocationId = fieldValue }).ConfigureAwait(false);
                    string storageLocationProductId = lastCreatedStorageLocation["StorageLocationProductId"];
                    updateContent = updateContent.JsonChangePropertyValue(ConstantValues.ProductLocationStorageLocationProductId, storageLocationProductId);
                }

                string nodeStorageLocationId = lastCreatedStorageLocation["NodeStorageLocationId"];
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.NodeStorageLocationId, nodeStorageLocationId);
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.ProductLocationNodeStorageLocationId, nodeStorageLocationId);
            }

            this.SetValue(Keys.RandomFieldValue, string.Concat($"Automation{"_"}", new Faker().Random.AlphaNumeric(5)));
            updateContent = updateContent.JsonChangePropertyValue(field1, this.GetValue(Keys.RandomFieldValue));
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"the field of ""(.*)"" must be updated for respective ""(.*)""")]
        public async Task ThenTheFieldOfMustBeUpdatedForRespectiveAsync(string field1, string field2)
        {
            Assert.IsNotNull(field2);
            Assert.IsNotNull(field1);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            var storageLocationName = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.StorageLocationName], args: new { nodeId = fieldValue }).ConfigureAwait(false);
            string nodeStorageLocationName = storageLocationName["Name"];
            Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), nodeStorageLocationName);
        }

        [Then(@"""(.*)"" to the field ""(.*)"" should be registered in audit log")]
        public async Task ThenToTheFieldShouldBeRegisteredInAuditLogForRespectiveAsync(string logType, string field1)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            var auditDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.NewAuditStorageLocationName], args: new { nodeId = fieldValue, newLogType = logType, field = field1 }).ConfigureAwait(false);
            Assert.AreEqual(field1, auditDetails["Field"]);
            Assert.AreEqual(logType, auditDetails["LogType"]);
            Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), auditDetails["NewValue"]);
        }
    }
}