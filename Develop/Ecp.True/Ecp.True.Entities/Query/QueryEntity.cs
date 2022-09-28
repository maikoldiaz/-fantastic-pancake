// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryEntity.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using System;
    using Ecp.True.Entities.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// The QueryEntity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class QueryEntity : Entity
    {
        /// <inheritdoc/>
        [ColumnIgnore]
        [JsonIgnore]
        public override string CreatedBy { get => base.CreatedBy; set => base.CreatedBy = value; }

        /// <inheritdoc/>
        [ColumnIgnore]
        [JsonIgnore]
        public override DateTime? CreatedDate { get => base.CreatedDate; set => base.CreatedDate = value; }

        /// <inheritdoc/>
        [ColumnIgnore]
        [JsonIgnore]
        public override string LastModifiedBy { get => base.LastModifiedBy; set => base.LastModifiedBy = value; }

        /// <inheritdoc/>
        [ColumnIgnore]
        [JsonIgnore]
        public override DateTime? LastModifiedDate { get => base.LastModifiedDate; set => base.LastModifiedDate = value; }
    }
}
