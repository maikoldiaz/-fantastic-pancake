// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeltaStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Interfaces
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    /// The IDeltaProcessor Strategy.
    /// </summary>
    public interface IDeltaStrategy
    {
        /// <summary>
        /// Builds the result.
        /// </summary>
        /// <param name="deltaData">The deltaData.</param>
        /// <returns>the task.</returns>
        IEnumerable<Movement> Build(DeltaData deltaData);
    }
}
