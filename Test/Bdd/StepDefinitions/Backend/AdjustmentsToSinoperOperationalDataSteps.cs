// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdjustmentsToSinoperOperationalDataSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Backend
{
    using System;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class AdjustmentsToSinoperOperationalDataSteps : EcpApiStepDefinitionBase
    {
        public AdjustmentsToSinoperOperationalDataSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Then(@"product should be stored in the destination product")]
        public async Task ThenProductShouldBeStoredInTheDestinationProductAsync()
        {
            var destinationProduct = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDestinationProductNameOfMovement, args: new { movementId = this.ScenarioContext["MOVEMENTID"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(ConstantValues.CRUDOCAMPOMAMBO, destinationProduct[ConstantValues.Name]);
        }

        [Then(@"product type should be stored in the destination producttype")]
        public async Task ThenProductTypeShouldBeStoredInTheDestinationProducttypeAsync()
        {
            var destinationProductType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDestinationProductTypeIdOfMovement, args: new { movementId = this.ScenarioContext["MOVEMENTID"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(this.ScenarioContext[ConstantValues.NameOfDestinationProductTypeId], destinationProductType[ConstantValues.NameOfDestinationProductTypeId]);
        }

        [Given(@"I want to register Movements in the system without destination product")]
        public async Task GivenIWantToRegisterMovementsInTheSystemWithoutDestinationProductAsync()
        {
            this.SetValue(ConstantValues.DestinationProductIsNotSet, "Yes");
            ////this.Given("I want to register an \"Movements\" in the system");
            await this.IWantToRegisterAnInTheSystemAsync("Movements").ConfigureAwait(false);
        }

        [Then(@"source product should be stored in the destination product")]
        public async Task ThenSourceProductShouldBeStoredInTheDestinationProductAsync()
        {
            var destinationProduct = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDestinationProductNameOfMovement, args: new { movementId = this.ScenarioContext["MOVEMENTID"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(ConstantValues.CRUDOSIMPORTADOS, destinationProduct[ConstantValues.Name]);
        }

        [Then(@"source product type should be stored in the destination producttype")]
        public async Task ThenSourceProductTypeShouldBeStoredInTheDestinationProducttypeAsync()
        {
            var destinationProductType = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDestinationProductTypeIdOfMovement, args: new { movementId = this.ScenarioContext["MOVEMENTID"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(this.ScenarioContext[ConstantValues.NameOfSourceProductTypeId], destinationProductType[ConstantValues.NameOfDestinationProductTypeId]);
        }

        [Given(@"I want to register an ""(.*)"" in the system with unique batch identifier")]
        public async Task GivenIWantToRegisterAnInTheSystemWithUniqueBatchIdentifierAsync(string entity)
        {
            this.SetValue(ConstantValues.InventoryWithMultipleProducts, "Yes");
            ////this.Given("I want to register an \"" + entity + "\" in the system");
            await this.IWantToRegisterAnInTheSystemAsync(entity).ConfigureAwait(false);
        }

        [Given(@"I want to adjust an ""(.*)"" in the system with unique batch identifier")]
        [Given(@"I want to cancel an ""(.*)"" in the system with unique batch identifier")]
        public async Task GivenIWantToCancelAnInTheSystemWithUniqueBatchIdentifierAsync(string entity)
        {
            this.SetValue(ConstantValues.InventoryWithMultipleProducts, "Yes");
            ////this.Given("I want to cancel an \"" + entity + "\"");
            await this.IWantToCancelOrAdjustAnAsync(entity).ConfigureAwait(false);
        }

        [Given(@"I want to register an ""(.*)"" in the system with same batch identifier")]
        public async Task GivenIWantToRegisterAnInTheSystemWithSameBatchIdentifierAsync(string entity)
        {
            this.SetValue(ConstantValues.InventoryWithSameBatchIdentifier, "Yes");
            await this.GivenIWantToRegisterAnInTheSystemWithUniqueBatchIdentifierAsync(entity).ConfigureAwait(false);
        }

        [Given(@"I want to cancel an ""(.*)"" in the system with same batch identifier")]
        public async Task GivenIWantToCancelAnInTheSystemWithSameBatchIdentifierAsync(string entity)
        {
            this.SetValue(ConstantValues.InventoryWithSameBatchIdentifier, "Yes");
            await this.GivenIWantToCancelAnInTheSystemWithUniqueBatchIdentifierAsync(entity).ConfigureAwait(false);
        }

        [StepDefinition(@"I provide batch identifier that exceeds (.*) characters for inventory")]
        public void WhenIProvideBatchIdentifierThatExceedsCharactersForInventory(int characters)
        {
            this.SetValue(ConstantValues.NumberOfBatchIdentifierCharacters, characters.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        [Given(@"I want to adjust an ""(.*)"" with same batch identifier")]
        public async Task GivenIWantToAdjustAnWithSameBatchIdentifierAsync(string entity)
        {
            this.SetValue(ConstantValues.InventoryWithMultipleProducts, "Yes");
            this.SetValue(ConstantValues.InventoryWithSameBatchIdentifier, "Yes");
            ////this.Given("I want to adjust an \"" + entity + "\"");
            await this.IWantToCancelOrAdjustAnAsync(entity).ConfigureAwait(false);
        }

        [Then(@"first inventory product should be registered in the system")]
        public async Task ThenFirstInventoryProductShouldBeRegisteredInTheSystemAsync()
        {
            Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetInventoryProductByNodeIdAndBatchId, args: new { nodeId = this.GetValue(ConstantValues.SourceNode), eventType = this.ScenarioContext[ConstantValues.EventType].ToString(), batchId = this.GetValue(ConstantValues.BATCHID + "_1") }).ConfigureAwait(false));
        }

        [Then(@"record multiple inventories with positive product volume")]
        [Then(@"multiple inventories should be registered")]
        public async Task ThenMultipleInventoriesShouldBeRegisteredAsync()
        {
            if (this.ScenarioContext[ConstantValues.EventType].ToString() == "Update")
            {
                Assert.AreEqual(4, await this.ReadSqlScalarAsync<int>(SqlQueries.GetInventoryProductByNodeId, args: new { nodeId = this.GetValue(ConstantValues.SourceNode), eventType = this.ScenarioContext[ConstantValues.EventType].ToString() }).ConfigureAwait(false));
            }
            else
            {
                Assert.AreEqual(2, await this.ReadSqlScalarAsync<int>(SqlQueries.GetInventoryProductByNodeId, args: new { nodeId = this.GetValue(ConstantValues.SourceNode), eventType = this.ScenarioContext[ConstantValues.EventType].ToString() }).ConfigureAwait(false));
            }
        }

        [When(@"the system is executing the operational cutoff")]
        public async Task WhenTheSystemIsExecutingTheOperationalCutoffAsync()
        {
            this.SetValue(ConstantValues.CategorySegment, "Transporte");
            this.ScenarioContext[ConstantValues.WordSegmentName] = this.GetValue(ConstantValues.CategorySegment);
            ////this.Given("I am logged in as \"profesional\"");
            this.LoggedInAsUser("profesional");
            ////this.Given("I have ownership strategy for node");
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeWithOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
            ////this.Given("I have ownership strategy for node products");
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeProductsWithOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
            ////this.Given("I have ownership strategy for node connections");
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeConnectionsWithOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
            ////this.When("I navigate to \"Operational Cutoff\" page");
            this.UiNavigation("Operational Cutoff");
            ////this.When("I click on \"NewCut\" \"button\"");
            this.IClickOn("NewCut", "button");
            ////this.When("I choose CategoryElement from \"InitTicket\" \"Segment\" \"combobox\"");
            this.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
            ////this.When("I select the FinalDate lessthan \"2\" days from CurrentDate on \"Cutoff\" DatePicker");
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(2, "Cutoff").ConfigureAwait(false);
            ////this.When("I click on \"InitTicket\" \"next\" \"button\"");
            this.IClickOn("InitTicket\" \"Submit", "button");
            ////this.Then("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("validateInitialInventory\" \"submit", "button");
            ////this.When("I click on \"checkInventories\" \"Next\" \"button\"");
            this.IClickOn("validateInitialInventory\" \"submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all pending records from grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            ////this.When("I click on \"ErrorsGrid\" \"AddNote\" \"button\"");
            this.IClickOn("ErrorsGrid\" \"AddNote", "button");
            ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
            this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
            ////this.When("I click on \"AddComment\" \"Submit\" \"button\"");
            this.IClickOn("AddComment\" \"Submit", "button");
            ////this.Then("validate that \"ErrorsGrid\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("ErrorsGrid\" \"Submit", "button");
            ////this.When("I click on \"ErrorsGrid\" \"Next\" \"button\"");
            this.IClickOn("ErrorsGrid\" \"Submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all unbalances in the grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            ////this.When("I click on \"consistencyCheck\" \"AddNote\" \"button\"");
            this.IClickOn("consistencyCheck\" \"AddNote", "button");
            ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
            this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
            ////this.When("I click on \"AddComment\" \"submit\" \"button\"");
            this.IClickOn("AddComment\" \"submit", "button");
            ////this.When("I click on \"ConsistencyCheck\" \"Next\" \"button\"");
            this.IClickOn("unbalancesGrid\" \"submit", "button");
            ////this.When("I click on \"Confirm\" \"Cutoff\" \"Submit\" \"button\"");
            this.IClickOn("ConfirmCutoff\" \"Submit", "button");
            ////this.When("I wait till cutoff ticket processing to complete");
            await this.WaitForTicketToCompleteProcessingAsync().ConfigureAwait(false);
        }

        [When(@"the system is executing the ownership calculation")]
        public async Task WhenTheSystemIsExecutingTheOwnershipCalculationAsync()
        {
            await this.WhenTheSystemIsExecutingTheOperationalCutoffAsync().ConfigureAwait(false);
            ////this.When("I navigate to \"ownershipcalculation\" page");
            this.UiNavigation("ownershipcalculation");
            ////this.When("I click on \"NewBalance\" \"button\"");
            this.IClickOn("NewBalance", "button");
            ////this.When("I select segment from \"OwnershipCal\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("OwnershipCal\" \"Segment", "dropdown");
            ////this.When("I select the FinalDate lessthan \"2\" days from CurrentDate on \"Ownership\" DatePicker");
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(2, "Ownership").ConfigureAwait(false);
            ////this.When("I click on \"ownershipCalCriteria\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
            ////this.Then("I see \"Todos los nodos tienen configurada una estrategia de propiedad.\" message for \"Nodos con estrategia de propiedad\"");
            this.ISeeMessageFor("Todos los nodos tienen configurada una estrategia de propiedad.", "Nodos con estrategia de propiedad");
            ////this.Then("I verify all \"9\" validations passed");
            this.IVerifyAllValidationsPassed(9);
            ////this.When("I click on \"ownershipCalValidations\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalulationValidations\" \"submit", "button");
            ////this.When("I click on \"OwnershipCalConfirmation\" \"Execute\" \"button\"");
            this.IClickOn("ownershipCalculationConfirmation\" \"submit", "button");
            ////this.Then("verify the ownership is calculated successfully");
            await this.VerifyTheOwnershipIsCalculatedSuccessfullyAsync().ConfigureAwait(false);
            ////this.When("I wait till ownership ticket geneation to complete");
            await this.WaitForOwnershipTicketProcessingToCompleteAsync().ConfigureAwait(false);
        }

        [Given(@"it not met batch identifier validation")]
        [When(@"there are inventories with the batch identifier on the period day of the ownership calculation")]
        [When(@"there are inventories with the batch identifier on the period day of the operational cutoff")]
        public void WhenThereAreInventoriesWithTheBatchIdentifierOnThePeriodDayOfTheOperationalCutoff()
        {
            // Method intentionally left empty.
        }

        [Then(@"created inventories should be included in final inventory calculation")]
        public async Task ThenCreatedInventoriesShouldBeIncludedInFinalInventoryCalculationAsync()
        {
            Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfFinalInventory, args: new { nodeId = this.GetValue(ConstantValues.SourceNode), ticketId = this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false));
        }

        [When(@"there are inventories with the batch identifier on the day before the period day of the operational cutoff")]
        public async Task WhenThereAreInventoriesWithTheBatchIdentifierOnTheDayBeforeThePeriodDayOfTheOperationalCutoffAsync()
        {
            try
            {
                var lastTicket = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTicketStatusOfSegment, args: new { segmentId = 10 }).ConfigureAwait(false);
                this.SetValue(ConstantValues.StartDate, lastTicket["Formatted" + ConstantValues.StartDate]);
                if (lastTicket["Status"] == "0")
                {
                    await this.ReadAllSqlAsync(SqlQueries.UpdateTicketStatusToFail, args: new { ticketId = lastTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                }

                await this.ReadAllSqlAsync(SqlQueries.UpdateInventoryDateOfInventoryProduct, args: new { previousDayOfTicketStartDate = lastTicket[ConstantValues.PreviousDayOfTicketStartDate], fileRegistrationTransactionId = this.GetValue(ApiContent.Ids[this.GetValue(Keys.EntityType)]) }).ConfigureAwait(false);
            }
            catch (ArgumentNullException e)
            {
                Logger.Info("There is no Ticket with Trasnporte Segment" + e.InnerException);
                await this.ReadAllSqlAsync(SqlQueries.UpdateInventoryDateOfInventoryProduct, args: new { fileRegistrationTransactionId = this.GetValue(ApiContent.Ids[this.GetValue(Keys.EntityType)]) }).ConfigureAwait(false);
            }
        }

        [Then(@"created inventories should be included in initial inventory calculation")]
        public async Task ThenCreatedInventoriesShouldBeIncludedInInitialInventoryCalculationAsync()
        {
            Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfInitialInventory, args: new { nodeId = this.GetValue(ConstantValues.SourceNode), ticketId = this.GetValue(ConstantValues.TicketId), calculationDate = this.GetValue(ConstantValues.StartDate) }).ConfigureAwait(false));
        }
    }
}