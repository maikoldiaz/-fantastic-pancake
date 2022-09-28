// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Registration.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Movement validator Interface.
    /// </summary>
    public interface IEventValidator
    {
        /// <summary>
        /// Validate the event asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <param name="homologatedJson">The homologated json.</param>
        /// <returns>
        /// The [True] if validation passes, [False] otherwise.
        /// </returns>
        Task<(bool isValid, Event eventObject)> ValidateEventAsync(FileRegistrationTransaction fileRegistrationTransaction, JToken homologatedJson);
    }
}