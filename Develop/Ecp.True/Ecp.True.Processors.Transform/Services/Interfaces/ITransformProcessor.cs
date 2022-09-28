// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransformProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Functions.Transform;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Transform Processor.
    /// </summary>
    public interface ITransformProcessor
    {
        /// <summary>
        /// Gets the input type.
        /// </summary>
        /// <value>
        /// The input type.
        /// </value>
        InputType InputType { get; }

        /// <summary>
        /// Transforms asynchronously.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The registration data.</returns>
        Task<RegistrationData> TransformAsync(TrueMessage message);

        /// <summary>
        /// Completes asynchronously.
        /// </summary>
        /// <param name="homologatedArray">The message.</param>
        /// <param name="trueMessage">The true message.</param>
        /// <returns>The task.</returns>
        Task CompleteAsync(JArray homologatedArray, TrueMessage trueMessage);
    }
}