namespace Ecp.True.Bdd.Tests.StepDefinitions.Api
{
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;
    using Flurl;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class OfficialBalanceApprovalInformationSteps : EcpApiStepDefinitionBase
    {
        public OfficialBalanceApprovalInformationSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Given(@"I have delta node to ""(.*)""")]
        public async Task GivenIHaveWithDeltasStatusAsync(string entity)
        {
            this.SetValue(Keys.EntityType, entity);
            this.SetValue(Keys.Route, ApiContent.Routes[entity]);
            var nodeDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["GetNodeWithSendForApproval"]).ConfigureAwait(false);
            this.ScenarioContext["NodeTicketId"] = nodeDetails["TicketId"].ToString();
            this.ScenarioContext["NodeId"] = nodeDetails["NodeId"].ToString();
            this.ScenarioContext["DeltaNodeId"] = nodeDetails["DeltaNodeId"].ToString();
            this.ScenarioContext["DeltaNodeName"] = nodeDetails["NodeName"].ToString();
            var approverDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["GetDeltaApproverAndRequestorPerNode"], args: new { nodeid = this.ScenarioContext["NodeId"] }).ConfigureAwait(false);
            this.ScenarioContext["Approvers"] = approverDetails["Approvers"].ToString();
            this.ScenarioContext["Editor"] = approverDetails["Editor"].ToString();
        }

        [Given(@"I have delta node to perform ""(.*)""")]
        public async Task GivenIHaveDeltaNodeToPerformAsync(string entity)
        {
            this.SetValue(Keys.EntityType, entity);
            this.SetValue(Keys.Route, ApiContent.Routes[entity]);
            var nodeDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["GetNodeWithSendForApproval"]).ConfigureAwait(false);
            this.ScenarioContext["NodeTicketId"] = nodeDetails["TicketId"].ToString();
            this.ScenarioContext["NodeId"] = nodeDetails["NodeId"].ToString();
            this.ScenarioContext["DeltaNodeId"] = nodeDetails["DeltaNodeId"].ToString();
            this.ScenarioContext["DeltaNodeName"] = nodeDetails["NodeName"].ToString();
        }

        [When(@"I get the node details with valid delta node")]
        public async Task WhenIGetTheNodeDetailsWithValidDeltaNodeAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            string value = SmartFormat.Smart.Format(ApiContent.Routes[entity], new { deltanNodeId = this.ScenarioContext["DeltaNodeId"] });
            string endPoint = this.FlowEndpoint.AppendPathSegments(value);
            await this.GivenIAmAuthenticatedForServiceAsync("sappoapi").ConfigureAwait(false);
            await this.SetResultsAsync(async () => await this.FlowGetAsync<dynamic>(endPoint).ConfigureAwait(false)).ConfigureAwait(false);
            var apiResults = this.GetValue<dynamic>(Entities.Keys.Results);
        }

        [Then(@"response should have level 1 approver and user requesting for approval")]
        public void ThenResponseShouldHaveLevelApproverAndUserRequestingForApproval()
        {
            var apiResults = this.GetValue<dynamic>(Entities.Keys.Results);
            Assert.AreEqual(apiResults["approverMail"].ToString(), this.ScenarioContext["Approvers"]);
            Assert.AreEqual(apiResults["balanceProfessionalEmail"].ToString(), this.ScenarioContext["Editor"]);
        }

        [When(@"I ""(.*)"" request with comment")]
        public async Task WhenIRejectTheRequestWithOutAsync(string status)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[status];
            string value = SmartFormat.Smart.Format(ApiContent.Routes[entity], new { deltanNodeId = this.ScenarioContext["DeltaNodeId"] });
            string endPoint = this.FlowEndpoint.AppendPathSegments(value);
            await this.GivenIAmAuthenticatedForServiceAsync("sappoapi").ConfigureAwait(false);
            content = content.JsonChangePropertyValue("comment", "Approving the Delta Node");
            await this.SetResultAsync(async () => await this.FlowPostAsync<dynamic>(endPoint, content).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I ""(.*)"" request with out comment")]
        public async Task WhenIRequestWithOutCommentAsync(string status)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[status];
            string value = SmartFormat.Smart.Format(ApiContent.Routes[entity], new { deltanNodeId = this.ScenarioContext["DeltaNodeId"] });
            string endPoint = this.FlowEndpoint.AppendPathSegments(value);
            await this.GivenIAmAuthenticatedForServiceAsync("sappoapi").ConfigureAwait(false);
            content = content.JsonRemoveObject("comment");
            await this.SetResultAsync(async () => await this.FlowPostAsync<dynamic>(endPoint, content).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"I validate node state with change date, and observation are stored")]
        public async Task ThenIValidateNodeStateWithChangeDateAndObservationIsStoredAsync()
        {
            var apiResults = this.GetValue<dynamic>(Entities.Keys.Results);
            var nodeDetails = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["GetNodeApprovalCommentAndDate"], args: new { nodeId = this.ScenarioContext["DeltaNodeId"] }).ConfigureAwait(false);
            Assert.AreEqual("Approving the Delta Node", nodeDetails["comment"].ToString());
        }
    }
}
