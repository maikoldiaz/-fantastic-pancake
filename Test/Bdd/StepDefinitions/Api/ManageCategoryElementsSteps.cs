// <copyright file="ManageCategoryElementsSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace Ecp.True.Bdd.Tests.StepDefinitions.Api
{
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class ManageCategoryElementsSteps : EcpApiStepDefinitionBase
    {
        public ManageCategoryElementsSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Then(@"the response should fail with messages ""(.*)"", ""(.*)"", ""(.*)""")]
        public async System.Threading.Tasks.Task ThenTheResponseShouldFailWithMessagesAsync(string expectedMessage1, string expectedMessage2, string expectedMessage3)
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.VerifyThat(() => Assert.AreEqual(dbResults.Count(), this.GetValue<dynamic>(Entities.Keys.InitialRowCount)));
            var jobjResult = JObject.Parse(await this.GetValue<dynamic>(Entities.Keys.Error).ReadAsStringAsync().ConfigureAwait(false));
            this.VerifyThat(() => Assert.AreEqual(expectedMessage1, jobjResult["errorCodes"][0]["message"].ToString()));
            this.VerifyThat(() => Assert.AreEqual(expectedMessage2, jobjResult["errorCodes"][1]["message"].ToString()));
            this.VerifyThat(() => Assert.AreEqual(expectedMessage3, jobjResult["errorCodes"][2]["message"].ToString()));
        }

        [Given(@"there are ""(.*)"" where Active column is True in the system")]
        public void GivenThereAreWhereColumnIsTrueInTheSystem(string entity)
        {
            this.SetValue(Entities.Keys.Route, ApiContent.Routes[entity]);
            var content = ApiContent.Updates[entity];
            this.ScenarioContext[ApiContent.Ids[entity]] = content.JsonGetValue(ApiContent.Ids[entity]);
        }

        [When(@"I Get records with Active field is True")]
        public async Task WhenIGetWithFieldIsAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            this.Endpoint = this.Endpoint.Replace("api", "odata");
            await this.SetResultsAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity].Contains("{0}") ? string.Format(CultureInfo.InvariantCulture, ApiContent.Routes[entity], this.ScenarioContext[ApiContent.Ids[entity]]) : ApiContent.Routes[entity])).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"the response should return requested records where Active column is True")]
        public async System.Threading.Tasks.Task ThenTheResponseShouldReturnRequestedCategoriesElementsDetailsWhereActiveColumnIsTrueAsync()
        {
            var apiResults = this.GetValue<dynamic>(Entities.Keys.Results);
            var jObjectResult = JObject.Parse(apiResults.ToString());
            var content = jObjectResult.value;
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            Assert.AreEqual(dbResults.Count(), content.Count);
        }

        [Given(@"I don't have any Active ""(.*)"" in the system")]
        public async System.Threading.Tasks.Task GivenIDonTHaveAnyActiveInTheSystemAsync(string entity)
        {
            this.SetValue(Entities.Keys.Route, ApiContent.Routes[entity]);
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
            this.ScenarioContext[ApiContent.Ids[entity]] = "0";
        }
    }
}
