// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeState.cs" company="Microsoft">
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
    /// The Node state type.
    /// </summary>
    public enum NodeState
    {
        /// <summary>
        /// The created node.
        /// </summary>
        CreatedNode = 1,

        /// <summary>
        /// The updated node.
        /// </summary>
        UpdatedNode = 2,

        /// <summary>
        /// The operative balance calculated.
        /// </summary>
        OperativeBalanceCalculated = 3,

        /// <summary>
        /// The operative balance calculated with ownership.
        /// </summary>
        OperativeBalanceCalculatedWithOwnership = 4,
    }
}
