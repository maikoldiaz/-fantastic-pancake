// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateHomologationSteps.cs" company="Microsoft">
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
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;
    using Flurl;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class UpdateHomologationSteps : EcpApiStepDefinitionBase
    {
        public UpdateHomologationSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [When(@"I delete existing ""(.*)"" from content")]
        [When(@"I update a homologation without any ""(.*)""")]
        public async Task WhenIUpdateAHomologationWithoutAnyAsync(string entity)
        {
            string content = this.ScenarioContext["HomologationData"].ToString();
            string groupId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationGroupId).ConfigureAwait(false);
            string objectId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationObjectId).ConfigureAwait(false);
            string dataMappingId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationDataMappingId).ConfigureAwait(false);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.GroupId], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps["Object HomologationGroupId"], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.HomologationObjectId], objectId);
            content = content.JsonAddField(JsonPaths.PathMaps["DataMapping HomologationGroupId"], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.HomologationDataMappingId], dataMappingId);
            content = content.JsonChangePropertyValue(entity, null);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update an existing ""(.*)""")]
        public async Task WhenIUpdateAnExistingAsync(string entity)
        {
            string content = this.ScenarioContext["HomologationData"].ToString();
            string groupId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationGroupId).ConfigureAwait(false);
            string objectId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationObjectId).ConfigureAwait(false);
            string dataMappingId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationDataMappingId).ConfigureAwait(false);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.GroupId], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps["Object HomologationGroupId"], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.HomologationObjectId], objectId);
            content = content.JsonAddField(JsonPaths.PathMaps["DataMapping HomologationGroupId"], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.HomologationDataMappingId], dataMappingId);
            if (entity == ConstantValues.HomologationObject)
            {
                bool num = bool.Parse(content.JsonGetFromValue(JsonPaths.PathMaps[ConstantValues.IsRequiredMapping]));
                content = content.JsonChangePropertyValue(JsonPaths.PathMaps[ConstantValues.IsRequiredMapping], !num);
            }
            else if (entity == ConstantValues.HomologationDataMapping)
            {
                content = content.JsonChangePropertyValue(ApiContent.Homologation[ConstantValues.Source], "random");
            }
            else if (entity == ConstantValues.HomologationGroup)
            {
                content = content.JsonChangePropertyValue(ApiContent.Homologation[ConstantValues.Source], "random");
            }

            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update a homologation with new ""(.*)""")]
        public async Task WhenIUpdateAHomologationWithNewAsync(string entity)
        {
            string content = this.ScenarioContext["HomologationData"].ToString();
            string groupId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationGroupId).ConfigureAwait(false);
            string objectId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationObjectId).ConfigureAwait(false);
            string dataMappingId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationDataMappingId).ConfigureAwait(false);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.GroupId], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps["Object HomologationGroupId"], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.HomologationObjectId], objectId);
            content = content.JsonAddField(JsonPaths.PathMaps["DataMapping HomologationGroupId"], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.HomologationDataMappingId], dataMappingId);
            if (entity == "HomologationObject")
            {
                string addData = ApiContent.HomologationObject;
                addData = addData.JsonAddField("HomologationGroupId", groupId);
                content = content.JsonAddObject("HomologationObj homologationObjects", addData);
            }
            else if (entity == "HomologationDataMapping")
            {
                string addData = ApiContent.HomologationDataMapping;
                addData = addData.JsonAddField("HomologationGroupId", groupId);
                content = content.JsonAddObject("HomologationObj homologationDataMapping", addData);
            }
            else if (entity == "HomlogationGroup")
            {
                await this.ReadAllSqlAsync(SqlQueries.DeleteAllHomologationData).ConfigureAwait(false);
                content = ApiContent.CreateHomologation;
            }

            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update existing ""(.*)""")]
        public async Task WhenIUpdateExistingAsync(string entity)
        {
            string content = this.ScenarioContext["HomologationData"].ToString();
            string groupId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationGroupId).ConfigureAwait(false);
            string objectId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationObjectId).ConfigureAwait(false);
            string dataMappingId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationDataMappingId).ConfigureAwait(false);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.GroupId], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps["Object HomologationGroupId"], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.HomologationObjectId], objectId);
            content = content.JsonAddField(JsonPaths.PathMaps["DataMapping HomologationGroupId"], groupId);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.HomologationDataMappingId], dataMappingId);
            var objectTypeId = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetHomologationTypeId).ConfigureAwait(false);

            if (entity == ConstantValues.HomologationObject)
            {
                content = content.JsonChangePropertyValue(JsonPaths.PathMaps[entity], objectTypeId[ConstantValues.HomologationObjectTypeId].ToString());
            }
            else if (entity == ConstantValues.HomologationDataMapping)
            {
                content = content.JsonChangePropertyValue(ApiContent.Homologation[ConstantValues.Source], ConstantValues.HomologationDataMapping);
            }

            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I delete existing ""(.*)""")]
        public async Task WhenIDeleteExistingAsync(string entity)
        {
            string content = this.ScenarioContext["HomologationData"].ToString();
            string groupId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationGroupId).ConfigureAwait(false);
            string objectId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationObjectId).ConfigureAwait(false);
            string dataMappingId = await this.ReadSqlScalarAsync<string>(SqlQueries.LastHomologationDataMappingId).ConfigureAwait(false);
            content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.GroupId], groupId);
            if (entity == ConstantValues.HomologationDataMapping)
            {
                content = content.JsonAddField(JsonPaths.PathMaps["Object HomologationGroupId"], groupId);
                content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.HomologationObjectId], objectId);
            }

            if (entity == ConstantValues.HomologationObject)
            {
                content = content.JsonAddField(JsonPaths.PathMaps["DataMapping HomologationGroupId"], groupId);
                content = content.JsonAddField(JsonPaths.PathMaps[ConstantValues.HomologationDataMappingId], dataMappingId);
            }

            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"the new Homologation should be registered with success message ""(.*)""")]
        public async Task ThenTheNewHomologationShouldBeRegisteredWithSuccessMessageAsync(string entity)
        {
            var result = this.GetValue<dynamic>(Entities.Keys.Result);
            var response = result as HttpResponseMessage;
            var jsonObject = response != null ? JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false)) : result;
            this.VerifyThat(() => Assert.AreEqual(entity, jsonObject["message"].ToString()));
        }

        [When(@"I update a homologation with ""(.*)"" that contains any alphabet")]
        [When(@"I update a homologation with ""(.*)"" that exceeds 100 characters")]
        public async Task WhenIUpdateAHomologationWithThatExceedsCharactersAsync(string entity)
        {
            string errString = "El valor en el sistema origen puede contener máximo 100 caracteresEl valor en el sistema origen puede contener máximo 100 caracteres";
            var content = this.ScenarioContext["HomologationData"].ToString();
            content = content.JsonChangePropertyValue(entity, errString);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
