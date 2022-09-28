// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageRelationshipMovementsTypesStep.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ManageRelationshipMovementsTypesStep : EcpWebStepDefinitionBase
    {
        [Then(@"I verify all columns ""(.*)"",""(.*)"",""(.*)"",""(.*)"",""(.*)"",""(.*)"",""(.*)"" are present in Grid")]
        public void ThenIVerifyAllColumnsArePresentInGrid(string col1, string col2, string col3, string col4, string col5, string col6, string col7)
        {
            var firstcolumnName = this.Get<ElementPage>().GetElement(nameof(Resources.GetHeadingsFromGrid), 1).Text;
            var secondcolumnName = this.Get<ElementPage>().GetElement(nameof(Resources.GetHeadingsFromGrid), 2).Text;
            var thirdcolumnName = this.Get<ElementPage>().GetElement(nameof(Resources.GetHeadingsFromGrid), 3).Text;
            var fourthcolumnName = this.Get<ElementPage>().GetElement(nameof(Resources.GetHeadingsFromGrid), 4).Text;
            var fifthcolumnName = this.Get<ElementPage>().GetElement(nameof(Resources.GetHeadingsFromGrid), 5).Text;
            var sixthcolumnName = this.Get<ElementPage>().GetElement(nameof(Resources.GetHeadingsFromGrid), 6).Text;
            var seventhcolumnName = this.Get<ElementPage>().GetElement(nameof(Resources.GetHeadingsFromGrid), 7).Text;

            Assert.IsTrue(col1.EqualsIgnoreCase(firstcolumnName));
            Assert.IsTrue(col2.EqualsIgnoreCase(secondcolumnName));
            Assert.IsTrue(col3.EqualsIgnoreCase(thirdcolumnName));
            Assert.IsTrue(col4.EqualsIgnoreCase(fourthcolumnName));
            Assert.IsTrue(col5.EqualsIgnoreCase(fifthcolumnName));
            Assert.IsTrue(col6.EqualsIgnoreCase(sixthcolumnName));
            Assert.IsTrue(col7.EqualsIgnoreCase(seventhcolumnName));
        }

        [Then(@"I verify the Relationship Movements Types list sorted by creation date in decending order")]
        public async System.Threading.Tasks.Task ThenIVerifyTheRelationshipMovementsTypesListSortedByCreationDateInDecendingOrderAsync()
        {
            var expectedLatestRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLatestAnnulationRecord).ConfigureAwait(false);

            var movementName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 1).Text;
            var cancellationName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 2).Text;
            var sourceNodeName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 3).Text;
            var destinationNodeName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 4).Text;
            var sourceProductName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 5).Text;
            var destinationProductName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 6).Text;

            Assert.AreEqual(expectedLatestRow["MovementType"], movementName);
            Assert.AreEqual(expectedLatestRow["CancellationType"], cancellationName);
            Assert.AreEqual(expectedLatestRow["Source"], sourceNodeName);
            Assert.AreEqual(expectedLatestRow["Destination"], destinationNodeName);
            Assert.AreEqual(expectedLatestRow["SourceProduct"], sourceProductName);
            Assert.AreEqual(expectedLatestRow["DestinationProduct"], destinationProductName);
        }

        [Then(@"I verify the results should be sorted according to ""(.*)""")]
        public async System.Threading.Tasks.Task ThenIVerifyTheResultsShouldBeSortedAccordingToAsync(string field)
        {
            IEnumerable<dynamic> annulationsElementRecords = null;

            switch (field)
            {
                case "Movements Type":
                    annulationsElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetAnnulationRecordsByMovementTypeDesc).ConfigureAwait(false);
                    var annulationsElementsList = annulationsElementRecords.ToDictionaryList();
                    foreach (var row in annulationsElementsList)
                    {
                        await Task.Delay(2000).ConfigureAwait(false);
                        Assert.AreEqual(row["MovementType"], this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 1).Text);
                    }

                    break;
                case "Cancellation type":
                    annulationsElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetAnnulationRecordsByCancellationTypeDesc).ConfigureAwait(false);
                    var annulationsElementsList1 = annulationsElementRecords.ToDictionaryList();
                    foreach (var row in annulationsElementsList1)
                    {
                        await Task.Delay(2000).ConfigureAwait(false);
                        Assert.AreEqual(row["CancellationType"], this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 2).Text);
                    }

                    break;
                case "Source":
                    annulationsElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetAnnulationRecordsBySourceNodeDesc).ConfigureAwait(false);
                    var annulationsElementsList2 = annulationsElementRecords.ToDictionaryList();
                    foreach (var row in annulationsElementsList2)
                    {
                        await Task.Delay(2000).ConfigureAwait(false);
                        Assert.AreEqual(row["Source"], this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 3).Text);
                    }

                    break;
                case "Destination":
                    annulationsElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetAnnulationRecordsByDestinationNodeDesc).ConfigureAwait(false);
                    var annulationsElementsList3 = annulationsElementRecords.ToDictionaryList();
                    foreach (var row in annulationsElementsList3)
                    {
                        await Task.Delay(2000).ConfigureAwait(false);
                        Assert.AreEqual(row["Destination"], this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 4).Text);
                    }

                    break;
                case "Source Prod.":
                    annulationsElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetAnnulationRecordsBySourceProductDesc).ConfigureAwait(false);
                    var annulationsElementsList4 = annulationsElementRecords.ToDictionaryList();
                    foreach (var row in annulationsElementsList4)
                    {
                        await Task.Delay(2000).ConfigureAwait(false);
                        Assert.AreEqual(row["SourceProduct"], this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 5).Text);
                    }

                    break;
                case "Destination Prod.":
                    annulationsElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetAnnulationRecordsByDestinationProductDesc).ConfigureAwait(false);
                    var annulationsElementsList5 = annulationsElementRecords.ToDictionaryList();
                    foreach (var row in annulationsElementsList5)
                    {
                        await Task.Delay(2000).ConfigureAwait(false);
                        Assert.AreEqual(row["DestinationProduct"], this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 6).Text);
                    }

                    break;
                default:
                    break;
            }
        }

        [When(@"I provide value for ""(.*)"" in the Grid for filter")]
        public async Task WhenIProvideValueForInTheGridForFilterAsync(string field)
        {
            if (field.EqualsIgnoreCase("Movements Type"))
            {
                var name = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSelectedMovementType).ConfigureAwait(false);
                this.SetValue(Keys.RandomFieldValue, name[ConstantValues.Name]);
                this.Get<ElementPage>().GetElement(nameof(Resources.GetSourceCategoryElement)).SendKeys(this.GetValue(Keys.RandomFieldValue));
                this.Get<ElementPage>().GetElement(nameof(Resources.GetSourceCategoryElement)).SendKeys(OpenQA.Selenium.Keys.Enter);
                await Task.Delay(3000).ConfigureAwait(false);
            }
            else if (field.EqualsIgnoreCase("Cancellation type"))
            {
                var name = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSelectedCancellationType).ConfigureAwait(false);
                this.SetValue(Keys.RandomFieldValue, name[ConstantValues.Name]);
                this.Get<ElementPage>().GetElement(nameof(Resources.GetCancellationTypeElement)).SendKeys(this.GetValue(Keys.RandomFieldValue));
                this.Get<ElementPage>().GetElement(nameof(Resources.GetCancellationTypeElement)).SendKeys(OpenQA.Selenium.Keys.Enter);
                await Task.Delay(3000).ConfigureAwait(false);
            }
            else if (field.EqualsIgnoreCase("Source"))
            {
                var name = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFirstOriginType).ConfigureAwait(false);
                this.SetValue(Keys.RandomFieldValue, name[ConstantValues.Name]);
                var sourceTypePage = this.Get<ElementPage>();
                sourceTypePage.GetElement(nameof(Resources.GetSourceNodeDropdown)).Click();
                this.Get<ElementPage>().GetElement(nameof(Resources.GetSourceNodeDropdown)).SendKeys(this.GetValue(Keys.RandomFieldValue));
                this.Get<ElementPage>().GetElement(nameof(Resources.GetSourceNodeDropdown)).SendKeys(OpenQA.Selenium.Keys.Enter);
            }
        }

        [Then(@"I should see the filtered data in the grid for ""(.*)""")]
        public void ThenIShouldSeeTheFilteredDataInTheGridFor(string field)
        {
            if (field.EqualsIgnoreCase("Movements Type"))
            {
                Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 1).Text);
            }
            else if (field.EqualsIgnoreCase("Cancellation type"))
            {
                Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 2).Text);
            }
            else if (field.EqualsIgnoreCase("Source"))
            {
                Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 3).Text);
            }
        }

        [Then(@"I should see an unauthorized error page")]
        public void ThenIShouldSeeAnUnauthorizedErrorPage()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.Error403Page));
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.Error403Page));
        }
    }
}
