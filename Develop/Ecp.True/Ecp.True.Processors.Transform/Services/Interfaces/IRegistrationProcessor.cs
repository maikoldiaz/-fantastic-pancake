// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistrationProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// -------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The IRegistrationProcessors.
    /// </summary>
    public interface IRegistrationProcessor
    {
        /// <summary>
        /// Registers the inventory asynchronous.
        /// </summary>
        /// <param name="message">The file registration transaction.</param>
        /// <returns>The task.</returns>
        Task RegisterInventoryAsync(FileRegistrationTransaction message);

        /// <summary>
        /// Registers the movement asynchronous.
        /// </summary>
        /// <param name="message">The file registration transaction.</param>
        /// <returns>The task.</returns>
        Task RegisterMovementAsync(FileRegistrationTransaction message);

        /// <summary>
        /// Registers the event asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        Task RegisterEventAsync(FileRegistrationTransaction message);

        /// <summary>
        /// Registers the contract asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        Task RegisterContractAsync(FileRegistrationTransaction message);
    }
}
