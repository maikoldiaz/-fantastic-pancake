// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassificationValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Registration.Validation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The classification validator.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class ClassificationValidator<T> : Validator<T>
        where T : class
    {
        /// <inheritdoc/>
        protected override Task<ValidationResult> ValidateMovementAsync(Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            if (movement.IsValidClassification)
            {
                return Task.FromResult(ValidationResult.Success);
            }

            var errorResponse = new List<ErrorInfo>
            {
                new ErrorInfo(Registration.Constants.InvalidClassificationMessage),
            };
            return Task.FromResult(new ValidationResult(errorResponse));
        }
    }
}
