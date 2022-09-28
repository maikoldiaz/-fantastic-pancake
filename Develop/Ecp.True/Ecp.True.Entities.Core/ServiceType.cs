// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceType.cs" company="Microsoft">
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
    /// The block chain service type.
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// The none.
        /// </summary>
        All,

        /// <summary>
        /// The node
        /// </summary>
        Node,

        /// <summary>
        /// The node connection
        /// </summary>
        NodeConnection,

        /// <summary>
        /// The movement
        /// </summary>
        Movement,

        /// <summary>
        /// The inventory
        /// </summary>
        InventoryProduct,

        /// <summary>
        /// The unbalance
        /// </summary>
        Unbalance,

        /// <summary>
        /// The ownership
        /// </summary>
        Ownership,

        /// <summary>
        /// The official blockchain movement
        /// </summary>
        OfficialMovement,

        /// <summary>
        /// The owner
        /// </summary>
        Owner,
    }
}
