// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryDataMappingHomologationServiceSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Entities;

    using Flurl;

    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class QueryDataMappingHomologationServiceSteps : EcpApiStepDefinitionBase
    {
        public QueryDataMappingHomologationServiceSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [When(@"I Get record by ""(.*)"" and ""(.*)""")]
        public async System.Threading.Tasks.Task WhenIGetRecordByAndAsync(string field1, string field2)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string idValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            this.ScenarioContext[ApiContent.Ids[entity]] = idValue;
            var correspondingRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupTypeId], args: new { homologationId = idValue }).ConfigureAwait(false);
            string groupTypeIdValue = correspondingRow[ApiContent.Ids[ConstantValues.HomologationGroupTypeId].ToPascalCase()];
            var updatedString = string.Empty;
            if (field2.EqualsIgnoreCase(ConstantValues.HomologationGroupTypeId))
            {
                updatedString = SmartFormat.Smart.Format(ApiContent.Routes[field2], new { homologationId = idValue, groupTypeId = groupTypeIdValue });
            }
            else if (field2.EqualsIgnoreCase(ConstantValues.HomologationGroupName))
            {
                var row = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[field2], args: new { groupTypeId = groupTypeIdValue }).ConfigureAwait(false);
                string groupNameValue = row[ConstantValues.GroupName];
                updatedString = SmartFormat.Smart.Format(ApiContent.Routes[field2], new { homologationId = idValue, groupName = groupNameValue });
            }

            await this.SetResultAsync(() => this.ApiExecutor.GetAsync<dynamic>(this.Endpoint.AppendPathSegment(updatedString), this.UserDetails)).ConfigureAwait(false);
            Assert.IsNotNull(field1);
        }
    }
}
