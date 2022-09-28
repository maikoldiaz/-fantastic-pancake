// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractType.cs" company="Microsoft">
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
    /// The Contract Types.
    /// </summary>
    public enum ContractType
    {
        /// <summary>
        /// The movement
        /// </summary>
        MOVEMENT = 1,

        /// <summary>
        /// The inventory
        /// </summary>
        INVENTORY = 2,

        /// <summary>
        /// The movementownership
        /// </summary>
        MOVEMENTOWNERSHIP = 3,

        /// <summary>
        /// The inventoryownership
        /// </summary>
        INVENTORYOWNERSHIP = 4,

        /// <summary>
        /// The node
        /// </summary>
        NODE = 5,

        /// <summary>
        /// The nodeconnection
        /// </summary>
        NODECONNECTION = 6,

        /// <summary>
        /// The contract
        /// </summary>
        CONTRACTFACTORY = 7,

        /// <summary>
        /// The movementdetail
        /// </summary>
        MOVEMENTDETAIL = 8,

        /// <summary>
        /// The movementdetailowner
        /// </summary>
        MOVEMENTDETAILOWNER = 9,

        /// <summary>
        /// The inventory product
        /// </summary>
        INVENTORYDETAIL = 10,

        /// <summary>
        /// The inventory product owner
        /// </summary>
        INVENTORYDETAILOWNER = 11,

        /// <summary>
        /// The NodeProductCalculations
        /// </summary>
        NODEPRODUCTCALCULATIONSFACTORY = 12,

        /// <summary>
        /// The node
        /// </summary>
        NODEDETAILSFACTORY = 13,

        /// <summary>
        /// The nodeconnection
        /// </summary>
        NODECONNECTIONDETAILSFACTORY = 14,

        /// <summary>
        /// The movement ownership factory
        /// </summary>
        MOVEMENTOWNERSHIPFACTORY = 15,

        /// <summary>
        /// The inventory ownership factory
        /// </summary>
        INVENTORYOWNERSHIPFACTORY = 16,
    }
}
