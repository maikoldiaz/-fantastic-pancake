// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateNodesSteps.cs" company="Microsoft">
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
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;
    using Flurl.Http;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class CreateNodesSteps : EcpApiStepDefinitionBase
    {
        public CreateNodesSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [StepDefinition(@"I have ""(.*)"" in the system")]
        public async Task GivenIHaveInTheSystemAsync(string entity)
        {
            if (this.UserDetails.Role == "admin" || this.UserDetails.Role == "administrador")
            {
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            }

            this.SetValue(Keys.EntityType, entity);
            this.SetValue(Keys.Route, ApiContent.Routes[entity]);
            if (!entity.EqualsIgnoreCase(ConstantValues.LogisticCenters) && !entity.EqualsIgnoreCase(ConstantValues.StorageLocations) && !entity.ContainsIgnoreCase(ConstantValues.Ticket)
                && !entity.EqualsIgnoreCase(ConstantValues.Rules) && !entity.EqualsIgnoreCase(ConstantValues.Products) && !entity.EqualsIgnoreCase(ConstantValues.NodeTags) && !entity.EqualsIgnoreCase(ConstantValues.FileRegistration) && !entity.EqualsIgnoreCase(ConstantValues.Scenarios))
            {
                var createContent = entity.EqualsIgnoreCase(ConstantValues.HomologationWithTwoGroups) ? ApiContent.Creates[ConstantValues.Homologations] : ApiContent.Creates[entity];
                if (entity.EqualsIgnoreCase(ConstantValues.Homologations) || entity.EqualsIgnoreCase(ConstantValues.HomologationWithTwoGroups) || entity.EqualsIgnoreCase(ConstantValues.HomologationGroup))
                {
                    try
                    {
                        var homologationRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetHomologationBySourceAndDestination).ConfigureAwait(false);
                        this.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                        var homologationGroup = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                        foreach (var homologationGroupRow in homologationGroup)
                        {
                            var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                            this.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                            await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = this.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                        }

                        await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                    }
                    catch (ArgumentNullException ex)
                    {
                        Assert.IsNotNull(ex);
                    }

                    await this.ReadAllSqlAsync(SqlQueries.DeleteAllHomologationData).ConfigureAwait(false);
                    var sourcevalue = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["Nodes"]).ConfigureAwait(false);
                    createContent = createContent.JsonChangePropertyValue(ApiContent.Homologation[ConstantValues.Destination], sourcevalue[ApiContent.Ids["Node"].ToPascalCase()]);
                    this.ScenarioContext["ControlLimit"] = string.Empty;
                    this.ScenarioContext["HomologationData"] = createContent;
                }
                else
                {
                    createContent = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateSourceAndDestinationNodesAsync(createContent).ConfigureAwait(false) : createContent.JsonChangeValue();
                    createContent = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(createContent).ConfigureAwait(false) : createContent;
                    this.ScenarioContext["ControlLimit"] = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? createContent.JsonGetValue("controlLimit") : string.Empty;
                }

                this.ScenarioContext["NodeName"] = entity.EqualsIgnoreCase(ConstantValues.Nodes) ? createContent.JsonGetValue("name") : string.Empty;
                this.ScenarioContext["CategoryName"] = entity.EqualsIgnoreCase("Transport Category") ? createContent.JsonGetValue("name") : string.Empty;
                this.ScenarioContext["ControlLimit"] = entity.EqualsIgnoreCase(ConstantValues.Nodes) ? ConstantValues.ValidValue : this.ScenarioContext["ControlLimit"];
                this.ScenarioContext["AcceptableBalancePercentage"] = entity.EqualsIgnoreCase(ConstantValues.Nodes) ? ConstantValues.ValidValue : string.Empty;
                createContent = entity.EqualsIgnoreCase(ConstantValues.Nodes) || entity.EqualsIgnoreCase(ConstantValues.Node) ? await this.NodeContentCreationAsync(createContent).ConfigureAwait(false) : createContent;
                await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(createContent)).ConfigureAwait(false)).ConfigureAwait(false);
                if (entity.EqualsIgnoreCase(ConstantValues.HomologationWithTwoGroups))
                {
                    createContent = ApiContent.Creates[entity];
                    await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(createContent)).ConfigureAwait(false)).ConfigureAwait(false);
                }

                var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
                this.ScenarioContext[ApiContent.Ids[entity].ToPascalCase()] = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
                if (entity.EqualsIgnoreCase(ConstantValues.HomologationWithTwoGroups) || entity.EqualsIgnoreCase(ConstantValues.Homologations))
                {
                  lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.HomologationWithTwoGroups]).ConfigureAwait(false);
                  this.SetValue(Keys.HomologationGroupId, lastCreatedRow[ConstantValues.HomologationGroupId]);
                  lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastHomologation).ConfigureAwait(false);
                  this.SetValue(Keys.HomologationId, lastCreatedRow[ApiContent.Ids[ConstantValues.Homologations].ToPascalCase()]);
                }
                ////this.ScenarioContext.Add("CategoryElementName", lastCreatedRow["CategoryId"].EqualsIgnoreCase("10") ? lastCreatedRow["Name"] : string.Empty);
            }

            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
        }

        [When(@"I provide ""(.*)"" value as ""(.*)"" and without ""(.*)""")]
        public async Task WhenIProvideValueAsAndWithoutAsync(string field1, string field1Value, string field2)
        {
            await this.CreateEntityWithoutFieldAsync(field2, field1, field1Value).ConfigureAwait(false);
        }

        [When(@"I don't provide any ""(.*)""")]
        public async Task WhenIDonTProvideAnyAssignedAsync(string field)
        {
            await this.CreateEntityWithoutFieldAsync(field).ConfigureAwait(false);
        }

        [When(@"I verify the existence of ""(.*)""")]
        public async Task WhenIVerifyTheExistenceOfAsync(string field)
        {
           var entity = this.GetValue(Entities.Keys.EntityType);
           var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
           var updatedString = string.Empty;
           HttpResponseMessage output = null;
           if (field != null && field.Contains(ConstantValues.StorageLocation))
            {
                string idValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
                var split = field?.Split(' ');
                var parent = split.FirstOrDefault();
                var child = split.LastOrDefault();
                var correspondingRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[parent], args: new { nodeId = idValue }).ConfigureAwait(false);
                var rowFieldValue = correspondingRow[child.ToPascalCase()];
                updatedString = SmartFormat.Smart.Format(ApiContent.Routes[field], new { nodeId = idValue, StorageLocationName = rowFieldValue });
            }
            else
            {
                string idValue = field.Contains(ConstantValues.Element) ? lastCreatedRow[ConstantValues.CategoryId] : string.Empty;
                string name = lastCreatedRow[ConstantValues.Name];
                if (field.Contains(ConstantValues.Node))
                {
                    updatedString = SmartFormat.Smart.Format(ApiContent.Routes[field], new { nodeName = name });
                }
                else
                {
                    updatedString = field.Contains(ConstantValues.Element) ? SmartFormat.Smart.Format(ApiContent.Routes[field], new { categoryId = idValue, elementName = name }) : SmartFormat.Smart.Format(ApiContent.Routes[field], new { categoryName = name });
                }
            }

           try
            {
                output = await this.ApiExecutor.GetResponseAsync(this.Endpoint.AppendPathSegment(updatedString), this.UserDetails).ConfigureAwait(false);
                this.ScenarioContext[Entities.Keys.Status] = output.StatusCode.ToString();
            }
            catch (FlurlHttpException ex)
            {
                this.ScenarioContext[Entities.Keys.Status] = ex.Call.Response.StatusCode.ToString();
            }
        }

        [Then(@"the response should be successful")]
        public async Task ThenTheResponseShouldBeSuccessfulAsync()
        {
            await ADataFactoryClient.RunADFPipelineAsync("ADF_OperativeMovementsWithOwnership").ConfigureAwait(false);
            this.VerifyThat(() => Assert.AreEqual("OK", this.GetValue<dynamic>(Entities.Keys.Status)));
        }

        protected async Task CreateEntityWithoutFieldAsync(string field2, string field1 = "name", string field1Value = "_Mod_")
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            if (field2 != null)
            {
                content = field2.Contains(ConstantValues.Homologation) ? content : content.JsonChangeValue();
            }

            content = field2.Contains(ConstantValues.Homologation) ? content : content.JsonChangePropertyValue(field1, field1Value);
            content = content.JsonChangePropertyValue(field2);
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}