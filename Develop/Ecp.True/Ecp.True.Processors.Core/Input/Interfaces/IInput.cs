// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInput.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Input.Interfaces
{
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The input type.
    /// </summary>
    /// <typeparam name="T">The type of input.</typeparam>
    public interface IInput<out T>
    {
        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        T Input { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        TrueMessage Message { get; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        object Arguments { get; }
    }
}
