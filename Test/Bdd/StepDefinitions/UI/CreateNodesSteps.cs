// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateNodesSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class CreateNodesSteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"I provide the value (for "".*"" "".*"")")]
        public void WhenIProvideTheValueFor(ElementLocator elementLocator)
        {
            this.IProvideTheValueForElement(elementLocator);
        }

        [When(@"I select any ""(.*)"" (from "".*"" "".*"")")]
        public void WhenISelectAny(string value, ElementLocator elementLocator)
        {
            this.ISelectAnyElement(value, elementLocator);
        }

        [When(@"I click on ""(.*)"" tab")]
        public void WhenIClickOnTab(string field)
        {
            this.IClickOnTab(field);
        }

        [Then(@"I should see the code and name as list (on "".*"" "".*"")")]
        public async System.Threading.Tasks.Task ThenIShouldSeeTheCodeAndNameAsListOnAsync(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().Click(elementLocator);
            var logisticCentersList = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBoxOption), formatArgs: UIContent.Conversion["LogisticCenter"]);
            var logisticCentersnListFromDB = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllStorageLocations).ConfigureAwait(true);
            Assert.AreEqual(logisticCentersnListFromDB.ToList().Count, logisticCentersList.Count);
        }

        [When(@"I click (on "".*"" "".*"") on the UI")]
        public void WhenIClickOnOnTheUI(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().ClientClick(elementLocator);
        }

        [When(@"I enter new ""(.*)"" into NodeStorageLocation name textbox")]
        public void WhenIEnterNewIntoNodeStorageLocationNameTextbox(string name)
        {
            this.IEnterNewIntoNodeStorageLocationNameTextbox(name);
        }

        [Then(@"it should be registered in the system with entered data")]
        public async System.Threading.Tasks.Task ThenItShouldBeRegisteredInTheSystemWithEnteredDataAsync()
        {
            var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastNode).ConfigureAwait(false);
            Assert.AreEqual(lastRow[ConstantValues.Name], this.GetValue(ConstantValues.CreatedNodeName));
        }

        [Then(@"I should see the list of Products as empty")]
        public void ThenIShouldSeeTheListOfProductsAsEmpty()
        {
            ////this.Then("I should see message \"Sin registros\"");
            this.IShouldSeeMessage("Sin registros");
        }

        [Then(@"I should be able to search all the ""(.*)"" belongs to the Storage Location")]
        public async Task ThenIShouldBeAbleToSearchAllTheBelongsToTheStorageLocationAsync(string field)
        {
            var nodeStoreageLocationRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNodeStorageLocationByStorageLocationId, args: new { storageLocationId = UIContent.Conversion[ConstantValues.StorageLocationValue] }).ConfigureAwait(false);
            var nodeStoreageLocationId = nodeStoreageLocationRow["NodeStorageLocationId"];
            var storageLocationProductRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetProductByNodeStorageLocation, args: new { NodeStoreageLocationId = nodeStoreageLocationId }).ConfigureAwait(false);
            var productId = storageLocationProductRow[ConstantValues.ProductId];
            var productRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetProductNameById, args: new { ProductId = @productId }).ConfigureAwait(false);
            Assert.AreEqual(UIContent.Conversion[field], productRow[ConstantValues.Name]);
        }

        [Then(@"I should see the Information of selected storgae Location")]
        public void ThenIShouldSeeTheInformationOfSelectedStorgaeLocation()
        {
            Assert.AreEqual(this.GetValue(Entities.Keys.RandomFieldValue), this.Get<ElementPage>().GetElements(nameof(Resources.GetStorageLocationName)).ElementAt(1).GetAttribute("value"));
        }

        [Then(@"I should see (.*) ""(.*)"" in the Product list grid")]
        public void ThenIShouldSeeInTheProductListGrid(int productNumber, string field)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.ElementByText));
            var count = this.Get<ElementPage>().GetElements(nameof(Resources.GetColumnCountfromGrid)).Count;
            Assert.AreEqual(UIContent.Conversion[field], this.Get<ElementPage>().GetElement(nameof(Resources.GetProductNameFromGrid), count - productNumber).Text);
        }

        [Then(@"I should see the Product deleted form the grid")]
        public void ThenIShouldSeeTheProductDeletedFormTheGrid()
        {
            this.ThenIShouldSeeTheListOfProductsAsEmpty();
        }

        [When(@"I provide existing NodeStrogeLoaction value (for "".*"" "".*"")")]
        public void WhenIProvideExistingNodeStrogeLoactionValueFor(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().GetElements(elementLocator).ElementAt(1).SendKeys(this.GetValue(Entities.Keys.RandomFieldValue));
        }

        [Then(@"I should be able to see the ""(.*)"" that belongs to the Node")]
        public void ThenIShouldBeAbleToSeeTheThatBelongsToTheNode(string field)
        {
            Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), this.Get<ElementPage>().GetElements(nameof(Resources.GetColumnCountfromGrid)).ElementAt(0).Text);
            Assert.IsNotNull(field);
        }

        [Then(@"I should see the ""(.*)"" that matches the search criteria")]
        public void ThenIShouldSeeTheThatMatchesTheSearchCriteria(string field)
        {
            this.ThenIShouldSeeInTheProductListGrid(1, field);
        }

        [Then(@"I should see the list of ""(.*)"" belongs to selected Storage Location")]
        public async Task ThenIShouldSeeTheListOfBelongsToSelectedStorageLocationAsync(string productName)
        {
            await this.ThenIShouldBeAbleToSearchAllTheBelongsToTheStorageLocationAsync(productName).ConfigureAwait(false);
        }

        [Then(@"I should be able to see the ""(.*)"" in the list")]
        public void ThenIShouldBeAbleToSeeTheInTheList(string productName)
        {
            this.ThenIShouldSeeTheThatMatchesTheSearchCriteria(productName);
        }

        [When(@"I select ElementValue (from "".*"" "".*"")")]
        [When(@"I select SegmentValue (from "".*"" "".*"")")]
        public async Task WhenISelectSegmentValueFromAsync(ElementLocator elementLocator)
        {
            await this.ISelectSegmentValueFromAsync(elementLocator).ConfigureAwait(false);
        }
    }
}