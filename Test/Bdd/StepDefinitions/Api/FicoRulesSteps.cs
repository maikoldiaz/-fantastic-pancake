// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FicoRulesSteps.cs" company="Microsoft">
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

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.DataSources;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json.Linq;

    using TechTalk.SpecFlow;

    [Binding]
    public class FicoRulesSteps : EcpApiStepDefinitionBase
    {
        private readonly AppInsightsDataSource appinsights = new AppInsightsDataSource();

        public FicoRulesSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Given(@"I have Fico Connection setup into the system")]
        public async Task GivenIHaveFicoConnectionSetupIntoTheSystemAsync()
        {
            await this.IHaveFicoConnectionSetupIntoTheSystemAsync().ConfigureAwait(false);
        }

        [When(@"I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: ""(.*)"" and estado: ""(.*)""")]
        public async Task WhenIInvokeTheFICOServiceToFetchTheStrategiesWithInputParameterTipoLlamadaAndEstadoAsync(string firstValue, string secondvalue)
        {
            await this.IInvokeTheFICOServiceToFetchTheStrategiesWithInputParameterTipoLlamadaAndEstadoAsync(firstValue, secondvalue).ConfigureAwait(false);
        }

        [Then(@"Validate that response are successfully loading into the table at ""(.*)"" Level for which id is ""(.*)""")]
        public async Task ThenValidateThatResponseAreSuccessfullyLoadingIntoTheTableAtLevelForWhichIdIsAsync(string level, string tokenid)
        {
            await this.ValidateThatResponseAreSuccessfullyLoadingIntoTheTableAtLevelForWhichIdIsAsync(level, tokenid).ConfigureAwait(false);
        }

        [When(@"FICO service responds with success response code")]
        public void WhenFICOServiceRespondsWithSuccessResponseCode()
        {
            ////this step was left intentional
        }

        [Then(@"verify that auditedStep from the response should be stored in the app insights")]
        public async Task ThenVerifyThatAuditedStepFromTheResponseShouldBeStoredInTheAppInsightsAsync()
        {
            var result = await this.appinsights.ReadAllAsync<dynamic>(Queries.FetchFicoAuditLogs).ConfigureAwait(false);
            var appInsightsResult = JObject.Parse(result.ToString());
            var appInsightskeyLayer = appInsightsResult["tables"][0]["rows"][1][1].ToString().Replace("Audited Steps: ", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
            Assert.IsNotNull(appInsightskeyLayer);
        }

        [Then(@"verify that response log time should be stored in the app insights")]
        public async Task ThenVerifyThatResponseLogTimeShouldBeStoredInTheAppInsightsAsync()
        {
            var result = await this.appinsights.ReadAllAsync<dynamic>(Queries.FetchFicoResponseTime).ConfigureAwait(false);
            var appInsightsResult = JObject.Parse(result.ToString());
            var appInsightskeyLayer = appInsightsResult["tables"][0]["rows"][1][1].ToString();
            Assert.IsNotNull(appInsightskeyLayer);
            Assert.AreEqual(ConstantValues.ResponseLogMessage, appInsightskeyLayer.Substring(0, 73));
        }
    }
}
