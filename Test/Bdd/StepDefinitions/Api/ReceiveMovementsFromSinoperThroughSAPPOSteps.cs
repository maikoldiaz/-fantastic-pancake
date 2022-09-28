// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReceiveMovementsFromSinoperThroughSAPPOSteps.cs" company="Microsoft">
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
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using TechTalk.SpecFlow;

    [Binding]
    public class ReceiveMovementsFromSinoperThroughSappoSteps : EcpApiStepDefinitionBase
    {
        //// private static readonly NameValueCollection Settings = ConfigurationManager.GetSection("SAPPOApi") as NameValueCollection;

        public ReceiveMovementsFromSinoperThroughSappoSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Given(@"I have data to process ""(.*)"" in system")]
        [Given(@"I have ""(.*)"" in the application")]
        public async Task GivenIHaveDataToProcessInSystemAsync(string entity)
        {
            await this.GivenIHaveInventoryOrMovementDataToProcessInSystemAsync(entity).ConfigureAwait(false);
        }

        [StepDefinition(@"I have (.*) movement")]
        [StepDefinition(@"I have (.*) movements")]
        public void WhenIHaveMovements(int movementsCount)
        {
            this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
            this.CommonMethodForMovementRegistration(movementsCount);
        }

        [When(@"I register ""(.*)"" with existing MovementId")]
        [When(@"I register ""(.*)"" with existing InventoryId")]
        [When(@"I register ""(.*)"" with exiting InventoryId and BatchId")]
        [When(@"I register ""(.*)"" in system")]
        [When(@"I register ""(.*)"" in system with all homologable attributes")]
        public async Task WhenIRegisterInSystemAsync(string entity)
        {
            await this.WhenIRegisterInventoriesOrMovementsInSystemThroughSappoAsync(entity).ConfigureAwait(false);
        }

        [When(@"I don't provide ""(.*)"" in json")]
        public void WhenIDonTProvideInJson(string field)
        {
            JArray movementArray = new JArray();
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            content = content.JsonChangePropertyValue(field);
            movementArray.Add(JsonConvert.DeserializeObject(content));
            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = movementArray;
        }

        [When(@"I provide ""(.*)"" with different ""(.*)""")]
        public void WhenIProvideWithDifferent(string field, string type)
        {
            JArray movementArray = new JArray();
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            if (type == "int")
            {
                content = content.JsonChangePropertyValue(field, new Faker().Random.Number(9999, 999999).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == "string")
            {
                content = content.JsonChangePropertyValue(field, new Faker().Random.Number(9999, 999999));
            }

            movementArray.Add(JsonConvert.DeserializeObject(content));
            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = movementArray;
        }

        [Given(@"I have ""(.*)"" in the system for SAP PO")]
        public async Task GivenIHaveInTheSystemForSAPPOAsync(string entity)
        {
            ////this.Given($"I have data to process \"{entity}\" in system");
            await this.GivenIHaveInventoryOrMovementDataToProcessInSystemAsync(entity).ConfigureAwait(false);
            if (entity.EndsWithIgnoreCase("Movements"))
            {
                ////this.When("I have 1 movement");
                this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                this.CommonMethodForMovementRegistration(1);
            }
            else
            {
                ////this.When("I have 1 inventory");
                this.SetValue(ConstantValues.TestData, "WithScenarioId");
                this.SetValue(ConstantValues.BATCHID, new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
                this.CommonMethodForInventoryRegistration(1);
            }

            ////this.When($"I register \"{entity}\" in system");
            await this.IRegisterInventoriesOrMovementsInSystemThroughSappoAsync(entity).ConfigureAwait(false);
            ////this.Then("response should be successful");
            await this.ResponseShouldBeSuccessfulAsync().ConfigureAwait(false);
        }
    }
}