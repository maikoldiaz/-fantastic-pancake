// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRoleType.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Common
{
    /// <summary>
    /// The User.
    /// </summary>
    public class UserRoleType
    {
        /// <summary>
        /// Gets or sets the name of the UserId.
        /// </summary>
        /// <value>
        /// The name of the UserId.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Name.
        /// </summary>
        /// <value>
        /// The name of the Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the UserType.
        /// </summary>
        /// <value>
        /// The name of the UserType.
        /// </value>
        public string UserType { get; set; }

        /// <summary>
        /// Gets or sets the name of the Email.
        /// </summary>
        /// <value>
        /// The name of the Email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the RoleId.
        /// </summary>
        /// <value>
        /// The RoleId.
        /// </value>
        public int RoleId { get; set; }
    }
}
