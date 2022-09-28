// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExcelTransformer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Services.Excel.Interfaces
{
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The excel transformer.
    /// </summary>
    public interface IExcelTransformer
    {
        /// <summary>
        /// Transforms the excel asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="excelType">The excel type.</param>
        /// <returns>The task.</returns>
        Task<JObject> TransformExcelAsync(TrueMessage message, Stream stream, OwnershipExcelType excelType);
    }
}
