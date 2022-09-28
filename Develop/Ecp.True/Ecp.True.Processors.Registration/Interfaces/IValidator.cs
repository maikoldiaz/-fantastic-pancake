// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValidator.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;

    /// <summary>
    /// The Validator Type.
    /// </summary>
    /// <typeparam name="T">The Type.</typeparam>
    public interface IValidator<T>
        where T : class
    {
        /// <summary>
        /// Validates the asynchronous.
        /// </summary>
        /// <typeparam name="T">The entity to validate.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>bool.</returns>
        Task<ValidationResult> ValidateAsync(T entity);
    }
}
