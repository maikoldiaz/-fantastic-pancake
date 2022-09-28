// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlobOperations.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Movement validator Interface.
    /// </summary>
    public interface IBlobOperations
    {
        /// <summary>
        /// Gets the homologated json asynchronous.
        /// </summary>
        /// <param name="blobPath">The BLOB path.</param>
        /// <param name="uploadId">The upload identifier.</param>
        /// <returns>The task.</returns>
        Task<JToken> GetHomologatedJsonAsync(string blobPath, string uploadId);

        /// <summary>
        /// Gets the homologated object.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="token">The token.</param>
        /// <param name="uploadId">The upload identifier.</param>
        /// <returns>The task.</returns>
        Tuple<TObject, List<string>, object> GetHomologatedObject<TObject>(JToken token, string uploadId);

        /// <summary>
        /// Gets the contract object.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="homologatedToken"> The homologated Contract.</param>
        /// <returns>The task.</returns>
        Tuple<TObject, List<string>, object> DoGetContractToDelete<TObject>(JToken homologatedToken);
    }
}