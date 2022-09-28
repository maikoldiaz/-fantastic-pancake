// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiStatusResult.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Result
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The entities action result.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.IActionResult" />
    public class MultiStatusResult : ObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiStatusResult"/> class.
        /// </summary>
        /// <param name="value">
        /// The object to  be passed as response.
        /// </param>
        public MultiStatusResult(object value)
            : base(value)
        {
            this.StatusCode = 207;
        }
    }
}
