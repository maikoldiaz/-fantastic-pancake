// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChaosEx.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Utils
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;

    using Flurl;

    using global::Bdd.Core.StepDefinitions;
    using global::Bdd.Core.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    public static class ChaosEx
    {
        public static async Task IHaveProcessedInTheSystemAsync(this StepDefinitionBase step, string queue)
        {
            switch (queue)
            {
                case ConstantValues.OperationalCutoffQueue:
                    await step.IHaveProcessedOperationalCutoffQueueInTheSystemAsync().ConfigureAwait(false);
                    break;
                case ConstantValues.OwnershipQueue:
                    await step.IHaveProcessedOwnershipQueueInTheSystemAsync().ConfigureAwait(false);
                    break;
                case ConstantValues.Excel:
                    await step.IHaveProcessedExcelQueueInTheSystemAsync(ConstantValues.Excel).ConfigureAwait(false);
                    break;
                case ConstantValues.ExcelContract:
                    await step.IHaveProcessedExcelQueueInTheSystemAsync(ConstantValues.Contract).ConfigureAwait(false);
                    break;
                case ConstantValues.ExcelEvent:
                    await step.IHaveProcessedExcelQueueInTheSystemAsync(ConstantValues.Events).ConfigureAwait(false);
                    break;
                case ConstantValues.SinoperInventory:
                    await step.IHaveProcessedSinoperQueueInTheSystemAsync(ConstantValues.InventoryQueueName).ConfigureAwait(false);
                    break;
                case ConstantValues.SinoperMovements:
                    await step.IHaveProcessedSinoperQueueInTheSystemAsync(ConstantValues.MovementQueueName).ConfigureAwait(false);
                    break;
                case ConstantValues.ConsolidationQueue:
                    await step.IHaveProcessedConsolidationQueueInTheSystemAsync().ConfigureAwait(false);
                    break;
                default:
                    break;
            }
        }

        public static async Task IHaveProcessedConsolidationQueueInTheSystemAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            var ticketDetails = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSegmentNodeDetailsForTicket, args: new { ticketType = 5 }).ConfigureAwait(false);
            var startDate = ticketDetails[ConstantValues.StartDate].ToDateTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
            var endDate = ticketDetails[ConstantValues.EndDate].ToDateTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
            var officialDeltaTicketRequest = ApiContent.Creates[ConstantValues.OfficialDeltaTicketRequest];
            officialDeltaTicketRequest = officialDeltaTicketRequest.JsonChangePropertyValue("Ticket startDate", startDate);
            officialDeltaTicketRequest = officialDeltaTicketRequest.JsonChangePropertyValue("Ticket endDate", endDate);
            officialDeltaTicketRequest = officialDeltaTicketRequest.JsonChangePropertyValue("Ticket categoryElementId", int.Parse(ticketDetails[ConstantValues.CategoryElementId], CultureInfo.InvariantCulture));

            ////sending request with chaos header
            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.TicketRequest]), JObject.Parse(officialDeltaTicketRequest), new System.Collections.Generic.Dictionary<string, object> { { ConstantValues.Chaos, ConstantValues.ConsolidationChaos } }).ConfigureAwait(false)).ConfigureAwait(false);

            //// we are waiting for 10 seconds to process the request and insering data into database
            await Task.Delay(10000).ConfigureAwait(true);

            ////verify whether ticket is in failed state
            var ticket = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFailedTicketBySegment, args: new { ticketType = 5, elementId = ticketDetails[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
            step.SetValueInternal(ConstantValues.TicketId, ticket[ConstantValues.TicketId]);
            step.SetValueInternal(ConstantValues.ErrorMessage, ticket[ConstantValues.ErrorMessage]);
        }

        public static async Task IHaveProcessedOperationalCutoffQueueInTheSystemAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            var ticketDetails = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSegmentNodeDetailsForTicket, args: new { ticketType = 1 }).ConfigureAwait(false);
            var startDate = ticketDetails[ConstantValues.StartDate].ToDateTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
            var endDate = ticketDetails[ConstantValues.EndDate].ToDateTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
            var operationalCutoffTicketRequest = ApiContent.Creates[ConstantValues.OperationalCutoffTicketRequest];
            operationalCutoffTicketRequest = operationalCutoffTicketRequest.JsonChangePropertyValue("Ticket startDate", startDate);
            operationalCutoffTicketRequest = operationalCutoffTicketRequest.JsonChangePropertyValue("Ticket endDate", endDate);
            operationalCutoffTicketRequest = operationalCutoffTicketRequest.JsonChangePropertyValue("Ticket categoryElementId", int.Parse(ticketDetails[ConstantValues.CategoryElementId], CultureInfo.InvariantCulture));

            ////sending request with chaos header
            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.TicketRequest]), JObject.Parse(operationalCutoffTicketRequest), new System.Collections.Generic.Dictionary<string, object> { { ConstantValues.Chaos, ConstantValues.CutoffChaosValue } }).ConfigureAwait(false)).ConfigureAwait(false);

            ////verify whether ticket is in processing state
            var ticket = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFailedTicketBySegment, args: new { ticketType = 1, elementId = ticketDetails[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
            step.SetValueInternal(ConstantValues.TicketId, ticket[ConstantValues.TicketId]);
        }

        public static async Task IHaveProcessedOwnershipQueueInTheSystemAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            var ticketDetails = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSegmentNodeDetailsForTicket, args: new { ticketType = 2 }).ConfigureAwait(false);
            var startDate = ticketDetails[ConstantValues.StartDate].ToDateTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
            var ownershipTicketRequest = ApiContent.Creates[ConstantValues.OwnershipTicketRequest];
            ownershipTicketRequest = ownershipTicketRequest.JsonChangePropertyValue("Ticket startDate", startDate);
            ownershipTicketRequest = ownershipTicketRequest.JsonChangePropertyValue("Ticket endDate", startDate);
            ownershipTicketRequest = ownershipTicketRequest.JsonChangePropertyValue("Ticket categoryElementId", int.Parse(ticketDetails[ConstantValues.CategoryElementId], CultureInfo.InvariantCulture));
            ownershipTicketRequest = ownershipTicketRequest.JsonChangePropertyValue("Ticket nodeid", int.Parse(ticketDetails[ConstantValues.NodeId], CultureInfo.InvariantCulture));

            ////sending request with chaos header
            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.TicketRequest]), JObject.Parse(ownershipTicketRequest), new System.Collections.Generic.Dictionary<string, object> { { ConstantValues.Chaos, ConstantValues.OwnershipChaosValue } }).ConfigureAwait(false)).ConfigureAwait(false);

            ////verify whether ticket is in processing state
            var ticket = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFailedTicketBySegment, args: new { ticketType = 2, elementId = ticketDetails[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
            step.SetValueInternal(ConstantValues.TicketId, ticket[ConstantValues.TicketId]);
        }

        public static async Task IHaveProcessedExcelQueueInTheSystemAsync(this StepDefinitionBase step, string queue, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            var uploadId = Guid.NewGuid().ToString();
            step.SetValueInternal(ConstantValues.UploadId, uploadId);
            var excelMovementAndInventoryRequest = ApiContent.Creates[ConstantValues.ExcelMovementAndInventoryRequest];
            excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("uploadId", uploadId);
            if (queue.ContainsIgnoreCase(ConstantValues.Excel))
            {
                await BlobExtensions.UploadExcelFileAsync("true/excel", uploadId, ConstantValues.ExcelChaosTest).ConfigureAwait(false);
                var fileRegistrationDetails = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetFileRegistrationDetailsBySystem, args: new { systemTypeId = 3 }).ConfigureAwait(false);
                var segmentId = fileRegistrationDetails[ConstantValues.SegmentId];
                excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("name", ConstantValues.ExcelChaosTest + ".xlsx");
                excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("systemTypeId", 3);
                excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("blobPath", "excel/" + uploadId);
                excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("segmentId", int.Parse(segmentId, CultureInfo.InvariantCulture));
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Contract))
            {
                await BlobExtensions.UploadExcelFileAsync("true/contract", uploadId, ConstantValues.ContractsChaosTest).ConfigureAwait(false);
                excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("name", ConstantValues.ContractsChaosTest + ".xlsx");
                excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("systemTypeId", 4);
                excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("blobPath", "contract/" + uploadId);
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Events))
            {
                await BlobExtensions.UploadExcelFileAsync("true/events", uploadId, ConstantValues.EventsChaosTest).ConfigureAwait(false);
                excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("name", ConstantValues.EventsChaosTest + ".xlsx");
                excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("systemTypeId", 5);
                excelMovementAndInventoryRequest = excelMovementAndInventoryRequest.JsonChangePropertyValue("blobPath", "events/" + uploadId);
            }

            ////sending request with chaos header
            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.ExcelMovementAndInventoryRequest]), JObject.Parse(excelMovementAndInventoryRequest), new System.Collections.Generic.Dictionary<string, object> { { ConstantValues.Chaos, ConstantValues.TransformChaosValue } }).ConfigureAwait(false)).ConfigureAwait(false);

            ////verify whether ticket is in processing state
            Assert.AreEqual(0, await step.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfProcessedFileRegistationRecords, args: new { uploadId = step.GetValueInternal(ConstantValues.UploadId) }).ConfigureAwait(false));
        }

        public static async Task IHaveProcessedSinoperQueueInTheSystemAsync(this StepDefinitionBase step, string queue, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            var uploadFileId = new Faker().Random.AlphaNumeric(35);
            step.SetValueInternal(ConstantValues.UploadId, uploadFileId);
            var serviceBusMessageForSinoper = ApiContent.Creates[ConstantValues.ServiceBusMessageForSinoper];
            serviceBusMessageForSinoper = serviceBusMessageForSinoper.JsonChangePropertyValue("UploadFileId", uploadFileId);
            string blobPath = string.Empty;
            if (queue.ContainsIgnoreCase(ConstantValues.InventoryQueueName))
            {
                blobPath = "/true/sinoper/xml/inventory/" + uploadFileId;
                ////step.Given(string.Format(CultureInfo.InvariantCulture, "I want to register an \"{0}\" in the system", queue));
                await step.IWantToRegisterAnInTheSystemAsync(queue).ConfigureAwait(false);
                ecpApiStep.UpdateXmlDefaultValue(step.GetValueInternal(Keys.EntityType));
                await BlobExtensions.UploadXmlFileAsync("true/sinoper/xml/inventory", uploadFileId, ConstantValues.InventoryQueueName).ConfigureAwait(false);
                await step.ReadSqlAsync(SqlQueries.InsertFileRegistrationForSinoper, args: new { uploadFileId, blobPath }).ConfigureAwait(false);

                ////sending request with chaos label
                await ServiceBusHelper.PutAsync(ConstantValues.InventoryQueueName, serviceBusMessageForSinoper, ConstantValues.TransformChaosValue).ConfigureAwait(false);
            }
            else
            {
                blobPath = "/true/sinoper/xml/movements/" + uploadFileId;
                ////step.Given(string.Format(CultureInfo.InvariantCulture, "I want to register an \"{0}\" in the system", queue));
                await step.IWantToRegisterAnInTheSystemAsync(queue).ConfigureAwait(false);
                ecpApiStep.UpdateXmlDefaultValue(step.GetValueInternal(Keys.EntityType));
                await BlobExtensions.UploadXmlFileAsync("true/sinoper/xml/movements", uploadFileId, ConstantValues.MovementQueueName).ConfigureAwait(false);
                await step.ReadSqlAsync(SqlQueries.InsertFileRegistrationForSinoper, args: new { uploadFileId, blobPath }).ConfigureAwait(false);

                ////sending request with chaos label
                await ServiceBusHelper.PutAsync(ConstantValues.MovementQueueName, serviceBusMessageForSinoper, ConstantValues.TransformChaosValue).ConfigureAwait(false);
            }

            ////verifying whether Sinoper record is in progress
            if (step.GetValueInternal(Keys.EntityType).ContainsIgnoreCase(ConstantValues.InventoryQueueName))
            {
                Assert.AreEqual(0, await step.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfInventoriesByInventoryIdAndNodeId, args: new { inventoryId = step.GetValueInternal(ConstantValues.InventoryId), nodeId = step.GetValueInternal(ConstantValues.SourceNode) }).ConfigureAwait(false));
            }
            else if (step.GetValueInternal(Keys.EntityType).ContainsIgnoreCase(ConstantValues.MovementQueueName))
            {
                Assert.AreEqual(0, await step.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMovementsByMovementId, args: new { movementId = step.GetValueInternal(ConstantValues.MovementId) }).ConfigureAwait(false));
            }
        }
    }
}