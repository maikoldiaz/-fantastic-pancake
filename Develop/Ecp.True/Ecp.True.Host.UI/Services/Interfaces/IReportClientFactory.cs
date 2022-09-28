// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportClientFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Services.Interfaces
{
    using Microsoft.PowerBI.Api;
    using Microsoft.Rest;

    /// <summary>
    /// The report client factory.
    /// </summary>
    public interface IReportClientFactory
    {
        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="powerBiApi">
        /// The API.
        /// </param>
        /// <param name="tokenCredentials">The token credentials.</param>
        /// <returns>
        /// The power BI client.
        /// </returns>
        IPowerBIClient GetClient(string powerBiApi, TokenCredentials tokenCredentials);
    }
}
