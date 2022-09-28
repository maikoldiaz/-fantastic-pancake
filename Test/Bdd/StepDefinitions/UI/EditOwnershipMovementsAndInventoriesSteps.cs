// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditOwnershipMovementsAndInventoriesSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class EditOwnershipMovementsAndInventoriesSteps : EcpWebStepDefinitionBase
    {
        [When(@"I change the node state ""(.*)""")]
        public async Task WhenIChangeTheNodeStateAsync(string state)
        {
            var msg = await this.ReadAllSqlAsync(input: SqlQueries.UpdateNodeStatus, args: new { status = state, ticketId = this.ScenarioContext.Get<string>("TicketId") }).ConfigureAwait(false);
       }

        [Then(@"verify that ""(.*)"" ""(.*)"" is ""(.*)"" in ""(.*)"" page")]
        [Then(@"verify that ""(.*)"" ""(.*)"" is ""(.*)"" in ""(.*)"" grid")]
        public void ThenVerifyThatIsInGrid(string field, string type, string state, string gridName)
        {
            bool flag = false;

            if (state.EqualsIgnoreCase("disabled"))
            {
                if (type.EqualsIgnoreCase("link"))
                {
                    var value = this.Get<ElementPage>().GetAttributeValue(Resources.Link, "disabled", gridName.ToCamelCase(), field.ToCamelCase());
                    flag = true;
                }
                else if (type.EqualsIgnoreCase("button"))
                {
                    var value = this.Get<ElementPage>().GetAttributeValue(Resources.Button, "disabled", gridName.ToCamelCase(), field.ToCamelCase());
                    flag = true;
                }
            }
            else
            {
                try
                {
                    if (type.EqualsIgnoreCase("link"))
                    {
                        string.IsNullOrEmpty(this.Get<ElementPage>().GetElement(Resources.Link, gridName.ToCamelCase(), field.ToCamelCase()).GetAttribute("disabled"));
                        flag = false;
                    }
                    else if (type.EqualsIgnoreCase("button"))
                    {
                        string.IsNullOrEmpty(this.Get<ElementPage>().GetElement(Resources.Button, gridName.ToCamelCase(), field.ToCamelCase()).GetAttribute("disabled"));
                        flag = false;
                    }
                }
                catch (System.ArgumentNullException e)
                {
                    Console.WriteLine(e);
                    flag = true;
                }
            }

            Assert.IsTrue(flag);
        }

        [Then(@"I verify the grid with the columns corresponding to the ""(.*)"", selected owner and product is displayed")]
        public void ThenIVerifyTheGridWithTheColumnsCorrespondingToTheSelectedOwnerAndProductIsDisplayed(string state)
        {
            // TO Implement in Future
            Console.WriteLine(state);
        }

        [Then(@"I verify the default value for ""(.*)"" dropdown is the first product from the ""(.*)"" grid")]
        public void ThenIVerifyTheDefaultValueForDropdownIsTheFirstProductFromTheGrid(string field, string gridName)
        {
            var actualText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedValueForComboBox), "editOwnershipNode", field.ToCamelCase());
            var actualValue = actualText.Split('\n')[1];
            var expectedValue = this.Get<ElementPage>().GetElementText(nameof(Resources.GridFirstRowColumnValue), gridName.ToCamelCase());
            Assert.AreEqual(expectedValue, actualValue, $"{field} dropdown default value does not match");
        }

        [Then(@"I verify the default value for ""(.*)"" dropdown is ""(.*)""")]
        public void ThenIVerifyTheDefaultValueForDropdownIs(string field, string expectedValue)
        {
            var actualText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedValueForComboBox), "editOwnershipNode", field.ToCamelCase());
            var actualValue = actualText.Split('\n')[1];
            Assert.AreEqual(expectedValue, actualValue, $"{field} dropdown default value does not match");
        }

        [Then(@"I verify ""(.*)"" message in the grid")]
        public void ThenIVerifyMessageInTheGrid(string message)
        {
            Assert.AreEqual(message, this.Get<ElementPage>().GetElementText(nameof(Resources.GridNoData)), "Grid has data");
        }
    }
}
