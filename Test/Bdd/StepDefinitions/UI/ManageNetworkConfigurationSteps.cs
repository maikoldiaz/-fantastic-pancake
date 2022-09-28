// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageNetworkConfigurationSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using OpenQA.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class ManageNetworkConfigurationSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have data configured for ""(.*)"" network")]
        public async Task GivenIHaveDataConfiguredForNetworkAsync(string entity)
        {
            var page = this.Get<ElementPage>();
            this.SetValue(Entities.Keys.EntityType, entity);
            ////this.Given("I want to create testdata for network configuration");
            await this.DataSetupForNetworkConfigurationAsync().ConfigureAwait(false);
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
                this.ISelectFromNodeTagsDropdown("Value", "Element", "1");
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

        [Then(@"I should see the active nodes that belong to the ""(.*)"" on the current date")]
        public void ThenIShouldSeeTheActiveNodesThatBelongToTheSegmentOnTheCurrentDate(string entity)
        {
            var page = this.Get<ElementPage>();
            Assert.IsNotNull(entity);
            Assert.AreEqual(3, page.GetElements(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: this.GetValue(ConstantValues.NodeId1)).Count);
            Assert.AreEqual(3, page.GetElements(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: this.GetValue(ConstantValues.NodeId2)).Count);
            Assert.AreEqual(3, page.GetElements(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: this.GetValue(ConstantValues.NodeId3)).Count);
        }

        [Then(@"I should see the inactive nodes that belong to the ""(.*)"" on the current date")]
        public void ThenIShouldSeeTheInactiveNodesThatBelongToTheSegmentOnTheCurrentDate(string entity)
        {
            Assert.IsNotNull(entity);
            Assert.AreEqual(3, this.Get<ElementPage>().GetElements(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: this.GetValue(ConstantValues.NodeId4)).Count);
        }

        [Then(@"I should see the active connections between nodes")]
        public void ThenIShouldSeeTheActiveConnectionsBetweenNodes()
        {
            var page = this.Get<ElementPage>();
            Assert.IsTrue(page.GetElement(nameof(Resources.ConnectionIdInGraphicNetwork), this.GetValue(ConstantValues.NodeId2), this.GetValue(ConstantValues.NodeId1)).Displayed);
            Assert.IsTrue(page.GetElement(nameof(Resources.ConnectionIdInGraphicNetwork), this.GetValue(ConstantValues.NodeId3), this.GetValue(ConstantValues.NodeId2)).Displayed);
        }

        [Then(@"I should see the inactive connections between nodes")]
        public void ThenIShouldSeeTheInactiveConnectionsBetweenNodes()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.ConnectionIdInGraphicNetwork), this.GetValue(ConstantValues.NodeId1), this.GetValue(ConstantValues.NodeId3)).Displayed);
        }

        [Then(@"I should see the ordered network so that the nodes and their connections can be visualized")]
        public void ThenIShouldSeeTheOrderedNetworkSoThatTheNodesAndTheirConnectionsCanBeVisualized()
        {
            this.LogToReport("Covered in previous step!");
        }

        [Then(@"I should see the connections according to the graphic standard defined for the ""(.*)"" connections")]
        public void ThenIShouldSeeTheConnectionsAccordingToTheGraphicStandardDefinedForTheConnections(string state)
        {
            var page = this.Get<ElementPage>();
            IWebElement element;
            if (state == ConstantValues.Active)
            {
                element = page.GetElement(nameof(Resources.ConnectionIdInGraphicNetwork), this.GetValue(ConstantValues.NodeId2), this.GetValue(ConstantValues.NodeId1));
            }
            else if (state == ConstantValues.Inactive)
            {
                element = page.GetElement(nameof(Resources.ConnectionIdInGraphicNetwork), this.GetValue(ConstantValues.NodeId1), this.GetValue(ConstantValues.NodeId3));
            }
            else
            {
                element = page.GetElement(nameof(Resources.ConnectionIdInGraphicNetwork), this.GetValue(ConstantValues.NodeId3), this.GetValue(ConstantValues.NodeId2));
            }

            Assert.IsTrue(element.Displayed);
            Assert.IsTrue(element.GetAttribute("class").Contains(state));
        }

        [When(@"I select Todos from Node dropdown")]
        public void WhenISelectTodosFromNodeDropdown()
        {
            var page = this.Get<ElementPage>();
            var nodeSelection = page.GetElement(nameof(Resources.NodeInputInGraphicNetwork));
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.SelectItemAtIndex(nodeSelection, 0);
        }

        [When(@"I select any ""(.*)"" node from node dropdown")]
        public void WhenISelectAnyNodeFromNodeDropdown(string state)
        {
            var page = this.Get<ElementPage>();
            var nodeSelection = page.GetElement(nameof(Resources.NodeInputInGraphicNetwork));
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            nodeSelection.Click();
            nodeSelection.SendKeys(state.EqualsIgnoreCase(ConstantValues.Active) ? this.GetValue(ConstantValues.Node1Name) : this.GetValue(ConstantValues.Node4Name));
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.SelectItemAtIndex(nodeSelection, 1);
        }

        [Then(@"I should see the selected node")]
        public void ThenIShouldSeeTheSelectedNode()
        {
            Assert.AreEqual(3, this.Get<ElementPage>().GetElements(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: this.GetValue(ConstantValues.NodeId1)).Count);
        }

        [Then(@"I should see all the specified details about the ""(.*)"" node")]
        public async System.Threading.Tasks.Task ThenIShouldSeeAllTheSpecifiedDetailsAboutTheNodeAsync(string state)
        {
            var page = this.Get<ElementPage>();
            var nodeDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = state.EqualsIgnoreCase(ConstantValues.Active) ? this.GetValue(ConstantValues.NodeId1) : this.GetValue(ConstantValues.NodeId4) }).ConfigureAwait(false);
            var uiNodeDetails = page.GetElements(nameof(Resources.NodeDetails), formatArgs: state.EqualsIgnoreCase(ConstantValues.Active) ? this.GetValue(ConstantValues.NodeId1) : this.GetValue(ConstantValues.NodeId4));
            Assert.AreEqual(nodeDetails[ConstantValues.Name].ToString(), uiNodeDetails[0].Text);
            Assert.AreEqual(this.GetValue(ConstantValues.CategorySegment), uiNodeDetails[1].Text);
            Assert.AreEqual(this.GetValue("OperatorName"), uiNodeDetails[2].Text);
            Assert.AreEqual(nodeDetails[ConstantValues.ControlLimit].ToString(), uiNodeDetails[3].Text.Substring(2));
            Assert.AreEqual(nodeDetails[ConstantValues.AcceptableBalancePercentageTitle].ToString().TrimEnd('0') + "%", uiNodeDetails[4].Text);
            uiNodeDetails = page.GetElements(nameof(Resources.NodeFooterDetails), formatArgs: state.EqualsIgnoreCase(ConstantValues.Active) ? this.GetValue(ConstantValues.NodeId1) : this.GetValue(ConstantValues.NodeId4));
            Assert.AreEqual(state.EqualsIgnoreCase(ConstantValues.Active) ? "1" : "0", uiNodeDetails[0].Text);
            Assert.AreEqual(state.EqualsIgnoreCase(ConstantValues.Active) ? "1" : "0", uiNodeDetails[1].Text);
        }

        [Then(@"I should see the node according to the graphic standard defined for the ""(.*)"" nodes")]
        public void ThenIShouldSeeTheNodeAccordingToTheGraphicStandardDefinedForTheActiveNodes(string state)
        {
            var element = this.Get<ElementPage>().GetElement(nameof(Resources.NodeInGraphicNetwork), formatArgs: state == ConstantValues.Active ? this.GetValue(ConstantValues.NodeId1) : this.GetValue(ConstantValues.NodeId4));
            if (state == ConstantValues.Active)
            {
                Assert.IsFalse(element.GetAttribute("class").Contains("inactive"));
            }
            else
            {
                Assert.IsTrue(element.GetAttribute("class").Contains("inactive"));
            }
        }

        [Then(@"I should see the node with the color defined for the segment to which the ""(.*)"" node belongs")]
        public async System.Threading.Tasks.Task ThenIShouldSeeTheNodeWithTheColorDefinedForTheSegmentToWhichTheNodeBelongsAsync(string state)
        {
            var dbElement = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetCategoryElementByName, args: new { name = this.GetValue(ConstantValues.CategorySegment) }).ConfigureAwait(false);
            var uiElement = this.Get<ElementPage>().GetElement(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: state == ConstantValues.Active ? this.GetValue(ConstantValues.NodeId1) : this.GetValue(ConstantValues.NodeId4));
            if (dbElement[ConstantValues.Color] == null)
            {
                Assert.IsTrue(uiElement.GetAttribute("class").Contains("#025449"));
            }
            else
            {
                Assert.IsTrue(uiElement.GetAttribute("class").Contains(dbElement[ConstantValues.Color].ToString()));
            }
        }

        [Then(@"I should see the node with locked icon")]
        public void ThenIShouldSeeTheNodeWithLockedIcon()
        {
            var element = this.Get<ElementPage>().GetElement(nameof(Resources.NodeIconLock), formatArgs: this.GetValue(ConstantValues.NodeId4));
            Assert.IsTrue(element.GetAttribute("class").Contains("lock"));
        }

        [Then(@"I should see the node with an icon that represents the node type")]
        public void ThenIShouldSeeTheNodeWithAnIconThatRepresentsTheNodeType()
        {
            this.LogToReport("Covered in previous step!");
        }
    }
}
