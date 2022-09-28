// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReviewCurrentPurchaseAndSalesContractsDailyToCreateNewMovementsSteps.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using global::Bdd.Core.Utils;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ReviewCurrentPurchaseAndSalesContractsDailyToCreateNewMovementsSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have a movement of contract identifier for same execution date of automatic process ""(.*)""")]
        [Given(@"I do not have a movement of contract identifier for same execution date of automatic process ""(.*)""")]
        public async Task GivenIHaveAmovementOfContractIdentifierForSameExecutionDateOfAutomaticProcessAsync(string frequency)
        {
            var firstWeek = "07-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[1] + "-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[2];
            var secondWeek = "15-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[1] + "-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[2];
            var thirdWeek = "21-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[1] + "-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[2];
            var lastWeek = "31-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[1] + "-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[2];
            var lastDayOfMonth = "30-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[1] + "-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[2];
            var febLastDay = "28-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[1] + "-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[2];
            var leapYearFebLastDay = "29-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[1] + "-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture).Split('-')[2];
            IDictionary<string, string> movementInforamtion = null;
            IList<IDictionary<string, object>> numberOfMovements = null;
            switch (frequency)
            {
                case "daily":
                    var contractsCount = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetMovementOfContractDaily, args: new { frequency = UIContent.Conversion[frequency] }).ConfigureAwait(false);
                    numberOfMovements = contractsCount.ToDictionaryList();
                    movementInforamtion = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementOfContractDaily, args: new { frequency = UIContent.Conversion[frequency] }).ConfigureAwait(false);
                    break;
                case "weekly":
                    contractsCount = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetMovementOfContractWeekly, args: new { frequency = UIContent.Conversion[frequency], firstWeek, secondWeek, thirdWeek, lastWeek, lastDayOfMonth, leapYearFebLastDay, febLastDay }).ConfigureAwait(false);
                    numberOfMovements = contractsCount.ToDictionaryList();
                    break;
                case "biweekly":
                    contractsCount = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetMovementOfContractBiWeekly, args: new { frequency = UIContent.Conversion[frequency], thirdWeek, lastWeek, lastDayOfMonth, leapYearFebLastDay, febLastDay }).ConfigureAwait(false);
                    numberOfMovements = contractsCount.ToDictionaryList();
                    break;
                case "monthly":
                    contractsCount = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetMovementOfContractMonthly, args: new { frequency = UIContent.Conversion[frequency], lastWeek, lastDayOfMonth, leapYearFebLastDay, febLastDay }).ConfigureAwait(false);
                    numberOfMovements = contractsCount.ToDictionaryList();
                    break;
                default:
                    break;
            }

            this.ScenarioContext["RegisteredMovementsCount"] = numberOfMovements.Count;

            if (movementInforamtion["MovementTypeId"].ToInt() == 49)
            {
                Assert.IsTrue(movementInforamtion["SourceNodeId"] == null && movementInforamtion["SourceProductId"] == null);
            }
            else if (movementInforamtion["MovementTypeId"].ToInt() == 50)
            {
                Assert.IsTrue(movementInforamtion["DestinationNodeId"] == null && movementInforamtion["DestinationProductId"] == null);
            }
        }

        [Then(@"movement should register ""(.*)""")]
        [Then(@"movement should not register ""(.*)""")]
        public void ThenMovementShouldNotRegister(string frequency)
        {
            Assert.IsNotNull(frequency);
            Assert.IsTrue(int.Parse(this.ScenarioContext["RegisteredMovementsCount"].ToString(), CultureInfo.InvariantCulture) == 1);
        }
    }
}
