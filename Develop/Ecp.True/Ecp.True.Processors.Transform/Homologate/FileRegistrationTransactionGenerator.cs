// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationTransactionGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Transform.Homologate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Transform.Homologate.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The file registration transaction generator.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Transform.Homologate.FileRegistrationTransactionGenerator" />
    public class FileRegistrationTransactionGenerator : IFileRegistrationTransactionGenerator
    {
        /// <summary>
        /// The movement ID.
        /// </summary>
        private readonly string movementId = "MovementId";

        /// <summary>
        /// The movement ID.
        /// </summary>
        private readonly string blobPath = "BlobPath";

        /// <summary>
        /// The event ID.
        /// </summary>
        private readonly string sessionId = "SessionId";

        /// <summary>
        /// Updates the file registration transactions.
        /// </summary>
        /// <param name="trueMessage">The true message.</param>
        /// <param name="token">The token.</param>
        public void UpdateFileRegistrationTransactions(TrueMessage trueMessage, JArray token)
        {
            ArgumentValidators.ThrowIfNull(trueMessage, nameof(trueMessage));
            ArgumentValidators.ThrowIfNull(token, nameof(token));
            var fileRegistrationTransactions = new List<FileRegistrationTransaction>();

            // Add valid records
            token
                .GroupBy(x => x[Constants.MessageId].ToString())
                .ForEach(
                    group =>
                    {
                        var groupId = 1;
                        group?.ForEach(item =>
                        {
                            var result = this.GetBlobBasePath(item, trueMessage);
                            var blobName = $"{result.Item2}_{trueMessage?.FileRegistration?.FileRegistrationId}_{groupId}";
                            item[this.blobPath] = blobName;
                            var fileRegistrationTransaction = new FileRegistrationTransaction
                            {
                                SessionId = result.Item1,
                                BlobPath = blobName,
                                StatusTypeId = StatusType.PROCESSING,
                                SystemTypeId = trueMessage.FileRegistration.SystemTypeId,
                                UploadId = trueMessage.FileRegistration.UploadId,
                                ActionType = trueMessage.FileRegistration.ActionType,
                                FileRegistrationCreatedDate = trueMessage.FileRegistration.CreatedDate,
                                MessageType = trueMessage.Message,
                                RecordId = Guid.NewGuid().ToString(),
                            };

                            fileRegistrationTransactions.Add(fileRegistrationTransaction);
                            groupId++;
                        });
                    });

            // Add failure records
            fileRegistrationTransactions.AddRange(Enumerable.Range(0, GetFailureCount(token.Path, trueMessage)).Select(r =>
            {
                return new FileRegistrationTransaction
                {
                    StatusTypeId = StatusType.FAILED,
                };
            }));

            trueMessage.FileRegistration.AddRecords(fileRegistrationTransactions);
        }

        /// <summary>
        /// Getting the inventory blob name.
        /// </summary>
        /// <param name="message">The TRUE message.</param>
        /// <returns>The task.</returns>
        private static string GetInventoryBlobName(TrueMessage message)
        {
            if (message.SourceSystem == SystemType.EXCEL)
            {
                return message.InventoryJsonBlobName;
            }

            return message.Message == MessageType.Inventory ? message.JsonBlobName : null;
        }

        /// <summary>
        /// Getting the movement blob name.
        /// </summary>
        /// <param name="message">The TRUE message.</param>
        /// <returns>The task.</returns>
        private static string GetMovementBlobName(TrueMessage message)
        {
            if (message.SourceSystem == SystemType.EXCEL)
            {
                return message.MovementJsonBlobName;
            }

            return message.Message != MessageType.Inventory ? message.JsonBlobName : null;
        }

        /// <summary>
        /// Getting the purchase blob name.
        /// </summary>
        /// <param name="message">The TRUE message.</param>
        /// <returns>The task.</returns>
        private static string GetPurchaseBlobName(TrueMessage message)
        {
            if (message.SourceSystem == SystemType.EXCEL)
            {
                return message.InventoryJsonBlobName;
            }

            return message.Message == MessageType.Purchase ? message.JsonBlobName : null;
        }

        /// <summary>
        /// Getting the purchase blob name.
        /// </summary>
        /// <param name="message">The TRUE message.</param>
        /// <returns>The task.</returns>
        private static string GetSaleBlobName(TrueMessage message)
        {
            if (message.SourceSystem == SystemType.EXCEL)
            {
                return message.InventoryJsonBlobName;
            }

            return message.Message == MessageType.Sale ? message.JsonBlobName : null;
        }

        /// <summary>
        /// Getting the event blob name.
        /// </summary>
        /// <param name="message">The TRUE message.</param>
        /// <returns>The task.</returns>
        private static string GetEventBlobName(TrueMessage message)
        {
            return message.EventJsonBlobName;
        }

        private static int GetFailureCount(string path, TrueMessage message)
        {
            if (path.Contains("Movements", StringComparison.OrdinalIgnoreCase))
            {
                return message.FailedMovements;
            }
            else if (path.Contains("Inventory", StringComparison.OrdinalIgnoreCase))
            {
                return message.FailedInventory;
            }
            else if (path.Contains("Contract", StringComparison.OrdinalIgnoreCase))
            {
                return message.FailedContracts;
            }
            else
            {
                return message.FailedEvents;
            }
        }

        private Tuple<string, string> GetBlobBasePath(JToken item, TrueMessage trueMessage)
        {
            var type = item[Constants.Type].ToString();
            string identifier = string.Empty;
            if (type.EqualsIgnoreCase(nameof(Movement)))
            {
                identifier = item[this.movementId].ToString();
                return Tuple.Create(identifier, GetMovementBlobName(trueMessage) + $"/{identifier}/{identifier}");
            }
            else if (type.EqualsIgnoreCase(nameof(Inventory)))
            {
                identifier = item[Constants.InventoryProductUniqueId].ToString();
                return Tuple.Create(identifier, GetInventoryBlobName(trueMessage) + $"/{identifier}/{identifier}");
            }
            else if (type.EqualsIgnoreCase(nameof(MessageType.Purchase)))
            {
                identifier = item[Constants.MessageId].ToString();
                return Tuple.Create(identifier, GetPurchaseBlobName(trueMessage) + $"/{identifier}/{identifier}");
            }
            else if (type.EqualsIgnoreCase(nameof(MessageType.Sale)))
            {
                identifier = item[Constants.MessageId].ToString();
                return Tuple.Create(identifier, GetSaleBlobName(trueMessage) + $"/{identifier}/{identifier}");
            }
            else
            {
                identifier = item[this.sessionId].ToString();
                return Tuple.Create(identifier, GetEventBlobName(trueMessage) + $"/{identifier}/{identifier}");
            }
        }
    }
}