// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateHomologationSteps.cs" company="Microsoft">
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

    using global::Bdd.Core.Hooks;
    using global::Bdd.Core.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json.Linq;

    using TechTalk.SpecFlow;

    [Binding]
    public class CreateHomologationSteps : EcpApiStepDefinitionBase
    {
        public CreateHomologationSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [When(@"I provide the valid data for ""(.*)"" of ""(.*)""")]
        public async Task WhenIProvideTheValidDataForOfAsync(string field1, string field2)
        {
            await this.DeleteAllHomologationsAsync().ConfigureAwait(false);
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count() + 1);
            var content = await this.GetHomologationDataAsync(field1, field2).ConfigureAwait(false);
            var sourceValue = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[field1]).ConfigureAwait(false);
            content = content.JsonChangePropertyValue(ApiContent.Homologation[field2 == ConstantValues.Destination ? ConstantValues.Source : ConstantValues.Destination], sourceValue[ApiContent.Ids[field1].ToPascalCase()]);
            this.LogToReport(JToken.Parse(content));
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
            var homologationGroupId = await this.ReadSqlAsStringDictionaryAsync(input: field2 == ConstantValues.Source ? SqlQueries.GetHomologationGroupIdByDestinationValue : SqlQueries.GetHomologationGroupIdBySourceValue, args: new { value = this.GetValue(Keys.RandomFieldValue) }).ConfigureAwait(false);
            this.SetValue(Keys.HomologationGroupId, homologationGroupId[ConstantValues.HomologationGroupId].ToPascalCase());
            var homologationId = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetHomologationId, args: new { homologationGroupId = homologationGroupId[ConstantValues.HomologationGroupId].ToPascalCase() }).ConfigureAwait(false);
            this.SetValue(Keys.HomologationId, homologationId[ConstantValues.HomologationId].ToPascalCase());
        }

        [When(@"I provide the invalid data for ""(.*)"" of ""(.*)""")]
        public async Task WhenIProvideTheInvalidDataForOfAsync(string field1, string field2)
        {
            await this.DeleteAllHomologationsAsync().ConfigureAwait(false);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = await this.GetHomologationDataAsync(field1, field2).ConfigureAwait(false);
            content = content.JsonChangePropertyValue(ApiContent.Homologation[field2 == ConstantValues.Destination ? ConstantValues.Source : ConstantValues.Destination], ConstantValues.InvalidData);
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide the existing data")]
        [BeforeAfterStep(afterStepMethod: "Query(DeleteHomologationData,HomologationGroupId:{sc},HomologationId:{sc})")]
        public async Task WhenIProvideTheExistingDataAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            var nodeId = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.Nodes]).ConfigureAwait(false);
            content = content.JsonChangePropertyValue(ApiContent.Homologation[ConstantValues.Destination], nodeId[ConstantValues.NodeId].ToPascalCase());
            this.LogToReport(JToken.Parse(content));

            // Inserting the homologation data into db for the first time
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);

            // Inserting the same above homologation data into db for the second time to reproduce the existing data error message
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);

            var homologationGroupId = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetHomologationGroupIdByDestinationValue, args: new { value = ConstantValues.AutomationNode }).ConfigureAwait(false);
            this.SetValue(Keys.HomologationGroupId, homologationGroupId[ConstantValues.HomologationGroupId].ToPascalCase());
            var homologationId = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetHomologationId, args: new { homologationGroupId = homologationGroupId[ConstantValues.HomologationGroupId].ToPascalCase() }).ConfigureAwait(false);
            this.SetValue(Keys.HomologationId, homologationId[ConstantValues.HomologationId].ToPascalCase());
            this.ScenarioContext.Set(nameof(Keys.HomologationGroupId), this.GetValue(Keys.HomologationGroupId));
            this.ScenarioContext.Set(nameof(Keys.HomologationId), this.GetValue(Keys.HomologationId));
        }

        [When(@"I don't provide at least one of the source or destination system corresponds to the TRUE system")]
        public async Task WhenIDonTProvideAtLeastOneOfTheSourceOrDestinationSystemCorrespondsToTheTRUESystemAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            var sinoperSystemId = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.SinoperSystemId]).ConfigureAwait(false);
            var cenitSystemId = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.ExcelId]).ConfigureAwait(false);
            var val = sinoperSystemId[ConstantValues.SystemTypeId].ToPascalCase();
            Assert.IsNotNull(val);
            content = content.JsonChangePropertyValue(ConstantValues.SourceSystemId, sinoperSystemId[ConstantValues.SystemTypeId].ToPascalCase());
            content = content.JsonChangePropertyValue(ConstantValues.DestinationSystemId, cenitSystemId[ConstantValues.SystemTypeId].ToPascalCase());
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"it should be registered in the Audit-log")]
        ////[BeforeAfterStep(afterStepMethod: "Query(DeleteHomologationData,HomologationGroupId:{sc},HomologationId:{sc})")]
        public async Task ThenItShouldBeRegisteredInTheAuditLogAsync()
        {
            var auditDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.HomologationAuditDetails]).ConfigureAwait(false);
            Assert.AreEqual(ConstantValues.HomologationGroupId, auditDetails["Field"]);
            Assert.AreEqual(this.GetValue(Keys.EntityType).EqualsIgnoreCase(ConstantValues.HomologationWithTwoGroups) ? ConstantValues.ActionDelete : ConstantValues.Insertion, auditDetails["LogType"]);
            Assert.AreEqual(this.GetValue(Keys.HomologationGroupId), auditDetails["NewValue"]);
            ////this.ScenarioContext.Set(nameof(Keys.HomologationGroupId), this.GetValue(Keys.HomologationGroupId));
            ////this.ScenarioContext.Set(nameof(Keys.HomologationId), this.GetValue(Keys.HomologationId));
        }

        private async Task<string> GetHomologationDataAsync(string field1, string field2)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];

            // If field2 is destination, changing the destination system to TRUE and source system to SINOPER. else is not required as json string contains source system as TRUE
            if (field2 == ConstantValues.Destination)
            {
                var trueSystemId = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.TrueSystemId]).ConfigureAwait(false);
                var sinoperSystemId = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.SinoperSystemId]).ConfigureAwait(false);
                content = content.JsonChangePropertyValue(ConstantValues.SourceSystemId, sinoperSystemId[ConstantValues.SystemTypeId].ToPascalCase());
                content = content.JsonChangePropertyValue(ConstantValues.DestinationSystemId, trueSystemId[ConstantValues.SystemTypeId].ToPascalCase());
            }

            // Changing groupTypeId, source or destination value based on field2 value
            var category = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.GroupTypeId], args: new { name = UIContent.Conversion[field1] }).ConfigureAwait(false);
            content = content.JsonChangePropertyValue(ApiContent.Homologation[field1], category[ConstantValues.CategoryId].ToPascalCase());
            this.SetValue(Keys.RandomFieldValue, string.Concat(field1, new Faker().Random.AlphaNumeric(5)));
            content = content.JsonChangePropertyValue(ApiContent.Homologation[field2], this.GetValue(Keys.RandomFieldValue));
            return content;
        }
    }
}
