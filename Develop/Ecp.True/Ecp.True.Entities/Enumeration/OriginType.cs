// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OriginType.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Enumeration
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The Origin Type.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OriginType
    {
        /// <summary>
        /// The official.
        /// </summary>
        OFICIAL = 1,

        /// <summary>
        /// The consolidated.
        /// </summary>
        CONSOLIDADO = 2,

        /// <summary>
        /// The delta official.
        /// </summary>
        DELTAOFICIAL = 3,
    }
}
