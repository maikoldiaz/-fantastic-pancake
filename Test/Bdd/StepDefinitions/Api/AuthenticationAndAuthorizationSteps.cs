// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationAndAuthorizationSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class AuthenticationAndAuthorizationSteps : EcpApiStepDefinitionBase
    {
        public AuthenticationAndAuthorizationSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [When(@"I filter the nodes")]
        public async System.Threading.Tasks.Task WhenIFilterTheNodesAsync()
        {
            var entity = ConstantValues.FilterNodes;
            var content = ApiContent.Creates[ConstantValues.FilterNodes];
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide the required fields for fileregistration")]
        public async Task WhenIProvideTheRequiredFieldsForFileregistrationAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            var uploadId = Guid.NewGuid();
            content = content.JsonChangePropertyValue(ConstantValues.UploadId, uploadId.ToString());
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"the response should be successfully authenticated")]
        public void ThenTheResponseShouldBeSuccessfullyAuthenticated()
        {
            this.VerifyThat(() => Assert.IsNull(this.GetValue<dynamic>(Entities.Keys.Status)));
        }

        [When(@"I Get all node connection products")]
        public async Task WhenIGetAllNodeConnectionProductsAsync()
        {
            var apiString = SmartFormat.Smart.Format(ApiContent.Routes[ConstantValues.NodeConnectionProducts], new { connectionId = "57" });
            apiString = apiString.Replace("api", "odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegment(apiString)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update a product in the node connection")]
        public async Task WhenIUpdateAProductInTheNodeConnectionAsync()
        {
            var content = ApiContent.Updates[ConstantValues.NodeConnectionProduct];
            var nodeConnectionProductRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.NodeConnectionProduct).ConfigureAwait(false);
            content = content.JsonChangePropertyValue(ConstantValues.NodeConnectionId, nodeConnectionProductRow[ConstantValues.NodeConnectionId]);
            content = content.JsonChangePropertyValue(ConstantValues.NodeConnectionProductId, nodeConnectionProductRow[ConstantValues.NodeConnectionProductId]);
            content = content.JsonChangePropertyValue(ConstantValues.NodeConnectionProductIds, nodeConnectionProductRow[ConstantValues.ProductId]);
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.NodeConnectionProduct]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update owners of a product in the node connection")]
        public async Task WhenIUpdateOwnersOfAProductInTheNodeConnectionAsync()
        {
            var content = ApiContent.Updates[ConstantValues.NodeConnectionProductOwner];
            var nodeConnectionProductOwnerRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.NodeConnectionProductOwner).ConfigureAwait(false);
            content = content.JsonChangePropertyValue(ConstantValues.NodeConnectionProductId, nodeConnectionProductOwnerRow[ConstantValues.NodeConnectionProductId]);
            content = content.JsonChangePropertyValue(ConstantValues.NodeConnectionProductOwnerId, nodeConnectionProductOwnerRow[ConstantValues.NodeConnectionProductOwnerId]);
            content = content.JsonChangePropertyValue(ConstantValues.NodeConnectionOwnerId, nodeConnectionProductOwnerRow[ConstantValues.OwnerId]);
            content = content.JsonChangePropertyValue(ConstantValues.NodeConnectionOwnershipPercentage, "100");
            var apiString = SmartFormat.Smart.Format(ApiContent.Routes[ConstantValues.NodeConnectionProductOwner], new { productId = "2" });
            List<JObject> data = new List<JObject>();
            data.Add(JObject.Parse(content));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(apiString), data).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)""")]
        public void GivenIHave(string entity)
        {
            this.SetValue(Keys.EntityType, entity);
            this.SetValue(Keys.Route, ApiContent.Routes[entity]);
        }

        [When(@"I get unbalances")]
        public async Task WhenIGetUnbalancesAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Updates[entity];
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I associate the nodetags")]
        public async Task WhenIAssociateTheNodetagsAsync()
        {
            var content = ApiContent.Updates[ConstantValues.NodeTags];
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.NodeTags]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get ""(.*)"" by node id")]
        public async Task WhenIGetByNodeIdAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var idRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string idValue = idRow[ApiContent.Ids[entity].ToPascalCase()];
            var apiString = SmartFormat.Smart.Format(ApiContent.Routes[field], new { nodeId = idValue });
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegment(apiString)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get register files by ids")]
        public async Task WhenIGetRegisterFilesByIdsAsync()
        {
            var fileRegistrationStatusRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetFileRegistrations).ConfigureAwait(false);
            var entity = ConstantValues.FileRegistrationStatus;
            var content = fileRegistrationStatusRow[ConstantValues.UploadId];
            List<string> senTest = new List<string>();
            senTest.Add(content);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), senTest).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get pending transaction errors")]
        public async Task WhenIGetPendingTransactionErrorsAsync()
        {
            var content = ApiContent.PendingTransactions;
            var route = this.GetValue(Entities.Keys.Route);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(route), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get upload access info for blob file name")]
        public async Task WhenIGetUploadAccessInfoForBlobFileNameAsync()
        {
            var apiString = SmartFormat.Smart.Format(ApiContent.Routes["BlobFileName"], new { blobFileName = "TestDataCutOff_Daywise.xlsx" });
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegment(apiString)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get read access info")]
        public async Task WhenIGetReadAccessInfoAsync()
        {
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes["ReadAccessInfo"])).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
