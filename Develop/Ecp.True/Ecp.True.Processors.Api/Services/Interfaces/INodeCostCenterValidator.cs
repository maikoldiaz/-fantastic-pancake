// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeCostCenterValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Services.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// Validator for the CostCenterMovements.
    /// </summary>
    public interface INodeCostCenterValidator
    {
        /// <summary>
        /// Validates whether a nodecostcenter can be safely eliminated.
        /// </summary>
        /// <param name="nodeCostCenter">The node cost center.</param>
        /// <returns>Whether the node cost center has movements.</returns>
        Task<ValidationResult> ValidateForDeletionAsync(NodeCostCenter nodeCostCenter);
    }
}