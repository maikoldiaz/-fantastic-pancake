// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureOwnershipRuleInNodesSteps.cs" company="Microsoft">
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

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class ConfigureOwnershipRuleInNodesSteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should see empty value in the grid")]
        public void ThenIShouldSeeEmptyValueInThe(Table table)
        {
            foreach (var row in table?.Rows.Select((value) => (Default: value["Rule"], Unused: string.Empty)))
            {
                this.VerifyThat(() => Assert.AreEqual(string.Empty, this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromGrid), UIContent.GridPosition[row.Default]).Text));
            }
        }

        [When(@"I select Rule from Respective dropdowns")]
        public void WhenISelectValueFrom(Table table)
        {
            foreach (var row in table?.Rows.Select((value) => (Rule: value["Rule"], Dropdown: value["Dropdown"])))
            {
                this.SetValue(row.Dropdown, UIContent.Conversion[row.Rule]);
                var dropDownSelection = this.Get<ElementPage>().GetElement(nameof(Resources.Rule_SelectBox), UIContent.Conversion[row.Dropdown]);
                dropDownSelection.Click();
                this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), UIContent.Conversion[row.Rule]).Click();
            }
        }

        [Then(@"I should see selected values in the grid")]
        public void ThenIShouldSeeSelectedValuesInTheGrid(Table table)
        {
            foreach (var row in table?.Rows.Select((value) => (Expected: value["Expected"], Selected: value["Selected"])))
            {
                this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                this.VerifyThat(() => Assert.AreEqual(this.GetValue(row.Expected), this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromGrid), UIContent.GridPosition[row.Selected]).Text));
            }
        }

        [Then(@"I should see the message on interface ""(.*)""")]
        public void ThenIShouldSeeTheMessageNear(string expectedMessage)
        {
            var errorMessage = this.Get<ElementPage>().GetElements(nameof(Resources.DeltaInitialMessage));
            foreach (var actualMessage in errorMessage)
            {
                Assert.AreEqual(expectedMessage, actualMessage.Text);
            }
        }
    }
}
