// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityInfoCreationStatus.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Enumeration
{
    /// <summary>
    /// The EntityInfoCreationStatus.
    /// </summary>
    public enum EntityInfoCreationStatus
    {
        /// <summary>
        /// Node connection created successfully.
        /// </summary>
        Created = 1,

        /// <summary>
        /// Node connection not created because of duplication.
        /// </summary>
        Duplicated = 2,

        /// <summary>
        /// Node connection updated.
        /// </summary>
        Updated = 3,

        /// <summary>
        /// Node connection with error.
        /// </summary>
        Error = 4,
    }
}