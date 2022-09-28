// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeadLetterManager.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure
{
    /// <summary>
    /// The deadlettered manager.
    /// </summary>
    public interface IDeadLetterManager
    {
        /// <summary>
        /// Gets a value indicating whether [deadlettered message].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [deadlettered message]; otherwise, <c>false</c>.
        /// </value>
        bool IsDeadLettered { get; }

        /// <summary>
        /// Initializes the chaos manager with chaos value.
        /// </summary>
        /// <param name="isDeadLettered">The value indicating whether deadlettered message.</param>
        void Initialize(bool isDeadLettered);
    }
}
