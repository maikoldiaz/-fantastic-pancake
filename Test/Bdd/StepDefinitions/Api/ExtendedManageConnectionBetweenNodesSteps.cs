// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedManageConnectionBetweenNodesSteps.cs" company="Microsoft">
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
    public class ExtendedManageConnectionBetweenNodesSteps : EcpApiStepDefinitionBase
    {
        public ExtendedManageConnectionBetweenNodesSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [When(@"I provide the required fields when sum of the ""(.*)"" list equal to (.*)")]
        public async Task WhenIProvideTheRequiredFieldsWhenSumOfTheListEqualToAsync(string field, int total)
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count() + 1);
            this.SetValue(Entities.Keys.InitialRowCount, total == 100 ? dbResults.Count() + 1 : dbResults.Count());
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateSourceAndDestinationNodesAsync(content).ConfigureAwait(false) : content.JsonChangeValue();
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(content).ConfigureAwait(false) : content;
            int split = total / 2;
            content = content.JsonChangePropertyValue("Product_1-Owner_1 " + field, split.ToString(CultureInfo.InvariantCulture));
            content = content.JsonChangePropertyValue("Product_1-Owner_2 " + field, split.ToString(CultureInfo.InvariantCulture));
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update a record when sum of the ""(.*)"" list equal to (.*)")]
        public async Task WhenIUpdateARecordWhenSumOfTheListEqualToAsync(string field, int total)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var updateNodeConnectionProductOwners = ApiContent.Updates["UpdateNodeConnectionProductOwners"];
            updateNodeConnectionProductOwners = await this.UpdateRequestJsonIdsAsync(updateNodeConnectionProductOwners, "UpdateNodeConnectionProductOwners").ConfigureAwait(false);
            int split = total / 2;
            updateNodeConnectionProductOwners = updateNodeConnectionProductOwners.JArrayChangePropertyValue("Owner_1 " + field, split.ToString(CultureInfo.InvariantCulture));
            updateNodeConnectionProductOwners = updateNodeConnectionProductOwners.JArrayChangePropertyValue("Owner_2 " + field, split.ToString(CultureInfo.InvariantCulture));
            ////updateNodeConnectionProductOwners = updateNodeConnectionProductOwners.Replace("\"{", "{").Replace("}\"", "}");
            ////this.LogToReport(JToken.Parse(updateNodeConnectionProductOwners));
            var lastCreatedNodeConnection = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            var nodeConnectionId = lastCreatedNodeConnection[ApiContent.Ids[entity].ToPascalCase()];
            var lastCreatedNodeConnectionProduct = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["NodeConnectionProductByNodeConnnectionID"], args: new { nodeConnectionId = nodeConnectionId }).ConfigureAwait(false);
            var nodeConnectionProductId = lastCreatedNodeConnectionProduct["NodeConnectionProductId"];
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], "/products/" + nodeConnectionProductId + "/owners"), JArray.Parse(updateNodeConnectionProductOwners)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)"" with (.*) products in the system")]
        public async Task GivenIHaveWithProductsInTheSystemAsync(string entity, int total)
        {
            if (this.UserDetails.Role == "admin" || this.UserDetails.Role == "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            }

            this.SetValue(Keys.EntityType, entity);
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count() + 1);
            var content = ApiContent.CreateConnectionWithFourProducts;
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateSourceAndDestinationNodesAsync(content).ConfigureAwait(false) : content.JsonChangeValue();
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(content).ConfigureAwait(false) : content;
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(content, "Product_2").ConfigureAwait(false) : content;
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(content, "Product_3").ConfigureAwait(false) : content;
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(content, "Product_4").ConfigureAwait(false) : content;
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
            await this.ProductCountAsync(entity, total).ConfigureAwait(false);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            this.ScenarioContext[ApiContent.Ids[entity].ToPascalCase()] = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
        }

        [Then(@"Validate (.*) ""(.*)"" are created")]
        public async Task ThenValidateAreCreatedAsync(int total, string field)
        {
            Assert.NotNull(field);
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.ProductCountAsync(entity, total).ConfigureAwait(false);
        }

        [When(@"I update a connection to add one product")]
        public async Task WhenIUpdateAConnectionToAddOneProductAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var updateContent = ApiContent.UpdateConnectionWithFiveProducts;
            updateContent = await this.UpdateRequestJsonIdsAsync(updateContent, entity).ConfigureAwait(false);
            updateContent = this.UpdateProductOwners(updateContent, "Product_2");
            updateContent = this.UpdateProductOwners(updateContent, "Product_3");
            updateContent = this.UpdateProductOwners(updateContent, "Product_4");
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update a connection to delete one product")]
        public async Task WhenIUpdateAConnectionToDeleteOneProductAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var updateContent = ApiContent.UpdateConnectionWithFourProducts;
            updateContent = await this.UpdateRequestJsonIdsAsync(updateContent, entity).ConfigureAwait(false);
            updateContent = this.UpdateProductOwners(updateContent, "Product_2");
            updateContent = this.UpdateProductOwners(updateContent, "Product_3");
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"Validate (.*) ""(.*)"" are active")]
        public async Task ThenValidateAreActiveAsync(int total, string field)
        {
            await this.ThenValidateAreCreatedAsync(total, field).ConfigureAwait(false);
        }

        public async Task ProductCountAsync(string entity, int total)
        {
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            this.ScenarioContext[ApiContent.Ids[entity]] = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            var nodeConnectionProductRow = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetNodeConnectionProductById, args: new { nodeConnectionId = this.ScenarioContext[ApiContent.Ids[entity]] }).ConfigureAwait(false);
            this.SetValue<dynamic>("ProductCount", nodeConnectionProductRow.Count());
            Assert.AreEqual(total, this.GetValue<dynamic>("ProductCount"));
        }
    }
}
