// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportType.cs" company="Microsoft">
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
    /// The Report Type Enumeration.
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// The operational data without cutoff
        /// </summary>
        BeforeCutOff = 1,

        /// <summary>
        /// The official initial balance.
        /// </summary>
        OfficialInitialBalance = 3,

        /// <summary>
        /// The operative balance with ownership.
        /// </summary>
        OperativeBalance = 4,

        /// <summary>
        /// The send to SAP balance.
        /// </summary>
        SapBalance = 5,

        /// <summary>
        /// The user roles and permissions balance.
        /// </summary>
        UserRolesAndPermissions = 6,
    }
}
