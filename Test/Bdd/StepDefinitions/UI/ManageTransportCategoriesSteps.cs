// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageTransportCategoriesSteps.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class ManageTransportCategoriesSteps : EcpWebStepDefinitionBase
    {
        [When(@"I provide value (for "".*"" "".*"") that exceeds ""(.*)"" characters")]
        [When(@"I update value (for "".*"" "".*"") that exceeds ""(.*)"" characters")]
        public void WhenIProvideValueForThatExceedsCharacters(ElementLocator elementLocator, int limit)
        {
            this.SetValue<dynamic>(Entities.Keys.RandomFieldValue, new Faker().Random.AlphaNumeric(limit + 1));
#pragma warning disable CA1062 // Validate arguments of public methods
            if (elementLocator.Value.EqualsIgnoreCase(ConstantValues.StorageLocationNameValue))
#pragma warning restore CA1062 // Validate arguments of public methods
            {
                this.Get<ElementPage>().GetElements(elementLocator).ElementAt(1).SendKeys(this.GetValue(Keys.RandomFieldValue));
            }
            else
            {
                this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue<dynamic>(Entities.Keys.RandomFieldValue));
                this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Tab);
            }
        }

        [When(@"I provide value (for "".*"" "".*"") that contains special characters other than expected")]
        [When(@"I update value (for "".*"" "".*"") that contains special characters other than expected")]
        public void WhenIProvideValueForThatContainsSpecialCharactersOtherThanExpected(ElementLocator elementLocator)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            if (elementLocator.Value.EqualsIgnoreCase(ConstantValues.StorageLocationNameValue))
#pragma warning restore CA1062 // Validate arguments of public methods
            {
                this.SetValue(Keys.RandomFieldValue, string.Concat($"Automation#", new Faker().Random.AlphaNumeric(5)));
                this.Get<ElementPage>().GetElements(elementLocator).ElementAt(1).SendKeys(this.GetValue(Keys.RandomFieldValue));
            }
            else
            {
                this.SetValue<dynamic>(Entities.Keys.RandomFieldValue, string.Concat($"AutomationCategory#", new Faker().Random.AlphaNumeric(5)));
                this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue<dynamic>(Entities.Keys.RandomFieldValue));
                this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Tab);
            }
        }

        [When(@"I update value (for "".*"" "".*"")")]
        public void WhenIUpdateValueFor(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().ClearText(elementLocator);
            this.SetValue<dynamic>(Entities.Keys.RandomFieldValue, string.Concat($"UpdatedCategory", new Faker().Random.AlphaNumeric(5)));
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue<dynamic>(Entities.Keys.RandomFieldValue));
            this.SetValue(Entities.Keys.Field, elementLocator != null ? elementLocator.Value : string.Empty);
        }

        [When(@"I provide value (for "".*"" "".*"") filter that doesn't matches with any record")]
        public void WhenIProvideValueForFilterThatDoesnTMatchesWithAnyRecord(ElementLocator elementLocator)
        {
            this.SetValue<dynamic>(Entities.Keys.RandomFieldValue, string.Concat($"Automation", new Faker().Random.AlphaNumeric(5)));
            if (elementLocator != null && elementLocator.Value.Contains(ConstantValues.Date))
            {
                this.Get<ElementPage>().GetElement(elementLocator).SendKeys(DateTime.Now.AddDays(1).ToString("dd/MM/yyy", CultureInfo.InvariantCulture));
                this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
            }
            else
            {
                this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue<dynamic>(Keys.RandomFieldValue));
                this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
            }
        }

        [StepDefinition(@"I should see ""(.*)"" interface")]
        public void ThenIShouldSeeInterface(string title)
        {
            this.IShouldSeeInterface(title);
        }

        [Then(@"the ""(.*)"" should be saved and showed in the list")]
        [Then(@"the ""(.*)"" should be updated in the list")]
        public async Task ThenTheShouldBeSavedAndShowedInTheListAsync(string entity)
        {
            this.Get<ElementPage>().GetElement(entity.EqualsIgnoreCase(ConstantValues.Category) ? nameof(Resources.CategoryNameFilter) : nameof(Resources.ElementNameFilter)).Clear();
            this.Get<ElementPage>().GetElement(entity.EqualsIgnoreCase(ConstantValues.Category) ? nameof(Resources.CategoryNameFilter) : nameof(Resources.ElementNameFilter)).SendKeys(this.GetValue(Keys.RandomFieldValue));
            this.Get<ElementPage>().GetElement(entity.EqualsIgnoreCase(ConstantValues.Category) ? nameof(Resources.CategoryNameFilter) : nameof(Resources.ElementNameFilter)).SendKeys(OpenQA.Selenium.Keys.Enter);
            await Task.Delay(10000).ConfigureAwait(false);
            if (!this.GetValue(Entities.Keys.Field).ContainsIgnoreCase(ConstantValues.Description) || !this.GetValue(Entities.Keys.Title).ContainsIgnoreCase(ConstantValues.Edit))
            {
                Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), this.Get<ElementPage>().GetElement(nameof(Resources.NameFromGrid)).Text);
            }

            if (this.GetValue(Entities.Keys.Field).ContainsIgnoreCase(ConstantValues.Description) && this.GetValue(Entities.Keys.Title).ContainsIgnoreCase(ConstantValues.Edit))
            {
                this.Get<ElementPage>().GetElement(entity.EqualsIgnoreCase(ConstantValues.Category) ? nameof(Resources.CategoryNameFilter) : nameof(Resources.ElementNameFilter)).Clear();
                this.Get<ElementPage>().GetElement(entity.EqualsIgnoreCase(ConstantValues.Category) ? nameof(Resources.CategoryNameFilter) : nameof(Resources.ElementNameFilter)).SendKeys(this.GetValue(Keys.Result));
                this.Get<ElementPage>().GetElement(entity.EqualsIgnoreCase(ConstantValues.Category) ? nameof(Resources.CategoryNameFilter) : nameof(Resources.ElementNameFilter)).SendKeys(OpenQA.Selenium.Keys.Enter);
                await Task.Delay(5000).ConfigureAwait(false);
                Assert.IsEmpty(this.Get<ElementPage>().GetElement(nameof(Resources.DescriptionFromGrid)).Text);
            }

            if (entity.ToPascalCase().Contains(ConstantValues.CategoryElement) && !this.GetValue(Keys.Title).Contains(ConstantValues.Edit))
            {
                Assert.AreEqual(this.GetValue(Keys.SelectedValue), this.Get<ElementPage>().GetElement(nameof(Resources.CategoryFromGrid)).Text);
            }
        }

        [Then(@"the popup should be closed")]
        public void ThenThePopupShouldBeClosed()
        {
            Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.PopupClose)));
        }

        [Then(@"I should see the information that matches the data entered for the ""(.*)""")]
        public void ThenIShouldSeeTheInformationThatMatchesTheDataEnteredForThe(string field)
        {
            Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: this.GetValue(Keys.EntityType).EqualsIgnoreCase(ConstantValues.Category) ? UIContent.GridPosition[field] : UIContent.CategoryElementsGridPosition[field]).Text);
        }

        [When(@"I provide value (for "".*"" "".*"") filter")]
        public async Task WhenIProvideValueForFilterAsync(ElementLocator elementLocator)
        {
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(this.GetValue(Keys.EntityType).EqualsIgnoreCase(ConstantValues.Category) ? SqlQueries.GetLastCategoryWithDescription : SqlQueries.GetLastCategoryElementWithDescription).ConfigureAwait(false);
            if (elementLocator != null)
            {
                var field = elementLocator.Value.Split('_')[2].ToPascalCase();
                if (field.ToPascalCase().EqualsIgnoreCase(ConstantValues.Category))
                {
                    var name = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryByCategoryId, args: new { categoryId = lastCreatedRow[ConstantValues.CategoryId] }).ConfigureAwait(false);
                    this.SetValue(Keys.RandomFieldValue, name[ConstantValues.Name]);
                }
                else
                {
                    this.SetValue(Keys.RandomFieldValue, lastCreatedRow[field]);
                }

                if (field.EqualsIgnoreCase(ConstantValues.IsActive) || field.EqualsIgnoreCase(ConstantValues.IsGrouper))
                {
                    this.SetValue(Keys.RandomFieldValue, field.Contains(ConstantValues.IsActive) ? UIContent.Conversion[ConstantValues.Active] : UIContent.Conversion[ConstantValues.Yes]);
                    this.Get<ElementPage>().Click(elementLocator);
                    this.Get<ElementPage>().Click(nameof(Resources.ElementByText), formatArgs: field.Contains(ConstantValues.IsActive) ? UIContent.Conversion[ConstantValues.Active] : UIContent.Conversion[ConstantValues.Yes]);
                }
                else
                {
                    if (field.EqualsIgnoreCase(ConstantValues.CreatedDate))
                    {
#pragma warning disable CA1305 // Specify IFormatProvider
                        var date = string.Format("{0:dd-MMM-yy}", Convert.ToDateTime(lastCreatedRow[field]));
#pragma warning restore CA1305 // Specify IFormatProvider
                        date = date.Replace(date.Split('-')[1], UIContent.Conversion[date.Split('-')[1]]);
                        this.SetValue(Keys.RandomFieldValue, date);
                    }

                    this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue(Keys.RandomFieldValue));
                    this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
                    await Task.Delay(10000).ConfigureAwait(false);
                }
            }
        }

        [When(@"I select existing value from ""(.*)""")]
        public async Task WhenISelectExistingValueFromAsync(string field)
        {
            await Task.Delay(5000).ConfigureAwait(false);
            var lastCreatedCategoryElement = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastButOneCategoryElement).ConfigureAwait(false);
            var name = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryByCategoryId, args: new { categoryId = lastCreatedCategoryElement[ConstantValues.CategoryId] }).ConfigureAwait(false);
            this.Get<ElementPage>().Click(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.SelectBoxMenu), 5, formatArgs: UIContent.Conversion[field]);
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.ElementByText), formatArgs: name[ConstantValues.Name]);
            if (elements.Count > 1)
            {
                elements[elements.Count - 1].Click();
            }
            else
            {
                elements[0].Click();
            }
        }

        [When(@"I provide existing value (for "".*"" "".*"")")]
        [When(@"I update existing value (for "".*"" "".*"")")]
        public async Task WhenIProvideExistingValueForAsync(ElementLocator elementLocator)
        {
            IDictionary<string, string> lastCreatedRow = null;

            if (this.GetValue(Entities.Keys.EntityType).EqualsIgnoreCase(ConstantValues.Category))
            {
                lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastButOneCategory).ConfigureAwait(false);
            }
            else if (this.GetValue(Entities.Keys.EntityType).ContainsIgnoreCase(ConstantValues.Element))
            {
                lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastButOneCategoryElement).ConfigureAwait(false);
            }
            else if (this.GetValue(Entities.Keys.EntityType).EqualsIgnoreCase(ConstantValues.CreateNodes))
            {
                lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastNode).ConfigureAwait(false);
            }

            this.Get<ElementPage>().GetElement(elementLocator).Clear();
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(lastCreatedRow[ConstantValues.Name]);
        }

        [When(@"I click on the ""(.*)""")]
        public void WhenIClickOnThe(string field)
        {
            this.Get<ElementPage>().Click(field.EqualsIgnoreCase(ConstantValues.Category) ? nameof(Resources.CategoryColumn) : nameof(Resources.ElementByText), formatArgs: UIContent.Conversion[field]);
        }

        [Then(@"the results should be sorted according to ""(.*)""")]
        public async Task ThenTheResultsShouldBeSortedAccordingToAsync(string field)
        {
            IEnumerable<dynamic> categoryElementRecords = null;
            switch (field)
            {
                case ConstantValues.Name:
                    categoryElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetCategoryElementsByNameDesc).ConfigureAwait(false);
                    break;
                case ConstantValues.Description:
                    categoryElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetCategoryElementsByDescriptionDesc).ConfigureAwait(false);
                    break;
                case ConstantValues.CreatedDate:
                    categoryElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetCategoryElementsByCreatedDateDesc).ConfigureAwait(false);
                    break;
                case ConstantValues.Category:
                    categoryElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetCategoryElementsByCategoryDesc).ConfigureAwait(false);
                    break;
                case ConstantValues.State:
                    categoryElementRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetCategoryElementsByIsActiveDesc).ConfigureAwait(false);
                    break;
            }

            var categoryElementsList = categoryElementRecords.ToDictionaryList();
            foreach (var row in categoryElementsList)
            {
                await Task.Delay(5000).ConfigureAwait(false);
                Assert.AreEqual(row[ConstantValues.Name], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.CategoryElementsGridPosition[ConstantValues.Name]).Text);
                if (!string.IsNullOrEmpty(row[ConstantValues.Description].ToString()))
                {
                    Assert.AreEqual(row[ConstantValues.Description], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.CategoryElementsGridPosition[ConstantValues.Description]).Text);
                }
                else
                {
                    Assert.IsEmpty(this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.CategoryElementsGridPosition[ConstantValues.Description]).Text);
                }

#pragma warning disable CA1305 // Specify IFormatProvider
                var date = string.Format("{0:dd-MMM-yy}", Convert.ToDateTime(row[ConstantValues.CreatedDate]));
#pragma warning restore CA1305 // Specify IFormatProvider
                date = date.Replace(date.Split('-')[1], UIContent.Conversion[date.Split('-')[1]]);
                Assert.AreEqual(date, this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.CategoryElementsGridPosition[ConstantValues.CreatedDate]).Text);
                var name = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryByCategoryId, args: new { categoryId = row[ConstantValues.CategoryId] }).ConfigureAwait(false);
                Assert.AreEqual(name[ConstantValues.Name], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.CategoryElementsGridPosition[ConstantValues.Category]).Text);
                Assert.AreEqual(row[ConstantValues.IsActive].Equals(true) ? UIContent.Conversion[ConstantValues.Active] : UIContent.Conversion[ConstantValues.Inactive], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.CategoryElementsGridPosition[ConstantValues.IsActive]).Text);
            }
        }
    }
}
