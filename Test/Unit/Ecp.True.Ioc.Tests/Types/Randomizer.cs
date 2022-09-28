// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Randomizer.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Ioc.Tests.Types
{
    using System;

    /// <summary>
    /// The randomizer.
    /// </summary>
    public static class Randomizer
    {
        /// <summary>
        /// The random.
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// Gets the next.
        /// </summary>
        /// <returns>The random number.</returns>
        public static int GetNext()
        {
            return Random.Next(0, int.MaxValue);
        }
    }
}