// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentExtensions.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Shared.Environment
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Core.Environment;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// The environment extensions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class EnvironmentExtensions
    {
        /// <summary>
        /// Determines whether [is dev or test].
        /// </summary>
        /// <param name="env">The env.</param>
        /// <returns>
        /// <c>true</c> if [is dev or test] [the specified env]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDevOrTest(this IWebHostEnvironment env)
        {
            ArgumentValidators.ThrowIfNull(env, nameof(env));
            return env.IsEnvironment(StandardEnvironment.Development) || env.IsEnvironment(StandardEnvironment.Test);
        }

        /// <summary>
        /// Determines whether this instance is performance.
        /// </summary>
        /// <param name="env">The env.</param>
        /// <returns>
        /// <c>true</c> if the specified env is performance; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPerformance(this IWebHostEnvironment env)
        {
            ArgumentValidators.ThrowIfNull(env, nameof(env));
            return env.IsEnvironment(StandardEnvironment.Performance);
        }

        /// <summary>
        /// Determines whether this instance is test.
        /// </summary>
        /// <param name="env">The env.</param>
        /// <returns>
        /// <c>true</c> if the specified env is test; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTest(this IWebHostEnvironment env)
        {
            return env.IsEnvironment(StandardEnvironment.Test);
        }
    }
}