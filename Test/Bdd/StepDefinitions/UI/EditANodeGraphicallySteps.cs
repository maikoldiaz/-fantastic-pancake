// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditANodeGraphicallySteps.cs" company="Microsoft">
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
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class EditANodeGraphicallySteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should see the Order Label on the right side of a node's graphical representation")]
        public void ThenIShouldSeeTheOrderLabelOnTheRightSideOfANodeSGraphicalRepresentation()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElements(nameof(Resources.OrderLabel)).ElementAt(0).Displayed);
        }

        [Then(@"Verify that value for the Order field for a node is appearing correctly")]
        public async Task ThenVerifyThatValueForTheOrderFieldForANodeIsAppearingCorrectlyAsync()
        {
            var expectedValue = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNodeDetials, args: new { NodeId = this.GetValue(ConstantValues.NodeId1) }).ConfigureAwait(false);
            var actualValue = this.Get<ElementPage>().GetElements(nameof(Resources.OrderValue)).ElementAt(0).Text;
            Assert.AreEqual(expectedValue["Order"].ToString(), actualValue);
        }

        [Then(@"the title of the grid should be ""(.*)""")]
        public void ThenTheTitleOfTheGridShouldBe(string expectedTitle)
        {
            var actualTitle = this.Get<ElementPage>().GetElement(nameof(Resources.GridTitle)).Text;
            Assert.AreEqual(UIContent.Conversion[expectedTitle], actualTitle);
        }

        [Then(@"I ""(.*)"" be able to perform any actions on graphics network")]
        public void ThenIBeAbleToPerformAnyActionsOnGraphicsNetwork(string state)
        {
            if (state == ConstantValues.ShouldNot)
            {
                Assert.IsTrue(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.GridVisibility)));
            }
            else if (state == ConstantValues.Should)
            {
                Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.GridVisibility)));
            }
        }

        [Then(@"Edit Node tab should be closed")]
        public void ThenEditNodeTabShouldBeClosed()
        {
            Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion["Edit Node"]));
        }

        [Then(@"Following message ""(.*)"" should be appear")]
        public void ThenFollowingMessageShouldBeAppear(string expectedMessage)
        {
            var actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.GraphicalGridError)).Text;
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [When(@"I click on ""(.*)"" Node Button")]
        public void WhenIClickOnNodeButton(string state)
        {
            this.Get<ElementPage>().GetElement(nameof(Resources.EditNodeGraphically), formatArgs: state).Click();
        }

        [Then(@"Edited Values for the Node should be updated into the Database")]
        public async Task ThenEditedValuesForTheNodeShouldBeUpdatedIntoTheDatabaseAsync()
        {
            var expectedValue = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNodeDetials, args: new { NodeId = this.GetValue(ConstantValues.NodeId1) }).ConfigureAwait(false);
            var actualValue = this.Get<ElementPage>().GetElements(nameof(Resources.GraphicalNodeName)).ElementAt(0).Text;
            Assert.AreEqual(expectedValue["Name"].ToString(), actualValue);
        }

        [When(@"I click (on "".*"" "".*"") without making any changes to the node")]
        public void WhenIClickOnWithoutMakingAnyChangesToTheNode(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.Get<ElementPage>().Click(elementLocator);
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 60);
        }

        [Then(@"the information updated should be displayed on the graphical network")]
        public void ThenTheInformationUpdatedShouldBeDisplayedOnTheGraphicalNetwork()
        {
            var actualValue = this.Get<ElementPage>().GetElements(nameof(Resources.GraphicalNodeName)).ElementAt(0).Text;
            Assert.AreEqual(this.GetValue(ConstantValues.CreatedNodeName), actualValue);
        }
    }
}