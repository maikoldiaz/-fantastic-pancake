// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditableEntity.cs" company="Microsoft">
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
    /// <summary>
    /// The auditable entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class AuditableEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditableEntity"/> class.
        /// </summary>
        public AuditableEntity()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditableEntity"/> class.
        /// </summary>
        /// <param name="isAuditable">The auditable status.</param>
        public AuditableEntity(bool isAuditable)
        {
            this.IsAuditable = isAuditable;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is auditable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is auditable; otherwise, <c>false</c>.
        /// </value>
        [ColumnIgnore]
        public bool IsAuditable { get; }
    }
}
