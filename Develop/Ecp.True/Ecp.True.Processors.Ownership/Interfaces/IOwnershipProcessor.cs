// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOwnershipProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;

    /// <summary>
    /// The IOwnershipProcessor.
    /// </summary>
    public interface IOwnershipProcessor : IProcessor
    {
        /// <summary>
        /// Calculate the ownership asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticketId.</param>
        /// <returns>The task.</returns>
        Task<Tuple<IEnumerable<OwnershipCalculation>, IEnumerable<SegmentOwnershipCalculation>, IEnumerable<SystemOwnershipCalculation>>> CalculateOwnershipAsync(int ticketId);

        /// <summary>
        /// Processes the ownership asynchronous.
        /// </summary>
        /// <param name="ownershipRuleData">The ownershipRuleData.</param>
        /// <returns>The task.</returns>
        Task CompleteAsync(OwnershipRuleData ownershipRuleData);
    }
}
