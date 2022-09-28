// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformOperationalMovementConsolidationSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.DataSources;
    using global::Bdd.Core.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class PerformOperationalMovementConsolidationSteps : EcpWebStepDefinitionBase
    {
        private readonly AppInsightsDataSource appinsights = new AppInsightsDataSource();

        [Given(@"that the TRUE system is processing the operative inventories consolidation")]
        [Given(@"that the TRUE system is processing the operative inventories consolidation with inventory date equal to the end date of the period")]
        [Given(@"that the TRUE system is processing the operative movements consolidation")]
        public async Task GivenThatTheTRUESystemIsProcessingTheOperativeMovementsConsolidationAsync()
        {
            await this.TheTRUESystemIsProcessingTheOperativeMovementsAndInventoriesConsolidationAsync().ConfigureAwait(false);
        }

        [When(@"that the TRUE system is processing the operative movements consolidation for all movement types")]
        public async Task WhenThatTheTRUESystemIsProcessingTheOperativeMovementsConsolidationForAllMovementTypesAsync()
        {
            this.SetValue(ConstantValues.TestdataForConsolidationExclusionMovements, "Yes");
            ////And I have ownership calculation data generated in the system
            await this.IHaveOwnershipCalculationDataGeneratedInTheSystemAsync().ConfigureAwait(false);

            //// Failing process when ownership ticket status is failed
            Assert.IsFalse(this.GetValue(ConstantValues.OwnershipTicketStatus).EqualsIgnoreCase("Fallido"));

            //// Updating Inventory volume and Movement volume of the excel
            await this.UpdatedInventoriesAndMovementsAsync().ConfigureAwait(false);

            //// Generation of Operative Delta Ticket
            await this.GenerateOperativeDeltaTicketAsync().ConfigureAwait(false);

            //// Generation of Cutoff ticket after Operative Delta
            await this.GenerationOfCutoffTicketAfterOperativeDeltaAsync().ConfigureAwait(false);

            //// Generation of Ownership ticket after Operative Delta
            await this.GenerationOfOwnershipTicketAfterOperativeDeltaAsync().ConfigureAwait(false);

            //// Failing process when ownership ticket status is failed
            Assert.IsFalse(this.GetValue(ConstantValues.OwnershipTicketStatus).EqualsIgnoreCase("Fallido"));

            //// Approving all nodes related segment
            await this.ReadAllSqlAsync(SqlQueries.UpdateOwnershipNodeStatusBasedOnSegmentName, args: new { ownershipStatusId = 9, segment = this.GetValue("SegmentName") }).ConfigureAwait(false);

            Assert.IsEmpty(await this.ReadAllSqlAsync(SqlQueries.ConsolidatedInventoryProductInformationWithSegmentName, args: new { segmentName = this.GetValue("SegmentName") }).ConfigureAwait(false));
            Assert.IsEmpty(await this.ReadAllSqlAsync(SqlQueries.ConsolidatedMovementInformationWithSegmentName, args: new { segmentName = this.GetValue("SegmentName") }).ConfigureAwait(false));

            //// Generation of official delta ticket
            await this.WhenThatTheTRUESystemIsProcessingTheOperativeMovementsAsync().ConfigureAwait(false);
        }

        [Given(@"that the TRUE system is processing the operative inventories consolidation for not SON segment")]
        [Given(@"that the TRUE system is processing the operative movements consolidation for not SON segment")]
        public async Task GivenThatTheTRUESystemIsProcessingTheOperativeMovementsConsolidationForNotSONSegmentAsync()
        {
            this.SetValue(ConstantValues.NoSONSegment, "Yes");
            await this.GivenThatTheTRUESystemIsProcessingTheOperativeMovementsConsolidationAsync().ConfigureAwait(false);
        }

        [Given(@"that the TRUE system is processing the operative inventories consolidation for not son segment has movements without owners")]
        [Given(@"that the TRUE system is processing the operative movements consolidation for not son segment has movements without owners")]
        public async Task GivenThatTheTRUESystemIsProcessingTheOperativeMovementsConsolidationForNotSonSegmentHasMovementsWithoutOwnersAsync()
        {
            this.SetValue(ConstantValues.MovementsWithoutOwners, "Yes");
            await this.GivenThatTheTRUESystemIsProcessingTheOperativeMovementsConsolidationForNotSONSegmentAsync().ConfigureAwait(false);
        }

        public async Task UpdatedInventoriesAndMovementsAsync()
        {
            this.WhenIUpdateTheExcelFileWithConsolidationData("TestData_Consolidation");
            ////When I navigate to "FileUpload" page
            this.NavigateToPage("FileUpload");
            ////And I click on "FileUpload" "button"
            this.IClickOn("FileUpload", "button");
            ////And I select segment from "FileUpload" "segment" "dropdown"
            this.ISelectSegmentFrom("FileUpload\" \"segment", "dropdown");
            ////And I select "Update" from FileUpload dropdown
            this.ISelectFileFromFileUploadDropdown("Update");
            ////And I click on "Browse" to upload
            this.IClickOnUploadButton("Browse");
            ////And I select "TestData_Consolidation" file from directory
            await this.ISelectFileFromDirectoryAsync("TestData_Consolidation").ConfigureAwait(false);
            ////And I click on "uploadFile" "Submit" "button"
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////And I wait till file upload to complete
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        public async Task GenerateOperativeDeltaTicketAsync()
        {
            ////And I navigate to "Calculation of deltas by operational adjust" page
            this.NavigateToPage("Calculation of deltas by operational adjust");
            ////And I click on "New Delta Calculation" "button"
            this.IClickOn("New Deltas Calculation", "button");
            ////And I select segment from "initDeltaTicket" "segment" "dropdown"
            this.ISelectSegmentFrom("initDeltaTicket\" \"segment", "dropdown");
            ////And I click on "initDeltaTicket" "Submit" "button"
            this.IClickOn("initDeltaTicket\" \"submit", "button");
            ////And I click on "pendingInventoriesGrid" "Submit" "button"
            this.IClickOn("pendingInventoriesGrid\" \"submit", "button");
            ////And I click on "pendingMovementsGrid" "Submit" "button"
            this.IClickOn("pendingMovementsGrid\" \"submit", "button");
            ////Then I should see "Modal" "confirmDeltaCalculation" "container"
            this.IShouldSee("Modal\" \"confirmDeltaCalculation", "container");
            ////And I click on "confirmDeltaCalculation" "submit" "Button"
            this.IClickOn("confirmDeltaCalculation\" \"submit", "button");
            ////And Verify that Delta Calculation should be successful
            await this.VerifyThatDeltaCalculationShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        public async Task GenerationOfCutoffTicketAfterOperativeDeltaAsync()
        {
            this.UiNavigation("Operational Cutoff");
            ////this.When("I click on \"NewCut\" \"button\"");
            this.IClickOn("NewCut", "button");
            ////this.When("I choose CategoryElement from \"InitTicket\" \"Segment\" \"combobox\"");
            this.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(26, "Consildation_AfterOperativeDelta").ConfigureAwait(false);
            ////this.When("I click on \"InitTicket\" \"next\" \"button\"");
            this.IClickOn("InitTicket\" \"Submit", "button");
            ////this.Then("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("validateInitialInventory\" \"submit", "button");
            ////this.When("I click on \"checkInventories\" \"Next\" \"button\"");
            this.IClickOn("validateInitialInventory\" \"submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all pending records from grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            this.ProvidedRequiredDetailsForPendingTransactionsGrid();
            ////this.Then("validate that \"ErrorsGrid\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("ErrorsGrid\" \"Submit", "button");
            ////this.When("I click on \"ErrorsGrid\" \"Next\" \"button\"");
            this.IClickOn("ErrorsGrid\" \"Submit", "button");
            ////this.When("I click on \"officialPointsGrid\" \"Next\" \"button\"");
            this.IClickOn("officialPointsGrid\" \"Submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all unbalances in the grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            this.ProvidedRequiredDetailsForUnbalancesGrid();
            ////this.When("I click on \"ConsistencyCheck\" \"Next\" \"button\"");
            this.IClickOn("unbalancesGrid\" \"submit", "button");
            ////this.When("I click on \"Confirm\" \"Cutoff\" \"Submit\" \"button\"");
            this.IClickOn("ConfirmCutoff\" \"Submit", "button");
            ////this.When("I wait till cutoff ticket processing to complete");
            await this.WaitForTicketToCompleteProcessingAsync().ConfigureAwait(false);
        }

        public async Task GenerationOfOwnershipTicketAfterOperativeDeltaAsync()
        {
            ////this.When("I navigate to \"ownershipcalculation\" page");
            this.UiNavigation("ownershipcalculation");
            ////this.When("I click on \"NewBalance\" \"button\"");
            this.IClickOn("NewBalance", "button");
            ////this.When("I select segment from \"OwnershipCal\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("OwnershipCal\" \"Segment", "dropdown");
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(30, "Consildation_Ownership_AfterOperativeDelta").ConfigureAwait(false);
            ////this.When("I click on \"ownershipCalCriteria\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
            ////this.When("I verify all validations passed");
            this.IVerifyValidationsPassed();
            ////this.When("I click on \"ownershipCalValidations\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalulationValidations\" \"submit", "button");
            ////this.When("I click on \"OwnershipCalConfirmation\" \"Execute\" \"button\"");
            this.IClickOn("ownershipCalculationConfirmation\" \"submit", "button");
            await Task.Delay(10000).ConfigureAwait(true);
            ////this.Then("verify the ownership is calculated successfully");
            await this.VerifyTheOwnershipIsCalculatedSuccessfullyAsync().ConfigureAwait(false);
            ////this.When("I wait till ownership ticket geneation to complete");
            await this.WaitForOwnershipTicketProcessingToCompleteAsync().ConfigureAwait(false);
        }

        [Given(@"that the TRUE system is processing the operative inventories consolidation data")]
        [When(@"segment does NOT have a consolidation process executed for another period")]
        [When(@"inventories have owners reported from the source systems")]
        [When(@"I have not SON segment that has operative inventories with an inventory date is equal to the end date of the period")]
        [When(@"inventories do not have owners reported from the source systems")]
        [When(@"segment is not SON has operative inventories with an operational date within the dates of a period")]
        [Then(@"join the information from the previous two points using node and product")]
        [When(@"segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period")]
        [When(@"I have SON segment that has operative inventories with an inventory date is equal to the end date of the period")]
        [When(@"inventories have an ownership ticket")]
        [Given(@"that the TRUE system is processing the operative movements consolidation data")]
        [Then(@"update the segment ticket to failed")]
        [When(@"I have SON segment that has operative movements with an operational date within the dates of a period")]
        [When(@"movements have an ownership ticket")]
        [Then(@"the ownership of the movements must be obtained from the ownership information returned by FICO")]
        [Then(@"join the information from the previous two points using source node, destination node, source product, destination product and movement type")]
        [When(@"there are operative movements with a source movement identifier")]
        [When(@"the movements with a source movement identifier are of different types than the cancellation types configured in the relationships between movement types")]
        [When(@"segment is not SON has operative movements with an operational date within the dates of a period")]
        [When(@"movements have owners reported from the source systems")]
        [Then(@"if the value of the owner was reported in percentage then the quantity must be calculated first")]
        [When(@"movements do not have owners reported from the source systems")]
        [When(@"the segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period")]
        public void GivenThatTheTRUESystemIsProcessingTheOperativeMovementsConsolidationData()
        {
            // Already taken care in some other stepdefinition
        }

        [When(@"the movement type is equal to one of the cancellation types configured in the relationships between movement types")]
        [When(@"the movement type is different to one of the cancellation types configured in the relationships between movement types")]
        [When(@"there are operative movements without a source movement identifier")]
        [When(@"movements have an operational date within the dates of a period")]
        [When(@"the movements belong to a SON segment")]
        [Then(@"get information from the source movement identifiers")]
        [Then(@"add the consolidation of these movements to consolidate list with operative movements with a source movement identifier and the movement type is different to one of the cancellation types configured in the relationships between movement types")]
        [Then(@"add the consolidation of these movements to consolidate list with operative movements without a source movement identifier")]
        [Then(@"consolidate the operative movements with a source movement identifier and the movement type is equal to one of the cancellation types configured in the relationships between movement types")]
        [Then(@"the quantity of the source movements subtract the value of one of its cancellation movements")]
        [Then(@"with the ownership percentages of each source movement \(ownership returned by FICO\) calculate the ownership quantity of the new movements")]
        [Then(@"consolidate the net quantity and gross quantity of the calculated movements grouping the information by source node, destination node, source product, destination product and movement type")]
        [Then(@"consolidate the ownership quantity of the calculated movements by source node, destination node, source product, destination product, movement type and owner")]
        public void WhenTheMovementTypeIsEqualToOneOfTheCancellationTypesConfiguredInTheRelationshipsBetweenMovementTypes()
        {
            // Already taken care in some other stepdefinition
        }

        [Then(@"store the consolidated inventories with ""(.*)""")]
        [Then(@"store the consolidated movements with ""(.*)""")]
        public void ThenStoreTheConsolidatedMovementsWith(string field, Table table)
        {
            Assert.IsNotNull(field);
            Assert.IsNotNull(table);
            //// Already taken care in some other stepdefinition
        }

        [Then(@"movements to consolidate list should contain consolidated information of the net quantity and gross quantity grouping the information by source node, destination node, source product, destination product and movement type")]
        [Then(@"consolidate the net quantity and gross quantity of the movements grouping the information by source node, destination node, source product, destination product and movement type")]
        public async Task ThenConsolidateTheNetQuantityAndGrossQuantityOfTheMovementsGroupingTheInformationBySourceNodeDestinationNodeSourceProductDestinationProductAndMovementTypeAsync()
        {
            var consolidatedMovementsWithoutDestinationMovementDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.ConsolidatedMovementInformationWithoutMovementSourceDetails, args: new { destinationNodeId = this.GetValue("NodeId_1"), destinationProductId = ConstantValues.SourceNodeProductId2, movementTypeId = this.GetValue("MovementTypeId") }).ConfigureAwait(false);
            this.ScenarioContext["ConsolidatedMovementIdWIthoutSource"] = consolidatedMovementsWithoutDestinationMovementDetails["ConsolidatedMovementId"];
            var consolidatedCuscoMovementsDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.ConsolidatedMovementsInformation, args: new { sourceNodeId = this.GetValue("NodeId_2"), destinationNodeId = this.GetValue("NodeId_1"), sourceProductId = ConstantValues.SourceNodeProductId2, destinationProductId = ConstantValues.SourceNodeProductId2, movementTypeId = this.GetValue("MovementTypeId") }).ConfigureAwait(false);
            this.ScenarioContext["ConsolidatedMovementIdForCusco"] = consolidatedCuscoMovementsDetails["ConsolidatedMovementId"];
            var consolidatedMamboMovementsDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.ConsolidatedMovementsInformation, args: new { sourceNodeId = this.GetValue("NodeId_2"), destinationNodeId = this.GetValue("NodeId_1"), sourceProductId = ConstantValues.OriginSourceProductId, destinationProductId = ConstantValues.OriginSourceProductId, movementTypeId = this.GetValue("MovementTypeId") }).ConfigureAwait(false);
            this.ScenarioContext["ConsolidatedMovementIdForMambo"] = consolidatedMamboMovementsDetails["ConsolidatedMovementId"];

            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForConsolidationExclusionMovements)))
            {
                Assert.AreEqual("200.00", consolidatedMovementsWithoutDestinationMovementDetails["GrossStandardVolume"].ToString());
                Assert.AreEqual("556786.00", consolidatedMovementsWithoutDestinationMovementDetails["NetStandardVolume"].ToString());

                Assert.AreEqual("400.00", consolidatedCuscoMovementsDetails["GrossStandardVolume"].ToString());
                Assert.AreEqual("1074558.00", consolidatedCuscoMovementsDetails["NetStandardVolume"].ToString());

                Assert.AreEqual("5800.00", consolidatedMamboMovementsDetails["GrossStandardVolume"].ToString());
                Assert.AreEqual("12894197.95", consolidatedMamboMovementsDetails["NetStandardVolume"].ToString());
            }
            else
            {
                Assert.AreEqual("400.00", consolidatedMovementsWithoutDestinationMovementDetails["GrossStandardVolume"].ToString());
                Assert.AreEqual("874558.55", consolidatedMovementsWithoutDestinationMovementDetails["NetStandardVolume"].ToString());

                Assert.AreEqual("400.00", consolidatedCuscoMovementsDetails["GrossStandardVolume"].ToString());
                Assert.AreEqual("874558.55", consolidatedCuscoMovementsDetails["NetStandardVolume"].ToString());

                Assert.AreEqual("6200.00", consolidatedMamboMovementsDetails["GrossStandardVolume"].ToString());
                Assert.AreEqual("13708756.50", consolidatedMamboMovementsDetails["NetStandardVolume"].ToString());
            }
        }

        [Then(@"consolidated information of the ownership quantity of the movements by source node, destination node, source product, destination product, movement type and owner")]
        [Then(@"the ownership of the movements must be obtained from the owners information reported by the source systems")]
        [Then(@"consolidate the ownership quantity of the movements by source node, destination node, source product, destination product, movement type and owner")]
        public async Task ThenConsolidateTheOwnershipQuantityOfTheMovementsBySourceNodeDestinationNodeSourceProductDestinationProductMovementTypeAndOwnerAsync()
        {
            var consolidatedOwnersInformationWithoutSource = await this.ReadAllSqlAsync(SqlQueries.ConsolidatedMovementOwnerInformation, args: new { consolidatedMovementId = this.ScenarioContext["ConsolidatedMovementIdWIthoutSource"].ToString() }).ConfigureAwait(false);
            var numberOfconsolidatedOwnersInformationWithoutSource = consolidatedOwnersInformationWithoutSource.ToDictionaryList();
            var consolidatedOwnersInformationForCusco = await this.ReadAllSqlAsync(SqlQueries.ConsolidatedMovementOwnerInformation, args: new { consolidatedMovementId = this.ScenarioContext["ConsolidatedMovementIdForCusco"].ToString() }).ConfigureAwait(false);
            var numberOfconsolidatedOwnersInformationForCusco = consolidatedOwnersInformationForCusco.ToDictionaryList();
            var consolidatedOwnersInformationForMambo = await this.ReadAllSqlAsync(SqlQueries.ConsolidatedMovementOwnerInformation, args: new { consolidatedMovementId = this.ScenarioContext["ConsolidatedMovementIdForMambo"].ToString() }).ConfigureAwait(false);
            var numberOfconsolidatedOwnersInformationForMambo = consolidatedOwnersInformationForMambo.ToDictionaryList();

            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForConsolidationExclusionMovements)))
            {
                for (int i = 0; i < numberOfconsolidatedOwnersInformationWithoutSource.Count; i++)
                {
                    Assert.AreEqual("30", numberOfconsolidatedOwnersInformationWithoutSource[i]["OwnerId"].ToString());
                    Assert.AreEqual("556786", numberOfconsolidatedOwnersInformationWithoutSource[i]["OwnershipVolume"].ToString());
                    Assert.AreEqual("100", numberOfconsolidatedOwnersInformationWithoutSource[i]["OwnershipPercentage"].ToString());
                }

                for (int i = 0; i < numberOfconsolidatedOwnersInformationForCusco.Count; i++)
                {
                    Assert.AreEqual("30", numberOfconsolidatedOwnersInformationForCusco[i]["OwnerId"].ToString());
                    Assert.AreEqual("1074558", numberOfconsolidatedOwnersInformationForCusco[i]["OwnershipVolume"].ToString());
                    Assert.AreEqual("100", numberOfconsolidatedOwnersInformationForCusco[i]["OwnershipPercentage"].ToString());
                }

                for (int i = 0; i < numberOfconsolidatedOwnersInformationForMambo.Count; i++)
                {
                    Assert.AreEqual("30", numberOfconsolidatedOwnersInformationForMambo[i]["OwnerId"].ToString());
                    Assert.AreEqual("12894197.95", numberOfconsolidatedOwnersInformationForMambo[i]["OwnershipVolume"].ToString());
                    Assert.AreEqual("100", numberOfconsolidatedOwnersInformationForMambo[i]["OwnershipPercentage"].ToString());
                }
            }
            else if (string.IsNullOrEmpty(this.GetValue(ConstantValues.NoSONSegment)))
            {
                for (int i = 0; i < numberOfconsolidatedOwnersInformationWithoutSource.Count; i++)
                {
                    Assert.AreEqual("30", numberOfconsolidatedOwnersInformationWithoutSource[i]["OwnerId"].ToString());
                    Assert.AreEqual("874558.55", numberOfconsolidatedOwnersInformationWithoutSource[i]["OwnershipVolume"].ToString());
                    Assert.AreEqual("100", numberOfconsolidatedOwnersInformationWithoutSource[i]["OwnershipPercentage"].ToString());
                }

                for (int i = 0; i < numberOfconsolidatedOwnersInformationForCusco.Count; i++)
                {
                    Assert.AreEqual("30", numberOfconsolidatedOwnersInformationForCusco[i]["OwnerId"].ToString());
                    Assert.AreEqual("874558.55", numberOfconsolidatedOwnersInformationForCusco[i]["OwnershipVolume"].ToString());
                    Assert.AreEqual("100", numberOfconsolidatedOwnersInformationForCusco[i]["OwnershipPercentage"].ToString());
                }

                for (int i = 0; i < numberOfconsolidatedOwnersInformationForMambo.Count; i++)
                {
                    Assert.AreEqual("30", numberOfconsolidatedOwnersInformationForMambo[i]["OwnerId"].ToString());
                    Assert.AreEqual("13708756.5", numberOfconsolidatedOwnersInformationForMambo[i]["OwnershipVolume"].ToString());
                    Assert.AreEqual("100", numberOfconsolidatedOwnersInformationForMambo[i]["OwnershipPercentage"].ToString());
                }
            }
            else if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.NoSONSegment)))
            {
                Assert.AreEqual("30", numberOfconsolidatedOwnersInformationWithoutSource[0]["OwnerId"].ToString());
                Assert.AreEqual("524735.13", numberOfconsolidatedOwnersInformationWithoutSource[0]["OwnershipVolume"].ToString());
                Assert.AreEqual("60", numberOfconsolidatedOwnersInformationWithoutSource[0]["OwnershipPercentage"].ToString());

                Assert.AreEqual("29", numberOfconsolidatedOwnersInformationWithoutSource[1]["OwnerId"].ToString());
                Assert.AreEqual("349823.42", numberOfconsolidatedOwnersInformationWithoutSource[1]["OwnershipVolume"].ToString());
                Assert.AreEqual("40", numberOfconsolidatedOwnersInformationWithoutSource[1]["OwnershipPercentage"].ToString());

                Assert.AreEqual("30", numberOfconsolidatedOwnersInformationForCusco[0]["OwnerId"].ToString());
                Assert.AreEqual("524735.13", numberOfconsolidatedOwnersInformationForCusco[0]["OwnershipVolume"].ToString());
                Assert.AreEqual("60", numberOfconsolidatedOwnersInformationForCusco[0]["OwnershipPercentage"].ToString());

                Assert.AreEqual("29", numberOfconsolidatedOwnersInformationForCusco[1]["OwnerId"].ToString());
                Assert.AreEqual("349823.42", numberOfconsolidatedOwnersInformationForCusco[1]["OwnershipVolume"].ToString());
                Assert.AreEqual("40", numberOfconsolidatedOwnersInformationForCusco[1]["OwnershipPercentage"].ToString());

                Assert.AreEqual("30", numberOfconsolidatedOwnersInformationForMambo[0]["OwnerId"].ToString());
                Assert.AreEqual("13358933.08", numberOfconsolidatedOwnersInformationForMambo[0]["OwnershipVolume"].ToString());
                Assert.AreEqual("97.45", numberOfconsolidatedOwnersInformationForMambo[0]["OwnershipPercentage"].ToString());

                Assert.AreEqual("29", numberOfconsolidatedOwnersInformationForMambo[1]["OwnerId"].ToString());
                Assert.AreEqual("349823.42", numberOfconsolidatedOwnersInformationForMambo[1]["OwnershipVolume"].ToString());
                Assert.AreEqual("2.55", numberOfconsolidatedOwnersInformationForMambo[1]["OwnershipPercentage"].ToString());
            }
        }

        [When(@"SON segment already has consolidated inventories for a date period or inventories with a date equal to the end date of the period")]
        [When(@"SON segment already has consolidated movements for a date period or inventories with a date equal to the end date of the period")]
        public async Task WhenSONSegmentAlreadyHasConsolidatedMovementsForADatePeriodOrInventoriesWithADateEqualToTheEndDateOfThePeriodAsync()
        {
            var consolidationAlreadyExists = await this.ReadSqlAsDictionaryAsync(SqlQueries.ConsolidatedInformationAlreadyExistsForSONSegment).ConfigureAwait(false);
            this.ScenarioContext["SegmentName"] = consolidationAlreadyExists["SegmentName"];
            this.SetValue("YearForPeriod", consolidationAlreadyExists["EndDate"].ToString().Split('-')[0]);
            this.SetValue("PreviousMonthName", UIContent.Conversion[consolidationAlreadyExists["EndDate"].ToString().Split('-')[1]].Substring(0, 3));
            Assert.IsNotNull(consolidationAlreadyExists);
        }

        [When(@"segment is not SON already has consolidated movements for a date period or inventories with a date equal to the end date of the period")]
        public async Task WhenSegmentIsNotSONAlreadyHasConsolidatedMovementsForADatePeriodOrInventoriesWithADateEqualToTheEndDateOfThePeriodAsync()
        {
            var consolidationAlreadyExists = await this.ReadSqlAsDictionaryAsync(SqlQueries.ConsolidatedInformationAlreadyExistsForNOSONSegment).ConfigureAwait(false);
            this.ScenarioContext["SegmentName"] = consolidationAlreadyExists["SegmentName"];
            this.SetValue("YearForPeriod", consolidationAlreadyExists["EndDate"].ToString().Split('-')[0]);
            this.SetValue("PreviousMonthName", UIContent.Conversion[consolidationAlreadyExists["EndDate"].ToString().Split('-')[1]].Substring(0, 3));
            Assert.IsNotNull(consolidationAlreadyExists);
        }

        [When(@"that the TRUE system is processing the operative inventories for not SON segment")]
        [When(@"that the TRUE system is processing the operative inventories")]
        [When(@"that the TRUE system is processing the operative movements")]
        [When(@"that the TRUE system is processing the operative movements for not SON segment")]
        public async Task WhenThatTheTRUESystemIsProcessingTheOperativeMovementsAsync()
        {
            await this.OfficialDeltaTicketGenerationAsync().ConfigureAwait(false);
        }

        [Then(@"do not run the process of operative inventories consolidation")]
        [Then(@"do not run the process of operative movements consolidation")]
        [Then(@"movements without owners should not be taken into account in the consolidation process")]
        public async Task ThenDoNotRunTheProcessOfOperativeMovementsConsolidationAsync()
        {
            Assert.IsEmpty(await this.ReadAllSqlAsync(SqlQueries.ConsolidatedInventoryProductInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("Official Delta TicketId") }).ConfigureAwait(false));
            Assert.IsEmpty(await this.ReadAllSqlAsync(SqlQueries.ConsolidatedMovementInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("Official Delta TicketId") }).ConfigureAwait(false));
        }

        [When(@"a technical exception occurs during the inventories consolidation process of a segment")]
        [When(@"a technical exception occurs during the movements consolidation process of a segment")]
        public async Task WhenATechnicalExceptionOccursDuringTheMovementsConsolidationProcessOfASegmentAsync()
        {
            //// this.Given I have processed "ConsolidationQueue" in the system
            await this.IHaveProcessedInTheSystemAsync(ConstantValues.ConsolidationQueue).ConfigureAwait(false);
        }

        [Then(@"store the message ""(.*)""")]
        public void ThenStoreTheMessage(string errorMessage)
        {
            Assert.AreEqual(errorMessage, this.GetValue(ConstantValues.ErrorMessage));
        }

        [Then(@"store the exception in Application Insights")]
        public async Task ThenStoreTheExceptionInApplicationInsightsAsync()
        {
            ////waiting for 4 min to write the message into app insights
            await Task.Delay(240000).ConfigureAwait(true);
            var result = await this.appinsights.ReadAllAsync<dynamic>(Queries.ConsolidationTechnicalExceptionMessage).ConfigureAwait(false);
            var appInsightsResult = JObject.Parse(result.ToString());
            Assert.IsTrue(appInsightsResult["tables"][0]["rows"].ToString().Contains(this.GetValue(ConstantValues.ErrorMessage)));
        }
    }
}