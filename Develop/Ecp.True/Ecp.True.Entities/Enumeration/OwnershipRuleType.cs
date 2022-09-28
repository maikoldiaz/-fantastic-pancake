// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleType.cs" company="Microsoft">
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
    /// The ownership rule type.
    /// </summary>
    public enum OwnershipRuleType
    {
        /// <summary>
        /// The node.
        /// </summary>
        Node = 0,

        /// <summary>
        /// The storage location products.
        /// </summary>
        StorageLocationProduct = 1,

        /// <summary>
        /// The node connection products
        /// </summary>
        NodeConnectionProduct = 2,

        /// <summary>
        /// The storage location product variable.
        /// </summary>
        StorageLocationProductVariable = 3,
    }
}
