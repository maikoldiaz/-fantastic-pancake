// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessPoisonedOrDeadLetteredMessagesSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Backend
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;
    using global::Bdd.Core.DataSources;
    using global::Bdd.Core.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json.Linq;

    using TechTalk.SpecFlow;

    [Binding]
    public class ProcessPoisonedOrDeadLetteredMessagesSteps : EcpApiStepDefinitionBase
    {
        private readonly AppInsightsDataSource appinsights = new AppInsightsDataSource();

        public ProcessPoisonedOrDeadLetteredMessagesSteps(FeatureContext featureContext)
          : base(featureContext)
        {
        }

        [StepDefinition(@"I have processed ""(.*)"" in the system")]
        public async Task GivenIHaveProcessedInTheSystemAsync(string queue)
        {
            await this.IHaveProcessedInTheSystemAsync(queue).ConfigureAwait(false);
        }

        [StepDefinition(@"""(.*)"" transactions are failed")]
        [StepDefinition(@"Sap Po ""(.*)"" transactions are failed")]
        public async Task WhenTransactionsAreFailedAsync(string queue)
        {
            if (queue != null)
            {
                try
                {
                    ////waiting for function to complete retries then message will be pushed to DeadletteredMessage container
                    await Task.Delay(25000).ConfigureAwait(true);
                    IDictionary<string, string> deadLetteredMessages = null;
                    if (queue.ContainsIgnoreCase(ConstantValues.Queue))
                    {
                        deadLetteredMessages = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDeadLetteredMessages, args: new { ticketId = this.GetValue(ConstantValues.TicketId), queue = queue.Trim(ConstantValues.Queue.ToCharArray()) }).ConfigureAwait(false);
                    }
                    else if (queue.ContainsIgnoreCase(ConstantValues.InventoryQueueName))
                    {
                        deadLetteredMessages = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDeadLetteredMessagesByQueue, args: new { queue = ConstantValues.InventoryQueueName }).ConfigureAwait(false);
                    }
                    else if (queue.ContainsIgnoreCase(ConstantValues.MovementQueueName))
                    {
                        deadLetteredMessages = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDeadLetteredMessagesByQueue, args: new { queue = ConstantValues.MovementQueueName }).ConfigureAwait(false);
                    }
                    else
                    {
                        deadLetteredMessages = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDeadLetteredMessagesByQueue, args: new { queue }).ConfigureAwait(false);
                    }

                    this.SetValue(ConstantValues.DeadletteredMessageId, deadLetteredMessages[ConstantValues.DeadletteredMessageId]);
                    this.SetValue(ConstantValues.BlobPath, deadLetteredMessages[ConstantValues.BlobPath]);
                    var container = deadLetteredMessages[ConstantValues.BlobPath].Remove(deadLetteredMessages[ConstantValues.BlobPath].LastIndexOf('/'));
                    this.SetValue(ConstantValues.Contanier, container);
                    var messageId = deadLetteredMessages[ConstantValues.BlobPath].Substring(deadLetteredMessages[ConstantValues.BlobPath].LastIndexOf('/') + 1);
                    this.SetValue(ConstantValues.MessageId, messageId);
                }
                catch (ArgumentNullException)
                {
                    Logger.Info("There is no Dead-lettered message in the DeadLetteredMessage Table");
                    Assert.Fail();
                }
            }
        }

        [StepDefinition(@"blobs should not be removed from contanier")]
        [StepDefinition(@"messages are moved into deadletter queue")]
        [StepDefinition(@"poisoned or deadlettered messages are generated")]
        [StepDefinition(@"messages are copied to blob storage")]
        [StepDefinition(@"a new blob is created for exception handling")]
        public async Task ThenMessagesMovedInToDeadletterQueueAsync()
        {
            Assert.IsTrue(await BlobExtensions.GetBlobFromMessageIdAsync(this.GetValue(ConstantValues.Contanier), this.GetValue(ConstantValues.MessageId)).ConfigureAwait(false));
        }

        [StepDefinition(@"blobs should be removed from contanier after configured interval")]
        public async Task ThenBlobsShouldBeRemovedFromContanierAfterConfiguredIntervalAsync()
        {
            Assert.IsFalse(await BlobExtensions.GetBlobFromMessageIdAsync(this.GetValue(ConstantValues.Contanier), this.GetValue(ConstantValues.MessageId)).ConfigureAwait(false));
        }

        [Then(@"messages are copied to new blob storage")]
        public async Task ThenMessagesAreCopiedToNewBlobStorageAsync()
        {
            Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfDeadLetteredMessagesByBlobPath, args: new { blobPath = this.GetValue(ConstantValues.BlobPath) }).ConfigureAwait(false));
        }

        [When(@"API is triggered for message processing")]
        public async Task WhenAPIIsTriggeredForMessageProcessingAsync()
        {
            JArray deadLetterredMessages = new JArray();
            deadLetterredMessages.Add(int.Parse(this.GetValue(ConstantValues.DeadletteredMessageId), CultureInfo.InvariantCulture));

            ////sending request without chaos header so it will succeed
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.DeadletteredMessages]), deadLetterredMessages).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I trigger an API request and it is failed")]
        public async Task WhenITriggerAnAPIRequestAndItIsFailedAsync()
        {
            JArray deadLetterredMessages = new JArray();
            deadLetterredMessages.Add(int.Parse(this.GetValue(ConstantValues.DeadletteredMessageId), CultureInfo.InvariantCulture));

            ////sending request with chaos header so it will fail
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.DeadletteredMessages]), deadLetterredMessages, additionalHeaders: new System.Collections.Generic.Dictionary<string, object> { { ConstantValues.Chaos, ConstantValues.ApiChaosValue } }).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [StepDefinition(@"""(.*)"" messages should be processed successfully")]
        public async Task ThenMessagesShouldBeProcessedSuccessfullyAsync(string queue)
        {
            ////waiting for function to process deadlettered message
            await Task.Delay(20000).ConfigureAwait(true);
            Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfProcessedDeadLetteredMessages, args: new { messageId = this.GetValue(ConstantValues.DeadletteredMessageId) }).ConfigureAwait(false));

            ////verifying whether message is processed by specific queue
            await Task.Delay(20000).ConfigureAwait(true);
            if (queue.ContainsIgnoreCase(ConstantValues.Queue))
            {
                Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfProcessedTickets, args: new { ticketId = this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false));
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Contract))
            {
                Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfProcessedFileRegistationRecords, args: new { uploadId = this.GetValue(ConstantValues.UploadId) }).ConfigureAwait(false));
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Event))
            {
                Assert.AreEqual(4, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfProcessedFileRegistationRecords, args: new { uploadId = this.GetValue(ConstantValues.UploadId) }).ConfigureAwait(false));
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Excel))
            {
                await Task.Delay(5000).ConfigureAwait(true);
                Assert.AreEqual(27, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfProcessedFileRegistationRecords, args: new { uploadId = this.GetValue(ConstantValues.UploadId) }).ConfigureAwait(false));
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Json))
            {
                if (this.GetValue(Keys.EntityType).ContainsIgnoreCase(ConstantValues.Inventory))
                {
                    Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfInventoriesByInventoryId, args: new { inventoryId = this.GetValue(ConstantValues.InventoryId) }).ConfigureAwait(false));
                }
                else if (this.GetValue(Keys.EntityType).ContainsIgnoreCase(ConstantValues.Movement))
                {
                    Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMovementsByMovementId, args: new { movementId = this.GetValue(ConstantValues.MovementId) }).ConfigureAwait(false));
                }
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.SinoperInventory.Trim(ConstantValues.InventoryQueueName.ToCharArray())) || queue.ContainsIgnoreCase(ConstantValues.SinoperInventory.Trim(ConstantValues.MovementQueueName.ToCharArray())))
            {
                if (queue.ContainsIgnoreCase(ConstantValues.InventoryQueueName))
                {
                    Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfInventoriesByInventoryIdAndNodeId, args: new { inventoryId = this.GetValue(ConstantValues.InventoryId), nodeId = this.GetValue(ConstantValues.SourceNode) }).ConfigureAwait(false));
                }
                else if (queue.ContainsIgnoreCase(ConstantValues.MovementQueueName))
                {
                    Assert.AreEqual(1, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMovementsByMovementId, args: new { movementId = this.GetValue(ConstantValues.MovementId) }).ConfigureAwait(false));
                }
            }
        }

        [Then(@"multiple ""(.*)"" messages should be processed successfully")]
        public async Task ThenAllMessagesShouldBeProcessedSuccessfullyAsync(string queue)
        {
            ////verifying whether message is processed by specific queue
            await Task.Delay(20000).ConfigureAwait(true);
            if (queue.ContainsIgnoreCase(ConstantValues.Queue))
            {
                Assert.AreEqual(2, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMultipleProcessedTickets, args: new { ticketId = this.GetValue(ConstantValues.TicketId), ticketId1 = this.GetValue(ConstantValues.EntityId) }).ConfigureAwait(false));
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Contract))
            {
                Assert.AreEqual(2, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMultipleProcessedFileRegistationRecords, args: new { uploadId = this.GetValue(ConstantValues.UploadId), uploadId1 = this.GetValue(ConstantValues.EntityId) }).ConfigureAwait(false));
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Event))
            {
                Assert.AreEqual(8, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMultipleProcessedFileRegistationRecords, args: new { uploadId = this.GetValue(ConstantValues.UploadId), uploadId1 = this.GetValue(ConstantValues.EntityId) }).ConfigureAwait(false));
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Excel))
            {
                await Task.Delay(20000).ConfigureAwait(true);
                Assert.AreEqual(54, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMultipleProcessedFileRegistationRecords, args: new { uploadId = this.GetValue(ConstantValues.UploadId), uploadId1 = this.GetValue(ConstantValues.EntityId) }).ConfigureAwait(false));
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Json))
            {
                if (this.GetValue(Keys.EntityType).ContainsIgnoreCase(ConstantValues.Inventory))
                {
                    Assert.AreEqual(2, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMultipleInventoriesByInventoryId, args: new { inventoryId = this.GetValue(ConstantValues.InventoryId), inventoryId1=this.GetValue(ConstantValues.EntityId) }).ConfigureAwait(false));
                }
                else if (this.GetValue(Keys.EntityType).ContainsIgnoreCase(ConstantValues.Movement))
                {
                    Assert.AreEqual(2, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMultipleMovementsByMovementId, args: new { movementId = this.GetValue(ConstantValues.MovementId), movementId1 = this.GetValue(ConstantValues.EntityId) }).ConfigureAwait(false));
                }
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Sinoper))
            {
                if (queue.ContainsIgnoreCase(ConstantValues.InventoryQueueName))
                {
                    Assert.AreEqual(2, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMultipleInventoriesByInventoryIdAndNodeId, args: new { inventoryId = this.GetValue(ConstantValues.InventoryId), nodeId = this.GetValue(ConstantValues.SourceNode), inventoryId1 = this.GetValue(ConstantValues.EntityId) }).ConfigureAwait(false));
                }
                else if (queue.ContainsIgnoreCase(ConstantValues.MovementQueueName))
                {
                    Assert.AreEqual(2, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMultipleMovementsByMovementId, args: new { movementId = this.GetValue(ConstantValues.MovementId), movementId1 = this.GetValue(ConstantValues.EntityId) }).ConfigureAwait(false));
                }
            }
        }

        [StepDefinition(@"API is triggered for ""(.*)"" message processing with more than one blob detail")]
        public async Task WhenAPIIsTriggeredForMessageProcessingWithMoreThanOneBlobDetailAsync(string queue)
        {
            JArray deadLetterredMessages = new JArray();
            deadLetterredMessages.Add(int.Parse(this.GetValue(ConstantValues.DeadletteredMessageId), CultureInfo.InvariantCulture));
            if (queue.ContainsIgnoreCase(ConstantValues.Json))
            {
                if (queue.SplitOrDefault('_')[0].ContainsIgnoreCase(ConstantValues.Inventory))
                {
                    this.SetValue(ConstantValues.EntityId, this.GetValue(ConstantValues.InventoryId));
                }
                else
                {
                    this.SetValue(ConstantValues.EntityId, this.GetValue(ConstantValues.MovementId));
                }

                await this.GivenIHaveProcessedSapPoInTheSystemAsync(queue.SplitOrDefault('_')[0]).ConfigureAwait(false);
                await this.WhenTransactionsAreFailedAsync(queue.SplitOrDefault('_')[1]).ConfigureAwait(false);
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Sinoper))
            {
                if (queue.ContainsIgnoreCase(ConstantValues.InventoryQueueName))
                {
                    this.SetValue(ConstantValues.EntityId, this.GetValue(ConstantValues.InventoryId));
                    this.SetValue(ConstantValues.NodeId, this.GetValue(ConstantValues.SourceNode));
                    await this.IHaveProcessedSinoperQueueInTheSystemAsync(ConstantValues.InventoryQueueName).ConfigureAwait(false);
                    await this.WhenTransactionsAreFailedAsync(ConstantValues.InventoryQueueName).ConfigureAwait(false);
                }
                else
                {
                    this.SetValue(ConstantValues.EntityId, this.GetValue(ConstantValues.MovementId));
                    await this.IHaveProcessedSinoperQueueInTheSystemAsync(ConstantValues.MovementQueueName).ConfigureAwait(false);
                    await Task.Delay(5000).ConfigureAwait(true);
                    await this.WhenTransactionsAreFailedAsync(ConstantValues.MovementQueueName).ConfigureAwait(false);
                }
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Queue))
            {
                this.SetValue(ConstantValues.EntityId, this.GetValue(ConstantValues.TicketId));
                await this.GivenIHaveProcessedInTheSystemAsync(queue).ConfigureAwait(false);
                await this.WhenTransactionsAreFailedAsync(queue).ConfigureAwait(false);
            }
            else if (queue.ContainsIgnoreCase(ConstantValues.Excel))
            {
                this.SetValue(ConstantValues.EntityId, this.GetValue(ConstantValues.UploadId));
                await this.GivenIHaveProcessedInTheSystemAsync(queue).ConfigureAwait(false);
                await this.WhenTransactionsAreFailedAsync(queue).ConfigureAwait(false);
            }

            await this.ThenMessagesMovedInToDeadletterQueueAsync().ConfigureAwait(false);
            this.SetValue(ConstantValues.MessageId, deadLetterredMessages[0].ToString());
            deadLetterredMessages.Add(int.Parse(this.GetValue(ConstantValues.DeadletteredMessageId), CultureInfo.InvariantCulture));

            ////sending request without chaos header so it will succeed
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.DeadletteredMessages]), deadLetterredMessages).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"multiple messages should be processed successfully")]
        public async Task ThenMultipleMessagesShouldBeProcessedSuccessfullyAsync()
        {
            ////waiting for function to process deadlettered message
            await Task.Delay(20000).ConfigureAwait(true);
            Assert.AreEqual(2, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMultipleProcessedDeadLetteredMessages, args: new { messageId1 = this.GetValue(ConstantValues.MessageId), messageId2 = this.GetValue(ConstantValues.DeadletteredMessageId) }).ConfigureAwait(false));
        }

        [Given(@"I have processed Sap Po ""(.*)"" in the system")]
        public async Task GivenIHaveProcessedSapPoInTheSystemAsync(string entity)
        {
            this.SetValue(Keys.EntityType, entity);
            ////this.Given(string.Format(CultureInfo.InvariantCulture, "I have data to process \"{0}\" in system", entity));
            await this.GivenIHaveInventoryOrMovementDataToProcessInSystemAsync(entity).ConfigureAwait(false);
            if (entity.ContainsIgnoreCase("Movements"))
            {
                ////this.When("I have 1 movement");
                this.SetValue(ConstantValues.MovementId, new Faker().Random.Number(19999, 9999999).ToString(CultureInfo.InvariantCulture));
                this.CommonMethodForMovementRegistration(1);
            }
            else
            {
                ////this.When("I have 1 inventory");
                this.SetValue(ConstantValues.TestData, "WithScenarioId");
                this.SetValue(ConstantValues.BATCHID, new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
                this.CommonMethodForInventoryRegistration(1);
            }

            var contentJson = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)];
            await this.SetResultsAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.Replace("true", "sap").AppendPathSegment(ApiContent.Routes[entity]), JArray.Parse(contentJson.ToString()), new System.Collections.Generic.Dictionary<string, object> { { ConstantValues.Chaos, ConstantValues.TransformChaosValue } }).ConfigureAwait(false)).ConfigureAwait(false);

            ////verifying whether sap po record is in progress
            if (this.GetValue(Keys.EntityType).ContainsIgnoreCase(ConstantValues.Inventory))
            {
                Assert.AreEqual(0, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfInventoriesByInventoryId, args: new { inventoryId = this.GetValue(ConstantValues.InventoryId) }).ConfigureAwait(false));
            }
            else if (this.GetValue(Keys.EntityType).ContainsIgnoreCase(ConstantValues.Movement))
            {
                Assert.AreEqual(0, await this.ReadSqlScalarAsync<int>(SqlQueries.GetCountOfMovementsByMovementId, args: new { movementId = this.GetValue(ConstantValues.MovementId) }).ConfigureAwait(false));
            }
        }

        [Then(@"""(.*)"" is logged in app insights")]
        public async Task ThenIsLoggedInAppInsightsAsync(string message)
        {
            ////waiting for 4 min to write the message into app insights
            await Task.Delay(240000).ConfigureAwait(true);
            var result = await this.appinsights.ReadAllAsync<dynamic>(Queries.CountOfDeadLetteredMessages).ConfigureAwait(false);
            var appInsightsResult = JObject.Parse(result.ToString());
            Assert.IsTrue(appInsightsResult["tables"][0]["rows"].ToString().Contains(message), "Dead letter label is not added to the request");
        }
    }
}