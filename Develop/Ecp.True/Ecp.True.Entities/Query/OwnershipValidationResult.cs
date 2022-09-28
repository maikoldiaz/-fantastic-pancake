// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipValidationResult.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The ownership validation result.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class OwnershipValidationResult : Entity
    {
        /// <summary>
        /// Gets or sets the name of the validation.
        /// </summary>
        /// <value>
        /// The name of the validation.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OwnershipValidationResult"/> is result.
        /// </summary>
        /// <value>
        ///   <c>true</c> if result; otherwise, <c>false</c>.
        /// </value>
        public bool Result { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>
        /// The display order.
        /// </value>
        public int DisplayOrder { get; set; }
    }
}
