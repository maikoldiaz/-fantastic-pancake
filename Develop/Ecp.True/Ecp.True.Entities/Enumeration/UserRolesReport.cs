﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRolesReport.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Enumeration
{
    /// <summary>
    /// The Report User Roles Enumeration.
    /// </summary>
    public enum UserRolesReport
    {
        /// <summary>
        /// The User Group Assignment Report
        /// </summary>
        UserGroupAssignmentReport = 1,

        /// <summary>
        /// The User Group Access Report
        /// </summary>
        UserGroupAccessReport = 2,

        /// <summary>
        /// The User Group And Assigned User Access Report
        /// </summary>
        UserGroupAndAssignedUserAccessReport = 3,
    }
}
