// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementPeriod.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Registration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// Movement Period Class.
    /// </summary>
    public class MovementPeriod : Entity
    {
        /// <summary>
        /// Gets or sets the movement period identifier.
        /// </summary>
        /// <value>
        /// The movement period identifier.
        /// </value>
        public int MovementPeriodId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.MovementStartTimeIsMandatory)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.MovementEndTimeIsMandatory)]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction.
        /// </summary>
        /// <value>
        /// The movement transaction.
        /// </value>
        public virtual Movement MovementTransaction { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var element = (MovementPeriod)entity;

            this.StartTime = element.StartTime;
            this.EndTime = element.EndTime;
        }
    }
}
