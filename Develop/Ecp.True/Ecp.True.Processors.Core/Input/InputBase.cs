// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Input
{
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Input.Interfaces;

    /// <summary>
    /// The input base type.
    /// </summary>
    /// <typeparam name="T">The type of input.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Transform.Input.Interfaces.IInput{T}" />
    public class InputBase<T> : IInput<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputBase{T}"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        protected InputBase(T element)
        {
            this.Input = element;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBase{T}" /> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="element">The element.</param>
        protected InputBase(object args, T element)
            : this(element)
        {
            this.Arguments = args;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBase{T}"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="message">The message.</param>
        protected InputBase(T element, TrueMessage message)
            : this(element)
        {
            this.Message = message;
        }

        /// <inheritdoc/>
        public T Input { get; private set; }

        /// <inheritdoc/>
        public object Arguments { get; private set; }

        /// <inheritdoc/>
        public TrueMessage Message { get; private set; }
    }
}
