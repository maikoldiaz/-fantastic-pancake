// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoCLifetime.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Attributes
{
    /// <summary>
    /// Contains the different lifetime that is supported. By default Transient lifetime is used.
    /// </summary>
    public enum IoCLifetime
    {
        /// <summary>
        /// The transient
        /// </summary>
        Transient,

        /// <summary>
        /// The hierarchical
        /// </summary>
        Hierarchical,

        /// <summary>
        /// The container controlled
        /// </summary>
        ContainerControlled,
    }
}