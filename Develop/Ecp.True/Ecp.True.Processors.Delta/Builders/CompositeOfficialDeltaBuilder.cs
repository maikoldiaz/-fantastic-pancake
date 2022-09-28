// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeOfficialDeltaBuilder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Builders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The composite official delta builder.
    /// </summary>
    public class CompositeOfficialDeltaBuilder : ICompositeOfficialDeltaBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeOfficialDeltaBuilder"/> class.
        /// </summary>
        /// <param name="officialDeltaBuilders">The officialDeltaBuilders.</param>
        public CompositeOfficialDeltaBuilder(IEnumerable<IOfficialDeltaBuilder> officialDeltaBuilders)
        {
            this.Children = officialDeltaBuilders;
        }

        /// <inheritdoc/>
        public IEnumerable<IOfficialDeltaBuilder> Children { get; private set; }

        /// <inheritdoc/>
        public async Task<IEnumerable<DeltaNodeError>> BuildErrorsAsync(OfficialDeltaData deltaData)
        {
            var deltaErrors = new List<DeltaNodeError>();
            var tasks = new List<Task<IEnumerable<DeltaNodeError>>>();
            this.Children.ForEach(builder => tasks.Add(builder.BuildErrorsAsync(deltaData)));
            await Task.WhenAll(tasks).ConfigureAwait(false);

            tasks.ForEach(r => deltaErrors.AddRange(r.Result));
            return deltaErrors;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Movement>> BuildMovementsAsync(OfficialDeltaData deltaData)
        {
            var movements = new List<Movement>();
            var tasks = new List<Task<IEnumerable<Movement>>>();

            this.Children.ForEach(builder => tasks.Add(builder.BuildMovementsAsync(deltaData)));
            await Task.WhenAll(tasks).ConfigureAwait(false);

            tasks.ForEach(r => movements.AddRange(r.Result));
            return movements;
        }
    }
}
