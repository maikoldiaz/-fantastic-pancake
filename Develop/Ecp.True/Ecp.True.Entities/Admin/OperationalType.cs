// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalType.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace EfCore.Models
{
    /// <summary>
    /// The Node Category Tagging class.
    /// </summary>
    public enum OperationalType
    {
        /// <summary>
        /// The add
        /// </summary>
        Add = 1,

        /// <summary>
        /// The change
        /// </summary>
        Change,

        /// <summary>
        /// The expire
        /// </summary>
        Expire,
    }
}
