// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuilder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Builders.Interfaces
{
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Builder interface.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    public interface IBuilder<in TInput>
    {
        /// <summary>
        /// Builds the object.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task<JObject> BuildAsync(TInput element);
    }
}