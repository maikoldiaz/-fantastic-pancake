// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModificationsInAdfPipelineStep.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Backend
{
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ModificationsInAdfPipelineStep : EcpApiStepDefinitionBase
    {
        public ModificationsInAdfPipelineStep(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Then(@"Modified Records from ""(.*)"" to ""(.*)"" date should be loaded into ""(.*)""")]
        public async Task ThenModifiedRecordsFromToDateShouldBeLoadedIntoAsync(string fromDate, string toDate, string fileName)
        {
            Assert.AreEqual("Succeeded", this.ScenarioContext[ConstantValues.Status]);
            var dataRow = await this.ReadSqlScalarAsync<int>(ApiContent.CountsWithDifferentOperationalDate[fileName], args: new { startDate = fromDate, endDate = toDate }).ConfigureAwait(false);
            Assert.IsTrue(dataRow == 7);
        }

        [Then(@"Modified Records from ""(.*)"" to ""(.*)"" date should not be loaded into ""(.*)""")]
        public async Task ThenModifiedRecordsFromToDateShouldNotBeLoadedIntoAsync(string fromDate, string toDate, string fileName)
        {
            Assert.AreEqual("Succeeded", this.ScenarioContext[ConstantValues.Status]);
            var dataRow = await this.ReadSqlScalarAsync<int>(ApiContent.CountsWithDifferentOperationalDate[fileName], args: new { startDate = fromDate, endDate = toDate }).ConfigureAwait(false);
            Assert.IsTrue(dataRow == 0);
        }

        [When(@"I initiate the data load process with load type as (.*)")]
        public async Task WhenIInitiateTheDataLoadProcessWithLoadTypeAsync(int parameterValue)
        {
            var name = this.ScenarioContext[ConstantValues.FileName].ToString();
            this.ScenarioContext[ConstantValues.Status] = await ADataFactoryClient.RunADFPipelineAsync(ApiContent.Pipeline[name], parameterValue).ConfigureAwait(false);
        }

        [Then(@"Modified Records other than ""(.*)"" to ""(.*)"" date should be stored into ""(.*)""")]
        public async Task ThenModifiedRecordsOtherThanToDateShouldBeStoredIntoAsync(string fromDate, string toDate, string fileName)
        {
            Assert.AreEqual("Succeeded", this.ScenarioContext[ConstantValues.Status]);
            var dataRow = await this.ReadSqlScalarAsync<int>(ApiContent.CountsWithOtherOperationalDate[fileName], args: new { startDate = fromDate, endDate = toDate }).ConfigureAwait(false);
            Assert.IsTrue(dataRow == 3);
        }

        [Given(@"I have deleted the data from Analytics Table stored previously")]
        public async Task GivenIHaveDeletedTheDataFromAnalyticsTableStoredPreviouslyAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.DeletePreviouslyloadAnalticsDataWithOwnership).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.DeletePreviouslyloadAnalticsDataWithoutOwnership).ConfigureAwait(false);
        }
    }
}