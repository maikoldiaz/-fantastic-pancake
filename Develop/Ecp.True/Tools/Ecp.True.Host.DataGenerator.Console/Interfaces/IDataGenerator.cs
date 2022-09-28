// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The delta generator interface.
    /// </summary>
    public interface IDataGenerator
    {
        /// <summary>
        /// Generates the specified parameters asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The identity.</returns>
        Task<int> GenerateAsync(IDictionary<string, object> parameters);
    }
}
