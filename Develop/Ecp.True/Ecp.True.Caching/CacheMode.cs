// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheMode.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Caching
{
    /// <summary>
    /// The cache mode.
    /// </summary>
    public enum CacheMode
    {
        /// <summary>
        /// The no cache.
        /// </summary>
        NoCache = 0,

        /// <summary>
        /// The local with backplane.
        /// </summary>
        LocalWithBackplane = 1,

        /// <summary>
        /// The local only.
        /// </summary>
        LocalOnly = 2,

        /// <summary>
        /// The distributed only.
        /// </summary>
        DistributedOnly = 3,
    }
}
