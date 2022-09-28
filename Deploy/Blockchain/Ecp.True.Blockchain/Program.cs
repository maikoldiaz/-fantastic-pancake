// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain
{
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Blockchain.Bootstrap;
    using Ecp.True.Blockchain.Interfaces;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The Program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The Arguments.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false);
            var configuration = builder.Build();

            var serviceProvider = ContainerConfiguration.Configure(configuration);

            var processor = (IBlockchainProcessor)serviceProvider.GetService(typeof(IBlockchainProcessor));
            await processor.ProcessAsync(args).ConfigureAwait(false);
        }
    }
}