// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGeneratorStrategy.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Host.DataGenerator.Console.Entities;

    /// <summary>
    /// The IDataGeneratorStrategy.
    /// </summary>
    public interface IDataGeneratorStrategy
    {
        /// <summary>
        /// Gets the configuration base.
        /// </summary>
        /// <returns>The ConfigBase.</returns>
        ConfigBase Config { get; }

        /// <summary>
        /// Generates asynchronous.
        /// </summary>
        /// <param name="overrideMenu">if set to <c>true</c> [override menu].</param>
        /// <returns>
        /// The ConfigBase.
        /// </returns>
        Task GenerateAsync(bool overrideMenu = false);

        /// <summary>
        /// Initializes.
        /// </summary>
        /// <param name="args">The arguments.</param>
        void Initialize(string[] args);

        /// <summary>
        /// Initializes.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="configBase">The unit of work.</param>
        void Initialize(string[] args, ConfigBase configBase);
    }
}
