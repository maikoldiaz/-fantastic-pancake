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

    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class PerformOperationalinventoryConsolidationSteps : EcpWebStepDefinitionBase
    {
        [Then(@"consolidate the net quantity and gross quantity of the inventories grouping the information by node and product but not to take into account the owners")]
        public async Task ThenConsolidateTheNetQuantityAndGrossQuantityOfTheInventoriesGroupingTheInformationByNodeAndProductButNotToTakeIntoAccountTheOwnersAsync()
        {
            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForAnotherPeriodConsolidation)))
            {
                var consolidatedInventoryProductForAnotherPeriod = await this.ReadSqlAsDictionaryAsync(SqlQueries.ConsolidatedInventoryInformationAsc, args: new { NodeId = this.GetValue("NodeId_1"), ProductId = ConstantValues.SourceNodeProductId2, officialDeltaTicketId = this.GetValue("AnotherPeriodOfficialDeltaTicket") }).ConfigureAwait(false);
                this.ScenarioContext["ConsolidatedInventoryIdForAnotherPeriod"] = consolidatedInventoryProductForAnotherPeriod["ConsolidatedInventoryProductId"];

                Assert.AreEqual("120.00", consolidatedInventoryProductForAnotherPeriod["GrossStandardQuantity"].ToString());
                Assert.AreEqual("10000.00", consolidatedInventoryProductForAnotherPeriod["ProductVolume"].ToString());
            }

            if (string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForAnotherPeriodConsolidation)))
            {
                var consolidatedInventoryProductPreviousPeriod = await this.ReadSqlAsDictionaryAsync(SqlQueries.ConsolidatedInventoryInformationAsc, args: new { NodeId = this.GetValue("NodeId_1"), ProductId = ConstantValues.SourceNodeProductId2, officialDeltaTicketId = this.GetValue("Official Delta TicketId") }).ConfigureAwait(false);
                this.ScenarioContext["ConsolidatedInventoryIdPreviousPeriod"] = consolidatedInventoryProductPreviousPeriod["ConsolidatedInventoryProductId"];

                Assert.AreEqual("120.00", consolidatedInventoryProductPreviousPeriod["GrossStandardQuantity"].ToString());
                Assert.AreEqual("10000.00", consolidatedInventoryProductPreviousPeriod["ProductVolume"].ToString());
            }

            var consolidatedInventoryProductPreviousPeriodCusko = await this.ReadSqlAsDictionaryAsync(SqlQueries.ConsolidatedInventoryInformationDesc, args: new { NodeId = this.GetValue("NodeId_1"), ProductId = ConstantValues.SourceNodeProductId2, officialDeltaTicketId = this.GetValue("Official Delta TicketId") }).ConfigureAwait(false);
            this.ScenarioContext["ConsolidatedInventoryIdForCusco"] = consolidatedInventoryProductPreviousPeriodCusko["ConsolidatedInventoryProductId"];
            var consolidatedInventoryProductPreviousPeriodMambo = await this.ReadSqlAsDictionaryAsync(SqlQueries.ConsolidatedInventoryInformationDesc, args: new { NodeId = this.GetValue("NodeId_1"), ProductId = ConstantValues.OriginSourceProductId, officialDeltaTicketId = this.GetValue("Official Delta TicketId") }).ConfigureAwait(false);
            this.ScenarioContext["ConsolidatedInventoryIdForMambo"] = consolidatedInventoryProductPreviousPeriodMambo["ConsolidatedInventoryProductId"];

            Assert.AreEqual("240.00", consolidatedInventoryProductPreviousPeriodCusko["GrossStandardQuantity"].ToString());
            Assert.AreEqual("11111111.10", consolidatedInventoryProductPreviousPeriodCusko["ProductVolume"].ToString());

            Assert.AreEqual("240.00", consolidatedInventoryProductPreviousPeriodMambo["GrossStandardQuantity"].ToString());
            Assert.AreEqual("11111111.10", consolidatedInventoryProductPreviousPeriodMambo["ProductVolume"].ToString());
        }

        [Then(@"consolidate the ownership quantity of the inventories by node, product and owner")]
        public async Task ThenConsolidateTheOwnershipQuantityOfTheInventoriesByNodeProductAndOwnerAsync()
        {
            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForAnotherPeriodConsolidation)))
            {
                var consolidatedInventoryOwnersInformationForAnotherPeriod = await this.ReadSqlAsDictionaryAsync(SqlQueries.ConsolidatedInventoryOwnerInformation, args: new { consolidatedProductId = this.ScenarioContext["ConsolidatedInventoryIdForAnotherPeriod"].ToString() }).ConfigureAwait(false);

                Assert.AreEqual("30", consolidatedInventoryOwnersInformationForAnotherPeriod["OwnerId"].ToString());
                Assert.AreEqual("10000.00", consolidatedInventoryOwnersInformationForAnotherPeriod["OwnershipVolume"].ToString());
                Assert.AreEqual("100.00", consolidatedInventoryOwnersInformationForAnotherPeriod["OwnershipPercentage"].ToString());
            }

            var consolidatedInventoryOwnersInformationPreviousPeriod = await this.ReadAllSqlAsync(SqlQueries.ConsolidatedInventoryOwnerInformation, args: new { consolidatedProductId = this.ScenarioContext["ConsolidatedInventoryIdPreviousPeriod"].ToString() }).ConfigureAwait(false);
            var numberOfconsolidatedOwnersInformationWithoutSource = consolidatedInventoryOwnersInformationPreviousPeriod.ToDictionaryList();

            var consolidatedOwnersInformationForCusco = await this.ReadAllSqlAsync(SqlQueries.ConsolidatedInventoryOwnerInformation, args: new { consolidatedProductId = this.ScenarioContext["ConsolidatedInventoryIdForCusco"].ToString() }).ConfigureAwait(false);
            var numberOfconsolidatedOwnersInformationForCusco = consolidatedOwnersInformationForCusco.ToDictionaryList();
            var consolidatedOwnersInformationForMambo = await this.ReadAllSqlAsync(SqlQueries.ConsolidatedInventoryOwnerInformation, args: new { consolidatedProductId = this.ScenarioContext["ConsolidatedInventoryIdForMambo"].ToString() }).ConfigureAwait(false);
            var numberOfconsolidatedOwnersInformationForMambo = consolidatedOwnersInformationForMambo.ToDictionaryList();

            if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.NoSONSegment)))
            {
                Assert.AreEqual("30", numberOfconsolidatedOwnersInformationWithoutSource[0]["OwnerId"].ToString());
                Assert.AreEqual("6000", numberOfconsolidatedOwnersInformationWithoutSource[0]["OwnershipVolume"].ToString());
                Assert.AreEqual("60", numberOfconsolidatedOwnersInformationWithoutSource[0]["OwnershipPercentage"].ToString());

                Assert.AreEqual("29", numberOfconsolidatedOwnersInformationWithoutSource[1]["OwnerId"].ToString());
                Assert.AreEqual("4000", numberOfconsolidatedOwnersInformationWithoutSource[1]["OwnershipVolume"].ToString());
                Assert.AreEqual("40", numberOfconsolidatedOwnersInformationWithoutSource[1]["OwnershipPercentage"].ToString());

                Assert.AreEqual("30", numberOfconsolidatedOwnersInformationForCusco[0]["OwnerId"].ToString());
                Assert.AreEqual("6666666.66", numberOfconsolidatedOwnersInformationForCusco[0]["OwnershipVolume"].ToString());
                Assert.AreEqual("60", numberOfconsolidatedOwnersInformationForCusco[0]["OwnershipPercentage"].ToString());

                Assert.AreEqual("29", numberOfconsolidatedOwnersInformationForCusco[1]["OwnerId"].ToString());
                Assert.AreEqual("4444444.44", numberOfconsolidatedOwnersInformationForCusco[1]["OwnershipVolume"].ToString());
                Assert.AreEqual("40", numberOfconsolidatedOwnersInformationForCusco[1]["OwnershipPercentage"].ToString());

                Assert.AreEqual("30", numberOfconsolidatedOwnersInformationForMambo[0]["OwnerId"].ToString());
                Assert.AreEqual("6666666.66", numberOfconsolidatedOwnersInformationForMambo[0]["OwnershipVolume"].ToString());
                Assert.AreEqual("60", numberOfconsolidatedOwnersInformationForMambo[0]["OwnershipPercentage"].ToString());

                Assert.AreEqual("29", numberOfconsolidatedOwnersInformationForMambo[1]["OwnerId"].ToString());
                Assert.AreEqual("4444444.44", numberOfconsolidatedOwnersInformationForMambo[1]["OwnershipVolume"].ToString());
                Assert.AreEqual("40", numberOfconsolidatedOwnersInformationForMambo[1]["OwnershipPercentage"].ToString());
            }
            else if (!string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForConsolidation)))
            {
                if (string.IsNullOrEmpty(this.GetValue(ConstantValues.TestdataForAnotherPeriodConsolidation)))
                {
                    Assert.AreEqual("29", numberOfconsolidatedOwnersInformationWithoutSource[0]["OwnerId"].ToString());
                    Assert.AreEqual("4000", numberOfconsolidatedOwnersInformationWithoutSource[0]["OwnershipVolume"].ToString());
                    Assert.AreEqual("40", numberOfconsolidatedOwnersInformationWithoutSource[0]["OwnershipPercentage"].ToString());

                    Assert.AreEqual("30", numberOfconsolidatedOwnersInformationWithoutSource[1]["OwnerId"].ToString());
                    Assert.AreEqual("6000", numberOfconsolidatedOwnersInformationWithoutSource[1]["OwnershipVolume"].ToString());
                    Assert.AreEqual("60", numberOfconsolidatedOwnersInformationWithoutSource[1]["OwnershipPercentage"].ToString());
                }

                Assert.AreEqual("30", numberOfconsolidatedOwnersInformationForCusco[0]["OwnerId"].ToString());
                Assert.AreEqual("11111111.1", numberOfconsolidatedOwnersInformationForCusco[0]["OwnershipVolume"].ToString());
                Assert.AreEqual("100", numberOfconsolidatedOwnersInformationForCusco[0]["OwnershipPercentage"].ToString());

                Assert.AreEqual("30", numberOfconsolidatedOwnersInformationForMambo[0]["OwnerId"].ToString());
                Assert.AreEqual("11111111.1", numberOfconsolidatedOwnersInformationForMambo[0]["OwnershipVolume"].ToString());
                Assert.AreEqual("100", numberOfconsolidatedOwnersInformationForMambo[0]["OwnershipPercentage"].ToString());
            }
        }

        [When(@"segment has already consolidation process executed for another period")]
        public void WhenSegmentHasAlreadyConsolidationProcessExecutedForAnotherPeriod()
        {
            // Already taken care in some other stepdefinition
        }

        [Given(@"that the TRUE system is processing the operative inventories consolidation for which segment is already having consolidation for another period")]
        public async Task GivenThatTheTRUESystemIsProcessingTheOperativeInventoriesConsolidationForWhichSegmentIsAlreadyHaivngConsolidationForAnotherPeriodAsync()
        {
            this.SetValue(ConstantValues.TestdataForAnotherPeriodConsolidation, "Yes");
            await this.TheTRUESystemIsProcessingTheOperativeMovementsAndInventoriesConsolidationAsync().ConfigureAwait(false);
        }
    }
}
