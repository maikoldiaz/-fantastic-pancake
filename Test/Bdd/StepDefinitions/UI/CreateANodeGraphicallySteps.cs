// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateANodeGraphicallySteps.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Executors;
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class CreateANodeGraphicallySteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"create node tab should be closed")]
        public void WhenCreateNodeTabShouldBeClosed()
        {
            Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion[ConstantValues.CreateNode]));
        }

        [When(@"I ""(.*)"" the newly created node")]
        public async Task WhenITheNewlyCreatedNodeAsync(string action)
        {
            if (action.EqualsIgnoreCase(ConstantValues.Selected))
            {
                var page = this.Get<ElementPage>();
                page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                var categoryElement = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetCategoryElementByName, args: new { name = this.GetValue(ConstantValues.CategorySegment) }).ConfigureAwait(false);
                if (int.Parse(categoryElement[ConstantValues.CategoryId], CultureInfo.InvariantCulture) == 8)
                {
                    page.Click(nameof(Resources.NodeNameOnGraphicalNetwork), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Segment]);
                }
                else if (int.Parse(categoryElement[ConstantValues.CategoryId], CultureInfo.InvariantCulture) == 2)
                {
                    page.Click(nameof(Resources.NodeNameOnGraphicalNetwork), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Node]);
                }
            }
            else if (action == ConstantValues.HoverOver)
            {
                var page = this.Get<MouseActionsPage>();
                var categoryElement = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetCategoryElementByName, args: new { name = this.GetValue(ConstantValues.CategorySegment) }).ConfigureAwait(false);
                if (int.Parse(categoryElement[ConstantValues.CategoryId], CultureInfo.InvariantCulture) == 8)
                {
                    page.MouseHover(nameof(Resources.NodeNameOnGraphicalNetwork), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Segment]);
                }
                else if (int.Parse(categoryElement[ConstantValues.CategoryId], CultureInfo.InvariantCulture) == 2)
                {
                    page.MouseHover(nameof(Resources.NodeNameOnGraphicalNetwork), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Node]);
                }
            }
        }

        [Then(@"I should see that ""(.*)"" button as ""(.*)""")]
        public void ThenIShouldSeeThatButtonAs(string element, string buttonName)
        {
            Assert.IsTrue(buttonName.EqualsIgnoreCase(this.Get<ElementPage>().GetElementText(nameof(Resources.ButtonName), formatArgs: element.ToCamelCase())));
        }

        [Then(@"created node should be shown on the graphical network")]
        public async Task ThenCreatedNodeShouldBeShownOnTheGraphicalNetworkAsync()
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            var categoryElement = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetCategoryElementByName, args: new { name = this.GetValue(ConstantValues.CategorySegment) }).ConfigureAwait(false);
            if (int.Parse(categoryElement[ConstantValues.CategoryId], CultureInfo.InvariantCulture) == 8)
            {
                Assert.AreEqual(this.GetValue(ConstantValues.CreatedNodeName), page.GetElementText(nameof(Resources.NodeNameOnGraphicalNetwork), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Segment]));
            }
            else if (int.Parse(categoryElement[ConstantValues.CategoryId], CultureInfo.InvariantCulture) == 2)
            {
                Assert.AreEqual(this.GetValue(ConstantValues.CreatedNodeName), page.GetElementText(nameof(Resources.NodeNameOnGraphicalNetwork), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Node]));
            }
        }

        [When(@"I see node with graphical state ""(.*)"" on the network")]
        [Then(@"I should see node with graphical state ""(.*)"" on the network")]
        public async Task ThenIShouldSeeNodeWithGraphicalStateOnTheNetworkAsync(string nodeGraphicalState)
        {
            var page = this.Get<ElementPage>();
            if (nodeGraphicalState == ConstantValues.UnsavedNode)
            {
                Assert.IsTrue(page.CheckIfElementIsPresent(nameof(Resources.GraphicalStateOfUnSavedNode)));
            }
            else if (nodeGraphicalState == ConstantValues.ActiveNode)
            {
                Assert.IsFalse(page.CheckIfElementIsPresent(nameof(Resources.GraphicalStateOfInactiveNode)));
            }
            else if (nodeGraphicalState == ConstantValues.SelectedNode)
            {
                Assert.IsTrue(page.CheckIfElementIsPresent(nameof(Resources.GraphicalStateOfSelectedNode)));
            }
            else if (nodeGraphicalState == ConstantValues.InActiveNode)
            {
                Assert.IsTrue(page.CheckIfElementIsPresent(nameof(Resources.GraphicalStateOfInactiveNode)));
            }
            else if (nodeGraphicalState == ConstantValues.HoveredNode)
            {
                var categoryElement = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetCategoryElementByName, args: new { name = this.GetValue(ConstantValues.CategorySegment) }).ConfigureAwait(false);
                if (int.Parse(categoryElement[ConstantValues.CategoryId], CultureInfo.InvariantCulture) == 8)
                {
                    Assert.AreEqual(ConstantValues.ShadowStyle, page.GetElement(nameof(Resources.GrphicalStateOfHoveredNode), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Segment]).GetCssValue(ConstantValues.BoxShadow));
                }
                else if (int.Parse(categoryElement[ConstantValues.CategoryId], CultureInfo.InvariantCulture) == 2)
                {
                    Assert.AreEqual(ConstantValues.ShadowStyle, page.GetElement(nameof(Resources.GrphicalStateOfHoveredNode), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Node]).GetCssValue(ConstantValues.BoxShadow));
                }
            }
        }

        [Then(@"data should be cleared (for "".*"" "".*"" "".*"")")]
        public void ThenDataShouldBeClearedFor(ElementLocator elementLocator)
        {
            Assert.IsTrue(string.IsNullOrEmpty(this.Get<ElementPage>().GetElement(elementLocator).GetAttribute(ConstantValues.Value)));
        }

        [StepDefinition(@"I see this message ""(.*)"" (on "".*"" "".*"" "".*"")")]
        public void ThenIShouldSeeThisMessageOn(string message, ElementLocator elementLocator)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Assert.AreEqual(message, this.Get<ElementPage>().GetElement(elementLocator).Text);
            }
        }

        [Then(@"node creation is failed")]
        public void ThenNodeCreationIsFailed()
        {
            // Method intentionally left empty.
        }

        [Then(@"created node should not be shown on the network")]
        public void ThenCreatedNodeShouldNotBeShownOnTheNetwork()
        {
            Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: this.GetValue(ConstantValues.CreatedNodeName)));
        }
    }
}