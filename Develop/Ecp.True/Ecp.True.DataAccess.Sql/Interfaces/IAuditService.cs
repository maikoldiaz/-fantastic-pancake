// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuditService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Interfaces
{
    using System.Collections.Generic;
    using Ecp.True.Entities;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    /// <summary>
    /// The audit service.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Gets the audit entries asynchronous.
        /// </summary>
        /// <param name="tracker">The tracker.</param>
        /// <returns>
        /// The audit logs.
        /// </returns>
        IEnumerable<AuditLog> GetAuditLogs(ChangeTracker tracker);
    }
}
