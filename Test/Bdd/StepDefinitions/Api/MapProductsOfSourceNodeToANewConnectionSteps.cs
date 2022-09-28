// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapProductsOfSourceNodeToANewConnectionSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class MapProductsOfSourceNodeToANewConnectionSteps : EcpApiStepDefinitionBase
    {
        public MapProductsOfSourceNodeToANewConnectionSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Given(@"I have a source node which has ""(.*)"" set up")]
        public void GivenIHaveASourceNodeWhichHasSetUp(string value)
        {
            Assert.IsNotNull(value);
        }

        [Then(@"the products associated with the ""(.*)"" on the source node should be added to the connection")]
        public async Task ThenTheProductsAssociatedWithTheOnTheSourceNodeShouldBeAddedToTheConnectionAsync(string field)
        {
            if (!field.ContainsIgnoreCase(ConstantValues.StorageLocations))
            {
                var connectionRow = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetConnectionProducts, args: new { sourceNodeId = this.GetValue(Keys.SelectedValue) }).ConfigureAwait(false);
                Assert.AreEqual(ConstantValues.SourceNodeProductId, connectionRow[ConstantValues.ProductId]);
            }
            else
            {
                var connectionRow = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetConnectionProducts, args: new { sourceNodeId = this.GetValue(Keys.SelectedValue) }).ConfigureAwait(false);
                var connectionRowList = connectionRow.ToDictionaryList();
                Assert.AreEqual(ConstantValues.SourceNodeProductId, connectionRowList[0][ConstantValues.ProductId]);
                Assert.AreEqual(ConstantValues.SourceNodeProductId2, connectionRowList[1][ConstantValues.ProductId]);
            }
        }

        [When(@"I update a node with new storage location")]
        public async Task WhenIUpdateANodeWithNewStorageLocationAsync()
        {
            var entity = ConstantValues.Node;
            ////var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            ////string fieldValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            var updateContent = ApiContent.Updates[entity].Replace("^^placeholder^^", ApiContent.AddStorageLocation);
            this.ScenarioContext["EntityId"] = this.GetValue(Keys.SelectedValue);
            updateContent = updateContent.JsonChangePropertyValue(ApiContent.Ids[entity], this.GetValue(Keys.SelectedValue));
            updateContent = updateContent.JsonChangeValue("name", "_Mod_");
            if (entity.ContainsIgnoreCase(ConstantValues.Node))
            {
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.StorageLocationNodeId, this.GetValue(Keys.SelectedValue));
                var lastCreatedStorageLocation = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.StorageLocations], args: new { nodeId = this.GetValue(Keys.SelectedValue) }).ConfigureAwait(false);
                string nodeStorageLocationId = lastCreatedStorageLocation["NodeStorageLocationId"];
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.NodeStorageLocationId, nodeStorageLocationId);
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.ProductLocationNodeStorageLocationId, nodeStorageLocationId);
                updateContent = updateContent.JsonChangePropertyValue(ConstantValues.NewStorageLocationNodeId, this.GetValue(Keys.SelectedValue));
            }

            this.SetValue(Keys.RandomFieldValue, string.Concat($"Automation{"_"}", new Faker().Random.AlphaNumeric(5)));
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.NewStorageLocationName, this.GetValue(Keys.RandomFieldValue));
            updateContent = entity.EqualsIgnoreCase(ConstantValues.Node) || entity.EqualsIgnoreCase(ConstantValues.Nodes) ? await this.UpdateContentForNodeAsync(updateContent).ConfigureAwait(false) : updateContent;
            updateContent = entity.EqualsIgnoreCase(ConstantValues.Node) || entity.EqualsIgnoreCase(ConstantValues.Nodes) ? updateContent.JsonChangePropertyValue(ConstantValues.NewStorageLocationStorageLocationTypeId, this.ScenarioContext["StorageLocationTypeId"].ToString()) : updateContent;
            this.LogToReport(JToken.Parse(updateContent));
            this.SetValue(Entities.Keys.Result, await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false));
        }
    }
}
