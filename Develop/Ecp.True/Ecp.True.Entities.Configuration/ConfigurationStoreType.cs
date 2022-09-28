// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationStoreType.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// Enum for supported configuration stores.
    /// </summary>
    public enum ConfigurationStoreType
    {
        /// <summary>
        /// File configuration store based on service configuration file
        /// </summary>
        File = 1,

        /// <summary>
        /// Storage configuration store based on Azure Table Storage
        /// </summary>
        Storage = 2,

        /// <summary>
        /// Secret configuration store based on Azure KeyVault
        /// </summary>
        Secret = 3,
    }
}