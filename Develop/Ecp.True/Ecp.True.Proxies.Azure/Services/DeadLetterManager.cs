// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeadLetterManager.cs" company="Microsoft">
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
    using Ecp.True.Core.Attributes;

    /// <summary>
    /// The deadlettered manager.
    /// </summary>
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class DeadLetterManager : IDeadLetterManager
    {
        /// <inheritdoc/>
        public bool IsDeadLettered { get; private set; }

        /// <inheritdoc/>
        public void Initialize(bool isDeadLettered)
        {
            this.IsDeadLettered = isDeadLettered;
        }
    }
}
