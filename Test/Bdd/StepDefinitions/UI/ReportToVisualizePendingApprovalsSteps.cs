// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportToVisualizePendingApprovalsSteps.cs" company="Microsoft">
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
    using System.Linq;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ReportToVisualizePendingApprovalsSteps : EcpWebStepDefinitionBase
    {
        [When(@"I click on ""(.*)"" Icon")]
        public void WhenIClickOnIcon(string value)
        {
            this.DriverContext.Driver.SwitchTo().Frame("widgetIFrame");
            this.Get<ElementPage>().Click(nameof(Resources.Icons), formatArgs: value);
        }

        [Then(@"""(.*)"" tab should be selected by default")]
        public void ThenTabShouldBeSelectedByDefault(string value)
        {
            var attributevalue = this.Get<ElementPage>().FindElementByXPath(nameof(Resources.ApprovalTabs), formatArgs: value).GetAttribute("class");
            Assert.IsTrue(attributevalue.Contains("active"));
        }

        [Then(@"I should see Pending Approvals Assigned to me")]
        public void ThenIShouldSeePendingApprovalsAssignedToMe()
        {
            this.DriverContext.Driver.SwitchTo().Frame("widgetIFrame");
            Assert.IsTrue(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ApprovalCard)));
        }

        [When(@"I click on the Confirm button")]
        public void WhenIClickOnTheConfirmButton()
        {
            this.Get<ElementPage>().Click(nameof(Resources.ApprovalConfirmRequest));
        }

        [Then(@"I should see the ""(.*)"" on the Node Approval page")]
        public void ThenIShouldSeeTheOnTheNodeApprovalPage(string field, Table table)
        {
            this.DriverContext.Driver.SwitchTo().Frame("widgetIFrame");
            foreach (var row in table?.Rows.Select((value) => (Default: value[field], Unused: string.Empty)))
            {
                Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.ApprovalTabs), row.Default).Displayed);
            }
        }

        [Then(@"""(.*)"" Message should be appeared")]
        public void ThenMessageShouldBeAppeared(string message)
        {
            var actualMessage = this.Get<ElementPage>().GetElementText(nameof(Resources.VerificationMessage));
            Assert.AreEqual(message, actualMessage);
        }
    }
}
