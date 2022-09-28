// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventDecoderFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain.Decoders
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Processors.Blockchain.Interfaces;

    /// <summary>
    /// The event decoder factory.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Blockchain.Interfaces.IEventDecoderFactory" />
    public class EventDecoderFactory : IEventDecoderFactory
    {
        /// <summary>
        /// The decoders.
        /// </summary>
        private readonly IEnumerable<IEventDecoder> decoders;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventDecoderFactory"/> class.
        /// </summary>
        /// <param name="decoders">The decoders.</param>
        public EventDecoderFactory(IEnumerable<IEventDecoder> decoders)
        {
            this.decoders = decoders;
        }

        /// <inheritdoc/>
        public IEventDecoder GetDecoder(int version)
        {
            return this.decoders.Single(d => d.Version == version);
        }
    }
}
