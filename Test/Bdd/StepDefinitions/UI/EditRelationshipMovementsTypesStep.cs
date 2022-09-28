// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditRelationshipMovementsTypesStep.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using OpenQA.Selenium.Interactions;
    using TechTalk.SpecFlow;

    [Binding]
    public class EditRelationshipMovementsTypesStep : EcpWebStepDefinitionBase
    {
        [When(@"I search for existing Movement type record using filter option")]
        public async Task WhenISearchForExistingMovementTypeRecordUsingFilterOptionAsync()
        {
            var name = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSelectedMovementType).ConfigureAwait(false);
            this.SetValue(Keys.RandomFieldValue, name[ConstantValues.Name]);
            this.Get<ElementPage>().GetElement(nameof(Resources.GetSourceCategoryElement)).SendKeys(this.GetValue(Keys.RandomFieldValue));
            this.Get<ElementPage>().GetElement(nameof(Resources.GetSourceCategoryElement)).SendKeys(OpenQA.Selenium.Keys.Enter);
            await Task.Delay(3000).ConfigureAwait(false);
        }

        [Then(@"Each list must show selected the element corresponding to the relationship that is being edited")]
        public async Task ThenEachListMustShowSelectedTheElementCorrespondingToTheRelationshipThatIsBeingEditedAsync()
        {
            var selectedRecord = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetRecordByMovementName).ConfigureAwait(false);

            var actualMovementTypeText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_source_movement", "Movements type");
            var actualValue = actualMovementTypeText.Split('\r')[0];
            Assert.AreEqual(selectedRecord["MovementType"], actualValue);

            var actualCancellationTypeText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_annulation_movement", "Cancellation Type");
            var actualCancellationTypeValue = actualCancellationTypeText.Split('\r')[0];
            Assert.AreEqual(selectedRecord["CancellationType"], actualCancellationTypeValue);

            var actualsourceText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_source_node", "Source");
            var actualsourceValue = actualsourceText.Split('\r')[0];
            Assert.AreEqual(selectedRecord["Source"], actualsourceValue);

            var actualDestinationText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_annulation_node", "Destination");
            var actualDestinationValue = actualDestinationText.Split('\r')[0];
            Assert.AreEqual(selectedRecord["Destination"], actualDestinationValue);

            var actualSourceProductText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_source_product", "Source product");
            var actualSourceProductValue = actualSourceProductText.Split('\r')[0];
            Assert.AreEqual(selectedRecord["SourceProduct"], actualSourceProductValue);

            var actualDestinationProductText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_annulation_product", "Destination product");
            var actualDestinationProductValue = actualDestinationProductText.Split('\r')[0];
            Assert.AreEqual(selectedRecord["DestinationProduct"], actualDestinationProductValue);
        }

        [Then(@"Active control should show the corresponding status to the record being edited")]
        public async Task ThenActiveControlShouldShowTheCorrespondingStatusToTheRecordBeingEditedAsync()
        {
            var selectedRecord = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetRecordByMovementName).ConfigureAwait(false);

            var buttonStatus = this.Get<ElementPage>().GetElement(nameof(Resources.ActiveButton)).Enabled.ToString();
            Assert.AreEqual(selectedRecord["IsActive"], buttonStatus);
        }

        [Then(@"""(.*)"" drop down should be loaded with active movement types and not selected item in the movement type list")]
        public async Task ThenDropDownShouldBeLoadedWithActiveMovementTypesAndNotSelectedItemInTheMovementTypeListAsync(string field)
        {
            var expectedCancellationType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllActiveMovementTypes).ConfigureAwait(false);
            var expectedCancellationTypeList = expectedCancellationType.ToDictionaryList();
            this.When("I click on \"annulation\" \"movement\" \"dropdown\"");
            var actualCancellationTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
            await Task.Delay(3000).ConfigureAwait(false);
            Assert.AreEqual(expectedCancellationTypeList.Count, actualCancellationTypes.Count);
        }

        [When(@"I remove value from ""(.*)"" dropdown")]
        public void WhenIRemoveValueFrom(string field)
        {
            switch (field)
            {
                case "Cancellation type":
                    this.Get<ElementPage>().Click(nameof(Resources.ClearDropdown), "dd_annulation_movement");

                    break;
                case "Source":
                    this.Get<ElementPage>().Click(nameof(Resources.ClearDropdown), "dd_source_node");

                    break;
                case "Destination":
                    this.Get<ElementPage>().Click(nameof(Resources.ClearDropdown), "dd_annulation_node");

                    break;
                case "Source product":
                    this.Get<ElementPage>().Click(nameof(Resources.ClearDropdown), "dd_source_product");

                    break;
                case "Destination product":
                    this.Get<ElementPage>().Click(nameof(Resources.ClearDropdown), "dd_annulation_product");

                    break;
                default:
                    break;
            }
        }

        [Then(@"I remove values from below fields to check all options present in the dropdown")]
        public void ThenIRemoveValuesFromBelowFieldsToCheckAllOptionsPresentInTheDropdown()
        {
            this.Get<ElementPage>().Click(nameof(Resources.ClearDropdown), "dd_source_node");
            this.Get<ElementPage>().Click(nameof(Resources.ClearDropdown), "dd_annulation_node");
            this.Get<ElementPage>().Click(nameof(Resources.ClearDropdown), "dd_source_product");
            this.Get<ElementPage>().Click(nameof(Resources.ClearDropdown), "dd_annulation_product");
        }

        [When(@"I select valid cancellation type from Cancellation type list")]
        public async Task WhenISelectValidCancellationTypeFromCancellationTypeListAsync()
        {
            var expectedCancellationType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNotSelectedCancellationType).ConfigureAwait(false);
            var cancellationTypePage = this.Get<ElementPage>();
            cancellationTypePage.GetElement(nameof(Resources.CancellationTypeDropdown)).Click();
            var cancellationTypeOption = cancellationTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedCancellationType["Name"]);
            Actions cancellationTypeaction = new Actions(this.DriverContext.Driver);
            cancellationTypeaction.MoveToElement(cancellationTypeOption).Perform();
            cancellationTypeOption.Click();
        }

        [Then(@"I should see the updated data of the relationship in the audit log")]
        public async Task ThenIShouldSeeTheUpdatedDataOfTheRelationshipInTheAuditLogAsync()
        {
            string expectedAuditLogValue = "AnnulationMovementTypeId";
            var actualUpdatedRecord = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLatestUpdatedAnnulationAuditLogField).ConfigureAwait(false);

            Assert.AreEqual(expectedAuditLogValue, actualUpdatedRecord["field"]);
        }

        [When(@"the cancellation type has not been assigned to another movement type")]
        public async Task WhenTheCancellationTypeHasNotBeenAssignedToAnotherMovementTypeAsync()
        {
            await this.WhenISelectValidCancellationTypeFromCancellationTypeListAsync().ConfigureAwait(false);
        }
    }
}
