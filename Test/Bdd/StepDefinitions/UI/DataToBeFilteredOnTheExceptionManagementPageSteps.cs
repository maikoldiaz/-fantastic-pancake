// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataToBeFilteredOnTheExceptionManagementPageSteps.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using TechTalk.SpecFlow;

    [Binding]
    public class DataToBeFilteredOnTheExceptionManagementPageSteps : EcpWebStepDefinitionBase
    {
        [When(@"records should be displayed for selected file")]
        public async Task WhenRecordsShouldBeDisplayedForSelectedFileAsync()
        {
            var count = await this.ReadSqlScalarAsync<int>(SqlQueries.GetExceptionCountLastHour).ConfigureAwait(false);
            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            Assert.AreEqual(count, paginationCount);
        }

        [Then(@"(""(.*)"" ""(.*)"" should) not be displayed")]
        public void ThenShouldNotBeDisplayed(ElementLocator elementLocator)
        {
            if (elementLocator != null)
            {
                Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(elementLocator.Value));
            }
        }

        [Then(@"I should see the errors information in the Exceptions grid for last 40 days")]
        public async Task ThenIShouldSeeTheErrorsInformationInTheGridForLastDaysAsync()
        {
            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            var count = await this.ReadAllSqlAsync(SqlQueries.GetExceptions).ConfigureAwait(false);
            Assert.AreEqual(paginationCount, count.Count());
        }
    }
}
