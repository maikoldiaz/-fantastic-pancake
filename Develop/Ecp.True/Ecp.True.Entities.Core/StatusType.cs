// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusType.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Core
{
    /// <summary>
    /// The file upload status.
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// The finalized.
        /// </summary>
        PROCESSED,

        /// <summary>
        /// The processing.
        /// </summary>
        PROCESSING,

        /// <summary>
        /// The failed.
        /// </summary>
        FAILED,

        /// <summary>
        /// The sent>
        /// </summary>
        SENT,

        /// <summary>
        /// The delta.
        /// </summary>
        DELTA,

        /// <summary>
        /// The Visualization.
        /// </summary>
        VISUALIZATION,

        /// <summary>
        /// The Error.
        /// </summary>
        ERROR,

        /// <summary>
        /// The Empty.
        /// </summary>
        EMPTY,

        /// <summary>
        /// The CancelLed.
        /// </summary>
        CANCELLED,

        /// <summary>
        /// The Forward.
        /// </summary>
        FORWARD,

        /// <summary>
        /// The CONCILIATIONFAILED.
        /// </summary>
        CONCILIATIONFAILED,
    }
}
