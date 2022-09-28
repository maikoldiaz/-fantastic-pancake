// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlockchainProcessor.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// The IBlockchainProcessor.
    /// </summary>
    public interface IBlockchainProcessor
    {
        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The task.</returns>
        Task ProcessAsync(string[] args);
    }
}
