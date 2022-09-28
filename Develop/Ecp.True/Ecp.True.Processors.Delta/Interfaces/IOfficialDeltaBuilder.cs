// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOfficialDeltaBuilder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;

    /// <summary>
    /// The official delta builder.
    /// </summary>
    public interface IOfficialDeltaBuilder
    {
        /// <summary>
        /// Builds the movement.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<IEnumerable<Movement>> BuildMovementsAsync(OfficialDeltaData deltaData);

        /// <summary>
        /// Builds the movement.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<IEnumerable<DeltaNodeError>> BuildErrorsAsync(OfficialDeltaData deltaData);
    }
}
