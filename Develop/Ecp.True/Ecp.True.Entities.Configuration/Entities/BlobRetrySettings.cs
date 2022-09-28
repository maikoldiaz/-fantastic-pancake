// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobRetrySettings.cs" company="Microsoft">
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
    /// The SQL connection configuration.
    /// </summary>
    public class BlobRetrySettings
    {
        /// <summary>
        /// Gets or sets the delta back off.
        /// </summary>
        /// <value>
        /// The delta back off.
        /// </value>
        public double DeltaBackOff { get; set; }

        /// <summary>
        /// Gets or sets the maximum attempts.
        /// </summary>
        /// <value>
        /// The maximum attempts.
        /// </value>
        public int MaxAttempts { get; set; }
    }
}
