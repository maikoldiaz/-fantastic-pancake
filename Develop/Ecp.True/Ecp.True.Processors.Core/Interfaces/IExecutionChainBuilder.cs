// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExecutionChainBuilder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Interfaces
{
    using Ecp.True.Processors.Core.Execution;

    /// <summary>
    /// The interface Execution Chain Builder.
    /// </summary>
    public interface IExecutionChainBuilder
    {
        /// <summary>
        /// Builds the execution chain.
        /// </summary>
        /// <param name="processType">The process type.</param>
        /// <param name="chainType">The chain type.</param>
        /// <returns>The executor.</returns>
        IExecutor Build(ProcessType processType, ChainType chainType);
    }
}
