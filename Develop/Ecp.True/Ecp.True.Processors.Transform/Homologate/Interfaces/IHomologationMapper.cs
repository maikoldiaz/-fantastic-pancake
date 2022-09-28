// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHomologationMapper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Homologate
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The homologation mapper class.
    /// </summary>
    public interface IHomologationMapper
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="refreshIntervalInSecs">The refresh interval in secs.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        Task InitializeAsync(int refreshIntervalInSecs);

        /// <summary>
        /// Homologate the specified source value.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="original">The original.</param>
        /// <returns>
        /// Return homologated value.
        /// </returns>
        object Homologate(TrueMessage message, string objectName, object original);
    }
}
