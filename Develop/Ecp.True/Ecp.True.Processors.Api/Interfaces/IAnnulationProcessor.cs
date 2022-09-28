// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnulationProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The category processor.
    /// </summary>
    public interface IAnnulationProcessor : IProcessor
    {
        /// <summary>
        /// Creates the Annulation asynchronous.
        /// </summary>
        /// <param name="annulation">The reversal.</param>
        /// <returns>State of create the reversal operation.</returns>
        Task CreateAnnulationRelationshipAsync(Annulation annulation);

        /// <summary>
        /// Updates the annulation relationship asynchronous.
        /// </summary>
        /// <param name="annulation">The annulation.</param>
        /// <returns>Updates the annulation row.</returns>
        Task UpdateAnnulationRelationshipAsync(Annulation annulation);

        /// <summary>
        /// Annulation relationship exists asynchronous.
        /// </summary>
        /// <param name="annulation">The reversal.</param>
        /// <returns>The task.</returns>
        Task<AnnulationInfo> ExistsAnnulationRelationshipAsync(Annulation annulation);
    }
}
