// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransformationProcessor.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The Tranformation processor.
    /// </summary>
    public interface ITransformationProcessor : IProcessor
    {
        /// <summary>Creates the transformation asynchronous.</summary>
        /// <param name="transformation">The transformation.</param>
        /// <returns>The task.</returns>
        Task CreateTransformationAsync(TransformationDto transformation);

        /// <summary>
        /// Updates the transformation asynchronous.
        /// </summary>
        /// <param name="transformation">The transformation element.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        Task UpdateTransformationAsync(TransformationDto transformation);

        /// <summary>
        /// Deletes the transformation asynchronous.
        /// </summary>
        /// <param name="transformation">The transformation.</param>
        /// <returns>
        /// Return the delete node connection.
        /// </returns>
        Task DeleteTransformationAsync(DeleteTransformation transformation);

        /// <summary>Validates the transformation asynchronous.</summary>
        /// <param name="transformationDto">The transformation.</param>
        /// <returns>The task.</returns>
        Task<IEnumerable<Transformation>> ExistsTransformationAsync(TransformationDto transformationDto);

        /// <summary>
        /// Gets the transformation info.
        /// </summary>
        /// <param name="transformationId">The transformation identifier.</param>
        /// <returns>
        /// The transformation.
        /// </returns>
        Task<TransformationInfo> GetTransformationInfoAsync(int transformationId);
    }
}