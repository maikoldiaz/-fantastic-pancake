// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleEntity.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Core
{
    using Ecp.True.Core;

    /// <summary>
    /// The rule entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class RuleEntity : Entity
    {
        /// <summary>
        /// Gets or sets the rule identifier.
        /// </summary>
        /// <value>
        /// The rule identifier.
        /// </value>
        public int RuleId { get; set; }

        /// <summary>
        /// Gets or sets the rule name.
        /// </summary>
        /// <value>
        /// The name of the rule.
        /// </value>
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            this.RuleName = ((RuleEntity)entity).RuleName;
            this.IsActive = true;
        }
    }
}
