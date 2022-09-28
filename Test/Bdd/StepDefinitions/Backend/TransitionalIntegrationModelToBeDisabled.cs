// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransitionalIntegrationModelToBeDisabled.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class TransitionalIntegrationModelToBeDisabled : EcpApiStepDefinitionBase
    {
        public TransitionalIntegrationModelToBeDisabled(FeatureContext featureContext)
          : base(featureContext)
        {
        }

        [When(@"I have ""(.*)"" disabled")]
        public void WhenIHaveDisabled(string app)
        {
            Assert.IsNotNull(app);
        }

        [When(@"I register ""(.*)"" in the system through SINOPER")]
        public async Task WhenRegisterInTheSystemThroughSINOPERAsync(string entity)
        {
            ////this.Given(string.Format(CultureInfo.InvariantCulture, "I want to register an \"{0}\" in the system", entity));
            await this.IWantToRegisterAnInTheSystemAsync(entity).ConfigureAwait(false);
            this.UpdateXmlDefaultValue(entity);
            await this.UploadSinoperXmlAsync(entity).ConfigureAwait(false);
        }

        [When(@"validate ""(.*)"" not registered through SINOPER")]
        public async Task ThenValidateNotRegisteredThroughSINOPERAsync(string entity)
        {
            Assert.IsNotNull(entity);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["FileRegistrationUsingUploadID"], args: new { uploadId = this.ScenarioContext[ConstantValues.MessageId] }).ConfigureAwait(false);
            Assert.IsNull(lastCreatedRow);
        }

        [StepDefinition(@"I register ""(.*)"" in the system through SAP PO")]
        public async Task ThenIRegisterInTheSystemThroughSAPPOAsync(string entity)
        {
            ////this.Given(string.Format(CultureInfo.InvariantCulture, "I have data to process \"{0}\" in system", entity));
            await this.GivenIHaveInventoryOrMovementDataToProcessInSystemAsync(entity).ConfigureAwait(false);
            if (entity.EqualsIgnoreCase("Movements"))
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

            ////this.And(string.Format(CultureInfo.InvariantCulture, "I register \"{0}\" in system", entity));
            await this.IRegisterInventoriesOrMovementsInSystemThroughSappoAsync(entity).ConfigureAwait(false);
        }

        [Then(@"validate ""(.*)"" registered through SAP PO")]
        public async Task ThenValidateRegisteredThroughSAPPOAsync(string entity)
        {
            Assert.IsNotNull(entity);
            /////this.Then("response should be successful");
            await this.ResponseShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        [When(@"I have '(.*)' and '(.*)' disabled")]
        [When(@"I have ""(.*)"" and ""(.*)"" enabled")]
        public void WhenIHaveAndEnabled(string app1, string app2)
        {
            Assert.IsNotNull(app1);
            Assert.IsNotNull(app2);
        }

        [When(@"validate ""(.*)"" registered through SINOPER")]
        public async Task ThenValidateRegisteredThroughSINOPERAsync(string entity)
        {
            Assert.IsNotNull(entity);
            ////this.And(string.Format(CultureInfo.InvariantCulture, "the \"{0}\" field is equal to \"{1}\"", "EventType", "Insert"));
            this.ScenarioContext[ConstantValues.EventType] = ApiContent.HomologateLogType["Insert"];
            ////this.Then("it should be registered");
            await this.ShouldBeRegisteredAsync().ConfigureAwait(false);
        }
    }
}
