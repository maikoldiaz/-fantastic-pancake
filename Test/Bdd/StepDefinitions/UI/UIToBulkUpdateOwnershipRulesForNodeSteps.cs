// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIToBulkUpdateOwnershipRulesForNodeSteps.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class UIToBulkUpdateOwnershipRulesForNodeSteps : EcpWebStepDefinitionBase
    {
        [When(@"I select all the records from grid")]
        [When(@"I select one or more records in the grid")]
        [StepDefinition(@"I select all the records from grid")]
        public void WhenISelectAllTheRecordsFromGrid()
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.WaitUntilElementToBeClickable(nameof(Resources.SelectAllCheckBox));
            page.Click(nameof(Resources.SelectAllCheckBox));
        }

        [When(@"I get all ownership strategies for (.*) records in the grid")]
        public void WhenIGetAllOwnershipStrategiesForRecorsInTheGrid(int numberOfRecords)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            string oldOwnershipStrategy = string.Empty;
            for (int i = 1; i <= numberOfRecords; i++)
            {
                var arguments = new object[] { i, UIContent.GridPosition[ConstantValues.OwnershipStrategyInformation] };
                var actualOwnershipStrategyPerNode = page.GetElement(nameof(Resources.GridColumnDetails), formatArgs: arguments).Text;

                if (!string.IsNullOrEmpty(actualOwnershipStrategyPerNode) && !oldOwnershipStrategy.Contains(actualOwnershipStrategyPerNode))
                {
                    oldOwnershipStrategy += actualOwnershipStrategyPerNode + ", ";
                }
            }

            this.SetValue(ConstantValues.OldOwnershipStrategyInformation, oldOwnershipStrategy.TrimEnd(',', ' '));
        }

        [When(@"I enter ownership strategy (in "".*"" "".*"" "".*"")")]
        public void WhenIEnterOwnershipStrategyInto(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            if (this.GetValue(ConstantValues.OldOwnershipStrategyInformation).Contains(','))
            {
                this.SetValue(ConstantValues.OldOwnershipStrategyInformation, this.GetValue(ConstantValues.OldOwnershipStrategyInformation).Split(',')[0]);
                page.GetElement(elementLocator).SendKeys(this.GetValue(ConstantValues.OldOwnershipStrategyInformation));
            }
            else
            {
                page.GetElement(elementLocator).SendKeys(this.GetValue(ConstantValues.OldOwnershipStrategyInformation));
            }

            page.GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [When(@"I see confirmation dialog with total number of records as (.*) and ownership strategy information")]
        public void WhenISeeConfirmationDialogWithTotalNumberOfRecordsAsAndOwnershipStrategyInformation(int numberOfRecords)
        {
            if (this.GetValue(ConstantValues.OldOwnershipStrategyInformation).Contains(','))
            {
                Assert.AreEqual(numberOfRecords, int.Parse(this.Get<ElementPage>().GetElementText(nameof(Resources.NumberOfNodesOnTheConfirmationPopUp)), CultureInfo.InvariantCulture));
            }
        }

        [When(@"I see button name as ""(.*)""")]
        public void WhenISeeButtonNameAs(string buttonName)
        {
            if (this.GetValue(ConstantValues.OldOwnershipStrategyInformation).Contains(','))
            {
                var actualButton = this.Get<ElementPage>().GetElementText(nameof(Resources.ButtonsOnCreateLogisticInterface), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Node]);
                Assert.IsTrue(buttonName.EqualsIgnoreCase(actualButton));
            }
        }

        [When(@"I click (on "".*"" "".*"") on confirmation popup")]
        public void WhenIClickOnOnConfirmationPopup(ElementLocator elementLocator)
        {
            if (this.GetValue(ConstantValues.OldOwnershipStrategyInformation).Contains(','))
            {
                this.Get<ElementPage>().Click(elementLocator);
            }
        }

        [StepDefinition(@"I see old strategies separated by comma on the interface")]
        public void WhenIShouldSeeOldStrategiesSeparatedByCommaOnTheInterface()
        {
            Assert.AreEqual(this.GetValue(ConstantValues.OldOwnershipStrategyInformation), this.Get<ElementPage>().GetElementText(nameof(Resources.OldOwnershipStrategiesInformation)));
        }

        [When(@"I enter operator (in "".*"" "".*"" "".*"")")]
        public void WhenIEnterOwnershipStrategyIn(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.GetElement(elementLocator).SendKeys(ConstantValues.EcoPetrol);
            page.GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [Then(@"(.*) records should be shown by default in the grid")]
        public void ThenRecordsShouldBeShownByDefaultInTheGrid(int numberOfReords)
        {
            var totalNumberOfRecords = int.Parse(this.Get<ElementPage>().GetElementText(nameof(Resources.PaginationCount)).Split(' ')[4], CultureInfo.InvariantCulture);
            var actualNumberOfRecordsInTheGrid = int.Parse(this.Get<ElementPage>().GetElementText(nameof(Resources.PaginationCount)).Split(' ')[2], CultureInfo.InvariantCulture);
            if (totalNumberOfRecords > 100)
            {
                Assert.AreEqual(numberOfReords, actualNumberOfRecordsInTheGrid);
            }
            else
            {
                Assert.AreEqual(totalNumberOfRecords, actualNumberOfRecordsInTheGrid);
            }
        }

        [Then(@"I should not see the old ownership strategy (from "".*"" "".*"" "".*"")")]
        public void ThenIShouldNotSeeTheOldOwnershipStrategyInThe(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().Click(elementLocator);
            var actualRules = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenu), formatArgs: UIContent.Conversion[ConstantValues.OwnershipStrategyInformation]);
            Assert.IsFalse(actualRules.Any(rule => rule.Contains(this.GetValue(ConstantValues.OldOwnershipStrategyInformation))));
        }

        [When(@"none of the records selected in the grid")]
        public void WhenNoneOfTheRecordsSelectedInTheGrid()
        {
            // Method intentionally left empty.
        }

        [Then(@"I see (.*) records in the grid are updated with new ownership strategy")]
        public void ThenISeeRecordsInTheGridAreUpdatedWithNewOwnershipStrategy(int numberOfRecords)
        {
            this.WhenIGetAllOwnershipStrategiesForRecorsInTheGrid(numberOfRecords);
            Assert.AreEqual(this.GetValue(ConstantValues.OwnershipStrategyInformation), this.GetValue(ConstantValues.OldOwnershipStrategyInformation));
        }

        [When(@"I select new ownership strategy (from "".*"" "".*"" "".*"")")]
        public async Task WhenISelectNewOwnershipStrategyFromAsync(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            try
            {
                var ownershipStrategyInformation = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetOwnershipStrategyInformation).ConfigureAwait(false);
                this.SetValue(ConstantValues.OwnershipStrategyInformation, ownershipStrategyInformation[ConstantValues.OwnershipStrategyInformation]);
                page.Click(elementLocator);
                page.Click(nameof(Resources.UploadType), this.GetValue(ConstantValues.OwnershipStrategyInformation), page.GetElements(nameof(Resources.UploadTypeName), this.GetValue(ConstantValues.OwnershipStrategyInformation)).Count);
            }
            catch (NullReferenceException)
            {
                Logger.Info("There are no ownership rules in the System");
            }
        }

        [When(@"I see (.*) records with same ownership strategy")]
        public void WhenISeeRecordsWithSameOwnershipStrategy(int numberOfRecords)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            string oldOwnershipStrategy = string.Empty;
            for (int i = 1; i <= numberOfRecords; i++)
            {
                var arguments = new object[] { i, UIContent.GridPosition[ConstantValues.OwnershipStrategyInformation] };
                var actualOwnershipStrategyPerNode = page.GetElement(nameof(Resources.GridColumnDetails), formatArgs: arguments).Text;

                if (!string.IsNullOrEmpty(actualOwnershipStrategyPerNode) && !oldOwnershipStrategy.Contains(actualOwnershipStrategyPerNode))
                {
                    oldOwnershipStrategy += actualOwnershipStrategyPerNode;
                }
            }

            Assert.AreEqual(this.GetValue(ConstantValues.OldOwnershipStrategyInformation), oldOwnershipStrategy);
        }
    }
}