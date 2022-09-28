// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorInfo.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Core.Entities
{
    using Newtonsoft.Json;

    /// <summary>
    /// The Error entity.
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorInfo"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public ErrorInfo(string error)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(error, nameof(error));

            var index = error.IndexOf('-', System.StringComparison.OrdinalIgnoreCase);
            this.Code = index == -1 ? "unknown" : error.Substring(0, index);
            int code = 0;
            if (index != -1 && int.TryParse(this.Code, out code))
            {
                this.Message = index == -1 ? error : error.Substring(index + 1, error.Length - index - 1);
            }
            else
            {
                this.Code = "unknown";
                this.Message = error;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorInfo"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        [JsonConstructor]
        public ErrorInfo(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; }
    }
}
