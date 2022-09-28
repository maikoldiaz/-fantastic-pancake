// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileRegistrationTransactionGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Transform.Homologate.Interfaces
{
    using Ecp.True.Entities.Dto;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The file registration transaction generator.
    /// </summary>
    public interface IFileRegistrationTransactionGenerator
    {
        /// <summary>
        /// Updates the file registration transactions.
        /// </summary>
        /// <param name="trueMessage">The true message.</param>
        /// <param name="token">The token.</param>
        void UpdateFileRegistrationTransactions(TrueMessage trueMessage, JArray token);
    }
}
