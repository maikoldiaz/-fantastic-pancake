// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransformationMapper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services.Interfaces
{
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The tranformation mapper interface.
    /// </summary>
    public interface ITransformationMapper
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
        /// Transforms the specified object.
        /// </summary>
        /// <param name="jobject">The jobject.</param>
        void Transform(JToken jobject);
    }
}
