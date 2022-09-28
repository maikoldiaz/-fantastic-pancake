// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOwnershipInputFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Input.Interfaces
{
    using System.IO;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Input;

    /// <summary>
    /// The factory to build the input from blob.
    /// </summary>
    public interface IOwnershipInputFactory
    {
        /// <summary>
        /// Gets the excel input asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>The excel input.</returns>
        ExcelInput GetExcelInput(TrueMessage message, Stream stream);
    }
}
