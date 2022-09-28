// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformationController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The Transformer controller.
    /// </summary>
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "balance transport")]
    public class TransformationController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly ITransformationProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public TransformationController(ITransformationProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Creates a new transformer.
        /// </summary>
        /// <param name="transformation">The Transformation.</param>
        /// <returns>Returns the status.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/transformations")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> CreateTransformationAsync([FromBody] TransformationDto transformation)
        {
            await this.processor.CreateTransformationAsync(transformation).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.TransformationCreatedSuccessfully);
        }

        /// <summary>
        /// Creates a new transformer.
        /// </summary>
        /// <param name="transformation">The Transformation.</param>
        /// <returns>Returns the status.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/transformations")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> UpdateTransformationAsync([FromBody] TransformationDto transformation)
        {
            await this.processor.UpdateTransformationAsync(transformation).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.TransformationUpdatedSuccessfully);
        }

        /// <summary>
        /// Queries the transformation asynchronous.
        /// </summary>
        /// <returns>Return the task.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("transformations")]
        [ODataRoute("transformations")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public Task<IQueryable<Transformation>> QueryTransformationAsync()
        {
            return this.processor.QueryAllAsync<Transformation>(null);
        }

        /// <summary>
        /// Deletes the transformation.
        /// </summary>
        /// <param name="transformation">The transformation.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/transformations")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> DeleteTransformationAsync(DeleteTransformation transformation)
        {
            ArgumentValidators.ThrowIfNull(transformation, nameof(transformation));
            await this.processor.DeleteTransformationAsync(transformation).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.TransformationDeletedSuccessfully);
        }

        /// <summary>
        /// Gets transformation.
        /// </summary>
        /// <returns>The count of transformation.</returns>
        /// <param name="transformationDto">The transformation.</param>
        [HttpPost]
        [Route("api/v{version:apiVersion}/transformations/exists")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> ExistsTransformationAsync(TransformationDto transformationDto)
        {
            ArgumentValidators.ThrowIfNull(transformationDto, nameof(transformationDto));
            var result = await this.processor.ExistsTransformationAsync(transformationDto).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the transformation info.
        /// </summary>
        /// <param name="transformationId">The transformation identifier.</param>
        /// <returns>The nodes.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/transformations/{transformationId}/info")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetTransformationInfoAsync(int transformationId)
        {
            var result = await this.processor.GetTransformationInfoAsync(transformationId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.TransformationDoesNotExists);
        }
    }
}