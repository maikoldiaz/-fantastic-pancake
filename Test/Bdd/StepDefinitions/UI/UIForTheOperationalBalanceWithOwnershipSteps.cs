// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIForTheOperationalBalanceWithOwnershipSteps.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TechTalk.SpecFlow;

    [Binding]
    public class UIForTheOperationalBalanceWithOwnershipSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have ownershipcalculated segment")]
        public async Task GivenIHaveOwnershipcalculatedSegmentAsync()
        {
            await this.IHaveOwnershipcalculatedSegmentAsync().ConfigureAwait(false);
        }

        [Then(@"I should see balance summary with ownership corresponding to ownershipnodeid")]
        public async Task ThenIShouldSeeBalanceSummaryWithOwnershipCorrespondingToOwnershipnodeidAsync()
        {
            var ownershipNode = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetOwnershipNodeId, args: new { ticketId = this.GetValue(ConstantValues.TicketId), nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            this.SetValue("OWNERSHIPNODEID", ownershipNode["OWNERSHIPNODEID"]);
            var expectedEditOwnershipCalculation = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetEditOwnershipNodeCalculationInformation, args: new { ownershipNodeId = this.GetValue("OWNERSHIPNODEID") }).ConfigureAwait(false);
            if (int.Parse(expectedEditOwnershipCalculation[ConstantValues.Control], CultureInfo.InvariantCulture) == 0)
            {
                expectedEditOwnershipCalculation[ConstantValues.Control] = ConstantValues.Error;
            }

            var page = this.Get<ElementPage>();
            var actualEditOwnershipCalculation = new Dictionary<string, string>()
            {
                { "InitialInventory", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 3).Text },
                { "Inputs", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 4).Text },
                { "Outputs", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 5).Text },
                { "IdentifiedLosses", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 6).Text },
                { "Interface", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 7).Text },
                { "Tolerance", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 8).Text },
                { "UnidentifiedLosses", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 9).Text },
                { "FinalInventory", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 10).Text },
                { "Volume", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 11).Text },
                { "MeasurementUnit", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 12).Text },
                { "Control", page.GetElement(nameof(Resources.EditOwnershipCalculationInformation), formatArgs: 13).Text },
            };
            Assert.IsTrue(this.VerifyDiffs(expectedEditOwnershipCalculation, actualEditOwnershipCalculation));
        }

        [When(@"the % Control of the node is greater than the % Acceptable Balance configured for the node")]
        public async Task WhenTheControlOfTheNodeIsGreaterThanTheAcceptableBalanceConfiguredForTheNodeAsync()
        {
            this.SetValue("ControlPercentage", this.Get<ElementPage>().GetElement(nameof(Resources.ControlPercentageInEditOwnershipGrid)).Text);
            var controlPercentage = double.Parse(this.GetValue("ControlPercentage").Trim('%'), CultureInfo.InvariantCulture);
            var acceptableBalancePercentage = double.Parse(this.GetValue(ConstantValues.AcceptableBalancePercentageTitle), CultureInfo.InvariantCulture);
            var updatePercentage = controlPercentage - 0.0001;
            if (controlPercentage < acceptableBalancePercentage)
            {
                await this.ReadSqlAsDictionaryAsync(input: SqlQueries.UpdateAcceptableBalancePercentage, args: new { acceptableBalancePercentage = updatePercentage, nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
                this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            }
        }

        [When(@"the % Control of the node is less than or equal to the % Acceptable Balance configured for the node")]
        public async Task WhenTheControlOfTheNodeIsLessThanOrEqualToTheAcceptableBalanceConfiguredForTheNodeAsync()
        {
            this.SetValue("ControlPercentage", this.Get<ElementPage>().GetElement(nameof(Resources.ControlPercentageInEditOwnershipGrid)).Text);
            var controlPercentage = double.Parse(this.GetValue("ControlPercentage").Trim('%'), CultureInfo.InvariantCulture);
            var acceptableBalancePercentage = double.Parse(this.GetValue(ConstantValues.AcceptableBalancePercentageTitle), CultureInfo.InvariantCulture);
            var updatePercentage = controlPercentage + 0.0001;
            if (controlPercentage > acceptableBalancePercentage)
            {
                await this.ReadSqlAsDictionaryAsync(input: SqlQueries.UpdateAcceptableBalancePercentage, args: new { acceptableBalancePercentage = updatePercentage, nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
                this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            }
        }

        [When(@"node state is equal to below status")]
        public async Task WhenNodeStateIsEqualToBelowStatusAsync(Table table)
        {
            foreach (var row in table?.Rows.Select((value) => (Default: value["Status"], Unused: string.Empty)))
            {
                if (this.GetValue("NodeStatus") != row.Default)
                {
                    await this.ReadSqlAsDictionaryAsync(input: SqlQueries.UpdateOwnershipNodeState, args: new { name = row.Default, nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
                    this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                    this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                }

                ////this.Then("verify that \"OwnershipNodeDetails\" \"SubmitToApproval\" \"link\" is \"enabled\"");
                this.VerifyThatIs("OwnershipNodeDetails\" \"SubmitToApproval", "link", "enabled");
            }
        }

        [When(@"node state is equal to following status")]
        public async Task WhenNodeStateIsEqualToFollowingStatusAsync(Table table)
        {
            foreach (var row in table?.Rows.Select((value) => (Default: value["Status"], Unused: string.Empty)))
            {
                if (this.GetValue("NodeStatus") != row.Default)
                {
                    await this.ReadSqlAsDictionaryAsync(input: SqlQueries.UpdateOwnershipNodeState, args: new { name = row.Default, nodeId = this.GetValue("NodeId_1"), ticketId = this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false);
                    this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                    this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                }

                ////this.When("I click on \"Actions\" \"combobox\"");
                this.IClickOn("Actions", "combobox");
                ////this.Then("verify that \"OwnershipNodeDetails\" \"ViewReport\" \"link\" is \"enabled\"");
                this.VerifyThatIs("OwnershipNodeDetails\" \"ViewReport", "link", "enabled");
            }
        }

        [When(@"node state is equal to ""(.*)""")]
        public async Task WhenNodeStateIsEqualToAsync(string nodeState)
        {
            if (this.GetValue("NodeStatus") != nodeState)
            {
                await this.ReadSqlAsDictionaryAsync(input: SqlQueries.UpdateOwnershipNodeState, args: new { name = nodeState, nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            }
        }

        [When(@"I have input value equals to zero")]
        public async Task WhenIHaveInutValueEqualsToZeroAsync()
        {
            var page = this.Get<ElementPage>();
            var inputValue1 = page.GetElement(nameof(Resources.InputValueOnEditOwnershipGrid), formatArgs: 1).Text;
            var inputValue2 = page.GetElement(nameof(Resources.InputValueOnEditOwnershipGrid), formatArgs: 2).Text;
            if (double.Parse(inputValue1, CultureInfo.InvariantCulture) != 0.00 || double.Parse(inputValue2, CultureInfo.InvariantCulture) != 0.00)
            {
                await this.ReadSqlAsDictionaryAsync(input: SqlQueries.UpdateInputVolumeForNodeWithZero, args: new { ticketId = this.GetValue(ConstantValues.TicketId), nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
                this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            }
        }

        [Then(@"I should see status of the Node and list of Actions")]
        public void ThenIShouldSeeStatusOfTheNodeAndListOfActions()
        {
            var page = this.Get<ElementPage>();
            Assert.AreEqual(this.GetValue("NodeStatus"), page.GetElement(nameof(Resources.NodeStatusInEditOwnershipGrid)).Text);
            Assert.IsTrue(page.CheckIfElementIsPresent(nameof(Resources.ActionDropDownInEditOwnershipGrid)));
        }

        [Then(@"I should see node name, segment name and period")]
        public void ThenIShouldSeeNodeNameSegmentNameAndPeriod()
        {
            Assert.AreEqual(this.GetValue("NodeName"), this.Get<ElementPage>().GetElement(nameof(Resources.EditOwnershipLabelsInformation), formatArgs: 1).Text);
            Assert.AreEqual(this.GetValue("SegmentName"), this.Get<ElementPage>().GetElement(nameof(Resources.EditOwnershipLabelsInformation), formatArgs: 2).Text);
            var period = this.GetValue("StartDateOfNode") + " al " + this.GetValue("EndDateOfNode");
            Assert.AreEqual(period, this.Get<ElementPage>().GetElement(nameof(Resources.EditOwnershipLabelsInformation), formatArgs: 3).Text);
        }

        [Then(@"I should see the icon where the node totals are displayed in red")]
        public void ThenIShouldSeeTheIconWhereTheNodeTotalsAreDisplayedInRed()
        {
            try
            {
                this.Get<ElementPage>().GetElement(nameof(Resources.RedStatusOnEditOwnershipGrid));
                Assert.IsTrue(true);
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException)
            {
                Assert.IsTrue(false);
            }
        }

        [Then(@"I should see the icon where the node totals are displayed in green")]
        public void ThenIShouldSeeTheIconWhereTheNodeTotalsAreDisplayedInGreen()
        {
            try
            {
                this.Get<ElementPage>().GetElement(nameof(Resources.GreenStatusOnEditOwnershipGrid));
                Assert.IsTrue(true);
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException)
            {
                Assert.IsTrue(false);
            }
        }

        [Then(@"text ""(.*)"" should be displayed in red under ""(.*)"" column")]
        public void ThenTextShouldBeDisplayedInRedUnderColumn(string message, string column)
        {
            Assert.IsNotNull(column);
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.ElementByText), formatArgs: ConstantValues.Error));
        }

        [Then(@"page to view the report of the operational balance with ownership by node must be displayed")]
        public void ThenPageToViewTheReportOfTheOperationalBalanceWithOwnershipByNodeMustBeDisplayed()
        {
            ////this.Then("I should see breadcrumb \"Balance operativo con o sin propiedad\"");
            this.IShouldSeeBreadcrumb("Balance operativo con o sin propiedad");
        }
    }
}