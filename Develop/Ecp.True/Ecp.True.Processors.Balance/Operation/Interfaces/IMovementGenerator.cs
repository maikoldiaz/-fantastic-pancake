// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMovementGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Operation.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Balance.Operation.Input;

    /// <summary>
    /// The movement generator.
    /// </summary>
    public interface IMovementGenerator
    {
        /// <summary>
        /// Generates the asynchronous.
        /// </summary>
        /// <param name="movementInput">The movement input.</param>
        /// <returns>The task.</returns>
        Task<IEnumerable<Movement>> GenerateAsync(MovementInput movementInput);
    }
}
