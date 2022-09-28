// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReopenAnApprovedNodeSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.StepDefinitions;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class ReopenAnApprovedNodeSteps : WebStepDefinitionBase
    {
        [When(@"I select the variable in the filter corresponds to ""(.*)""")]
        public void WhenISelectTheVariableInTheFilterCorrespondsTo(string type)
        {
            this.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), UIContent.Conversion[type]);
        }

        [When(@"I select the variable in the filter different from Initial Inventory and Final Inventory")]
        public void WhenISelectTheVariableInTheFilterDifferentFromInitialInventoryAndFinalInventory()
        {
            this.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), UIContent.Conversion["Interface"]);
        }

        [Then(@"I should see reopen option next to node state")]
        public void ThenIShouldSeeReopenOptionNextToNodeState()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.ReopenButton)).Displayed);
        }

        [Then(@"update the ticket node status to ""(.*)"" at the top right of the page")]
        public void ThenUpdateTheTicketNodeStatusToAtTheTopRightOfThePage(string state)
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), UIContent.Conversion[state]).Displayed);
        }

        [Then(@"I should not see reopen option next to node state")]
        public void ThenIShouldNotSeeReopenOptionNextToNodeState()
        {
            try
            {
                Assert.IsFalse(this.Get<ElementPage>().GetElement(nameof(Resources.ReopenButton)).Displayed);
            }
            catch (WebDriverTimeoutException message)
            {
                Assert.IsNotNull(message);
            }
        }

        [When(@"I click on reopen button")]
        public void WhenIClickOnReopenButton()
        {
            this.Get<ElementPage>().Click(nameof(Resources.ReopenButton));
        }

        [Then(@"I should see the tooltip ""(.*)"" text for reopen button")]
        public void ThenIShouldSeeTheTooltipTextForReopenButton(string tooltipValue)
        {
            this.Get<ElementPage>().HoverElement(nameof(Resources.ReopenButton));
            var tooltip = this.Get<ElementPage>().GetElement(nameof(Resources.ReopenToolTip)).Text;
            Assert.AreEqual(UIContent.Conversion[tooltipValue], tooltip);
        }
    }
}
