// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StylesandValidationInGraphicalNodeConnectionsSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Executors;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class StylesandValidationInGraphicalNodeConnectionsSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have duplicate data configured for  ""(.*)""")]
        [Given(@"I have Inactive data configured for ""(.*)"" network")]
        public async Task GivenIHaveInactiveDataConfiguredForNetworkAsync(string entity)
        {
            var page = this.Get<ElementPage>();
            this.SetValue(Entities.Keys.EntityType, entity);
            ////this.Given("I want to create testdata for Inactive Configuration Validation");
            await this.DataSetupForInactiveNetworkConfigurationAsync().ConfigureAwait(false);

            if (entity != ConstantValues.Segmento)
            {
                ////this.When("I navigate to \"Grouping Categories\" page");
                this.UiNavigation("Grouping Categories");
                ////this.When("I click on \"Search\" \"button\"");
                this.IClickOn("Search", "button");
                ////this.When("I select \"System\" from NodeTags \"Category\" dropdown \"1\"");
                this.ISelectFromNodeTagsDropdown("System", "Category", "1");
                this.FeatureContext["CategoryElementName"] = this.GetValue("SegmentName");
                ////this.When("I select \"Value\" from NodeTags \"Element\" dropdown \"1\"");
                this.ISelectFromNodeTagsDropdown("System", "Category", "1");
                ////this.When("I click on \"NodeTags\" \"Or\" \"button\"");
                this.IClickOn("NodeTags\" \"Or", "button");
                ////this.When("I select \"Segment\" from NodeTags \"Category\" dropdown \"2\"");
                this.ISelectFromNodeTagsDropdown("Segment", "Category", "2");
                this.ScenarioContext["NodeTypeName"] = this.GetValue("CategorySegment");
                ////this.When("I select \"Value\" from NodeTags \"Element\" dropdown \"2\"");
                this.ISelectFromNodeTagsDropdown("Value", "Element", "2");
                ////this.When("I click on \"NodeTags\" \"apply\" \"button\"");
                this.IClickOn("NodeTags\" \"apply", "button");
                page.WaitUntilElementToBeClickable(nameof(Resources.SelectAllCheckBox));
                page.Click(nameof(Resources.SelectAllCheckBox));
                ////this.When("I select any \"NewValue\" from \"GroupingTitle\" \"dropdown\"");
                this.ISelectAnyElement("NewValue", "GroupingTitle", "dropdown");
                this.SetValue("CategorySegment", this.GetValue("SegmentName"));
                ////this.When("I select required Date from \"NodeTag\" \"operationDate\" \"datepicker\"");
                this.ISelectRequiredDateFrom("NodeTag\" \"operationDate", "datepicker");
                ////this.When("I click on \"NodeTag\" \"save\" \"button\"");
                this.IClickOn("NodeTag\" \"save", "button");
                await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue(ConstantValues.NodeId1) }).ConfigureAwait(false);
                await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue(ConstantValues.NodeId2) }).ConfigureAwait(false);
                await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue(ConstantValues.NodeId3) }).ConfigureAwait(false);
                await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue(ConstantValues.NodeId4) }).ConfigureAwait(false);
            }
        }

        [When(@"I select the output of an ""(.*)"" node and drag and drop it into the input of an ""(.*)"" node")]
        public void WhenISelectTheOutputOfAnNodeAndDragAndDropItIntoTheInputOfAnNode(string initial, string final)
        {
            var page = this.Get<ElementPage>();
            IWebElement outelement;
            IWebElement inelement;

            outelement = (initial == ConstantValues.Inactive) ? page.GetElement(nameof(Resources.NodeOut), formatArgs: this.GetValue(ConstantValues.NodeId3)) : page.GetElement(nameof(Resources.NodeOut), this.GetValue(ConstantValues.NodeId1));
            inelement = (final == ConstantValues.Inactive) ? page.GetElement(nameof(Resources.NodeIn), this.GetValue(ConstantValues.NodeId3)) : page.GetElement(nameof(Resources.NodeIn), this.GetValue(ConstantValues.NodeId1));

            this.Get<MouseActionsPage>().DragAndDrop(outelement, inelement);
        }

        [Then(@"Represent the connection attempt graphically with graphic style Improper Connections")]
        public void ThenRepresentTheConnectionAttemptGraphicallyWithGraphicStyleImproperConnections()
        {
            // method intentionally left
        }

        [Then(@"Following message ""(.*)"" should be appear")]
        public void ThenFollowingMessageShouldBeAppear(string expectedMessage)
        {
            var page = this.Get<ElementPage>();
            var actualMessage = page.GetElement(nameof(Resources.ConfirmationMessage)).Text;
            this.VerifyThat(() => Assert.AreEqual(expectedMessage, actualMessage));
        }

        [When(@"Create a connection between both the nodes")]
        public async Task WhenCreateAConnectionBetweenBothTheNodesAsync()
        {
            ////this.Given("Creating a Connection between 2 \"Active\" Nodes");
            await this.GraphicalNodeConnectionAsync("Active").ConfigureAwait(false);
        }

        [When(@"I update the record and connect both the node which is inactive")]
        public async Task WhenIUpdateTheRecordAndConnectBothTheNodeWhichIsInactiveAsync()
        {
            ////this.Given("Creating a Connection between 2 \"Inacitve\" Nodes");
            await this.GraphicalNodeConnectionAsync("Inacitve").ConfigureAwait(false);
        }

        [When(@"Again create a duplicate connection between both the nodes")]
        public void WhenAgainCreateADuplicateConnectionBetweenBothTheNodes()
        {
            var page = this.Get<ElementPage>();
            IWebElement outelement = page.GetElement(nameof(Resources.NodeOut), this.GetValue(ConstantValues.NodeId2));
            IWebElement inelement = page.GetElement(nameof(Resources.NodeIn), this.GetValue(ConstantValues.NodeId1));
            this.Get<MouseActionsPage>().DragAndDrop(outelement, inelement);
        }

        [When(@"I select the output of an inactive node and drag and drop it into the input of an inactive node")]
        public void WhenISelectTheOutputOfAnInactiveNodeAndDragAndDropItIntoTheInputOfAnInactiveNode()
        {
            var page = this.Get<ElementPage>();
            IWebElement outelement = page.GetElement(nameof(Resources.NodeOut), this.GetValue(ConstantValues.NodeId3));
            IWebElement inelement = page.GetElement(nameof(Resources.NodeIn), this.GetValue(ConstantValues.NodeId4));
            this.Get<MouseActionsPage>().DragAndDrop(outelement, inelement);
        }

        [When(@"I update the record and make second node as inactive")]
        public async Task WhenIUpdateTheRecordAndMakeSecondNodeAsInactiveAsync()
        {
            await this.ReadAllSqlAsync(SqlQueries.UpdateNodeState, args: new { NodeId = this.GetValue(ConstantValues.NodeId2) }).ConfigureAwait(false);
        }

        [When(@"I select the output of an inactive node and drag and drop it into the input of an active node for which the connections already established")]
        [When(@"I select the output of an active node and drag and drop it into the input of an inactive node for which the connections already established")]
        public void WhenISelectTheOutputOfAnActiveNodeAndDragAndDropItIntoTheInputOfAnInactiveNodeForWhichTheConnectionsAlreadyEstablished()
        {
            var currentPage = this.Get<ElementPage>();
            IWebElement outelement = currentPage.GetElement(nameof(Resources.NodeOut), this.GetValue(ConstantValues.NodeId2));
            IWebElement inelement = currentPage.GetElement(nameof(Resources.NodeIn), this.GetValue(ConstantValues.NodeId1));
            this.Get<MouseActionsPage>().DragAndDrop(outelement, inelement);
        }

        [When(@"I update the record and make first node as inactive")]
        public async Task WhenIUpdateTheRecordAndMakeFirstNodeAsInactiveAsync()
        {
            await this.ReadAllSqlAsync(SqlQueries.UpdateNodeState, args: new { NodeId = this.GetValue(ConstantValues.NodeId1) }).ConfigureAwait(false);
        }
    }
}