// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeTagProcessor.cs" company="Microsoft">
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
    using Ecp.True.Processors.Core.Interfaces;
    using EfCore.Models;

    /// <summary>
    /// The Node Processor Interface.
    /// </summary>
    public interface INodeTagProcessor : IProcessor
    {
        /// <summary>
        /// Saves the node asynchronous.
        /// </summary>
        /// <param name="taggedNodeInfo">The tagged node information.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task TagNodeAsync(TaggedNodeInfo taggedNodeInfo);
    }
}