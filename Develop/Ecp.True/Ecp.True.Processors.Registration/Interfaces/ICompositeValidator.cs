// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICompositeValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Registration.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// The Composite Validator.
    /// </summary>
    /// <typeparam name="T">The Type param.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Api.Inventory.Validator.Interface.IValidator{T}" />
    public interface ICompositeValidator<T> : IValidator<T>
        where T : class
    {
        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        IEnumerable<IValidator<T>> Children { get; }
    }
}