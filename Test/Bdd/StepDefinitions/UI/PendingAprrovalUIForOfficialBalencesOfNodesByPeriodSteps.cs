namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class PendingAprrovalUIForOfficialBalencesOfNodesByPeriodSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have delta node with official balance and Level 1 approver is not configured")]
        public async Task WhenIHaveDeltaNodeWithOfficialBalanceAndLevelApproverIsNotConfiguredAsync()
        {
            var nodeDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNodeWithDeltas).ConfigureAwait(false);
            this.ScenarioContext["NodeTicketId"] = nodeDetails["TicketId"].ToString();
            this.ScenarioContext["DeltaNodeId"] = nodeDetails["NodeId"].ToString();
            this.ScenarioContext["DeltaNodeName"] = nodeDetails["NodeName"].ToString();

            await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.UpdateL1ApproverValue, args: new { nodeId = this.ScenarioContext["DeltaNodeId"], email = string.Empty }).ConfigureAwait(false);
        }

        [StepDefinition(@"I have delta node with official balance and Level 1 approver is configured")]
        public async Task GivenIHaveDeltaNodeWithOfficialBalanceAndLevelApproverIsConfiguredAsync()
        {
            var nodeDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNodeWithDeltas).ConfigureAwait(false);
            this.ScenarioContext["NodeTicketId"] = nodeDetails["TicketId"].ToString();
            this.ScenarioContext["DeltaNodeId"] = nodeDetails["NodeId"].ToString();
            this.ScenarioContext["DeltaNodeName"] = nodeDetails["NodeName"].ToString();
            this.ScenarioContext["NodeNameForRejection"] = nodeDetails["NodeName"].ToString();

            await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.UpdateL1ApproverValue, args: new { nodeId = this.ScenarioContext["DeltaNodeId"], email = "trueadmin@ecopetrol.com.co;trueapprover@ecopetrol.com.co" }).ConfigureAwait(false);
        }

        [Then(@"I select node from the grid")]
        public void ThenISelectNodeFromTheGrid()
        {
            this.Get<ElementPage>().SetValue(nameof(Resources.TicketInDeltasPerNodeGrid), this.ScenarioContext["NodeTicketId"].ToString());
            this.Get<ElementPage>().SendEnterKey(nameof(Resources.TicketInDeltasPerNodeGrid));
        }

        [When(@"I open the approval flow of the respective node")]
        public void WhenIClickOnTileForTheRespectiveNode()
        {
            this.DriverContext.Driver.SwitchTo().Frame("widgetIFrame");
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ManualApprovalOfOwnershipInFrame), formatArgs: this.ScenarioContext["NodeNameForRejection"].ToString());
            this.Get<ElementPage>().Click(nameof(Resources.ManualApprovalOfOwnershipInFrame), formatArgs: this.ScenarioContext["NodeNameForRejection"].ToString());
        }

        [StepDefinition(@"I click on ""(.*)"" link and validate report is opened in new tab")]
        public void ThenIClickOnLinkValidateReportIsOpenedInNewTab(string reportName)
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ManuaApprovalFlowReportLink), formatArgs: reportName);
            this.Get<ElementPage>().Click(nameof(Resources.ManuaApprovalFlowReportLink), formatArgs: reportName);
            var currentDriver = this.DriverContext.Driver;
            var tabs = currentDriver.WindowHandles;
            currentDriver.SwitchTo().Window(tabs[1]);
            this.IShouldSeeBreadcrumb(reportName);
            currentDriver.Close();
            currentDriver.SwitchTo().Window(tabs[0]);
        }

        [Then(@"I confirm approve or reject the request")]
        public void ThenIConfirmApproveOrRejectTheRequest()
        {
            this.DriverContext.Driver.SwitchTo().Frame("widgetIFrame");
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ManualApprovalRejectionConfirmButtonInFrame));
            this.Get<ElementPage>().Click(nameof(Resources.ManualApprovalRejectionConfirmButtonInFrame));
            this.DriverContext.Driver.SwitchTo().DefaultContent();
        }

        [Then(@"I validate node status is ""(.*)""")]
        public async Task ThenIValidateNodeStatusIsAsync(string status)
        {
            var actualStatus = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNodeStatus, args: new { nodeid = this.ScenarioContext["DeltaNodeId"] }).ConfigureAwait(false);
            Assert.AreEqual(status, actualStatus.ToString());
        }
    }
}
