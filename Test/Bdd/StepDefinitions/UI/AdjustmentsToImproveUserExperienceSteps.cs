// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdjustmentsToImproveUserExperienceSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using TechTalk.SpecFlow;

    [Binding]
    public class AdjustmentsToImproveUserExperienceSteps : EcpWebStepDefinitionBase
    {
        [When(@"I have filter data")]
        public async Task WhenIHaveFilterDataAsync()
        {
            var ticketRow = await this.ReadSqlAsStringDictionaryAsync(ApiContent.LastCreated[ConstantValues.Ticket]).ConfigureAwait(false);
            this.Get<ElementPage>().FindElementByXPath(Resources.TicketIdFilter).SendKeys(ticketRow[ConstantValues.TicketId]);
            this.Get<ElementPage>().GetElement(Resources.TicketIdFilter).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [Then(@"I verify that the source node selected is not in the target list dropdown")]
        public void ThenIVerifyThatTheSourceNodeSelectedIsNotInTheTargetListDropdown()
        {
            this.Get<ElementPage>().GetElement(Resources.FilterForNodeName).SendKeys(this.GetValue(ConstantValues.Node1Name));
        }
    }
}
