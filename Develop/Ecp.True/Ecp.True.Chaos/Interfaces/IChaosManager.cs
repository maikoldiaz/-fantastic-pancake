// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChaosManager.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Chaos.Interfaces
{
    /// <summary>
    /// The chaos manager.
    /// </summary>
    public interface IChaosManager
    {
        /// <summary>
        /// Gets a value indicating whether [should forward chaos].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [should forward chaos]; otherwise, <c>false</c>.
        /// </value>
        bool HasChaos { get; }

        /// <summary>
        /// Gets the chaos value.
        /// </summary>
        /// <value>
        /// The chaos value.
        /// </value>
        string ChaosValue { get; }

        /// <summary>
        /// Tries to trigger the chaos.
        /// </summary>
        /// <param name="caller">Name of the caller.</param>
        /// <returns>The chaos error message.</returns>
        string TryTriggerChaos(string caller);

        /// <summary>
        /// Initializes the chaos manager with chaos value.
        /// </summary>
        /// <param name="value">The value.</param>
        void Initialize(string value);
    }
}
