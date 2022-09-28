// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicalNetworkConfigurationFiltersSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions
{
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class GraphicalNetworkConfigurationFiltersSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have data configured for ""(.*)""")]
        public async Task GivenIHaveDataConfirguredForAsync(string category)
        {
            if (category == ConstantValues.Segment)
            {
                ////this.Given("I have Node with Segment Category");
                await this.NodeWithSegmentCategoryAsync().ConfigureAwait(false);
            }
            else if (category == ConstantValues.System)
            {
                ////this.Given("I have System category in the system");
                await this.SystemCategoryIntheSystemAsync().ConfigureAwait(false);
            }
        }

        [Then(@"I should see the ""(.*)"" dropdown")]
        public void ThenIShouldSeeTheDropdown(string dropdownName)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.ReactDropdown), formatArgs: dropdownName).Displayed);
        }

        [Then(@"I should see the Node textbox")]
        public void ThenIShouldSeeTheNodeTextbox()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.NodeReactTextBox)).Displayed);
        }

        [Then(@"I  see the ""(.*)"" label (on "".*"" "".*"" "".*"")")]
        public void ThenISeeTheLabelOn(string value, ElementLocator elementLocator)
        {
            Assert.AreEqual(UIContent.Conversion[value], this.Get<ElementPage>().GetElement(elementLocator).Text);
        }

        [When(@"I enter ""(.*)"" in the Category dropdown")]
        public void WhenIEnterInTheCategoryDropdown(string category)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.GetElement(nameof(Resources.CategoryAutofillTextBox)).SendKeys(UIContent.Conversion[category]);
            page.WaitUntilElementIsVisible(nameof(Resources.CategoryAutofillTextBox)).SendKeys(OpenQA.Selenium.Keys.Enter);
            this.ScenarioContext["SelectedCategory"] = category;
        }

        [Then(@"I should see the values in Element according to the selected Category_Options")]
        public void ThenIShouldSeeTheValuesInElementAccordingToTheSelectedCategoryOptions()
        {
            var createdElement = this.GetValue("CategorySegment");
            this.Get<ElementPage>().GetElement(nameof(Resources.ElementAutoFillTextbox)).SendKeys(createdElement);
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ElementAutoFillTextbox)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [When(@"I enter ""(.*)"" into Node Texbox")]
        public void WhenIEnterIntoNodeTexbox(string value)
        {
            var page = this.Get<ElementPage>();
            if (value == ConstantValues.Todos)
            {
                page.EnterText(nameof(Resources.NodeTextbox), ConstantValues.Todos);
                page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                page.Click(nameof(Resources.ElementByText), formatArgs: ConstantValues.Todos);
            }
            else
            {
                var nodeName = this.GetValue("NodeName");
                page.GetElement(nameof(Resources.NodeTextbox)).SendKeys(nodeName);
                page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                page.Click(nameof(Resources.ElementByText), nodeName);
            }
        }

        [When(@"I enter NodeName into Node Texbox")]
        public void WhenIEnterNodeNameIntoNodeTexbox()
        {
            var nodeName = this.GetValue(ConstantValues.NodeName);
            this.Get<ElementPage>().GetElement(nameof(Resources.NodeTextbox)).SendKeys(this.GetValue("NodeName"));
        }

        [When(@"I select category element value from dropdown")]
        public void WhenISelectAnyElementvalueFromElementDropdownFor()
        {
            var page = this.Get<ElementPage>();
            var enteredElement = this.GetValue(ConstantValues.CategorySegment);
            page.GetElement(nameof(Resources.ElementAutoFillTextbox)).SendKeys(enteredElement);
            page.WaitUntilElementIsVisible(nameof(Resources.ElementAutoFillTextbox)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [Then(@"I should see the ""(.*)"" Message below each field")]
        public void ThenIShouldSeeTheMessageBelowEachField(string expectedMessage)
        {
            var errorMessage = this.Get<ElementPage>().GetElements(nameof(Resources.GetErrorMessageFromModel));
            foreach (var actualMessage in errorMessage)
            {
                Assert.AreEqual(expectedMessage, actualMessage.Text);
            }
        }

        [Then(@"I should see the selected node in the graphical network page")]
        public void ThenIShouldSeeTheSelectedNode()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.GraphicalNodeIcon), formatArgs: this.GetValue("NodeId_1")).Displayed);
            var actualNodeName = this.Get<ElementPage>().GetElement(nameof(Resources.GraphicalNodeName)).Text;
            Assert.AreEqual(this.GetValue("NodeName"), actualNodeName);
        }

        [Then(@"I should see the nodes and Connections")]
        [When(@"I see graphic network configuration")]
        public void ThenIShouldSeeTheNodesAndConnections()
        {
            if (this.ScenarioContext["SelectedCategory"].Equals(ConstantValues.Segment))
            {
                var actualElementName = this.Get<ElementPage>().GetElements(nameof(Resources.GraphicalElementName)).ElementAt(0).Text;
                Assert.AreEqual(this.GetValue(ConstantValues.CategorySegment), actualElementName);
            }
        }
    }
}