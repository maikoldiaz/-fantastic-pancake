namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class InvokeOperationalInformationWithOwnershipSteps : EcpWebStepDefinitionBase
    {
        /// <summary>
        /// Approves or Rejects a Manual approval request on the 'Aprobación del balance con propiedad por nodo' page.
        /// </summary>
        /// <param name="approveOrReject">determines whether to approve or reject a manual request.</param>
        [When(@"I ""(.*)"" the respective request")]
        public void WhenITheRespectiveRequest(string approveOrReject)
        {
            switch (approveOrReject)
            {
                case "approve":
                    this.DriverContext.Driver.SwitchTo().Frame("widgetIFrame");
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ManualApprovalOfOwnershipInFrame), formatArgs: this.ScenarioContext["NodeNameForRejection"].ToString());
                    this.Get<ElementPage>().Click(nameof(Resources.ManualApprovalOfOwnershipInFrame), formatArgs: this.ScenarioContext["NodeNameForRejection"].ToString());
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ManualApprovalRejectionConfirmButtonInFrame));
                    this.Get<ElementPage>().Click(nameof(Resources.ManualApprovalRejectionConfirmButtonInFrame));
                    this.DriverContext.Driver.SwitchTo().DefaultContent();
                    break;
                case "reject":
                    this.DriverContext.Driver.SwitchTo().Frame("widgetIFrame");
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ManualRejectionOfOwnershipInFrame), formatArgs: this.ScenarioContext["NodeNameForRejection"].ToString());
                    this.Get<ElementPage>().Click(nameof(Resources.ManualRejectionOfOwnershipInFrame), formatArgs: this.ScenarioContext["NodeNameForRejection"].ToString());
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ManualApprovalRejectionConfirmButtonInFrame));
                    this.Get<ElementPage>().Click(nameof(Resources.ManualApprovalRejectionConfirmButtonInFrame));
                    this.DriverContext.Driver.SwitchTo().DefaultContent();
                    break;
            }
        }

        /// <summary>
        /// Navigate to the Node details page of one of the nodes created as part of pre-requisite.
        /// </summary>
        /// <param name="approvalType">Determines if the approval type is either automatic or manual.</param>
        /// <returns>returns nothing.</returns>
        [When(@"I search for a node and a raise a ""(.*)"" approval request against it")]
        public async Task WhenISearchForANodeAndARaiseAApprovalRequestAgainstItAsync(string approvalType)
        {
            await this.ISelectANodeToRaiseAnApprovalAsync(this.GetValue("TicketId"), approvalType).ConfigureAwait(false);
        }

        /// <summary>
        /// Validate if the required ownership process' approval/rejection request ended without any failure
        /// Once the above process ends successfully, also validate if the analytical model process ended successfully in case of approval request is approved.
        /// </summary>
        /// <param name="invokeStatus">determines whether to check if the analytical model is invoked or not.</param>
        /// <returns>returns a boolean value based on the success/failure of the validation.</returns>
        [Then(@"I validate that the TRUE system ""(.*)"" the process for operational data with ownership loading in the analytical model")]
        public async Task ThenIValidateThatTheTRUESystemTheProcessForOperationalDataWithOwnershipLoadingInTheAnalyticalModelAsync(string invokeStatus)
        {
            List<string> expectedOwnershipDetails = new List<string>();

            //// Get the ownership node details (i.e OwnershipStatusId, OwnershipAnalyticsStatus, OwnershipAnalyticsErrorMessage) from the 'OwnershipNode' data table
            var tabledataOfOwnershipNodes = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOwnershipDetails, args: new { ownershipNodeId = this.ScenarioContext["CurrentOwnershipNodeId"] }).ConfigureAwait(false);
            var tableDataAsDict = tabledataOfOwnershipNodes.ToDictionaryList();

            List<string> ownershipDetailsFromDb = new List<string>();
            //// Retrive the value of 'OwnershipStatusId' column from Db
            ownershipDetailsFromDb.Add(tableDataAsDict[0]["OwnershipStatusId"].ToString());

            switch (invokeStatus)
            {
                //// Manual/Automatic approval successful. Analytical model process ended successfully
                case "invoked":
                    expectedOwnershipDetails.Add("9");
                    expectedOwnershipDetails.Add("1");
                    expectedOwnershipDetails.Add("NULL");

                    //// Retrive the value of 'OwnershipAnalyticsStatus' column from Db
                    ownershipDetailsFromDb.Add(tableDataAsDict[0]["OwnershipAnalyticsStatus"].ToString());
                    //// Retrive the value of 'OwnershipAnalyticsErrorMessage' column from Db
                    if (tableDataAsDict[0]["OwnershipAnalyticsErrorMessage"] == null)
                    {
                        ownershipDetailsFromDb.Add("NULL");
                    }

                    break;
                //// Manual Approval Rejected
                case "didn't invoke":
                    expectedOwnershipDetails.Add("8");
                    expectedOwnershipDetails.Add("NULL");
                    expectedOwnershipDetails.Add("NULL");

                    //// Retrive the value of 'OwnershipAnalyticsStatus' column from Db
                    if (tableDataAsDict[0]["OwnershipAnalyticsStatus"] == null)
                    {
                        ownershipDetailsFromDb.Add("NULL");
                    }
                    //// Retrive the value of 'OwnershipAnalyticsErrorMessage' column from Db
                    if (tableDataAsDict[0]["OwnershipAnalyticsErrorMessage"] == null)
                    {
                        ownershipDetailsFromDb.Add("NULL");
                    }

                    break;
            }

            //// Print the values from the expected and actual lists on concole for logs
            for (int i = 0; i < expectedOwnershipDetails.Count; i++)
            {
                Console.WriteLine("Expected value from Db : " + expectedOwnershipDetails[i] + "; Actual value in Db : " + ownershipDetailsFromDb[i]);
            }

            Assert.IsTrue(expectedOwnershipDetails.SequenceEqual(ownershipDetailsFromDb, StringComparer.OrdinalIgnoreCase), "Validation of approval/rejection of a node with OwnershipNodeId '" + this.GetValue("CurrentOwnershipNodeId") + "' failed");
        }
    }
}
