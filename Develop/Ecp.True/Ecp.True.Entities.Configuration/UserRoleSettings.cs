// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRoleSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The User Role Settings.
    /// </summary>
    public class UserRoleSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleSettings"/> class.
        /// </summary>
        public UserRoleSettings()
        {
            this.Mapping = new Dictionary<Role, string>();
        }

        /// <summary>
        /// Gets the mapping.
        /// </summary>
        /// <value>
        /// The mapping.
        /// </value>
        public Dictionary<Role, string> Mapping { get; private set; }
    }
}
