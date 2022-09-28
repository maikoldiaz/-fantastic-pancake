// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReconcileService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Deadletter.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// The reconcile service.
    /// </summary>
    public interface IReconcileService : IReconciler
    {
        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        IEnumerable<IReconciler> Children { get; }
    }
}
