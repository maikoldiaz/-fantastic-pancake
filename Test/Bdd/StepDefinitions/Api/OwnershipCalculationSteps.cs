// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipCalculationSteps.cs" company="Microsoft">
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
    using System.Configuration;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using Newtonsoft.Json.Linq;

    using TechTalk.SpecFlow;

    [Binding]
    public class OwnershipCalculationSteps : EcpApiStepDefinitionBase
    {
        private string ownershipURL;

        public OwnershipCalculationSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Given(@"I want to create an ""(.*)"" in the system")]
        public void GivenIWantToCreateAnInTheSystem(string ownership)
        {
            this.ownershipURL = ConfigurationManager.AppSettings.Get(ownership);
            this.SetValue(Entities.Keys.EntityType, ApiContent.Creates[ownership]);
        }

        [When(@"I provide the required attributes")]
        public async Task WhenIProvideTheRequiredFieldsForTheAsync()
        {
          var entity = this.GetValue(Entities.Keys.EntityType);
          await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.ownershipURL, JObject.Parse(entity)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [StepDefinition(@"response should be successful")]
        public async Task ThenResponseShouldBeSuccessfulAsync()
        {
            await this.ResponseShouldBeSuccessfulAsync().ConfigureAwait(false);
        }
    }
}
