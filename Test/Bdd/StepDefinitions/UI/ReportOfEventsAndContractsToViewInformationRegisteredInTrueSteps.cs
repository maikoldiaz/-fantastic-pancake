// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportOfEventsAndContractsToViewInformationRegisteredInTrueSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ocaramba.Types;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class ReportOfEventsAndContractsToViewInformationRegisteredInTrueSteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should see Segment list with all ""(.*)"" category elements")]
        public void ThenIShouldSeeSegmentListWithAllCategoryElements(string text)
        {
            var page = this.Get<ElementPage>();
            ////this.When("I select \"Transporte\" from \"segment\"");
            this.SelectValueFromDropDown("Transporte", "segment");
            page.GetElement(nameof(Resources.NodeFilterSelectBox)).SendKeys(text);
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            var count = this.Get<ElementPage>().GetElements(nameof(Resources.NodeOptions)).Count;
            Assert.IsTrue(count > 0);
        }

        [Then(@"I should see Node control with ""(.*)"" option")]
        public void ThenIShouldSeeNodeControlWithOption(string text)
        {
            var page = this.Get<ElementPage>();
            page.GetElement(nameof(Resources.NodeFilterSelectBox)).SendKeys(Keys.Control + "a" + Keys.Delete);
            page.GetElement(nameof(Resources.NodeFilterSelectBox)).SendKeys(text + OpenQA.Selenium.Keys.Enter);
        }

        [When(@"I enter ""(.*)"" into node")]
        public void WhenIEnterIntoNode(string value)
        {
            var page = this.Get<ElementPage>();
            page.GetElement(nameof(Resources.NodeFilterSelectBox)).Click();
            page.Get<ElementPage>().GetElement(nameof(Resources.NodeFilterSelectBox)).SendKeys(value);
        }

        [Then(@"I should see all the node values of the selected segment")]
        public void ThenIShouldSeeAllTheNodeValuesOfTheSelectedSegment()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            var count = this.Get<ElementPage>().GetElements(nameof(Resources.NodeOptions)).Count;
            Assert.IsTrue(count > 0);
        }

        [Then(@"I should see ""(.*)"" message")]
        public void ThenIShouldSeeMessage(string expected)
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ErrorMessageInInicioTab));
            Assert.AreEqual(expected, this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessageInInicioTab)).Text);
        }

        [When(@"I enter date ""(.*)"" (into "".*"" "".*"")")]
        public void WhenIEnterDateInto(string date, ElementLocator elementLocator)
        {
            this.Get<ElementPage>().GetElement(elementLocator).Click();
            this.EnterValueIntoTextBox(elementLocator, date);
        }
    }
}
