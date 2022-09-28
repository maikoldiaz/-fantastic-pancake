// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutoffAdjustmentsSteps.cs" company="Microsoft">
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
    public class OperationalCutoffAdjustmentsSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I want to calculate the daywise ""(.*)"" in the system")]
        public async Task GivenIWantToCalculateTheDaywiseInTheSystemAsync(string type)
        {
            ////this.Given("I want create TestData for Operational Cutoff \"TestDataCutOff_Daywise\"");
            await this.TestDataForCutOffAsync("TestDataCutOff_Daywise").ConfigureAwait(false);
            Assert.IsNotNull(type);
        }

        [Then(@"I should see the Node Interfaces/Balance Tolerance/Unidentified losses are calculated for each day")]
        public async System.Threading.Tasks.Task ThenIShouldSeeTheNodeInterfacesBalanceToleranceUnidentifiedLossesAreCalculatedForEachDayAsync()
        {
            ////this.When("I wait till file ticket processing to complete");
            await this.WaitForTicketToCompleteProcessingAsync().ConfigureAwait(false);
            var rows = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetUnabalnceByTicketId, args: new { TicketId = this.GetValue("TicketId") }).ConfigureAwait(false);
            Assert.AreEqual(ConstantValues.Count, rows.ToDictionaryList().Count);
        }
    }
}
