// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemDto.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// Rule entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class SystemDto : Entity
    {
        /// <summary>
        /// Gets or sets the system identifier.
        /// </summary>
        /// <value>
        /// The system identifier.
        /// </value>
        public int SystemId { get; set; }

        /// <summary>
        /// Gets or sets the system name.
        /// </summary>
        /// <value>
        /// The system name.
        /// </value>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the calculation date.
        /// </summary>
        /// <value>
        /// The calculation date.
        /// </value>
        public DateTime OperationDate { get; set; }
    }
}
