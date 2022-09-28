// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FicoValidationErrors.cs" company="Microsoft">
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
    using System.Threading;
    using System.Threading.Tasks;
    using AventStack.ExtentReports.Utils;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class FicoValidationErrors : EcpWebStepDefinitionBase
    {
        [Given(@"I have ticket with ""(.*)"" error")]
        public async Task GivenIHaveTicketWithErrorAsync(string error)
        {
            if (error.EqualsIgnoreCase("Technical"))
            {
                await this.ReadSqlAsStringDictionaryAsync(SqlQueries.InsertTicketForDeltaError).ConfigureAwait(false);
                var ticket = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTicketsByIdDesc).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.TicketId] = ticket["TicketId"];
            }
            else
            {
                await this.ReadSqlAsStringDictionaryAsync(SqlQueries.InsertTicketForDeltaBusinessError).ConfigureAwait(false);
                var ticket = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTicketsByIdDesc).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.TicketId] = ticket["TicketId"];
                var movementtransID = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementTransId).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.MovementTransactionId] = movementtransID["MovementTransactionId"];
                var inventoryProdID = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetInventoryProdId).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.InventoryProductId] = inventoryProdID["InventoryProductId"];
                await this.ReadSqlScalarAsync<int>(SqlQueries.InsertIntoDeltaErrorTable, args: new { Ticket = this.ScenarioContext[ConstantValues.TicketId], MovementTransID = this.ScenarioContext[ConstantValues.MovementTransactionId], InventoryProdID = this.ScenarioContext[ConstantValues.InventoryProductId] }).ConfigureAwait(false);
            }
        }

        [When(@"I click on error icon and see the error popup ""(.*)"" appear")]
        public void WhenIClickOnErrorIconAndSeeTheErrorPopupAppear(string errorMessage)
        {
            string error = errorMessage;
            Assert.IsTrue(this.IsDeltaTechErrorPopupAvailable(error));
        }

        public bool IsDeltaTechErrorPopupAvailable(string errorMessage)
        {
            string ticketId = this.ScenarioContext[ConstantValues.TicketId].ToString();
            if (errorMessage.EqualsIgnoreCase("Technical"))
            {
                this.Get<ElementPage>().SetValue(nameof(Resources.DeltaCalculationFilter), ticketId);
                this.Get<ElementPage>().SendEnterKey(nameof(Resources.DeltaCalculationFilter));
                Thread.Sleep(3000);
                this.Get<ElementPage>().Click(nameof(Resources.DeltaTicketTechErrorIcon));
                if (this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.DeltaTechError)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                this.Get<ElementPage>().SetValue(nameof(Resources.DeltaCalculationFilter), ticketId);
                this.Get<ElementPage>().SendEnterKey(nameof(Resources.DeltaCalculationFilter));
                Thread.Sleep(3000);
                this.Get<ElementPage>().Click(nameof(Resources.DeltaTicketBusinessErrorIcon));
                if (this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.DeltaBusinessError)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [When(@"I see the error message ""(.*)"" in popup wizard")]
        public void WhenISeeTheErrorMessageInPopupWizard(string errorMessage)
        {
            if (!errorMessage.IsNullOrEmpty())
            {
                Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.DeltaTechErrorText)).Displayed);
                Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.DeltaTechErrorText1)).Displayed);
            }
        }

        [When(@"I should see Segmento as a section header")]
        public void WhenIShouldSeeSegmentoAsASectionHeader()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.DeltaErrorSegmentHeader)).Displayed);
        }

        [Then(@"I see the error popup closed on click of Aceptar")]
        public void WhenISeeTheErrorPopupClosedOnClickOfAceptar()
        {
            this.Get<ElementPage>().Click(nameof(Resources.DeltaCalAcceptBtn));
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.NewDeltabtn)).Displayed);
        }
    }
}