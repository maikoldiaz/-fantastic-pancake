// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementsToBeRecordedToHaveChainInformationSteps.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;

    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class MovementsToBeRecordedToHaveChainInformationSteps : EcpApiStepDefinitionBase
    {
        public MovementsToBeRecordedToHaveChainInformationSteps(FeatureContext featureContext)
        : base(featureContext)
        {
        }

        [When(@"I have (.*) movement when optional attributes are not provided")]
        public void WhenIHaveMovementWhenOptionalAttributesAreNotProvided(int movementsCount)
        {
            this.SetValue(ConstantValues.TestData, "MovementWithOutOptionalAttributes");
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [When(@"I have (.*) movement when DestinationProductId is null and MovementSource has value")]
        public void WhenIHaveMovementWhenDestinationProductIdIsNullAndMovementSourceHasValue(int movementsCount)
        {
            this.SetValue(ConstantValues.TestData, "MovementWithOutDestinationProductIdAndMovementSourceHasValue");
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [Then(@"sourceProductId value should be used as destinationProductId value")]
        public async Task ThenSourceProductIdValueShouldBeUsedAsDestinationProductIdValueAsync()
        {
            await Task.Delay(30000).ConfigureAwait(true);
            Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(input: SqlQueries.GetCountOfMovementsWhenDestinationProductIdIsSameAsSourceProductId, args: new { movementId = this.GetValue(ConstantValues.MovementId) }).ConfigureAwait(false));
        }

        [When(@"I have (.*) movement when DestinationProductId is null and MovementSource has no value")]
        public void WhenIHaveMovementWhenDestinationProductIdIsNullAndMovementSourceHasNoValue(int movementsCount)
        {
            this.SetValue(ConstantValues.TestData, "MovementWithOutDestinationProductIdAndMovementSourceHasNoValue");
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [Given(@"I have records in movements table")]
        public void GivenIHaveRecordsInMovementsTable()
        {
            // Method intentionally left empty.
        }

        [Then(@"for all old records in movements scenarioId should be (.*)")]
        public async Task ThenForAllOldRecordsInMovementsScenarioIdShouldBeAsync(int value)
        {
            Assert.IsEmpty(await this.ReadAllSqlAsync(input: SqlQueries.GetInventoriesOtherthanOperativoScenarioForOldRecords, args: new { scenarioId = value }).ConfigureAwait(false));
        }

        [Given(@"I have source sytem category in the system")]
        public async Task GivenIHaveSourceSytemCategoryInTheSystemAsync()
        {
            var categoryDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategory, args: new { categoryId = 22 }).ConfigureAwait(false);
            Assert.AreEqual(ConstantValues.SistemaOrigen, categoryDetails[ConstantValues.Name]);
        }

        [Then(@"""(.*)"" and ""(.*)"" data should present")]
        public async Task ThenAndDataShouldPresentAsync(string name, string description, Table table)
        {
            var i = 0;
            var sourceSytemDetails = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetSourceSystemElements).ConfigureAwait(false);
            foreach (var row in table?.Rows.Select((value) => (Name: value[name], Description: value[description])))
            {
                Assert.AreEqual(sourceSytemDetails.ToDictionaryList().ElementAt(i)[ConstantValues.Name], row.Name);
                Assert.AreEqual(sourceSytemDetails.ToDictionaryList().ElementAt(i)[ConstantValues.Description], row.Description);
                i++;
            }
        }
    }
}
