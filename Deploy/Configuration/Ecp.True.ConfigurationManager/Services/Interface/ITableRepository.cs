// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITableRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ConfigurationManager.Console.Repositories.Interface
{
    using System.Threading.Tasks;

    /// <summary>
    /// The table repository.
    /// </summary>
    public interface ITableRepository
    {
        /// <summary>
        /// The initialization of repository.
        /// </summary>
        /// <returns>The task.</returns>
        Task InitializeAsync();

        /// <summary>
        /// Upserts the configuration settings asynchronous.
        /// </summary>
        /// <param name="ignorables">The ignorables.</param>
        /// <param name="forceUpdates">The force update.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task UpsertConfigSettingsAsync(string ignorables, string forceUpdates);
    }
}
