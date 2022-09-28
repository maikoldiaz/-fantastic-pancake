// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISaveIntegrationOwnershipFile.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Integration
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The interface save integration file.
    /// </summary>
    public interface ISaveIntegrationOwnershipFile
    {
        /// <summary>
        /// Registers any integration.
        /// </summary>
        /// <param name="integrationData">The integration data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<string> RegisterIntegrationAsync(IntegrationData integrationData);
    }
}