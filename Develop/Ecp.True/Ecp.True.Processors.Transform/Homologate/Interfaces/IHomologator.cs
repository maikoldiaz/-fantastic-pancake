// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHomologator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Homologate
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The homologator interface.
    /// </summary>
    public interface IHomologator
    {
        /// <summary>
        /// The Homologate method.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <param name="shouldHomologate">if set to <c>true</c> [should homologate].</param>
        /// <returns>
        /// A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.
        /// </returns>
        Task<JArray> HomologateAsync(TrueMessage message, JToken data, bool shouldHomologate);

        /// <summary>
        /// Homologate the single.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <returns>Returns a JArray.</returns>
        JArray HomologateObject(TrueMessage message, JToken data);
    }
}
