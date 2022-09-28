// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessType.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Execution
{
    /// <summary>
    /// The process type.
    /// </summary>
    public enum ProcessType
    {
        /// <summary>
        /// True ownership
        /// </summary>
        Ownership = 1,

        /// <summary>
        /// The delta
        /// </summary>
        Delta = 2,

        /// <summary>
        /// The official delta
        /// </summary>
        OfficialDelta = 3,
    }
}
