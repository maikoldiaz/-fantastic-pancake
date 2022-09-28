// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventsPage.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Blockchain.Events
{
    using System.Collections.Generic;
    using Ecp.True.Core;
    using Ecp.True.Processors.Blockchain.Interfaces;

    /// <summary>
    /// The events page.
    /// </summary>
    public class EventsPage
    {
        /// <summary>
        /// The events.
        /// </summary>
        private readonly IList<IBlockchainEvent> events;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsPage" /> class.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="tailBlock">The tail block.</param>
        public EventsPage(int pageSize, ulong tailBlock)
        {
            this.events = new List<IBlockchainEvent>();
            this.TailBlock = tailBlock;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsPage" /> class.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="headBlock">The head block.</param>
        /// <param name="tailBlock">The tail block.</param>
        /// <param name="events">The events.</param>
        public EventsPage(int pageSize, ulong headBlock, ulong tailBlock, IEnumerable<IBlockchainEvent> events)
        {
            this.events = new List<IBlockchainEvent>(events);
            this.HeadBlock = headBlock;
            this.TailBlock = tailBlock;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Gets or sets the head block.
        /// </summary>
        /// <value>
        /// The head block.
        /// </value>
        public ulong HeadBlock { get; set; }

        /// <summary>
        /// Gets the tail block.
        /// </summary>
        /// <value>
        /// The tail block.
        /// </value>
        public ulong TailBlock { get; private set; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public IEnumerable<IBlockchainEvent> Events => this.events;

        /// <summary>
        /// Gets a value indicating whether this instance is page full.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is page full; otherwise, <c>false</c>.
        /// </value>
        public bool IsPageFull => this.PageSize == this.events.Count;

        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="evt">The evt.</param>
        public void AddEvent(IBlockchainEvent evt)
        {
            ArgumentValidators.ThrowIfNull(evt, nameof(evt));

            this.events.Add(evt);
            this.HeadBlock = evt.BlockNumber;
        }
    }
}
