// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateAndDeleteTheTransformationSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class UpdateAndDeleteTheTransformationSteps : EcpWebStepDefinitionBase
    {
        [Then(@"it should be registered in the system with updated data")]
        public void ThenItShouldBeRegisteredInTheSystemWithUpdatedData()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"it should be deleted from the system")]
        public async System.Threading.Tasks.Task ThenItShouldBeDeletedFromTheSystemAsync()
        {
            var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastUpdatedTransformation).ConfigureAwait(false);
            Assert.AreEqual(ConstantValues.Deleted, lastRow["IsDeleted"]);
        }

        [Then(@"it should refresh the ""(.*)"" list on the grid")]
        public void ThenItShouldRefreshTheListOnTheGrid(string type)
        {
            _ = type is null ? throw new System.ArgumentNullException(type) : type;

            if (type.Contains("inventories"))
            {
                this.Get<ElementPage>().GetElement(nameof(Resources.InventoryNodeOrginTextBox)).SendKeys(this.ScenarioContext["Origin_NodeName"].ToString());
                this.Get<ElementPage>().GetElement(nameof(Resources.InventoryNodeOrginTextBox)).SendKeys(OpenQA.Selenium.Keys.Enter);
            }
            else if (type.Contains("movements"))
            {
                this.Get<ElementPage>().GetElement(nameof(Resources.MovementNodeOrginTextBox)).SendKeys(this.ScenarioContext["Origin_NodeName"].ToString());
                this.Get<ElementPage>().GetElement(nameof(Resources.MovementNodeOrginTextBox)).SendKeys(OpenQA.Selenium.Keys.Enter);
            }

            ////this.Then("I should see message \"Sin registros\"");
            this.IShouldSeeMessage("Sin registros");
        }

        [When(@"I click (on "".*"" "".*"") for existing transformation")]
        public void WhenIClickOnForExistingTransformation(ElementLocator elementLocator)
        {
            _ = elementLocator is null ? throw new System.ArgumentNullException(nameof(elementLocator)) : elementLocator;
            if (elementLocator.Value.Contains("inventories"))
            {
                this.Get<ElementPage>().GetElement(nameof(Resources.InventoryNodeOrginTextBox)).SendKeys(this.ScenarioContext["Origin_NodeName"].ToString());
                this.Get<ElementPage>().GetElement(nameof(Resources.InventoryNodeOrginTextBox)).SendKeys(OpenQA.Selenium.Keys.Enter);
            }
            else if (elementLocator.Value.Contains("movements"))
            {
                this.Get<ElementPage>().GetElement(nameof(Resources.MovementNodeOrginTextBox)).SendKeys(this.ScenarioContext["Origin_NodeName"].ToString());
                this.Get<ElementPage>().GetElement(nameof(Resources.MovementNodeOrginTextBox)).SendKeys(OpenQA.Selenium.Keys.Enter);
            }

            this.Get<ElementPage>().GetElement(elementLocator).Click();
        }
    }
}
