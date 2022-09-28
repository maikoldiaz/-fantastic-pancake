// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateRelationshipMovementsTypesStep.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using OpenQA.Selenium.Interactions;
    using TechTalk.SpecFlow;

    [Binding]
    public class CreateRelationshipMovementsTypesStep : EcpWebStepDefinitionBase
    {
        [Then(@"""(.*)"" drop down should be loaded with their corresponding values")]
        public async Task ThenDropDownShouldBeLoadedWithTheirCorrespondingValuesAsync(string field)
        {
            switch (field)
            {
                case "Movements type":
                    var expectedMovementType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllMovementTypes).ConfigureAwait(false);
                    var expectedMovementTypeList = expectedMovementType.ToDictionaryList();
                    ////this.When("I click on \"source\" \"movement\" \"dropdown\"");
                    this.IClickOn("source\" \"movement", "dropdown");
                    var actualMovementTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);
                    Assert.AreEqual(expectedMovementTypeList.Count, actualMovementTypes.Count);

                    break;
                case "Cancellation type":
                    var expectedCancellationType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllMovementTypes).ConfigureAwait(false);
                    var expectedCancellationTypeList = expectedCancellationType.ToDictionaryList();
                    ////this.When("I click on \"annulation\" \"movement\" \"dropdown\"");
                    this.IClickOn("annulation\" \"movement", "dropdown");
                    var actualCancellationTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);
                    Assert.AreEqual(expectedCancellationTypeList.Count, actualCancellationTypes.Count);

                    break;
                case "Source":
                    var expectedSourceType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllOriginType).ConfigureAwait(false);
                    var expectedSourceTypeList = expectedSourceType.ToDictionaryList();
                    ////this.When("I click on \"source\" \"node\" \"dropdown\"");
                    this.IClickOn("source\" \"node", "dropdown");
                    var actualSourceTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);
                    Assert.AreEqual(expectedSourceTypeList.Count, actualSourceTypes.Count);

                    break;
                case "Destination":
                    var expectedDestinationType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllOriginType).ConfigureAwait(false);
                    var expectedDestinationTypeList = expectedDestinationType.ToDictionaryList();
                    ////this.When("I click on \"annulation\" \"node\" \"dropdown\"");
                    this.IClickOn("annulation\" \"node", "dropdown");
                    var actualDestinationTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);
                    Assert.AreEqual(expectedDestinationTypeList.Count, actualDestinationTypes.Count);

                    break;
                case "Source product":
                    var expectedSourceProductType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllOriginType).ConfigureAwait(false);
                    var expectedSourceProductTypeList = expectedSourceProductType.ToDictionaryList();
                    ////this.When("I click on \"source\" \"product\" \"dropdown\"");
                    this.IClickOn("source\" \"product", "dropdown");
                    var actualSourceProductTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);
                    Assert.AreEqual(expectedSourceProductTypeList.Count, actualSourceProductTypes.Count);

                    break;
                case "Destination product":
                    var expectedDestinationProductType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllOriginType).ConfigureAwait(false);
                    var expectedDestinationProductTypeList = expectedDestinationProductType.ToDictionaryList();
                    ////this.When("I click on \"annulation\" \"product\" \"dropdown\"");
                    this.IClickOn("annulation\" \"product", "dropdown");
                    var actualDestinationProductTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);
                    Assert.AreEqual(expectedDestinationProductTypeList.Count, actualDestinationProductTypes.Count);

                    break;
                default:
                    break;
            }
        }

        [Then(@"I should see the default value for ""(.*)"" dropdown is ""(.*)""")]
        public void ThenIShouldSeeTheDefaultValueForDropdownIs(string field, string expectedValue)
        {
            switch (field)
            {
                case "Movements type":
                    var actualText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_source_movement", field.ToCamelCase());
                    var actualValue = actualText.Split('\r')[0];
                    Assert.AreEqual(expectedValue, actualValue, $"{field} dropdown default value does not match");

                    break;

                case "Cancellation type":
                    var actualCancellationTypeText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_annulation_movement", field.ToCamelCase());
                    var actualCancellationTypeValue = actualCancellationTypeText.Split('\r')[0];
                    Assert.AreEqual(expectedValue, actualCancellationTypeValue, $"{field} dropdown default value does not match");

                    break;

                case "source":
                    var actualsourceText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_source_node", field.ToCamelCase());
                    var actualsourceValue = actualsourceText.Split('\r')[0];
                    Assert.AreEqual(expectedValue, actualsourceValue, $"{field} dropdown default value does not match");

                    break;

                case "Destination":
                    var actualDestinationText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_annulation_node", field.ToCamelCase());
                    var actualDestinationValue = actualDestinationText.Split('\r')[0];
                    Assert.AreEqual(expectedValue, actualDestinationValue, $"{field} dropdown default value does not match");

                    break;

                case "Source product":
                    var actualSourceProductText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_source_product", field.ToCamelCase());
                    var actualSourceProductValue = actualSourceProductText.Split('\r')[0];
                    Assert.AreEqual(expectedValue, actualSourceProductValue, $"{field} dropdown default value does not match");

                    break;

                case "Destination product":
                    var actualDestinationProductText = this.Get<ElementPage>().GetElementText(nameof(Resources.SelectedTitleForComboBox), "dd_annulation_product", field.ToCamelCase());
                    var actualDestinationProductValue = actualDestinationProductText.Split('\r')[0];
                    Assert.AreEqual(expectedValue, actualDestinationProductValue, $"{field} dropdown default value does not match");

                    break;

                case "Segment":
                    var actualSegmentText = this.Get<ElementPage>().GetElement(nameof(Resources.SegmentDefaultValue)).Text;
                    Assert.IsTrue(actualSegmentText.EqualsIgnoreCase(expectedValue));

                    break;

                default:
                    break;
            }
        }

        [Then(@"I should see the Active control ""(.*)"" by default")]
        public void ThenIShouldSeeTheActiveControlByDefault(string status)
        {
            var buttonStatus = this.Get<ElementPage>().GetElement(nameof(Resources.ActiveButton)).Enabled.ToString();
            Assert.AreEqual(status, buttonStatus);
        }

        [Then(@"I verify filter the content of the list ""(.*)""")]
        public async Task ThenIVerifyFilterTheContentOfTheListAsync(string field)
        {
            switch (field)
            {
                case "Movements type":
                    var expectedMovementType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.MovementTypes).ConfigureAwait(false);
                    var movementTypePage = this.Get<ElementPage>();
                    movementTypePage.GetElement(nameof(Resources.SourceMovementDropdown)).Click();
                    var movementTypeoption = movementTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedMovementType["Name"]);
                    Actions movementTypeAction = new Actions(this.DriverContext.Driver);
                    movementTypeAction.MoveToElement(movementTypeoption).Perform();
                    movementTypeoption.Click();

                    break;
                case "Cancellation type":
                    var expectedCancellationType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.CancellationType).ConfigureAwait(false);
                    var cancellationTypePage = this.Get<ElementPage>();
                    cancellationTypePage.GetElement(nameof(Resources.CancellationTypeDropdown)).Click();
                    var cancellationTypeOption = cancellationTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedCancellationType["Name"]);
                    Actions cancellationTypeaction = new Actions(this.DriverContext.Driver);
                    cancellationTypeaction.MoveToElement(cancellationTypeOption).Perform();
                    cancellationTypeOption.Click();

                    break;
                case "Source":
                    var expectedSourceType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFirstOriginType).ConfigureAwait(false);
                    var sourceTypePage = this.Get<ElementPage>();
                    sourceTypePage.GetElement(nameof(Resources.Source)).Click();
                    var sourceTypeOption = sourceTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedSourceType["Name"]);
                    Actions sourceTypeaction = new Actions(this.DriverContext.Driver);
                    sourceTypeaction.MoveToElement(sourceTypeOption).Perform();
                    sourceTypeOption.Click();

                    break;
                case "Destination":
                    var expectedDestination = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOriginType).ConfigureAwait(false);
                    var destinationPage = this.Get<ElementPage>();
                    destinationPage.GetElement(nameof(Resources.Destination)).Click();
                    var destinationOption = destinationPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedDestination["Name"]);
                    Actions destinationaction = new Actions(this.DriverContext.Driver);
                    destinationaction.MoveToElement(destinationOption).Perform();
                    destinationOption.Click();

                    break;
                case "Source product":
                    var expectedSourceProduct = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFirstOriginType).ConfigureAwait(false);
                    var sourceProductPage = this.Get<ElementPage>();
                    sourceProductPage.GetElement(nameof(Resources.SourceProduct)).Click();
                    var sourceProductOption = sourceProductPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedSourceProduct["Name"]);
                    Actions sourceProductaction = new Actions(this.DriverContext.Driver);
                    sourceProductaction.MoveToElement(sourceProductOption).Perform();
                    sourceProductOption.Click();

                    break;
                case "Destination product":
                    var expectedDestinationProduct = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOriginType).ConfigureAwait(false);
                    var destinationProductPage = this.Get<ElementPage>();
                    destinationProductPage.GetElement(nameof(Resources.DestinationProduct)).Click();
                    var destinationProductOption = destinationProductPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedDestinationProduct["Name"]);
                    Actions destinationProductaction = new Actions(this.DriverContext.Driver);
                    destinationProductaction.MoveToElement(destinationProductOption).Perform();
                    destinationProductOption.Click();

                    break;
                default:
                    break;
            }
        }

        [Then(@"I select existing Movement Type from Movements type dropdown then ""(.*)"" list should not contain the above selected movement type")]
        public async Task ThenISelectExistingMovementTypeFromMovementsTypeDropdownThenListShouldNotContainTheAboveSelectedMovementTypeAsync(string field)
        {
            var expectedMovementType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.MovementTypes).ConfigureAwait(false);
            var movementTypePage = this.Get<ElementPage>();
            movementTypePage.GetElement(nameof(Resources.SourceMovementDropdown)).Click();
            var movementTypeoption = movementTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedMovementType["Name"]);
            Actions movementTypeAction = new Actions(this.DriverContext.Driver);
            movementTypeAction.MoveToElement(movementTypeoption).Perform();
            movementTypeoption.Click();

            var expectedCancellationType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllMovementTypes).ConfigureAwait(false);
            var expectedCancellationTypeList = expectedCancellationType.ToDictionaryList();
            ////this.When("I click on \"annulation\" \"movement\" \"dropdown\"");
            this.IClickOn("annulation\" \"movement", "dropdown");
            var actualCancellationTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
            await Task.Delay(3000).ConfigureAwait(false);

            for (int i = 0; i < actualCancellationTypes.Count; i++)
            {
                Assert.IsFalse(expectedMovementType[ConstantValues.Name].EqualsIgnoreCase(actualCancellationTypes[i]));
            }
        }

        [Then(@"I select existing Cancellation Type from Cancellation type dropdown then ""(.*)"" list should not contain the above selected cancellation type")]
        public async Task ThenISelectExistingCancellationTypeFromCancellationTypeDropdownThenListShouldNotContainTheAboveSelectedCancellationTypeAsync(string field)
        {
            var expectedCancellationType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.CancellationType).ConfigureAwait(false);
            var cancellationTypePage = this.Get<ElementPage>();
            cancellationTypePage.GetElement(nameof(Resources.CancellationTypeDropdown)).Click();
            var cancellationTypeOption = cancellationTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedCancellationType["Name"]);
            Actions cancellationTypeaction = new Actions(this.DriverContext.Driver);
            cancellationTypeaction.MoveToElement(cancellationTypeOption).Perform();
            cancellationTypeOption.Click();

            var expectedMovementType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllMovementTypes).ConfigureAwait(false);
            var expectedMovementTypeList = expectedMovementType.ToDictionaryList();
            ////this.When("I click on \"source\" \"movement\" \"dropdown\"");
            this.IClickOn("source\" \"movement", "dropdown");
            var actualMovementTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
            await Task.Delay(3000).ConfigureAwait(false);

            for (int i = 0; i < actualMovementTypes.Count; i++)
            {
                Assert.IsFalse(expectedCancellationType[ConstantValues.Name].EqualsIgnoreCase(actualMovementTypes[i]));
            }
        }

        [Given(@"I create Movements and its Annulation Type")]
        public async Task GivenICreateMovementsAndItsAnnulationTypeAsync()
        {
            await this.ActiveMovementTypeAndItsAnnulationTypeAsync().ConfigureAwait(false);
        }

        [When(@"I select ""(.*)"" from ""(.*)"" list")]
        public async Task WhenISelectFromListAsync(string value, string field)
        {
            switch (field)
            {
                case "Movements type":
                    if (value.EqualsIgnoreCase("Movements type"))
                    {
                    var expectedMovementType = this.GetValue("MovementTypeName");
                    var movementTypePage = this.Get<ElementPage>();
                    movementTypePage.GetElement(nameof(Resources.SourceMovementDropdown)).Click();
                    var movementTypeoption = movementTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedMovementType);
                    Actions movementTypeAction = new Actions(this.DriverContext.Driver);
                    movementTypeAction.MoveToElement(movementTypeoption).Perform();
                    movementTypeoption.Click();
                    var temp = field;
                    }

                    if (value.EqualsIgnoreCase("existing movement type"))
                    {
                        var expectedExistingMovementType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSelectedMovementType).ConfigureAwait(false);
                        var existingMovementTypePage = this.Get<ElementPage>();
                        existingMovementTypePage.GetElement(nameof(Resources.SourceMovementDropdown)).Click();
                        var existingMovementTypeoption = existingMovementTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedExistingMovementType["Name"]);
                        Actions existingMovementTypeAction = new Actions(this.DriverContext.Driver);
                        existingMovementTypeAction.MoveToElement(existingMovementTypeoption).Perform();
                        existingMovementTypeoption.Click();
                    }

                    break;
                case "Cancellation type":

                    if (value.EqualsIgnoreCase("cancellation type"))
                    {
                        var expectedCancellationType = this.GetValue("AnnulationMovementName");
                        var cancellationTypePage = this.Get<ElementPage>();
                        cancellationTypePage.GetElement(nameof(Resources.CancellationTypeDropdown)).Click();
                        var cancellationTypeOption = cancellationTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedCancellationType);
                        Actions cancellationTypeaction = new Actions(this.DriverContext.Driver);
                        cancellationTypeaction.MoveToElement(cancellationTypeOption).Perform();
                        cancellationTypeOption.Click();
                    }

                    if (value.EqualsIgnoreCase("existing cancellation type"))
                    {
                        var expectedexistingCancellationType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSelectedCancellationType).ConfigureAwait(false);
                        var existingcancellationTypePage = this.Get<ElementPage>();
                        existingcancellationTypePage.GetElement(nameof(Resources.CancellationTypeDropdown)).Click();
                        var existingcancellationTypeOption = existingcancellationTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedexistingCancellationType["Name"]);
                        Actions existingcancellationTypeaction = new Actions(this.DriverContext.Driver);
                        existingcancellationTypeaction.MoveToElement(existingcancellationTypeOption).Perform();
                        existingcancellationTypeOption.Click();
                    }

                    break;
                case "Source":
                    if (value.EqualsIgnoreCase("source"))
                    {
                        var expectedSourceType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFirstOriginType).ConfigureAwait(false);
                        var sourceTypePage = this.Get<ElementPage>();
                        sourceTypePage.GetElement(nameof(Resources.Source)).Click();
                        var sourceTypeOption = sourceTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedSourceType["Name"]);
                        Actions sourceTypeaction = new Actions(this.DriverContext.Driver);
                        sourceTypeaction.MoveToElement(sourceTypeOption).Perform();
                        sourceTypeOption.Click();
                    }

                    if (value.EqualsIgnoreCase("destination"))
                    {
                        var expectedSourceType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSecondOriginType).ConfigureAwait(false);
                        var sourceTypePage = this.Get<ElementPage>();
                        sourceTypePage.GetElement(nameof(Resources.Source)).Click();
                        var sourceTypeOption = sourceTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedSourceType["Name"]);
                        Actions sourceTypeaction = new Actions(this.DriverContext.Driver);
                        sourceTypeaction.MoveToElement(sourceTypeOption).Perform();
                        sourceTypeOption.Click();
                    }

                    if (value.EqualsIgnoreCase("none"))
                    {
                        var expectedSourceValue = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOriginType).ConfigureAwait(false);
                        var sourceValuePage = this.Get<ElementPage>();
                        sourceValuePage.GetElement(nameof(Resources.Source)).Click();
                        var sourceValueOption = sourceValuePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedSourceValue["Name"]);
                        Actions sourceValueaction = new Actions(this.DriverContext.Driver);
                        sourceValueaction.MoveToElement(sourceValueOption).Perform();
                        sourceValueOption.Click();
                    }

                    break;
                case "Destination":
                    if (value.EqualsIgnoreCase("source"))
                    {
                        var expectedDestination = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFirstOriginType).ConfigureAwait(false);
                        var destinationPage = this.Get<ElementPage>();
                        destinationPage.GetElement(nameof(Resources.Destination)).Click();
                        var destinationOption = destinationPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedDestination["Name"]);
                        Actions destinationaction = new Actions(this.DriverContext.Driver);
                        destinationaction.MoveToElement(destinationOption).Perform();
                        destinationOption.Click();
                    }

                    if (value.EqualsIgnoreCase("destination"))
                    {
                        var expectedDestination = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSecondOriginType).ConfigureAwait(false);
                        var destinationPage = this.Get<ElementPage>();
                        destinationPage.GetElement(nameof(Resources.Destination)).Click();
                        var destinationOption = destinationPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedDestination["Name"]);
                        Actions destinationaction = new Actions(this.DriverContext.Driver);
                        destinationaction.MoveToElement(destinationOption).Perform();
                        destinationOption.Click();
                    }

                    if (value.EqualsIgnoreCase("none"))
                    {
                        var expectedDestination = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOriginType).ConfigureAwait(false);
                        var destinationPage = this.Get<ElementPage>();
                        destinationPage.GetElement(nameof(Resources.Destination)).Click();
                        var destinationOption = destinationPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedDestination["Name"]);
                        Actions destinationaction = new Actions(this.DriverContext.Driver);
                        destinationaction.MoveToElement(destinationOption).Perform();
                        destinationOption.Click();
                    }

                    break;
                case "Source product":
                    if (value.EqualsIgnoreCase("source"))
                    {
                        var expectedSourceProduct = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFirstOriginType).ConfigureAwait(false);
                        var sourceProductPage = this.Get<ElementPage>();
                        sourceProductPage.GetElement(nameof(Resources.SourceProduct)).Click();
                        var sourceProductOption = sourceProductPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedSourceProduct["Name"]);
                        Actions sourceProductaction = new Actions(this.DriverContext.Driver);
                        sourceProductaction.MoveToElement(sourceProductOption).Perform();
                        sourceProductOption.Click();
                    }

                    if (value.EqualsIgnoreCase("destination"))
                    {
                        var expectedSourceType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSecondOriginType).ConfigureAwait(false);
                        var sourceTypePage = this.Get<ElementPage>();
                        sourceTypePage.GetElement(nameof(Resources.SourceProduct)).Click();
                        var sourceTypeOption = sourceTypePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedSourceType["Name"]);
                        Actions sourceTypeaction = new Actions(this.DriverContext.Driver);
                        sourceTypeaction.MoveToElement(sourceTypeOption).Perform();
                        sourceTypeOption.Click();
                    }

                    if (value.EqualsIgnoreCase("none"))
                    {
                        var expectedSourceValue = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOriginType).ConfigureAwait(false);
                        var sourceValuePage = this.Get<ElementPage>();
                        sourceValuePage.GetElement(nameof(Resources.SourceProduct)).Click();
                        var sourceValueOption = sourceValuePage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedSourceValue["Name"]);
                        Actions sourceValueaction = new Actions(this.DriverContext.Driver);
                        sourceValueaction.MoveToElement(sourceValueOption).Perform();
                        sourceValueOption.Click();
                    }

                    break;
                case "Destination product":
                    if (value.EqualsIgnoreCase("source"))
                    {
                        var expectedDestination = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFirstOriginType).ConfigureAwait(false);
                        var destinationPage = this.Get<ElementPage>();
                        destinationPage.GetElement(nameof(Resources.DestinationProduct)).Click();
                        var destinationOption = destinationPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedDestination["Name"]);
                        Actions destinationaction = new Actions(this.DriverContext.Driver);
                        destinationaction.MoveToElement(destinationOption).Perform();
                        destinationOption.Click();
                    }

                    if (value.EqualsIgnoreCase("destination"))
                    {
                        var expectedDestination = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSecondOriginType).ConfigureAwait(false);
                        var destinationPage = this.Get<ElementPage>();
                        destinationPage.GetElement(nameof(Resources.DestinationProduct)).Click();
                        var destinationOption = destinationPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedDestination["Name"]);
                        Actions destinationaction = new Actions(this.DriverContext.Driver);
                        destinationaction.MoveToElement(destinationOption).Perform();
                        destinationOption.Click();
                    }

                    if (value.EqualsIgnoreCase("none"))
                    {
                        var expectedDestinationProduct = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOriginType).ConfigureAwait(false);
                        var destinationProductPage = this.Get<ElementPage>();
                        destinationProductPage.GetElement(nameof(Resources.DestinationProduct)).Click();
                        var destinationProductOption = destinationProductPage.GetElement(nameof(Resources.SelectBoxOption), formatArgs: expectedDestinationProduct["Name"]);
                        Actions destinationProductaction = new Actions(this.DriverContext.Driver);
                        destinationProductaction.MoveToElement(destinationProductOption).Perform();
                        destinationProductOption.Click();
                    }

                    break;

                default:

                    break;
            }
        }

        [Then(@"I should see message as ""(.*)"" for ""(.*)"" assigned")]
        public async Task ThenIShouldSeeMessageAsForAssignedAsync(string message, string field)
        {
            switch (field)
            {
                case "Movements type":
                    var expectedMovementType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementTypeofSelectedCancellationType).ConfigureAwait(false);
                    this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.AnnulationMessage));
                    var actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.AnnulationMessage)).Text;
                    var expectedMessage = message + " " + expectedMovementType["Name"] + ".";
                    Assert.AreEqual(expectedMessage, actualMessage);

                    break;
                case "Cancellation type":
                    var expectedCancellationType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetCancellationTypeofSelectedMovementType).ConfigureAwait(false);
                    this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.AnnulationMessage));
                    var actualMessage1 = this.Get<ElementPage>().GetElement(nameof(Resources.AnnulationMessage)).Text;
                    var expectedMessage1 = message + " " + expectedCancellationType["Name"] + ".";
                    Assert.AreEqual(expectedMessage1, actualMessage1);

                    break;
                default:

                    break;
            }
        }

        [Then(@"""(.*)"" list should not contain ""(.*)"" option")]
        public async Task ThenListShouldNotContainOptionAsync(string field, string value)
        {
            switch (field)
            {
                case "Destination":
                    var selectedSourceValue = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOriginType).ConfigureAwait(false);

                    ////this.When("I click on \"annulation\" \"node\" \"dropdown\"");
                    this.IClickOn("annulation\" \"product", "dropdown");
                    var actualDestinationTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    var temp = value;
                    await Task.Delay(3000).ConfigureAwait(false);

                    for (int i = 0; i < actualDestinationTypes.Count; i++)
                    {
                        Assert.IsFalse(selectedSourceValue[ConstantValues.Name].EqualsIgnoreCase(actualDestinationTypes[i]));
                    }

                    break;
                case "Source":
                    var selectedDestinationValue = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOriginType).ConfigureAwait(false);

                    ////this.When("I click on \"source\" \"node\" \"dropdown\"");
                    this.IClickOn("source\" \"product", "dropdown");
                    var actualSourceValues = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);

                    for (int i = 0; i < actualSourceValues.Count; i++)
                    {
                        Assert.IsFalse(selectedDestinationValue[ConstantValues.Name].EqualsIgnoreCase(actualSourceValues[i]));
                    }

                    break;
                case "Destination product":
                    var selectedSourceProductValue = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOriginType).ConfigureAwait(false);

                    ////this.When("I click on \"annulation\" \"product\" \"dropdown\"");
                    this.IClickOn("annulation\" \"product", "dropdown");
                    var actualDistinationProductTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);

                    for (int i = 0; i < actualDistinationProductTypes.Count; i++)
                    {
                        Assert.IsFalse(selectedSourceProductValue[ConstantValues.Name].EqualsIgnoreCase(actualDistinationProductTypes[i]));
                    }

                    break;
                case "Source product":
                    var selectedDestinationProductValue = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastOriginType).ConfigureAwait(false);

                    ////this.When("I click on \"source\" \"product\" \"dropdown\"");
                    this.IClickOn("source\" \"product", "dropdown");
                    var actualSourceProductTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);

                    for (int i = 0; i < actualSourceProductTypes.Count; i++)
                    {
                        Assert.IsFalse(selectedDestinationProductValue[ConstantValues.Name].EqualsIgnoreCase(actualSourceProductTypes[i]));
                    }

                    break;
                default:

                    break;
            }
        }

        [Then(@"""(.*)"" list should contain all options")]
        public async Task ThenListShouldContainOptionAsync(string field)
        {
            switch (field)
            {
                case "Destination":
                    var expectedDestinationType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllOriginType).ConfigureAwait(false);
                    var expectedDestinationTypeList = expectedDestinationType.ToDictionaryList();
                    ////this.When("I click on \"annulation\" \"node\" \"dropdown\"");
                    this.IClickOn("annulation\" \"product", "dropdown");
                    var actualDestinationTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);

                    for (int i = 0; i < actualDestinationTypes.Count; i++)
                    {
                        Assert.IsTrue(expectedDestinationTypeList[i][ConstantValues.Name].Equals(actualDestinationTypes[i]));
                    }

                    break;
                case "Source":
                    var expectedSourceType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllOriginType).ConfigureAwait(false);
                    var expectedSourceTypeList = expectedSourceType.ToDictionaryList();
                    ////this.When("I click on \"source\" \"node\" \"dropdown\"");
                    this.IClickOn("source\" \"product", "dropdown");
                    var actualSourceTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);

                    for (int i = 0; i < actualSourceTypes.Count; i++)
                    {
                        Assert.IsTrue(expectedSourceTypeList[i][ConstantValues.Name].Equals(actualSourceTypes[i]));
                    }

                    break;
                case "Destination product":
                    var expectedDestinationProductType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllOriginType).ConfigureAwait(false);
                    var expectedDestinationProductTypeList = expectedDestinationProductType.ToDictionaryList();
                    ////this.When("I click on \"annulation\" \"product\" \"dropdown\"");
                    this.IClickOn("annulation\" \"product", "dropdown");
                    var actualDestinationProductTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);

                    for (int i = 0; i < actualDestinationProductTypes.Count; i++)
                    {
                        Assert.IsTrue(expectedDestinationProductTypeList[i][ConstantValues.Name].Equals(actualDestinationProductTypes[i]));
                    }

                    break;
                case "Source product":
                    var expectedSourceProductType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllOriginType).ConfigureAwait(false);
                    var expectedSourceProductTypeList = expectedSourceProductType.ToDictionaryList();
                    ////this.When("I click on \"source\" \"product\" \"dropdown\"");
                    this.IClickOn("source\" \"product", "dropdown");
                    var actualSourceProductTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);

                    for (int i = 0; i < actualSourceProductTypes.Count; i++)
                    {
                        Assert.IsTrue(expectedSourceProductTypeList[i][ConstantValues.Name].Equals(actualSourceProductTypes[i]));
                    }

                    break;
                default:

                    break;
            }

            await Task.Delay(3000).ConfigureAwait(false);
        }
    }
}