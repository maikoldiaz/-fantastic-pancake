// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkGraphStylesAndEventsSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Executors;
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class NetworkGraphStylesAndEventsSteps : EcpWebStepDefinitionBase
    {
        [When(@"I select ""(.*)"" value (from "".*"" "".*"" "".*"")")]
        public void WhenISelectValueFrom(string value, ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            page.Click(elementLocator);
            page.GetElement(elementLocator).SendKeys(value == ConstantValues.Todos ? value : this.GetValue(ConstantValues.Node1Name));
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.GetElement(nameof(Resources.SelectBoxOptionByValue), value == ConstantValues.Todos ? value : this.GetValue(ConstantValues.Node1Name)).Click();
        }

        [When(@"I hovers over a node")]
        public void WhenIHoversOverANode()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: this.GetValue(ConstantValues.NodeId1));
            this.Get<MouseActionsPage>().MouseHover(elements[0]);
        }

        [Then(@"I should see the node according to the style defined for ""(.*)""")]
        public void ThenIShouldSeeTheNodeAccordingToTheStyleDefinedFor(string state)
        {
            var page = this.Get<ElementPage>();
            if (state.EqualsIgnoreCase(ConstantValues.HoveredNode))
            {
                Assert.AreEqual(ConstantValues.ShadowStyle, page.GetElement(nameof(Resources.GrphicalStateOfHoveredNode), formatArgs: ConstantValues.OrderNumber).GetCssValue(ConstantValues.BoxShadow));
            }
            else
            {
                var elements = page.GetElements(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: this.GetValue(ConstantValues.NodeId1));
                Assert.IsTrue(elements[0].GetAttribute("class").Contains("selected"));
            }
        }

        [When(@"I select a node")]
        public void WhenISelectANode()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: this.GetValue(ConstantValues.NodeId1));
            elements[0].Click();
        }

        [When(@"I hovers over an active connection")]
        public void WhenIHoversOverAnActiveConnection()
        {
            var element = this.Get<ElementPage>().GetElement(nameof(Resources.ConnectionIdInGraphicNetwork), this.GetValue(ConstantValues.NodeId2), this.GetValue(ConstantValues.NodeId1));
            this.Get<MouseActionsPage>().MouseHover(element);
        }

        [When(@"I select an active connection")]
        public void WhenISelectAnActiveConnection()
        {
            this.Get<ElementPage>().GetElement(nameof(Resources.ConnectionIdInGraphicNetwork), this.GetValue(ConstantValues.NodeId2), this.GetValue(ConstantValues.NodeId1)).Click();
        }

        [Then(@"I should see the input and output active connections of the node according to the style defined for ""(.*)""")]
        [Then(@"I should see the connection according to the style defined for ""(.*)""")]
        public void ThenIShouldSeeTheConnectionAccordingToTheStyleDefinedFor(string state)
        {
            var page = this.Get<ElementPage>();
            if (state.EqualsIgnoreCase(ConstantValues.HoveredActiveConnection))
            {
                Assert.AreEqual(ConstantValues.StrokeStyle, page.GetElement(nameof(Resources.GraphicalStateOfHoveredConnection), this.GetValue(ConstantValues.NodeId2), this.GetValue(ConstantValues.NodeId1)).GetCssValue(ConstantValues.Stroke));
            }
            else
            {
                var element = page.GetElement(nameof(Resources.ConnectionIdInGraphicNetwork), this.GetValue(ConstantValues.NodeId2), this.GetValue(ConstantValues.NodeId1));
                Assert.IsTrue(element.GetAttribute("class").Contains("selected"));
            }
        }

        [When(@"I move a node with the click and drag action over any area of the node")]
        public void WhenIMoveANodeWithTheClickAndDragActionOverAnyAreaOfTheNode()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: this.GetValue(ConstantValues.NodeId1));
            this.SetValue(Entities.Keys.RandomFieldValue, elements[0].GetAttribute("style"));
            this.Get<MouseActionsPage>().DragAndDropToARandomPosition(elements[0], 250, 150);
        }

        [Then(@"the node should move within the grid")]
        public void ThenTheNodeShouldMoveWithinTheGrid()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.NodeIdInGraphicNetwork), formatArgs: this.GetValue(ConstantValues.NodeId1));
            Assert.AreNotEqual(this.GetValue(Entities.Keys.RandomFieldValue), elements[0].GetAttribute("style"));
        }

        [Then(@"the system should perform the corresponding action")]
        public void ThenTheSystemShouldPerformTheCorrespondingAction()
        {
            Assert.IsFalse(this.Get<ElementPage>().GetElement(nameof(Resources.BackgroundGridInGraph)).GetAttribute("class").Contains("bg"));
        }

        [Then(@"the system should perform the ""(.*)"" action on the network graph")]
        public void ThenTheSystemShouldPerformTheActionOnTheNetworkGraph(string action)
        {
            var page = this.Get<ElementPage>();
            if (action.EqualsIgnoreCase(ConstantValues.ZoomIn))
            {
                Assert.IsTrue(page.GetElement(nameof(Resources.ZoomValueInGraph)).GetAttribute("style").Contains("1.1"));
            }
            else if (action.EqualsIgnoreCase(ConstantValues.ZoomOut))
            {
                Assert.IsTrue(page.GetElement(nameof(Resources.ZoomValueInGraph)).GetAttribute("style").Contains("0.9"));
            }
            else
            {
                Assert.IsTrue(page.GetElement(nameof(Resources.ZoomValueInGraph)).GetAttribute("style").Contains("1"));
            }
        }
    }
}