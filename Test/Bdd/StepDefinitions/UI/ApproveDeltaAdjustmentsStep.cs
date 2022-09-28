// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApproveDeltaAdjustmentsStep.cs" company="Microsoft">
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
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using OpenQA.Selenium.Interactions;
    using TechTalk.SpecFlow;

    [Binding]
    public class ApproveDeltaAdjustmentsStep : EcpWebStepDefinitionBase
    {
        [Given(@"I have Calculate deltas by official adjustment")]
        public async System.Threading.Tasks.Task GivenIHaveCalculateDeltasByOfficialAdjustmentAsync()
        {
            this.SetValue("DeltaCategorySegment", this.GetValue("SegmentName"));
            ////When I navigate to "Calculation of deltas by official adjust" page
            this.NavigateToPage("Calculation of deltas by official adjustment");
            ////And I click on "newDeltasCalculation" "button"
            this.IClickOn("newDeltasCalculation", "button");
            ////And I select delta segment from "initOfficialDeltaTicket" "segment" "dropdown"
            this.ISelectOfficialDeltaSegmentFrom("initOfficialDeltaTicket\" \"segment", "dropdown");
            ////And I select "2020" year from drop down
            this.ISelectYearFromDropDown(this.GetValue("YearForPeriod"));
            ////And I select "Jun" period from drop down
            this.ISelectAPeriodFromDropDown(this.GetValue("PreviousMonthName").ToPascalCase());
            ////And I click on "initOfficialDeltaTicket" "submit" "button"
            this.IClickOn("initOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "validateOfficialDeltaTicket" "submit" "button"
            this.IClickOn("validateOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "confirmOfficialDeltaTicket" "submit" "button"
            this.IClickOn("confirmOfficialDeltaTicket\" \"submit", "button");
            //// wait till ticket processing complete
            await this.VerifyThatOfficialDeltaCalculationShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        [Given(@"I have existing official node balance")]
        public void GivenIHaveExistingOfficialNodeBalance()
        {
            ////Keeping this line intentially blank
        }

        [When(@"I select ""(.*)"" from the ""(.*)"" ""(.*)"" ""(.*)""")]
        public async System.Threading.Tasks.Task WhenISelectFromTheAsync(string value, string field, string field1, string field2)
        {
            var segmentName = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSegmentForSentToApproveStatus).ConfigureAwait(false);

            var officialBbalanceReportPage = this.Get<ElementPage>();
            officialBbalanceReportPage.GetElement(nameof(Resources.SegmentDropdownInReport)).Click();
            var movementTypeoption = officialBbalanceReportPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: segmentName["Segment"]);
            Actions movementTypeAction = new Actions(this.DriverContext.Driver);
            movementTypeAction.MoveToElement(movementTypeoption).Perform();
            movementTypeoption.Click();
            Assert.IsNotNull(value);
            Assert.IsNotNull(field);
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
        }

        [When(@"I choose node from ""(.*)"" ""(.*)"" which state is ""(.*)""")]
        public async System.Threading.Tasks.Task WhenIChooseNodeFromWhichStateIsAsync(string value, string field, string field1)
        {
            var name = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNodeNameForSentToApproveStatus).ConfigureAwait(false);
            this.SetValue(Keys.RandomFieldValue, name["NodeName"]);
            this.Get<ElementPage>().GetElement(nameof(Resources.NodeDropdownTextInReport)).SendKeys(this.GetValue(Keys.RandomFieldValue));
            this.Get<ElementPage>().GetElement(nameof(Resources.NodeDropdownTextInReport)).SendKeys(OpenQA.Selenium.Keys.Enter);
            await Task.Delay(3000).ConfigureAwait(false);
            Assert.IsNotNull(value);
            Assert.IsNotNull(field);
            Assert.IsNotNull(field1);
        }

        [When(@"I Select period from ""(.*)"" ""(.*)""")]
        public async Task WhenISelectPeriodFromAsync(string field, string field1)
        {
            this.IClickOn(new ElementLocator(Ocaramba.Locator.XPath, "//*[text()=\"Período\"]/following-sibling::div//*[text()=\"Seleccionar\"]"));
            this.IClickOn(new ElementLocator(Ocaramba.Locator.XPath, "//div[contains(@id,'react-select')]"));
            await Task.Delay(6000).ConfigureAwait(false);
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field);
        }

        [Then(@"I should see ""(.*)"" ""(.*)"" is hidden")]
        public void ThenIShouldSeeIsHidden(ElementLocator elementLocator)
        {
            Assert.IsFalse(this.Get<ElementPage>().GetElement(elementLocator).Displayed);
        }

        [When(@"I filter NodeName in the grid which has ""(.*)""")]
        public async Task WhenIFilterNodeNameInTheGridWhichHasAsync(string value)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            if (value.Contains("no predecessors"))
#pragma warning restore CA1062 // Validate arguments of public methods
            {
                var name = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNodeNameForSentToApproveStatus).ConfigureAwait(false);
                this.ScenarioContext["nodeName"] = name["NodeName"];
                this.SetValue(Keys.RandomFieldValue, name[ConstantValues.NodeName]);
                this.Get<ElementPage>().GetElement(nameof(Resources.OfficialDeltaNodesGridNodeNameFilter)).SendKeys(this.GetValue(Keys.RandomFieldValue));
                this.Get<ElementPage>().GetElement(nameof(Resources.OfficialDeltaNodesGridNodeNameFilter)).SendKeys(OpenQA.Selenium.Keys.Enter);
                await Task.Delay(3000).ConfigureAwait(false);
            }
        }

        [Then(@"I should be redirected to ""(.*)"" page")]
        public void ThenIShouldBeRedirectedToPage(string value)
        {
            Assert.IsNotNull(value);
            ////Keeping this line intentially blank
        }

        [Then(@"I filter NodeName which is selected above in the grid which has ""(.*)""")]
        public async Task ThenIFilterNodeNameWhichIsSelectedAboveInTheGridWhichHasAsync(string value)
        {
            this.SetValue(Keys.RandomFieldValue, this.ScenarioContext["nodeName"]);
            this.Get<ElementPage>().GetElement(nameof(Resources.OfficialDeltaNodesGridNodeNameFilter)).SendKeys(this.GetValue(Keys.RandomFieldValue));
            this.Get<ElementPage>().GetElement(nameof(Resources.OfficialDeltaNodesGridNodeNameFilter)).SendKeys(OpenQA.Selenium.Keys.Enter);
            await Task.Delay(3000).ConfigureAwait(false);
            Assert.IsNotNull(value);
        }

        [Then(@"I should see the state of the node as ""(.*)""")]
        public async Task ThenIShouldSeeTheStateOfTheNodeAsAsync(string status)
        {
            var nodeStatus = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetStatusNamefromOfficialDeltaNode, args: new { nodename = this.ScenarioContext["nodeName"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(status, nodeStatus);
        }

        [When(@"I Validate that all predecessor nodes of the same segment with lower order in the chain are in the “Sent to Approval"" state")]
        public void WhenIValidateThatAllPredecessorNodesOfTheSameSegmentWithLowerOrderInTheChainAreInTheSentToApprovalState()
        {
            ////Keeping this line intentially blank
        }
    }
}