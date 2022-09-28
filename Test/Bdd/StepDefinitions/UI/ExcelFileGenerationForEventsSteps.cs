// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelFileGenerationForEventsSteps.cs" company="Microsoft">
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
    using System.IO;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.DataSources;
    using global::Bdd.Core.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    public class ExcelFileGenerationForEventsSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have ownershipcalculation for segment and events information for same segment within the Range of OwnershipCalculation date")]
        public async Task GivenIWantToCalculateTheOwnershipOfInventoriesAndMovementsOfASegmentAsync()
        {
            ////this.Given("Ownership is Calculated for Segment and Ticket is Generated and \"with\" Events are processed");
            await this.CalculatedOwnershipForSegmentAndTicketGeneratedAsync("with").ConfigureAwait(false);
        }

        [Given(@"I have ownershipcalculation for segment and did not have events information for same segment within the Range of OwnershipCalculation date")]
        public async Task GivenIDidNotHaveEventsInformationForSegmentAsync()
        {
            ////this.Given("Ownership is Calculated for Segment and Ticket is Generated and \"without\" Events are processed");
            await this.CalculatedOwnershipForSegmentAndTicketGeneratedAsync("without").ConfigureAwait(false);
        }

        [Given(@"I have events information for segment but not within the range of OwnershipCalculation date")]
        public async Task GivenIHaveEventsInformationForSegmentButNotWithinTheRangeOfOwnershipCalculationDateAsync()
        {
            ////this.Given("Ownership is Calculated for Segment and Ticket is Generated and \"within the ownership calculated range none of the\" Events are processed");
            await this.CalculatedOwnershipForSegmentAndTicketGeneratedAsync("within the ownership calculated range none of the").ConfigureAwait(false);
        }

        [Then(@"excel file should be generated with the information required to calculate the ownership")]
        public async Task ThenExcelFileShouldBeGeneratedWithTheInformationRequiredToCalculateTheOwnershipAsync()
        {
            await Task.Delay(120000).ConfigureAwait(true);
            BlobStorageDataSource blobStorageDataSource = new BlobStorageDataSource();
            var blobContainer = blobStorageDataSource.Read("ownership");
            var blob = await blobStorageDataSource.Read(blobContainer, "DatosOperativosyConfiguraciones_" + this.GetValue(ConstantValues.TicketId) + ".xlsx").ConfigureAwait(false);
            await blobStorageDataSource.Download(blob, Path.Combine(FilePaths.EventsFilePath.GetFullPath(), "OperationalDataAndConfiguration.xlsx")).ConfigureAwait(false);
        }

        [Then(@"excel file should contains Events information")]
        public async Task ThenExcelFileShouldContainsEventsInformationAsync()
        {
            ////this.Then("I should see actual events infromation in the Excel generated in the Blob");
            await this.ExcelInformationOfEventsAsync().ConfigureAwait(false);
        }

        [Then(@"excel file should contains titles without information")]
        public void ThenExcelFileShouldContainsTitlesWithoutInformation()
        {
            ////this.Then("I should see headers of events in the Excel generated in the Blob");
            this.ExcelInformationWithoutEvents();
        }
    }
}
