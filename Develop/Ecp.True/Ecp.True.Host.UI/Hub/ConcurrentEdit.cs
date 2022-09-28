// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrentEdit.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Hub
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Caching;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.UI.Models;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;

    /// <summary>
    /// The ConcurrentEdit.
    /// </summary>
    [TrueAuthorize]
    public class ConcurrentEdit : Hub
    {
        /// <summary>
        /// The on connected.
        /// </summary>
        private const string OnConnectedCallBack = "onConnected";

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ConcurrentEdit> logger;

        /// <summary>
        /// The node ownership processor.
        /// </summary>
        private readonly INodeOwnershipProcessor nodeOwnershipProcessor;

        /// <summary>
        /// The SQL token provider.
        /// </summary>
        private readonly ISqlTokenProvider sqlTokenProvider;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The cache handler.
        /// </summary>
        private readonly ICacheHandler<string> cacheHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentEdit" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="nodeOwnershipProcessor">The node ownership processor.</param>
        /// <param name="sqlTokenProvider">The SQL token provider.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="cacheHandler">The cache handler.</param>
        public ConcurrentEdit(
            ITrueLogger<ConcurrentEdit> logger,
            INodeOwnershipProcessor nodeOwnershipProcessor,
            ISqlTokenProvider sqlTokenProvider,
            ITelemetry telemetry,
            ICacheHandler<string> cacheHandler)
        {
            this.logger = logger;
            this.nodeOwnershipProcessor = nodeOwnershipProcessor;
            this.sqlTokenProvider = sqlTokenProvider;
            this.telemetry = telemetry;
            this.cacheHandler = cacheHandler;
        }

        /// <summary>
        /// Joins the group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>The task.</returns>
        public async Task JoinGroup(string groupName)
        {
            var sanitizedGroupName = JsonConvert.DeserializeObject<string>(groupName);
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, sanitizedGroupName).ConfigureAwait(false);
        }

        /// <summary>
        /// Leaves the group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>The task.</returns>
        public async Task LeaveGroup(string groupName)
        {
            var sanitizedGroupName = JsonConvert.DeserializeObject<string>(groupName);
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, sanitizedGroupName).ConfigureAwait(false);
        }

        /// <summary>
        /// Called when [edit start asynchronous].
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>The task.</returns>
        public async Task OnEditStart(string entityId)
        {
            this.logger.LogInformation(entityId, LoggingConstants.SignalRTag);
            try
            {
                await this.sqlTokenProvider.InitializeAsync().ConfigureAwait(false);

                // Step 1. Get the editor information.
                var editorInfo = this.GetEditorInfo(entityId);

                var ownershipNode = new OwnershipNode
                {
                    OwnershipNodeId = editorInfo.OwnershipNodeId,
                    OwnershipStatus = OwnershipNodeStatusType.LOCKED,
                    EditorConnectionId = editorInfo.EditorConnectionId,
                    Editor = editorInfo.Editor,
                };

                // Step 2. Update the OwnershipNode record with status LOCKED.
                await this.nodeOwnershipProcessor.UpdateOwnershipNodeStatusAsync(editorInfo.OwnershipNodeId, ownershipNode).ConfigureAwait(false);

                // Step 3. Notify self to update nodedetails.
                await this.Clients.Caller.SendAsync(OnConnectedCallBack, editorInfo.OwnershipNodeId).ConfigureAwait(false);

                // Step 3. Leave the group so not to get the "EDIT START" notification.
                await this.LeaveGroup(editorInfo.OwnershipNodeId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

                // Step 2. Notify all the other connected client for {OwnershipNodeId} group.
                await this.Clients.Group(editorInfo.OwnershipNodeId.ToString(CultureInfo.InvariantCulture)).SendAsync("onEditStart", editorInfo).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error in OnEditStart", LoggingConstants.SignalRTag);
            }
        }

        /// <summary>
        /// Called when [publishing asynchronous].
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>The task.</returns>
        public async Task OnNodePublishing(string entityId)
        {
            this.logger.LogInformation(entityId, LoggingConstants.SignalRTag);
            try
            {
                await this.sqlTokenProvider.InitializeAsync().ConfigureAwait(false);

                var node = JsonConvert.DeserializeObject<EditorInfo>(entityId);

                // Step 1. Check if node is already publishing.
                var existingNode = await this.nodeOwnershipProcessor.GetOwnershipNodeIdAsync(node.OwnershipNodeId).ConfigureAwait(false);
                if (existingNode.OwnershipStatus == OwnershipNodeStatusType.PUBLISHING ||
                    !await this.CheckNodeInPublishingAsync(node.SessionId, node.OwnershipNodeId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false))
                {
                    await this.Clients.Caller.SendAsync("dbException").ConfigureAwait(false);
                    return;
                }

                // Step 2. Get the editor information.
                var editorInfo = this.GetEditorInfo(node.OwnershipNodeId.ToString(CultureInfo.InvariantCulture));

                var ownershipNode = new OwnershipNode
                {
                    OwnershipNodeId = editorInfo.OwnershipNodeId,
                    OwnershipStatus = OwnershipNodeStatusType.PUBLISHING,
                    EditorConnectionId = editorInfo.EditorConnectionId,
                    Editor = editorInfo.Editor,
                };

                // Step 3. Update the OwnershipNode record with status PUBLISHING.
                await this.nodeOwnershipProcessor.UpdateOwnershipNodeStatusAsync(editorInfo.OwnershipNodeId, ownershipNode).ConfigureAwait(false);

                // Step 4. Leave the group so not to get the "NODE PUBLISHING" notification.
                await this.LeaveGroup(editorInfo.OwnershipNodeId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

                // Step 5. Notify success to client.
                await this.Clients.Caller.SendAsync("publishSuccess").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error in OnNodePublishing", LoggingConstants.SignalRTag);
            }
            finally
            {
                var node = JsonConvert.DeserializeObject<EditorInfo>(entityId);
                await this.cacheHandler.DeleteAsync(node.OwnershipNodeId.ToString(CultureInfo.InvariantCulture), Repositories.Constants.CacheRegionName).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Called when [edit end].
        /// </summary>
        /// <param name="entityId">The entityId.</param>
        /// <returns>The task.</returns>
        public async Task OnEditEnd(string entityId)
        {
            this.logger.LogInformation(entityId, LoggingConstants.SignalRTag);
            try
            {
                await this.sqlTokenProvider.InitializeAsync().ConfigureAwait(false);

                // Step 1. Get the editor information.
                var editorInfo = this.GetEditorInfo(entityId);

                var ownershipNode = new OwnershipNode
                {
                    OwnershipNodeId = editorInfo.OwnershipNodeId,
                    OwnershipStatus = OwnershipNodeStatusType.UNLOCKED,
                    EditorConnectionId = null,
                    Editor = null,
                };

                // Step 2. Update the OwnershipNode record with status UNLOCKED.
                await this.nodeOwnershipProcessor.UpdateOwnershipNodeStatusAsync(editorInfo.OwnershipNodeId, ownershipNode).ConfigureAwait(false);

                // Step 3. Notify self to update nodedetails.
                await this.Clients.Caller.SendAsync(OnConnectedCallBack, editorInfo.OwnershipNodeId).ConfigureAwait(false);

                // Step 4. Leave the group so that the current user will not be notified for onEditStart callback.
                await this.LeaveGroup(editorInfo.OwnershipNodeId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

                // Step 5. Notify to group with OwnershipNode.
                await this.Clients.Group(editorInfo.OwnershipNodeId.ToString(CultureInfo.InvariantCulture)).SendAsync("onEditEnd", editorInfo).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error in OnEditEnd", LoggingConstants.SignalRTag);
            }
        }

        /// <summary>
        /// Ons the request un lock.
        /// </summary>
        /// <param name="entityId">The entityId.</param>
        /// <returns>The task.</returns>
        public async Task OnRequestUnLock(string entityId)
        {
            this.logger.LogInformation(entityId, LoggingConstants.SignalRTag);
            try
            {
                await this.sqlTokenProvider.InitializeAsync().ConfigureAwait(false);

                // Step 1. Get the editor information.
                var editorInfo = this.GetEditorInfo(entityId);

                // Step 2. Get the ownershipnode record by id.
                var ownershipNode = await this.nodeOwnershipProcessor.GetOwnershipNodeIdAsync(editorInfo.OwnershipNodeId).ConfigureAwait(false);

                editorInfo.NodeName = ownershipNode.Node.Name;

                if (ownershipNode.OwnershipStatus == OwnershipNodeStatusType.LOCKED)
                {
                    // Step 3. Send the notification to the user who has blocked the node, to release it.
                    await this.Clients.Clients(new List<string> { ownershipNode.EditorConnectionId }).SendAsync("onRequestUnLock", editorInfo).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error in OnRequestUnLock", LoggingConstants.SignalRTag);
            }
        }

        /// <summary>
        /// Called when [accept un lock].
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>The task.</returns>
        public async Task OnAcceptUnLock(string entityId)
        {
            this.logger.LogInformation(entityId, LoggingConstants.SignalRTag);
            try
            {
                await this.sqlTokenProvider.InitializeAsync().ConfigureAwait(false);

                // Step 1. Get the editor information.
                var editorInfo = this.GetEditorInfo(entityId);

                var ownershipNode = new OwnershipNode
                {
                    OwnershipNodeId = editorInfo.OwnershipNodeId,
                    OwnershipStatus = OwnershipNodeStatusType.UNLOCKED,
                    EditorConnectionId = null,
                    Editor = null,
                };

                // Step 2. Get the ownershipnode record by id.
                var ownershipNodeEntity = await this.nodeOwnershipProcessor.GetOwnershipNodeIdAsync(editorInfo.OwnershipNodeId).ConfigureAwait(false);

                // Step 3. Update the OwnershipNode record with status UNLOCKED.
                await this.nodeOwnershipProcessor.UpdateOwnershipNodeStatusAsync(editorInfo.OwnershipNodeId, ownershipNode).ConfigureAwait(false);

                editorInfo.NodeName = ownershipNodeEntity.Node.Name;

                // Step 4. Notify self to update nodedetails.
                await this.Clients.Caller.SendAsync(OnConnectedCallBack, editorInfo.OwnershipNodeId).ConfigureAwait(false);

                // Step 5. Notify to group with OwnershipNode.
                await this.Clients.Group(editorInfo.OwnershipNodeId.ToString(CultureInfo.InvariantCulture)).SendAsync("onAcceptUnLock", editorInfo).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error in OnAcceptUnLock", LoggingConstants.SignalRTag);
            }
        }

        /// <summary>
        /// Called when a new connection is established with the hub.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task" /> that represents the asynchronous connect.
        /// </returns>
        public override Task OnConnectedAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when a connection with the hub is terminated.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The task.</returns>
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await this.sqlTokenProvider.InitializeAsync().ConfigureAwait(false);

                // Remove Editor connection Id and set Status to UNLOCK of OwnershipNodes for the editor and connectionId.
                var currentUser = this.Context.User.Claims.Where(a => a.Type == "name").Select(a => a.Value).SingleOrDefault();
                var ownershipNodeEntity = await this.nodeOwnershipProcessor.GetLockedOwnershipNodeByEditorAndConnectionIdAsync(currentUser, this.Context.ConnectionId).ConfigureAwait(false);
                if (ownershipNodeEntity != null)
                {
                    var ownershipNode = new OwnershipNode
                    {
                        OwnershipNodeId = ownershipNodeEntity.OwnershipNodeId,
                        OwnershipStatus = OwnershipNodeStatusType.UNLOCKED,
                        EditorConnectionId = null,
                        Editor = null,
                    };

                    await this.nodeOwnershipProcessor.UpdateOwnershipNodeStatusAsync(ownershipNode.OwnershipNodeId, ownershipNode).ConfigureAwait(false);
                    await this.Clients.Group(ownershipNode.OwnershipNodeId.ToString(CultureInfo.InvariantCulture)).SendAsync("onExiting").ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error in OnDisconnectedAsync", LoggingConstants.SignalRTag);
            }
            finally
            {
                stopwatch.Stop();
                this.telemetry.TrackMetric(LoggingConstants.SignalRTag, $"ConcurrentEdit: OnDisconnectedAsync", stopwatch.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Gets the editor info.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>
        /// The EditorInfo.
        /// </returns>
        private EditorInfo GetEditorInfo(string entityId)
        {
            var currentUser = this.Context.User.Claims.Where(a => a.Type == "name").Select(a => a.Value).SingleOrDefault();
            var sanitizedEntityId = JsonConvert.DeserializeObject<int>(entityId);
            return new EditorInfo
            {
                EditorConnectionId = this.Context.ConnectionId,
                Editor = currentUser,
                OwnershipNodeId = sanitizedEntityId,
            };
        }

        private async Task<bool> CheckNodeInPublishingAsync(string sessionId, string nodeId)
        {
            var existing = await this.cacheHandler.GetAsync(nodeId, Repositories.Constants.CacheRegionName).ConfigureAwait(false);
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) };
            if (string.IsNullOrWhiteSpace(existing))
            {
                await this.cacheHandler.SetAsync(nodeId, sessionId, Repositories.Constants.CacheRegionName, options).ConfigureAwait(false);
                var existingAfterSet = await this.cacheHandler.GetAsync(nodeId, Repositories.Constants.CacheRegionName).ConfigureAwait(false);
                if (existingAfterSet != sessionId)
                {
                    return false;
                }
            }
            else if (existing == sessionId)
            {
                await this.cacheHandler.SetAsync(nodeId, sessionId, Repositories.Constants.CacheRegionName, options).ConfigureAwait(false);
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}