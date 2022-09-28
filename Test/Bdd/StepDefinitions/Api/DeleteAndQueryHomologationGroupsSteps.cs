// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteAndQueryHomologationGroupsSteps.cs" company="Microsoft">
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

    using Ecp.True.Bdd.Tests.Entities;

    using Flurl;

    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class DeleteAndQueryHomologationGroupsSteps : EcpApiStepDefinitionBase
    {
        public DeleteAndQueryHomologationGroupsSteps(FeatureContext featureContext)
           : base(featureContext)
        {
        }

        [When(@"I delete the existing Homologation Group from ""(.*)""")]
        public async System.Threading.Tasks.Task WhenIDeleteTheExistingAsync(string field)
        {
            var entity = field;
            var updatedString = SmartFormat.Smart.Format(ApiContent.Routes[entity], new { homologationId = this.GetValue(Keys.HomologationId), homologationGroupId = this.GetValue(Keys.HomologationGroupId) });
            await this.SetResultAsync(() => this.ApiExecutor.DeleteWithResponseAsync(this.Endpoint.AppendPathSegment(updatedString), this.UserDetails)).ConfigureAwait(false);
        }

        [Then(@"the data corresponding to the Homologation Group should be cascade removed")]
        public async System.Threading.Tasks.Task ThenTheDataCorrespondingToTheHomologationGroupShouldBeCascadeRemovedAsync()
        {
            var dataMappingRow = await this.ReadAllSqlAsync(input: SqlQueries.GetHomologationDataMapping, args: new { homologationGroupId = this.GetValue<string>(Keys.HomologationGroupId) }).ConfigureAwait(false);
            var homologationObjectRow = await this.ReadAllSqlAsync(input: SqlQueries.GetHomologationObject, args: new { homologationGroupId = this.GetValue<string>(Keys.HomologationGroupId) }).ConfigureAwait(false);
            var homologationGroupRow = await this.ReadAllSqlAsync(input: SqlQueries.GetHomologationGroup, args: new { homologationGroupId = this.GetValue<string>(Keys.HomologationGroupId) }).ConfigureAwait(false);
            Assert.IsEmpty(dataMappingRow);
            Assert.IsEmpty(homologationObjectRow);
            Assert.IsEmpty(homologationGroupRow);
        }

        [Then(@"the respective homologation should also be deleted")]
        public async System.Threading.Tasks.Task ThenTheRespectiveHomologationShouldAlsoBeDeletedAsync()
        {
            var homologationRow = await this.ReadAllSqlAsync(input: SqlQueries.GetHomologation, args: new { homologationId = this.GetValue<string>(Keys.HomologationId) }).ConfigureAwait(false);
            Assert.IsEmpty(homologationRow);
        }

        [When(@"I delete the existing Homologation Group by invalid id from ""(.*)""")]
        public async System.Threading.Tasks.Task WhenIDeleteTheExistingByInvalidIdFromAsync(string field)
        {
            var entity = field;
            var updatedString = SmartFormat.Smart.Format(ApiContent.Routes[entity], new { homologationId = this.GetValue(Keys.HomologationId), homologationGroupId = ConstantValues.InvalidData });
            await this.SetResultAsync(() => this.ApiExecutor.DeleteWithResponseAsync(this.Endpoint.AppendPathSegment(updatedString), this.UserDetails)).ConfigureAwait(false);
        }

        [When(@"I delete the existing ""(.*)"" by invalid id")]
        public async System.Threading.Tasks.Task WhenIDeleteTheExistingByInvalidIdAsync(string field)
        {
            Assert.IsNotNull(field);
            var entity = this.GetValue(Entities.Keys.EntityType);
            var updatedString = SmartFormat.Smart.Format(ApiContent.Routes[entity], new { homologationId = this.GetValue(Keys.HomologationId), homologationGroupId = ConstantValues.InvalidData });
            await this.SetResultAsync(() => this.ApiExecutor.DeleteWithResponseAsync(this.Endpoint.AppendPathSegment(updatedString), this.UserDetails)).ConfigureAwait(false);
        }

        [When(@"I Get all Homologation Group records")]
        public async System.Threading.Tasks.Task WhenIGetAllHomologationGroupRecordsAsync()
        {
            this.Endpoint = this.Endpoint.Replace("/api/v1", "/odata");
            string finalEndPoint = this.Endpoint + ApiContent.Routes[ConstantValues.HomologationGroups];
            await this.SetResultsAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get the record by ""(.*)"" ""(.*)"" and ""(.*)""")]
        public async System.Threading.Tasks.Task WhenIGetTheRecordByAndAsync(string p0, string p1, string p2)
        {
            Assert.IsNotNull(p0);
            Assert.IsNotNull(p1);
            Assert.IsNotNull(p2);
            this.Endpoint = this.Endpoint.Replace("/api/v1", "/odata");
            string finalEndPoint = this.Endpoint + ApiContent.Routes[ConstantValues.HomologationGroupByGroupType];
            await this.SetResultsAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"the response should return all valid odata records")]
        public async System.Threading.Tasks.Task ThenTheResponseShouldReturnAllValidOdataRecordsAsync()
        {
            var apiResults = this.GetValue<dynamic>(Entities.Keys.Results)["value"];
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.VerifyThat(() => Assert.AreEqual(dbResults.Count(), apiResults.Count));
        }

        [Then(@"the response should return requested odata record details")]
        public void ThenTheResponseShouldReturnRequestedOdataRecordDetails()
        {
            var apiResults = this.GetValue<dynamic>(Entities.Keys.Results)["value"][0];
            this.VerifyThat(() => Assert.AreEqual(this.ScenarioContext[ApiContent.Ids[this.GetValue(Entities.Keys.EntityType)].ToPascalCase()], apiResults[ApiContent.Ids[this.GetValue(Entities.Keys.EntityType)]].ToString()));
        }

        [Then(@"""(.*)"" should be registered in the Audit-log for ""(.*)""")]
        public async System.Threading.Tasks.Task ThenShouldBeRegisteredInTheAuditLogForAsync(string logType, string entity)
        {
            var auditDetails = await this.ReadSqlAsStringDictionaryAsync(input: entity.EqualsIgnoreCase(ConstantValues.HomologationGroup) ? SqlQueries.GetAuditLogDetailsForHomologationGroup : SqlQueries.GetAuditLogDetailsOfHomologation).ConfigureAwait(false);
            Assert.AreEqual(entity.EqualsIgnoreCase(ConstantValues.HomologationGroup) ? ConstantValues.HomologationGroupId : ConstantValues.HomologationId, auditDetails["Field"]);
            Assert.AreEqual(logType, auditDetails["LogType"]);
            Assert.AreEqual(this.GetValue(Keys.HomologationGroupId), auditDetails["OldValue"]);
        }

        [When(@"I Get the record by ""(.*)"" ""(.*)"" and invalid ""(.*)""")]
        public async System.Threading.Tasks.Task WhenIGetTheRecordByAndInvalidAsync(string p0, string p1, string p2)
        {
            Assert.IsNotNull(p0);
            Assert.IsNotNull(p1);
            Assert.IsNotNull(p2);
            this.Endpoint = this.Endpoint.Replace("/api/v1", "/odata");
            string finalEndPoint = this.Endpoint + ApiContent.Routes[ConstantValues.HomologationGroupByGroupType];
            finalEndPoint = finalEndPoint.Replace("13", ConstantValues.InvalidData);
            await this.SetResultsAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"the response should return empty data")]
        public void ThenTheResponseShouldReturnEmptyData()
        {
            var apiResults = this.GetValue<dynamic>(Entities.Keys.Results)["value"];
            Assert.IsEmpty(apiResults);
        }
    }
}
