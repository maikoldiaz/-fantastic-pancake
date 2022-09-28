// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiVersions.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The version constants.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ApiVersions
    {
        /// <summary>
        /// The one zero.
        /// </summary>
        public static readonly string OneZero = @"1";

        /// <summary>
        /// The one one.
        /// </summary>
        public static readonly string OneOne = @"1.1";

        /// <summary>
        /// The two zero.
        /// </summary>
        public static readonly string TwoZero = @"2";

        /// <summary>
        /// The two one.
        /// </summary>
        public static readonly string TwoOne = @"2.1";
    }
}