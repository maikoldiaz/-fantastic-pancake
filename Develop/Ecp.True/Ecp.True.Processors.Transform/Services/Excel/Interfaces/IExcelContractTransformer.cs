// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExcelContractTransformer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services.Excel.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Processors.Core.Input;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The excel event transformer.
    /// </summary>
    public interface IExcelContractTransformer
    {
        /// <summary>
        /// Transforms event excel asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        Task<JArray> TransformContractAsync(ExcelInput message);
    }
}
