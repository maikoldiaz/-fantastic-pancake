// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Role.cs" company="Microsoft">
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
    /// The Role.
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// The Administrator
        /// </summary>
        Administrator = 1,

        /// <summary>
        /// The Approver
        /// </summary>
        Approver = 2,

        /// <summary>
        /// The ProfessionalSegmentBalances
        /// </summary>
        ProfessionalSegmentBalances = 3,

        /// <summary>
        /// The Programmer
        /// </summary>
        Programmer = 4,

        /// <summary>
        /// The Query
        /// </summary>
        Query = 5,

        /// <summary>
        /// The auditor
        /// </summary>
        Auditor = 6,

        /// <summary>
        /// The chain
        /// </summary>
        Chain = 7,
    }
}
