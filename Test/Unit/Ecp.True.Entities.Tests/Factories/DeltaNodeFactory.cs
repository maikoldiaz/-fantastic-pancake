// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeFactory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Tests.Factories
{
    using Ecp.True.Entities.Admin;
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Enumeration;

    public class DeltaNodeFactory
    {
        /// <summary>
        /// Gets the new delta movement.
        /// </summary>
        /// <returns>A list of delta nodes.</returns>
        public IEnumerable<DeltaNode> GetDeltaNodes()
        {
            yield return
                new DeltaNode
                {
                    TicketId = 1,
                    NodeId = 1,
                    Status = OwnershipNodeStatusType.APPROVED,
                    LastApprovedDate = DateTime.Now.AddMonths(-1),
                    Comment = null,
                    Editor = null,
                    Approvers = null,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now.AddMonths(-1),
                };
            yield return new DeltaNode
            {
                TicketId = 2,
                NodeId = 1,
                Status = OwnershipNodeStatusType.FAILED,
                LastApprovedDate = DateTime.Now.AddMonths(-1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(-1),
            };
            yield return new DeltaNode
            {
                TicketId = 3,
                NodeId = 1,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now.AddMonths(-1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(-1),
            };
            yield return new DeltaNode
            {
                TicketId = 2,
                NodeId = 2,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now.AddMonths(-1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(-1),
            };
            yield return new DeltaNode
            {
                TicketId = 2,
                NodeId = 3,
                Status = OwnershipNodeStatusType.OWNERSHIP,
                LastApprovedDate = DateTime.Now.AddMonths(-1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(-1),
            };
            yield return new DeltaNode
            {
                TicketId = 2,
                NodeId = 4,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now.AddMonths(-1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(-1),
            };
            yield return new DeltaNode
            {
                TicketId = 3,
                NodeId = 4,
                Status = OwnershipNodeStatusType.DELTAS,
                LastApprovedDate = DateTime.Now.AddMonths(-1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(-1),
            };
            yield return new DeltaNode
            {
                TicketId = 4,
                NodeId = 4,
                Status = OwnershipNodeStatusType.PUBLISHED,
                LastApprovedDate = DateTime.Now.AddMonths(-1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(-1),
            };
            yield return new DeltaNode
            {
                TicketId = 3,
                NodeId = 5,
                Status = OwnershipNodeStatusType.OWNERSHIP,
                LastApprovedDate = DateTime.Now.AddMonths(-1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(-1),
            };
            yield return new DeltaNode
            {
                TicketId = 4,
                NodeId = 5,
                Status = OwnershipNodeStatusType.DELTAS,
                LastApprovedDate = DateTime.Now.AddMonths(-1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(-1),
            };
            yield return new DeltaNode
            {
                TicketId = 5,
                NodeId = 1,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now,
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
            };
            yield return new DeltaNode
            {
                TicketId = 6,
                NodeId = 1,
                Status = OwnershipNodeStatusType.FAILED,
                LastApprovedDate = DateTime.Now,
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
            };
            yield return new DeltaNode
            {
                TicketId = 7,
                NodeId = 1,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now,
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
            };
            yield return new DeltaNode
            {
                TicketId = 6,
                NodeId = 2,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now,
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
            };
            yield return new DeltaNode
            {
                TicketId = 6,
                NodeId = 3,
                Status = OwnershipNodeStatusType.OWNERSHIP,
                LastApprovedDate = DateTime.Now,
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
            };
            yield return new DeltaNode
            {
                TicketId = 6,
                NodeId = 4,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now,
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
            };
            yield return new DeltaNode
            {
                TicketId = 7,
                NodeId = 4,
                Status = OwnershipNodeStatusType.DELTAS,
                LastApprovedDate = DateTime.Now,
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
            };
            yield return new DeltaNode
            {
                TicketId = 8,
                NodeId = 4,
                Status = OwnershipNodeStatusType.PUBLISHED,
                LastApprovedDate = DateTime.Now,
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
            };
            yield return new DeltaNode
            {
                TicketId = 7,
                NodeId = 5,
                Status = OwnershipNodeStatusType.OWNERSHIP,
                LastApprovedDate = DateTime.Now,
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
            };
            yield return new DeltaNode
            {
                TicketId = 8,
                NodeId = 5,
                Status = OwnershipNodeStatusType.DELTAS,
                LastApprovedDate = DateTime.Now,
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
            };
            yield return new DeltaNode
            {
                TicketId = 9,
                NodeId = 1,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now.AddMonths(1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(1),
            };
            yield return new DeltaNode
            {
                TicketId = 10,
                NodeId = 1,
                Status = OwnershipNodeStatusType.FAILED,
                LastApprovedDate = DateTime.Now.AddMonths(1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(1),
            };
            yield return new DeltaNode
            {
                TicketId = 11,
                NodeId = 1,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now.AddMonths(1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(1),
            };
            yield return new DeltaNode
            {
                TicketId = 10,
                NodeId = 2,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now.AddMonths(1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(1),
            };
            yield return new DeltaNode
            {
                TicketId = 10,
                NodeId = 3,
                Status = OwnershipNodeStatusType.OWNERSHIP,
                LastApprovedDate = DateTime.Now.AddMonths(1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(1),
            };
            yield return new DeltaNode
            {
                TicketId = 10,
                NodeId = 4,
                Status = OwnershipNodeStatusType.APPROVED,
                LastApprovedDate = DateTime.Now.AddMonths(1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(1),
            };
            yield return new DeltaNode
            {
                TicketId = 11,
                NodeId = 4,
                Status = OwnershipNodeStatusType.DELTAS,
                LastApprovedDate = DateTime.Now.AddMonths(1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(1),
            };
            yield return new DeltaNode
            {
                TicketId = 12,
                NodeId = 4,
                Status = OwnershipNodeStatusType.PUBLISHED,
                LastApprovedDate = DateTime.Now.AddMonths(1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(1),
            };
            yield return new DeltaNode
            {
                TicketId = 11,
                NodeId = 5,
                Status = OwnershipNodeStatusType.OWNERSHIP,
                LastApprovedDate = DateTime.Now.AddMonths(1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(1),
            };
            yield return new DeltaNode
            {
                TicketId = 12,
                NodeId = 5,
                Status = OwnershipNodeStatusType.DELTAS,
                LastApprovedDate = DateTime.Now.AddMonths(1),
                Comment = null,
                Editor = null,
                Approvers = null,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.AddMonths(1),
            };
        }
    }
}