// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelProcessingForEventsSteps.cs" company="Microsoft">
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
    using global::Bdd.Core.Web.StepDefinitions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TechTalk.SpecFlow;

    [Binding]
    public class ExcelProcessingForEventsSteps : WebStepDefinitionBase
    {
        [Then(@"I should see the ""(.*)"" ""(.*)"" in the system")]
        public async Task ThenIShouldSeeTheInTheSystemAsync(string entity, string type)
        {
            await this.IShouldSeeTheInTheSystemAsync(entity, type).ConfigureAwait(false);
        }

        [Then(@"I should see ""(.*)"" in PendingTransactions for ""(.*)""")]
        public async Task ThenIShouldSeeInPendingTransactionsForAsync(string message, string entity)
        {
            await Task.Delay(10000).ConfigureAwait(true);
            if (entity.EqualsIgnoreCase("Event"))
            {
                var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetEventPendingTransactionError).ConfigureAwait(false);
                Assert.AreEqual(message, lastRow["ErrorJson"].Split('"')[1].Trim('"'));
            }
            else if (entity.EqualsIgnoreCase("Contract"))
            {
                var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetContractPendingTransactionError).ConfigureAwait(false);
                Assert.AreEqual(message, lastRow["ErrorJson"].Split('"')[1].Trim('"'));
            }
        }
    }
}