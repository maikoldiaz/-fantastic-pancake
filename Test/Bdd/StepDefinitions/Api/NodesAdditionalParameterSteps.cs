// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodesAdditionalParameterSteps.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class NodesAdditionalParameterSteps : EcpApiStepDefinitionBase
    {
        private const string ControlLimit = "controlLimit";
        private const string AceptableBalancePercentage = "acceptableBalancePercentage";
        private const string APIKey = "NodeAddProperties";

        public NodesAdditionalParameterSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [When(@"I provide the Acceptable Balance Percentage and Control Limit")]
        public async Task WhenIProvideTheAcceptableBalancePercentageAndControlLimitAsync()
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            var jsonContent = dbResults.ToExpandoList().Last().ToJson();
            int nodeId = int.Parse(jsonContent.JsonGetValueforSP("NodeId"), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            string updateNodeAttribute = this.CreateUpdateNodeAttributesRequest(getNodeResponse, 39.43, 47.53);
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes["Transport Nodes"], "attributes"), JObject.Parse(updateNodeAttribute)).ConfigureAwait(false)).ConfigureAwait(false);
            this.ScenarioContext["nodeId"] = nodeId;
        }

        [When(@"I provide the uncertainity percentage")]
        public async Task WhenIProvideTheUncertainityPercentageAsync()
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            var jsonContent = dbResults.ToExpandoList().Last().ToJson();
            int nodeId = int.Parse(jsonContent.JsonGetValueforSP("NodeId"), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            string updateNodeProductUncertainity = this.CreateUpdateUnceratinityPercentageRequest(getNodeResponse, 77.22);
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes["NodeProductUncertainity"]), JObject.Parse(updateNodeProductUncertainity)).ConfigureAwait(false)).ConfigureAwait(false);
            this.ScenarioContext["nodeId"] = nodeId;
        }

        [When(@"I provide the owner data")]
        public async Task WhenIProvideTheOwnerDataAsync()
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            var jsonContent = dbResults.ToExpandoList().Last().ToJson();
            int nodeId = int.Parse(jsonContent.JsonGetValueforSP("NodeId"), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            string updateOwnerRequest = this.CreateUpdateOwnerRequest(getNodeResponse, "11", "100");
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(this.ScenarioContext["updateOwnerAPIPath"]), JArray.Parse(updateOwnerRequest)).ConfigureAwait(false)).ConfigureAwait(false);
            this.ScenarioContext["storageLocationProductId"] = getNodeResponse["nodeStorageLocations"][0]["products"][0]["storageLocationProductId"];
        }

        [When(@"I provide the owner data without ownerId")]
        public async Task WhenIProvideTheOwnerDataWithoutOwnerIdAsync()
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            var jsonContent = dbResults.ToExpandoList().Last().ToJson();
            int nodeId = int.Parse(jsonContent.JsonGetValueforSP("NodeId"), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            string updateOwnerRequest = this.CreateUpdateOwnerRequest(getNodeResponse, string.Empty, "20");
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(this.ScenarioContext["updateOwnerAPIPath"]), JArray.Parse(updateOwnerRequest)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide the owner data without ownership percentage")]
        public async Task WhenIProvideTheOwnerDataWithoutOwnershipPercentageAsync()
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            var jsonContent = dbResults.ToExpandoList().Last().ToJson();
            int nodeId = int.Parse(jsonContent.JsonGetValueforSP("NodeId"), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            string updateOwnerRequest = this.CreateUpdateOwnerRequest(getNodeResponse, "10", string.Empty);
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(this.ScenarioContext["updateOwnerAPIPath"]), JArray.Parse(updateOwnerRequest)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide the ownership value and the percentages sum of the owners list is more than (.*)%")]
        public async Task WhenIProvideTheOwnershipValueAndThePercentagesSumOfTheOwnersListIsMoreThanAsync(int p0)
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            var jsonContent = dbResults.ToExpandoList().Last().ToJson();
            int nodeId = int.Parse(jsonContent.JsonGetValueforSP("NodeId"), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            string updateOwnerRequest = this.CreateUpdateOwnerRequest(getNodeResponse, "10", (p0 + 20).ToString(CultureInfo.InvariantCulture));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(this.ScenarioContext["updateOwnerAPIPath"]), JArray.Parse(updateOwnerRequest)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide the Control Limit")]
        public async Task WhenIProvideTheControlLimitAsync()
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            var jsonContent = dbResults.ToExpandoList().Last().ToJson();
            int nodeId = int.Parse(jsonContent.JsonGetValueforSP("NodeId"), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            string updateNodeAttribute = this.CreateUpdateNodeAttributesRequest(getNodeResponse, 57.13, 0.00);
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes["Transport Nodes"], "attributes"), JObject.Parse(updateNodeAttribute)).ConfigureAwait(false);
            this.ScenarioContext["nodeId"] = nodeId;
        }

        [When(@"I provide the Acceptable Balance Percentage")]
        public async Task WhenIProvideTheAcceptableBalancePercentageAsync()
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            var jsonContent = dbResults.ToExpandoList().Last().ToJson();
            int nodeId = int.Parse(jsonContent.JsonGetValueforSP("NodeId"), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            string updateNodeAttribute = this.CreateUpdateNodeAttributesRequest(getNodeResponse, 0.00, 17.23);
            await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes["Transport Nodes"], "attributes"), JObject.Parse(updateNodeAttribute)).ConfigureAwait(false);
            this.ScenarioContext["nodeId"] = nodeId;
        }

        [Then(@"the response should succeed")]
        public void ThenTheResponseShouldSucceed()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the response should return requested additional details")]
        public async Task ThenTheResponseShouldReturnRequestedAdditionalDetailsAsync()
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            var jsonContent = dbResults.ToExpandoList().Last().ToJson();
            int nodeId = int.Parse(jsonContent.JsonGetValueforSP("NodeId"), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            Assert.IsTrue(getNodeResponse.ContainsKey(ControlLimit));
        }

        [Then(@"the response should succeed with Control Limit and Acceptable Balance Percentage updated")]
        public async Task ThenTheResponseShouldSucceedWithControlLimitAndAcceptableBalancePercentageUpdatedAsync()
        {
            int nodeId = int.Parse(this.ScenarioContext["nodeId"].ToString(), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            Assert.AreEqual("39.43", getNodeResponse[ControlLimit].ToString());
            Assert.AreEqual("47.53", getNodeResponse[AceptableBalancePercentage].ToString());
        }

        [Then(@"the response should succeed with uncertainity updated")]
        public async Task ThenTheResponseShouldSucceedWithUncertainityUpdatedAsync()
        {
            int nodeId = int.Parse(this.ScenarioContext["nodeId"].ToString(), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            Assert.AreEqual("77.22", getNodeResponse["nodeStorageLocations"][0]["products"][0]["uncertaintyPercentage"].ToString());
            ////JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
        }

        [Then(@"the response should succeed with changes with owner updated")]
        public async Task ThenTheResponseShouldSucceedWithChangesWithOwnerUpdatedAsync()
        {
            string storageLocationProductId = this.ScenarioContext["storageLocationProductId"].ToString();
            var ownershipData = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.LastCreated["GetOwnershipData"], args: new { StorageLocationProductId = storageLocationProductId }).ConfigureAwait(false);
            Console.WriteLine(ownershipData);
            var ownerId = Newtonsoft.Json.Linq.JArray.Parse(ownershipData.ToJson())[0]["OwnerId"];
            var ownershipPercentage = Newtonsoft.Json.Linq.JArray.Parse(ownershipData.ToJson())[0]["OwnershipPercentage"];
            Assert.AreEqual("11", ownerId.ToString());
            Assert.AreEqual("100", ownershipPercentage.ToString());
        }

        [Then(@"the response should succeed with Control Limit updated")]
        public async Task ThenTheResponseShouldSucceedWithControlLimitUpdatedAsync()
        {
            int nodeId = int.Parse(this.ScenarioContext["nodeId"].ToString(), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            Assert.AreEqual("57.13", getNodeResponse[ControlLimit].ToString());
        }

        [Then(@"the response should succeed with Acceptable Balance Percentage updated")]
        public async Task ThenTheResponseShouldSucceedWithAcceptableBalancePercentageUpdatedAsync()
        {
            int nodeId = int.Parse(this.ScenarioContext["nodeId"].ToString(), CultureInfo.InvariantCulture);
            string finalEndPoint = this.Endpoint + ApiContent.Routes[APIKey].Replace("{NodeId}", nodeId.ToString(CultureInfo.InvariantCulture));
            finalEndPoint = finalEndPoint.Replace("/api", "/odata");
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(finalEndPoint).ConfigureAwait(false)).ConfigureAwait(false);
            JObject getNodeResponse = this.GetValue<dynamic>(Entities.Keys.Result)["value"][0];
            Assert.AreEqual("17.23", getNodeResponse[AceptableBalancePercentage].ToString());
        }

        public string CreateUpdateNodeAttributesRequest(JObject getNodeResponse, double controlLimit, double acceptablePercentage)
        {
            string updateNodeAtrributes = ApiContent.UpdateNodeControlLimitandAcceptablePercentage;
#pragma warning disable CA1062 // Validate arguments of public methods
            updateNodeAtrributes = updateNodeAtrributes.Replace("{NodeId}", getNodeResponse["nodeId"].ToString());
            updateNodeAtrributes = updateNodeAtrributes.Replace("{Name}", getNodeResponse["name"].ToString());
            updateNodeAtrributes = updateNodeAtrributes.Replace("{Description}", getNodeResponse["description"].ToString());
            updateNodeAtrributes = updateNodeAtrributes.Replace("{NodeTypeId}", "null");
            updateNodeAtrributes = updateNodeAtrributes.Replace("{SegmentId}", "null");
            updateNodeAtrributes = updateNodeAtrributes.Replace("{OperatorId}", "null");
            updateNodeAtrributes = updateNodeAtrributes.Replace("{LogisticCenterId}", getNodeResponse["logisticCenterId"].ToString());
            updateNodeAtrributes = updateNodeAtrributes.Replace("{NodeStorageLocationName}", getNodeResponse["nodeStorageLocations"][0]["name"].ToString());
            updateNodeAtrributes = updateNodeAtrributes.Replace("{NodeStorageDescription}", getNodeResponse["nodeStorageLocations"][0]["description"].ToString());
            updateNodeAtrributes = updateNodeAtrributes.Replace("{NodeStorageLocationId}", getNodeResponse["nodeStorageLocations"][0]["storageLocationId"].ToString());
            updateNodeAtrributes = updateNodeAtrributes.Replace("{NodeStorageLocationTypeId}", getNodeResponse["nodeStorageLocations"][0]["storageLocationTypeId"].ToString());
            updateNodeAtrributes = updateNodeAtrributes.Replace("{NodeProductStorageLocationId}", getNodeResponse["nodeStorageLocations"][0]["products"][0]["nodeStorageLocationId"].ToString());
            updateNodeAtrributes = updateNodeAtrributes.Replace("{ProductId}", getNodeResponse["nodeStorageLocations"][0]["products"][0]["productId"].ToString());
            if (controlLimit == 0.00)
            {
                updateNodeAtrributes = updateNodeAtrributes.Replace("{ControlLimit}", "null");
            }
            else
            {
                updateNodeAtrributes = updateNodeAtrributes.Replace("{ControlLimit}", controlLimit.ToString(CultureInfo.InvariantCulture));
            }

            if (acceptablePercentage == 0.00)
            {
                updateNodeAtrributes = updateNodeAtrributes.Replace("{AcceptablePercentage}", "null");
            }
            else
            {
                updateNodeAtrributes = updateNodeAtrributes.Replace("{AcceptablePercentage}", acceptablePercentage.ToString(CultureInfo.InvariantCulture));
            }
#pragma warning restore CA1062 // Validate arguments of public methods
            Console.WriteLine(updateNodeAtrributes);
            Console.WriteLine(getNodeResponse);
            return updateNodeAtrributes;
        }

        public string CreateUpdateUnceratinityPercentageRequest(JObject getNodeResponse, double uncertainityPercentage)
        {
            string updateNodeProductUncertainity = ApiContent.UpdateNodeUncertaininty;
#pragma warning disable CA1062 // Validate arguments of public methods
            updateNodeProductUncertainity = updateNodeProductUncertainity.Replace("{StorageLocationProductId}", getNodeResponse["nodeStorageLocations"][0]["products"][0]["storageLocationProductId"].ToString());
            updateNodeProductUncertainity = updateNodeProductUncertainity.Replace("{ProductId}", getNodeResponse["nodeStorageLocations"][0]["products"][0]["productId"].ToString());
            updateNodeProductUncertainity = updateNodeProductUncertainity.Replace("{UncertainityPercentage}", uncertainityPercentage.ToString(CultureInfo.InvariantCulture));
#pragma warning restore CA1062 // Validate arguments of public methods
            return updateNodeProductUncertainity;
        }

        public string CreateUpdateOwnerRequest(JObject getNodeResponse, string ownerId, string ownershipPercentage)
        {
            string ownerAPIPath = ApiContent.Routes["NodeUpdateOwner"];
#pragma warning disable CA1062 // Validate arguments of public methods
            ownerAPIPath = ownerAPIPath.Replace("{storageLocationProductId}", getNodeResponse["nodeStorageLocations"][0]["products"][0]["storageLocationProductId"].ToString());
            this.ScenarioContext["updateOwnerAPIPath"] = ownerAPIPath;
            string updateOwnerRequest = ApiContent.UpdateOwnerInformation;
            updateOwnerRequest = updateOwnerRequest.Replace("{StorageLocationProductId}", getNodeResponse["nodeStorageLocations"][0]["products"][0]["storageLocationProductId"].ToString());
            if (string.IsNullOrEmpty(ownerId))
            {
                updateOwnerRequest = updateOwnerRequest.Replace("\"ownerId\":{OwnerId}", "ownerId: null");
            }
            else
            {
                updateOwnerRequest = updateOwnerRequest.Replace("{OwnerId}", ownerId);
            }

            if (string.IsNullOrEmpty(ownershipPercentage))
            {
                updateOwnerRequest = updateOwnerRequest.Replace("\"ownershipPercentage\":{OwnershipPercentage}", "ownershipPercentage: null");
            }

            updateOwnerRequest = updateOwnerRequest.Replace("{OwnershipPercentage}", ownershipPercentage);
#pragma warning restore CA1062 // Validate arguments of public methods
            return updateOwnerRequest;
        }
    }
}
