// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferPointSteps.cs" company="Microsoft">
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

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class TransferPointSteps : EcpWebStepDefinitionBase
    {
        [When(@"I have inserted both operative and official movements to verify whether SAP Po got required information")]
        public async Task WhenIHaveInsertedBothOperativeAndOfficialMovementsToVerifyWhetherSAPPoGotRequiredInformationAsync()
        {
            //// to set nodes from different segments
            this.SetValue("TransferPointNodes", "True");
            //// Creating Excel homologation
            await this.IHaveHomologationDataInTheSystemAsync(systemType: "Excel").ConfigureAwait(false);
            //// Updating Excel file
            this.IHaveExcelHavingMovementsWithTransferPoint("SapPo_TransferPoint");
            //// Upload excel into system with eventType Insert
            await this.IUploadTheExcelIntoTheSystemAsync("SapPo_TransferPoint").ConfigureAwait(false);
        }

        [When(@"I have updated both operative and official movements to verify whether SAP Po got required information")]
        public async Task WhenIHaveUpdatedBothOperativeAndOfficialMovementsToVerifyWhetherSAPPoGotRequiredInformationAsync()
        {
            //// Upload excel into system with eventType update
            await this.IUploadTheExcelIntoTheSystemAsync("SapPo_TransferPoint", "Update").ConfigureAwait(false);
        }

        [When(@"I have deleted movements to verify whether SAP Po got required information")]
        public async Task WhenIHaveDeletedMovementsToVerifyWhetherSAPPoGotRequiredInformationAsync()
        {
            //// Upload excel into system with eventType delete
            await this.IUploadTheExcelIntoTheSystemAsync("SapPo_TransferPoint", "Delete").ConfigureAwait(false);
        }

        [Then(@"application must mark the operative movement as a transfer point sent to SAP PO")]
        [Then(@"application must mark the sap inserted operative movement as a transfer point sent to SAP PO")]
        [Then(@"sap inserted official movements should not be reported to SAP PO as a transfer point")]
        [Then(@"inserted official movements should not be reported to SAP PO as a transfer point")]
        public async Task ThenApplicationMustMarkTheOperativeMovementAsATransferPointSentToSAPPOAsync()
        {
            await Task.Delay(60000).ConfigureAwait(false);
            var count = await this.ReadSqlScalarAsync<int>(SqlQueries.GetSapTrackingDetails, args: new { segment = this.GetValue("CategorySegment"), action = "Insert" }).ConfigureAwait(false);
            Assert.AreEqual(1, count);
        }

        [Then(@"application must mark the updated operative movement as a transfer point sent to SAP PO")]
        [Then(@"updated official movements should not be reported to SAP PO as a transfer point")]
        [Then(@"sap updated official movements should not be reported to SAP PO as a transfer point")]
        [Then(@"application must mark the sap updated operative movement as a transfer point sent to SAP PO")]
        public async Task ThenApplicationMustMarkTheUpdatedOperativeMovementAsATransferPointSentToSAPPOAsync()
        {
            await Task.Delay(60000).ConfigureAwait(false);
            var count = await this.ReadSqlScalarAsync<int>(SqlQueries.GetSapTrackingDetails, args: new { segment = this.GetValue("CategorySegment"), action = "Update" }).ConfigureAwait(false);
            Assert.AreEqual(1, count);
        }

        [Then(@"these movements should not be reported to SAP PO as a transfer point")]
        [Then(@"sap inserted movements should not be reported to SAP PO as a transfer point")]
        public async Task ThenTheseMovementsShouldNotBeReportedToSAPPOAsATransferPointAsync()
        {
            var count = await this.ReadSqlScalarAsync<int>(SqlQueries.GetSapTrackingDetails, args: new { segment = this.GetValue("CategorySegment"), action = "Delete" }).ConfigureAwait(false);
            Assert.AreEqual(0, count);
        }
    }
}