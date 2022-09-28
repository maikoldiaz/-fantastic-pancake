// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateConnectionBetweenNodesGraphicallySteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Executors;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using TechTalk.SpecFlow;

    [Binding]
    public class CreateConnectionBetweenNodesGraphicallySteps : EcpApiStepDefinitionBase
    {
        private readonly FeatureContext featureContext;

        public CreateConnectionBetweenNodesGraphicallySteps(FeatureContext featureContext)
           : base(featureContext)
        {
            this.featureContext = featureContext;
        }

        [Given(@"I have data configured for ""(.*)"" network configuration")]
        public async Task GivenIHaveDataConfiguredForNetworkAsync(string field)
        {
            this.ScenarioContext["Count"] = 0;
            Assert.IsNotNull(field);
            ////this.Given("I am authenticated as \"admin\"");
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            ////Create Nodes
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
            this.SetValue("NodeId_1", this.GetValue("NodeId"));
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
            this.SetValue("NodeId_2", this.GetValue("NodeId"));
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
            this.SetValue("NodeId_3", this.GetValue("NodeId"));
            this.SetValue("CategorySegment", this.GetValue("SegmentName"));
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
            this.SetValue("NodeId_4", this.GetValue("NodeId"));
            await this.CreateNodesStepsAsync().ConfigureAwait(false);

            this.SetValue("CategorySegment1", this.GetValue("SegmentName"));
            this.ScenarioContext["Count"] = 0;
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
            this.SetValue("NodeId_5", this.GetValue("NodeId"));
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
            this.SetValue("NodeId_6", this.GetValue("NodeId"));
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
            this.SetValue("NodeId_7", this.GetValue("NodeId"));
            this.SetValue("CategorySegment", this.GetValue("SegmentName"));
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
            this.SetValue("NodeId_8", this.GetValue("NodeId"));
            await this.CreateNodesStepsAsync().ConfigureAwait(false);

            this.SetValue("CategorySegment2", this.GetValue("SegmentName"));
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_1"), this.GetValue("NodeId_2"), 2, "0.07", "0.06").ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_2"), this.GetValue("NodeId_3"), 2, "0.05", "0.04").ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_3"), this.GetValue("NodeId_4"), 1, "0.02", "0.06").ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_2"), this.GetValue("NodeId_4"), 1, "0.02", "0.06").ConfigureAwait(false);

            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_1"), this.GetValue("NodeId_5"), 2, "0.07", "0.06").ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_2"), this.GetValue("NodeId_6"), 2, "0.05", "0.04").ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_3"), this.GetValue("NodeId_7"), 1, "0.02", "0.06").ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_4"), this.GetValue("NodeId_8"), 1, "0.02", "0.06").ConfigureAwait(false);

            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_5"), this.GetValue("NodeId_6"), 2, "0.07", "0.06").ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_6"), this.GetValue("NodeId_7"), 2, "0.05", "0.04").ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_7"), this.GetValue("NodeId_8"), 1, "0.02", "0.06").ConfigureAwait(false);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_5"), this.GetValue("NodeId_8"), 1, "0.02", "0.06").ConfigureAwait(false);
        }

        [When(@"I click on Segment dropdown from ""(.*)"" dropdown")]
        public void WhenIClickOnSegmentDropdownFromDropdown(string segment)
        {
            ////this.Get<ElementPage>().Click(nameof(Resources.NodeFilterCategory));
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), ConstantValues.Segmento).Click();
            Assert.IsNotNull(segment);
        }

        [When(@"I send the text ""(.*)"" to Node textbox")]
        public void WhenISendTheTextToNodeTextbox(string text)
        {
            Assert.IsNotNull(text);
            this.Get<ElementPage>().GetElement(nameof(Resources.NodeConnectionTextbox)).Click();
            this.Get<ElementPage>().GetElement(nameof(Resources.NodeConnectionTextboxMagnifier)).Click();
        }

        [When(@"I select the output of an active node and drag and drop it into the input of an active node")]
        public void WhenISelectTheOutputOfAnActiveNodeAndDragAndDropItIntoTheInputOfAnActiveNode()
        {
            var sourceElement = this.Get<ElementPage>().GetElement(nameof(Resources.SourceNode));
            var destinationElement = this.Get<ElementPage>().GetElement(nameof(Resources.DestinationNode));
            MouseActionsPage mouseActions = new MouseActionsPage(this.DriverContext);
            mouseActions.DragAndDrop(sourceElement, destinationElement);
        }

        [When(@"I select the output of an active node and drag and drop it into the input of the same active node")]
        public void WhenISelectTheOutputOfAnActiveNodeAndDragAndDropItIntoTheInputOfSameActiveNode()
        {
            var sourceElement = this.Get<ElementPage>().GetElement(nameof(Resources.SourceNode));
            var destinationElement = this.Get<ElementPage>().GetElement(nameof(Resources.DestinationNode2));
            MouseActionsPage mouseActions = new MouseActionsPage(this.DriverContext);
            mouseActions.DragAndDrop(sourceElement, destinationElement);
        }

        [When(@"I see confirmation dialog with accept and cancel options")]
        public void WhenISeeConfirmationDialogWithAcceptAndCancelOptions()
        {
            WebDriverWait wait = new WebDriverWait(this.DriverContext.Driver, TimeSpan.FromSeconds(10) /*timeout in seconds*/);
            Assert.IsNotNull(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent(), "Alert message not found");
        }

        [When(@"a connection created between the two nodes should be ""(.*)""")]
        [Then(@"a connection created between the two nodes should be ""(.*)""")]
        public async Task ThenIShouldSeeAConnectionCreatedBetweenTheTwoNodesAsync(string condition)
        {
            var page = this.Get<ElementPage>();
            var node1Id = this.ScenarioContext["NodeId_1"].ToString();
            var node2Id = this.ScenarioContext["NodeId_2"].ToString();
            var nodeConnectorId = "out_" + node1Id + "-in_" + node2Id;
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["node-connection"]).ConfigureAwait(false);
            var sourceNodeID = lastCreatedRow[ConstantValues.SourceNode];
            var targetNodeID = lastCreatedRow[ConstantValues.DestinationNode];
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (condition.Equals("ABSENT", StringComparison.InvariantCultureIgnoreCase))
            {
                Assert.IsFalse(page.CheckIfElementIsPresent(nameof(Resources.NodeConnector), formatArgs: nodeConnectorId), "The node connector is found");
            }
            else if (condition.Equals("PRESENT", StringComparison.InvariantCultureIgnoreCase))
            {
                Assert.IsTrue(sourceNodeID == node1Id && targetNodeID == node2Id, "Connection is not registered in the database");
                Assert.IsTrue(page.CheckIfElementIsPresent(nameof(Resources.NodeConnector), formatArgs: nodeConnectorId), "The node connector is not found");
                var webElement = page.GetElement(nameof(Resources.NodeConnector), formatArgs: nodeConnectorId);
                var classText = webElement.FindElement(By.XPath(".//*/*")).GetAttribute("class");
                Assert.IsTrue(!classText.Contains(ConstantValues.Unsaved), "Node connector is in unsaved state");
            }
            else
            {
                var webElement = page.GetElement(nameof(Resources.NodeConnector), formatArgs: nodeConnectorId);
                var classText = webElement.FindElement(By.XPath(".//*/*")).GetAttribute("class");
                Assert.IsTrue(classText.Contains(ConstantValues.Unsaved), "Node connector is not in unsaved state");
            }
        }

        [When(@"I make the node inactive")]
        public async Task WhenIMakeTheNodeInactiveAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.SetNodeToInactive, args: new { nodeId = this.ScenarioContext["NodeId_5"] }).ConfigureAwait(false);
        }

        [Then(@"I should see a node creation created for the same node is ""(.*)""")]
        public async Task ThenIShouldSeeANodeCreationCreatedForTheSameNodeAsync(string condition)
        {
            var page = this.Get<ElementPage>();
            var node1Id = this.ScenarioContext["NodeId_5"].ToString();
            var node2Id = this.ScenarioContext["NodeId_5"].ToString();
            var nodeConnectorId = "out_" + node1Id + "-in_" + node2Id;
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["node-connection"]).ConfigureAwait(false);
            var sourceNodeID = lastCreatedRow[ConstantValues.SourceNode];
            var targetNodeID = lastCreatedRow[ConstantValues.DestinationNode];
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (condition.Equals("ABSENT", StringComparison.InvariantCultureIgnoreCase))
            {
                Assert.IsFalse(page.CheckIfElementIsPresent(nameof(Resources.NodeConnector), formatArgs: nodeConnectorId), "The node connector is found");
            }
            else if (condition.Equals("PRESENT", StringComparison.InvariantCultureIgnoreCase))
            {
                Assert.IsTrue(sourceNodeID == node1Id && targetNodeID == node2Id, "Connection is not registered in the database");
                Assert.IsTrue(page.CheckIfElementIsPresent(nameof(Resources.NodeConnector), formatArgs: nodeConnectorId), "The node connector is not found");
                var webElement = page.GetElement(nameof(Resources.NodeConnector), formatArgs: nodeConnectorId);
                var classText = webElement.FindElement(By.XPath(".//*/*")).GetAttribute("class");
                Assert.IsTrue(!classText.Contains(ConstantValues.Unsaved), "Node connector is in unsaved state");
            }
            else
            {
                var webElement = page.GetElement(nameof(Resources.NodeConnector), formatArgs: nodeConnectorId);
                var classText = webElement.FindElement(By.XPath(".//*/*")).GetAttribute("class");
                Assert.IsTrue(classText.Contains(ConstantValues.Unsaved), "Node connector is not in unsaved state");
            }
        }
    }
}
