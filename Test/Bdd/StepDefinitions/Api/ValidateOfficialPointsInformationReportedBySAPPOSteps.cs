// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateOfficialPointsInformationReportedBySAPPOSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class ValidateOfficialPointsInformationReportedBySappoSteps : EcpApiStepDefinitionBase
    {
        public ValidateOfficialPointsInformationReportedBySappoSteps(FeatureContext featureContext)
    : base(featureContext)
        {
        }

        [When(@"the json object do not have the defined structure")]
        public async Task WhenTheJsonObjectDoNotHaveTheDefinedStructureAsync()
        {
            await this.ValidateOfficialPointAsync("MalformedJson").ConfigureAwait(false);
        }

        [When(@"the json object do not have the data types defined")]
        public async Task WhenTheJsonObjectDoNotHaveTheDataTypesDefinedAsync()
        {
            await this.ValidateOfficialPointAsync("DataTypeError").ConfigureAwait(false);
        }

        [Then(@"the response should fail with technical exception")]
        public async Task ThenTheResponseShouldFailWithTechnicalExceptionAsync()
        {
            var jsonResult = JObject.Parse(await this.GetValue<dynamic>(Entities.Keys.Error).ReadAsStringAsync().ConfigureAwait(false));
            var actualMessage = (jsonResult["message"] == null) ? jsonResult["errorCodes"][0]["message"].ToString() : jsonResult["message"].ToString();
            this.VerifyThat(() => StringAssert.Contains("Cannot deserialize the current JSON object", actualMessage));
        }

        [Then(@"the response should fail with message ""(.*)"" and with relevant movementid")]
        public async Task ThenTheResponseShouldFailWithMessageAndWithRelevantMovementidAsync(string expectedMessage)
        {
            var jsonResult = JObject.Parse(await this.GetValue<dynamic>(Entities.Keys.Error).ReadAsStringAsync().ConfigureAwait(false));
            var actualMessage = (jsonResult["message"] == null) ? jsonResult["errorCodes"][0]["message"].ToString() : jsonResult["message"].ToString();
            expectedMessage = expectedMessage == null ? expectedMessage : expectedMessage.Replace("(Movement Id)", this.ScenarioContext["movementId"].ToString());
            this.VerifyThat(() => Assert.AreEqual(expectedMessage, actualMessage));
        }

        [Then(@"the response should fail with data type related technical exception")]
        public async Task ThenTheResponseShouldFailWithDataTypeRelatedTechnicalExceptionAsync()
        {
            var jsonResult = JObject.Parse(await this.GetValue<dynamic>(Entities.Keys.Error).ReadAsStringAsync().ConfigureAwait(false));
            var actualMessage = (jsonResult["message"] == null) ? jsonResult["errorCodes"][0]["message"].ToString() : jsonResult["message"].ToString();
            this.VerifyThat(() => StringAssert.Contains("Could not convert string to DateTime", actualMessage));
        }

        [When(@"the json object has more than two objects")]
        [When(@"the movements do not have the officialInformation collection")]
        [When(@"two movements arrive and both are marked as official points")]
        [When(@"only one movement arrives and the movement was not reported to SAP")]
        public async Task WhenTheJsonObjectHasMoreThanTwoObjectsAsync()
        {
            await this.ValidateOfficialPointAsync("NoneOfTheOptions").ConfigureAwait(false);
        }

        [When(@"any of the movements do not have the global identifier")]
        public async Task WhenAnyOfTheMovementsDoNotHaveTheGlobalIdentifierAsync()
        {
            await this.ValidateOfficialPointAsync("GlobalIdentifierMissing").ConfigureAwait(false);
        }

        [When(@"two movements arrive and only one of them is marked as an official point but does not have the backup movement identifier")]
        public async Task WhenTwoMovementsArriveAndOnlyOneOfThemIsMarkedAsAnOfficialPointButDoesNotHaveTheBackupMovementIdentifierAsync()
        {
            await this.ValidateOfficialPointAsync("BackupMovementIdentifierMissing").ConfigureAwait(false);
        }

        [When(@"two movements arrive and only one of them is marked as an official point but it has the incorrect backup movement identifier because this does not match the movement identifier marked as unofficial")]
        public async Task WhenTwoMovementsArriveAndOnlyOneOfThemIsMarkedAsAnOfficialPointButItHasTheIncorrectBackupMovementIdentifierBecauseThisDoesNotMatchTheMovementIdentifierMarkedAsUnofficialAsync()
        {
            await this.ValidateOfficialPointAsync("IncorrectBackupMovementIdentifier").ConfigureAwait(false);
        }

        [When(@"two movements arrive and the global identifier of them is different")]
        public async Task WhenTwoMovementsArriveAndTheGlobalIdentifierOfThemIsDifferentAsync()
        {
            await this.ValidateOfficialPointAsync("BackupMovementIdentifierMismatch").ConfigureAwait(false);
        }

        [When(@"only one movement arrives and is not marked as official")]
        public async Task WhenOnlyOneMovementArrivesAndIsNotMarkedAsOfficialAsync()
        {
            await this.ValidateOfficialPointAsync("OneMovementNotMarkedOfficial").ConfigureAwait(false);
        }

        [When(@"only one movement arrives and the movement is marked as official and it has global identifier but is not registered in TRUE")]
        public async Task WhenOnlyOneMovementArrivesAndTheMovementIsMarkedAsOfficialAndItHasGlobalIdentifierButIsNotRegisteredInTRUEAsync()
        {
            await this.ValidateOfficialPointAsync("OneMovementNotRegistered").ConfigureAwait(false);
        }

        [When(@"two movements arrive and both are not registered in TRUE")]
        public async Task WhenTwoMovementsArriveAndBothAreNotRegisteredInTRUEAsync()
        {
            await this.ValidateOfficialPointAsync("TwoMovementsNotRegistered").ConfigureAwait(false);
        }

        [When(@"two movements arrive and both the movements have different data on the source node destination node source product and the destination product")]
        public async Task WhenTwoMovementsArriveAndBothTheMovementsHaveDifferentDataOnTheSourceNodeDestinationNodeSourceProductAndTheDestinationProductAsync()
        {
            await this.ValidateOfficialPointAsync("TwoMovementsDifferentData").ConfigureAwait(false);
        }

        [When(@"two movements arrive and both of the movements were not reported to SAP")]
        public async Task WhenTwoMovementsArriveAndBothOfTheMovementsWereNotReportedToSAPAsync()
        {
            await this.ValidateOfficialPointAsync("TwoMovementsNotReportedToSAP").ConfigureAwait(false);
        }

        [Then(@"the response should return a successful status code and processing of movements should happen")]
        public void ThenTheResponseShouldReturnASuccessfulStatusCodeAndProcessingOfMovementsShouldHappen()
        {
            this.VerifyThat(() => Assert.IsNull(this.GetValue<dynamic>(Entities.Keys.Results)));
        }

        [When(@"only one movement arrives and the movement has incorrect data")]
        public async Task WhenOnlyOneMovementArrivesAndTheMovementHasIncorrectDataAsync()
        {
            await this.ValidateOfficialPointAsync("OneMovementIncorrectData").ConfigureAwait(false);
        }

        [When(@"two movements arrive and both the movements have incorrect data")]
        public async Task WhenTwoMovementsArriveAndBothTheMovementsHaveIncorrectDataAsync()
        {
            await this.ValidateOfficialPointAsync("TwoMovementsIncorrectData").ConfigureAwait(false);
        }

        [When(@"all the validations are met")]
        public async Task WhenAllTheValidationsAreMetAsync()
        {
            await this.ValidateOfficialPointAsync("AllValidationsMet").ConfigureAwait(false);
        }
    }
}