// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlobGenerator.cs" company="Microsoft">
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
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The blob generator for homologated messages.
    /// </summary>
    public interface IBlobGenerator
    {
        /// <summary>
        /// Getting the blobs for JARRAY.
        /// </summary>
        /// <param name="homologatedArray">The homologated array.</param>
        /// <param name="trueMessage">The TRUE message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task GenerateBlobsArrayAsync(JArray homologatedArray, TrueMessage trueMessage);

        /// <summary>
        /// Generates single blob for JARRAY.
        /// </summary>
        /// <param name="array">The jarray which is to be store in blob.</param>
        /// <param name="blobPath">The blob path.</param>
        /// <param name="containerName">The container name.</param>
        /// <returns>the blob name.</returns>
        Task GenerateBlobsArrayAsync(JArray array, string blobPath, string containerName);

        /// <summary>
        /// Getting the blobs for Object.
        /// </summary>
        /// <param name="objectToSave">The object.</param>
        /// <param name="trueMessage">The TRUE message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task GenerateBlobsAsync(JObject objectToSave, TrueMessage trueMessage);

        /// <summary>
        /// Generates single blob for JObject.
        /// </summary>
        /// <param name="objectToSave">The object which is to be store in blob.</param>
        /// <param name="blobPath">The blob path.</param>
        /// <param name="containerName">The container name.</param>
        /// <returns>the blob name.</returns>
        Task GenerateBlobsAsync(JObject objectToSave, string blobPath, string containerName);
    }
}
