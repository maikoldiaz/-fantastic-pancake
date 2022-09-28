// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExpressionEvaluator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Approval.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// The approval processor.
    /// </summary>
    public interface IExpressionEvaluator
    {
        /// <summary>
        /// Gets the ownership node balance summary data.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The ownership nodes summary aggregate data.</returns>
        Task EvaluateAsync(int ownershipNodeId);
    }
}