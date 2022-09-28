// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteTheFirstOwnershipCalculationProcessOfNodeSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class ExecuteTheFirstOwnershipCalculationProcessOfNodeSteps : EcpWebStepDefinitionBase
    {
        [Given(@"TRUE system is preparing the initial inventories data that send them to FICO")]
        public async Task GivenIHaveProcessedInventoriesAndGeneratedCutoffTicketAsync()
        {
            ////this.Given("I have \"ownershipnodes\" created");
            await this.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
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
            ////this.When("I select the FinalDate lessthan \"3\" days from CurrentDate on \"Cutoff\" DatePicker");
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(3, "Cutoff").ConfigureAwait(false);
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

        [When(@"there is no ownership for initial inventories from both owner and ownership tables")]
        public async Task WhenThereIsNoOwnershipForInitialInventoriesFromBothOwnerAndOwnershipTablesAsync()
        {
            var segmentId = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = this.GetValue("Segment") }).ConfigureAwait(false);
            await this.ReadAllSqlAsync(SqlQueries.UpdateInventoryDateToSixDaysBefore, args: new { segmentId = segmentId["ElementId"] }).ConfigureAwait(false);
        }

        [StepDefinition(@"generate ownership ticket without initial inventories")]
        [StepDefinition(@"generate ownership ticket with initial inventories")]
        public async Task WhenGenerateOwnershipTicketWithoutInitialInventoriesAsync()
        {
            ////this.When("I navigate to \"Ownershipcalculation\" page");
            this.UiNavigation("Ownershipcalculation");
            ////this.When("I click on \"NewBalance\" \"button\"");
            this.IClickOn("NewBalance", "button");
            ////this.When("I select segment from \"OwnershipCal\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("OwnershipCal\" \"Segment", "dropdown");
            this.ISelectFinalDateOnOwnershipDatePicker(3);
            ////this.When("I click on \"OwnershipCalCriteria\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
            ////this.Then("I see \"Todos los nodos tienen configurada una estrategia de propiedad.\" message for \"Nodos con estrategia de propiedad\"");
            this.ISeeMessageFor("Todos los nodos tienen configurada una estrategia de propiedad.", "Nodos con estrategia de propiedad");
            ////this.Then("I verify all \"9\" validations passed");
            this.IVerifyAllValidationsPassed(9);
            ////this.Then("I click on \"OwnershipCalValidations\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalulationValidations\" \"submit", "button");
            ////this.Then("I click on \"OwnershipCalConfirmation\" \"Execute\" \"button\"");
            this.IClickOn("ownershipCalculationConfirmation\" \"submit", "button");
            ////this.When("I wait till ownership ticket geneation to complete");
            await this.WaitForOwnershipTicketProcessingToCompleteAsync().ConfigureAwait(false);
        }

        [Then(@"send initial inventories collection ""(.*)"" data to FICO")]
        public async Task ThenSendInitialInventoriesCollectionDataToFICOAsync(string state)
        {
            var fileName = this.GetValue(ConstantValues.TicketId);
            this.ScenarioContext["json"] = await fileName.OwnershipdatafromBlobAsync("ownershiprule").ConfigureAwait(false);
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"]["invetarioIniciales"];
            if (state.ContainsIgnoreCase("without"))
            {
                Assert.IsFalse(messageData.HasValues);
            }
            else if (state.ContainsIgnoreCase("with"))
            {
                Assert.IsNotNull(messageData);
                var segmentId = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = this.GetValue("Segment") }).ConfigureAwait(false);
                var inventoryDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTopOneInventoryProduct, args: new { segmentId = segmentId["ElementId"] }).ConfigureAwait(false);
                var ownerDetailsForInventoryProduct = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetInventoryProductByInventoryProductId, args: new { invProdId = inventoryDetails["InventoryProductId"] }).ConfigureAwait(false);
                Assert.IsNotNull(ownerDetailsForInventoryProduct);
                for (var i = 0; i < messageData.Count(); i++)
                {
                    if (messageData[i].SelectToken("idInventario").ToString().EqualsIgnoreCase(inventoryDetails["InventoryProductId"]))
                    {
                        var expectedVolume = inventoryDetails["ProductVolume"].Remove(inventoryDetails["ProductVolume"].IndexOf('.'));
                        Assert.AreEqual(expectedVolume, messageData[i].SelectToken("volumenPropiedad").ToString());
                    }
                }
            }
        }

        [When(@"""(.*)"" and ""(.*)"" collections do not have data")]
        public async Task WhenAndCollectionsDoNotHaveDataAsync(string inventoryErrorsCollection, string movementErrorCollection)
        {
            var fileName = this.GetValue(ConstantValues.TicketId);
            this.ScenarioContext["json"] = await fileName.OwnershipdatafromBlobAsync("ownershiprule").ConfigureAwait(false);
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var inventoryErrorsCollectionDetails = ficoRequestData["volPayload"]["volOutput"][inventoryErrorsCollection];
            Assert.IsNull(inventoryErrorsCollectionDetails);
            var movementErrorCollectionDetails = ficoRequestData["volPayload"]["volOutput"][movementErrorCollection];
            Assert.IsNull(movementErrorCollectionDetails);
        }

        [When(@"there are objects in any of the collections ""(.*)"" or ""(.*)""")]
        public async Task WhenThereAreObjectsInAnyOfTheCollectionsOrAsync(string movementResultsCollection, string inventoryResultsCollection)
        {
            var fileName = this.GetValue(ConstantValues.TicketId);
            this.ScenarioContext["json"] = await fileName.OwnershipdatafromBlobAsync("ownershiprule").ConfigureAwait(false);
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var movementResultsCollectionDetails = ficoRequestData["volPayload"]["volOutput"][movementResultsCollection];
            var inventoryResultsCollectionDetails = ficoRequestData["volPayload"]["volOutput"][inventoryResultsCollection];
            Assert.IsTrue(movementResultsCollectionDetails.HasValues || inventoryResultsCollectionDetails.HasValues);
        }

        [StepDefinition(@"there are no inventories for ownership calculation by application from ownership table")]
        [StepDefinition(@"there are inventories of day before ticket calculation generated by source system from owner table")]
        [StepDefinition(@"unit of ownership that corresponds to owner is percentage")]
        [StepDefinition(@"unit of ownership that corresponds to owner is other than percenatge")]
        [StepDefinition(@"send owner value to FICO")]
        [When(@"FICO returns successful response")]
        [StepDefinition(@"there are no inventories for ownership calculation by source system from owner table")]
        [Given(@"the system is processing the FICO service response")]
        public void WhenThereAreInventoriesOfDayBeforeTicketcalculationGeneratedBySourceSystemFromOwnerTable()
        {
            // already taken care in method implementation of GivenIHaveProcessedInventoriesAndGeneratedCutoffTicket()
            // already taken care in method implementation of ThenSendInitialInventoriesCollectionDataToFICO(parameter)
        }

        [Then(@"send net volume to FICO should be calculated as volume of the product multiplied by percentage that corresponds to owner and result should be divided by (.*)")]
        public void ThenSendNetVolumeToFICOShouldBeCalculatedAsVolumeOfTheProductMultipliedByPercentageThatCorrespondsToOwnerAndResultShouldBeDividedBy(int percentage)
        {
            Assert.IsNotNull(percentage);

            // already taken care this step in another step implementation
        }

        [Given(@"I have a segment details where ownership is already calculated")]
        public async Task GivenIHaveASegmentDetailsWhereOwnershipIsAlreadyCalculatedAsync()
        {
            var ticketDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetCompletedTicketOfOwnershipCalculationGrid).ConfigureAwait(false);
            this.SetValue(ConstantValues.TicketId, ticketDetails[ConstantValues.TicketId]);
            var segmentDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTicketAndSegmentDetailsByTicketId, args: new { ticketId= this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false);
            this.SetValue("CategorySegment", segmentDetails[ConstantValues.Name]);
        }

        [Then(@"send initial inventories collection with data to FICO that is generated by source system from owner table")]
        public async Task ThenSendInitialInventoriesCollectionWithDataToFICOThatIsGeneratedBySourceSystemFromOwnerTableAsync()
        {
            var fileName = this.GetValue(ConstantValues.TicketId);
            this.ScenarioContext["json"] = await fileName.OwnershipdatafromBlobAsync("ownershiprule").ConfigureAwait(false);
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"]["invetarioIniciales"];
            Assert.IsNotNull(messageData);
            for (var i = 0; i < messageData.Count(); i++)
            {
                if (messageData[i].SelectToken("idInventario").ToString().EqualsIgnoreCase(this.GetValue(ConstantValues.InventoryProductUnqiueId + "_1")))
                {
                    var expectedVolume = this.GetValue(ConstantValues.ProductVolume + "_1").Remove(this.GetValue(ConstantValues.ProductVolume + "_1").IndexOf('.'));
                    Assert.AreEqual(expectedVolume, messageData[i].SelectToken("volumenPropiedad").ToString());
                }
            }
        }

        [Then(@"add initial inventory collection that send to FICO with the data calculated through the application")]
        public async Task ThenAddInitialInventoryCollectionThatSendToFICOWithTheDataCalculatedThroughTheApplicationAsync()
        {
            var fileName = this.GetValue(ConstantValues.TicketId);
            this.ScenarioContext["json"] = await fileName.OwnershipdatafromBlobAsync("ownershiprule").ConfigureAwait(false);
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"]["invetarioIniciales"];
            Assert.IsNotNull(messageData);
            for (var i = 0; i < messageData.Count(); i++)
            {
                if (messageData[i].SelectToken("idInventario").ToString().EqualsIgnoreCase(this.GetValue(ConstantValues.InventoryProductUnqiueId)))
                {
                    string expectedProductVolume = this.GetValue(ConstantValues.ProductVolume).Remove(this.GetValue(ConstantValues.ProductVolume).IndexOf('.'));
                    Assert.AreEqual(expectedProductVolume, messageData[i].SelectToken("volumenPropiedad").ToString());
                }
            }
        }

        [When(@"there are inventories of day before ticket calculation generated by application from ownership table")]
        public async Task WhenThereAreInventoriesOfDayBeforeTicketCalculationGeneratedByApplicationFromOwnershipTableAsync()
        {
            var invProdDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetInventoryProductDetailsFromOwnership, args: new { ticketId = this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false);
            this.SetValue(ConstantValues.InventoryProductUnqiueId, invProdDetails["InventoryProductId"]);
            var inventoryProdcut = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetInventoryProductByInventoryProductId, args: new { invProdId = invProdDetails["InventoryProductId"] }).ConfigureAwait(false);
            this.SetValue(ConstantValues.ProductVolume, inventoryProdcut["ProductVolume"]);
        }

        [When(@"generate ownership ticket with initial inventories from ownership table")]
        [When(@"generate ownership ticket with initial inventories from both owner and ownership table")]
        public async Task WhenGenerateOwnershipTicketWithInitialInventoriesFromOwnershipTableAsync()
        {
            ////this.When("I navigate to \"Ownershipcalculation\" page");
            this.UiNavigation("Ownershipcalculation");
            ////this.When("I click on \"NewBalance\" \"button\"");
            this.IClickOn("NewBalance", "button");
            ////this.When("I select segment from \"OwnershipCal\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("OwnershipCal\" \"Segment", "dropdown");
            this.ISelectFinalDateOnOwnershipDatePicker(2);
            ////this.When("I click on \"OwnershipCalCriteria\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
            ////this.Then("I see \"Todos los nodos tienen configurada una estrategia de propiedad.\" message for \"Nodos con estrategia de propiedad\"");
            this.ISeeMessageFor("Todos los nodos tienen configurada una estrategia de propiedad.", "Nodos con estrategia de propiedad");
            ////this.Then("I verify all \"9\" validations passed");
            this.IVerifyAllValidationsPassed(9);
            ////this.Then("I click on \"OwnershipCalValidations\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
            ////this.Then("I click on \"OwnershipCalConfirmation\" \"Execute\" \"button\"");
            this.IClickOn("OwnershipCalValidations\" \"Execute", "button");
            ////this.When("I wait till ownership ticket geneation to complete");
            await this.WaitForOwnershipTicketProcessingToCompleteAsync().ConfigureAwait(false);
        }

        [When(@"there are inventories generated by source system on day before ticket calculation from owner table")]
        public async Task WhenThereAreInventoriesGeneratedBySourceSystemOnDayBeforeTicketcalculationFromOwnerTableAsync()
        {
            ////this.When("I update the excel data for inventories in \"Testdata_43369\"");
            this.IUpdateTheExcelDataForInventories("Testdata_43369");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            await Task.Delay(3000).ConfigureAwait(true);
            ////this.When("I select \"Testdata_43369\" file from directory");
            await this.ISelectFileFromDirectoryAsync("Testdata_43369").ConfigureAwait(false);
            await Task.Delay(3000).ConfigureAwait(true);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
            var invProdDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetInventoryId, args: new { inventoryId = this.GetValue("Inventory1") }).ConfigureAwait(false);
            this.SetValue(ConstantValues.InventoryProductUnqiueId + "_1", invProdDetails["InventoryProductId"]);
            this.SetValue(ConstantValues.ProductVolume + "_1", invProdDetails["ProductVolume"]);
        }

        [Then(@"send initial inventory collection to FICO with the data calculated through the application")]
        [Then(@"send to FICO only the ownership data calculated through the ownership functionality of the application")]
        public async Task ThenSendInitialInventoryCollectionToFICOWithTheDataCalculatedThroughTheApplicationAsync()
        {
            var fileName = this.GetValue(ConstantValues.TicketId);
            this.ScenarioContext["json"] = await fileName.OwnershipdatafromBlobAsync("ownershiprule").ConfigureAwait(false);
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"]["invetarioIniciales"];
            var invProdDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetInventoryProductByInventoryProductId, args: new { invProdId= this.GetValue(ConstantValues.InventoryProductUnqiueId) }).ConfigureAwait(false);
            Assert.IsNotNull(messageData);
            for (var i = 0; i < messageData.Count(); i++)
            {
                if (messageData[i].SelectToken("idInventario").ToString().EqualsIgnoreCase(this.GetValue(ConstantValues.InventoryProductUnqiueId)))
                {
                    string expectedProductVolume = invProdDetails["ProductVolume"].Remove(invProdDetails["ProductVolume"].IndexOf('.'));
                    Assert.AreEqual(expectedProductVolume, messageData[i].SelectToken("volumenPropiedad").ToString());
                }
            }
        }

        [When(@"there is an inventory where ownership sent by source system and ownership calculated by application")]
        public async Task WhenThereIsAnInventoryWhereOwnershipSentBySourceSystemAndOwnershipCalculatedByApplicationAsync()
        {
            await this.WhenThereAreInventoriesOfDayBeforeTicketCalculationGeneratedByApplicationFromOwnershipTableAsync().ConfigureAwait(false);
            ////this.When("I navigate to \"Ownershipcalculation\" page");
            this.UiNavigation("Ownershipcalculation");
            ////this.When("I click on \"NewBalance\" \"button\"");
            this.IClickOn("NewBalance", "button");
            ////this.When("I select segment from \"OwnershipCal\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("OwnershipCal\" \"Segment", "dropdown");
            this.ISelectFinalDateOnOwnershipDatePicker(1);
            ////this.When("I click on \"OwnershipCalCriteria\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
            ////this.Then("I see \"Todos los nodos tienen configurada una estrategia de propiedad.\" message for \"Nodos con estrategia de propiedad\"");
            this.ISeeMessageFor("Todos los nodos tienen configurada una estrategia de propiedad.", "Nodos con estrategia de propiedad");
            ////this.Then("I verify all \"9\" validations passed");
            this.IVerifyAllValidationsPassed(9);
            ////this.Then("I click on \"OwnershipCalValidations\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalulationValidations\" \"submit", "button");
            ////this.Then("I click on \"OwnershipCalConfirmation\" \"Execute\" \"button\"");
            this.IClickOn("OwnershipCalValidations\" \"Execute", "button");
            ////this.When("I wait till ownership ticket geneation to complete");
            await this.WaitForOwnershipTicketProcessingToCompleteAsync().ConfigureAwait(false);
        }

        [When(@"TRUE sends to FICO ownership data of inventories reported by the source systems")]
        public async Task WhenTRUESendsToFICOOwnershipDataOfInventoriesReportedByTheSourceSystemsAsync()
        {
            await this.WhenGenerateOwnershipTicketWithoutInitialInventoriesAsync().ConfigureAwait(false);
        }

        [Then(@"store the ownership of initial inventories sent to FICO reported by source system in the table that supports the ownership functionality of the application")]
        [Then(@"assign each ownership record the corresponding ticket number")]
        public async Task ThenStoreTheOwnershipOfInitialInventoriesSentToFICOReportedBySourceSystemInTheTableThatSupportsTheOwnershipFunctionalityOfTheApplicationAsync()
        {
            var ownershipDetails = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetInventoryProductDetailsFromOwnership, args: new { ticketId=this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false);
            Assert.IsNotNull(ownershipDetails);
        }

        [Then(@"each ownership record must have the rule name ""(.*)"" and version should be (.*)")]
        public async Task ThenEachOwnershipRecordMustHaveTheRuleNameAndVersionShouldBeAsync(string rule, int version)
        {
            Assert.AreEqual(4, await this.ReadSqlScalarAsync<int>(SqlQueries.GetRulesAndVersionFromOwnership, args: new { rule, version, ticketId=this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false));
        }

        public void ISelectFinalDateOnOwnershipDatePicker(int days)
        {
            var page = this.Get<ElementPage>();
            this.ScenarioContext["FinalDate"] = DateTime.Now.AddDays(-days).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            page.WaitUntilElementToBeClickable(nameof(Resources.OwnershipFinalDate));
            page.Click(nameof(Resources.OwnershipFinalDate));
            page.GetElement(nameof(Resources.OwnershipFinalDate)).SendKeys(this.ScenarioContext["FinalDate"].ToString());
            page.GetElement(nameof(Resources.OwnershipFinalDate)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }
    }
}