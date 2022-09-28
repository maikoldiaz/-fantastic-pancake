// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortFinder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Ecp.True.Core;

    /// <summary>
    /// The port finder.
    /// </summary>
    public class PortFinder
    {
        private static PortFinder instance;
        private int portNumber;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static PortFinder Instance
        {
            get
            {
                instance ??= new PortFinder();
                return instance;
            }
        }

        /// <summary>
        /// Gets the api port.
        /// </summary>
        /// <value>
        /// The api port.
        /// </value>
        public int PortNumber => this.portNumber != 0 ? this.portNumber : 63087;

        /// <summary>
        /// Sets the path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Configure(string path)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(path, nameof(path));
            try
            {
                XDocument document;
                var basePath = path.Substring(0, path.IndexOf("Develop", System.StringComparison.OrdinalIgnoreCase));
                var configPath = $@"{basePath}Develop\.vs\Ecp.True.All\config\applicationhost.config";
                using (var stream = File.OpenRead(configPath))
                {
                    document = XDocument.Load(stream);
                }

                var sites = document.Root.Element("system.applicationHost").Element("sites").Elements();
                var site = sites.First(x => x.Attribute("name").Value == "Ecp.True.Host.Api");
                var binding = site.Element("bindings").Elements().First();
                var bindingInfo = binding.Attribute("bindingInformation").Value;
                var values = bindingInfo.Split(':');
                this.portNumber = int.Parse(values[1], CultureInfo.InvariantCulture);
            }
            catch
            {
                // No need to log
            }
        }
    }
}
