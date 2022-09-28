// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeOrderManager.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// The Node Processor Interface.
    /// </summary>
    public interface INodeOrderManager
    {
        /// <summary>
        /// Tries the reorder asynchronous.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="nodeRepository">The node repository.</param>
        /// <returns>The task.</returns>
        Task TryReorderAsync(Node node, IRepository<Node> nodeRepository);
    }
}