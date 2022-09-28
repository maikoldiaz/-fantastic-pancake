// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardEnvironment.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Environment
{
    using System;

    /// <summary>
    /// Standard Environment.
    /// </summary>
    public static class StandardEnvironment
    {
        /// <summary>
        /// The ASP net core env.
        /// </summary>
        private const string AspNetCoreEnv = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// Gets the development.
        /// </summary>
        /// <value>
        /// The development.
        /// </value>
        public static string Development => "Development";

        /// <summary>
        /// Gets the test.
        /// </summary>
        /// <value>
        /// The test.
        /// </value>
        public static string Test => "Test";

        /// <summary>
        /// Gets the performance.
        /// </summary>
        /// <value>
        /// The performance.
        /// </value>
        public static string Performance => "Performance";

        /// <summary>
        /// Gets the staging.
        /// </summary>
        /// <value>
        /// The staging.
        /// </value>
        public static string Staging => "Staging";

        /// <summary>
        /// Gets the production.
        /// </summary>
        /// <value>
        /// The production.
        /// </value>
        public static string Production => "Production";

        /// <summary>
        /// Gets a value indicating whether this instance is development.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is development; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDevelopment => IsEnvironment(Development);

        /// <summary>
        /// Gets a value indicating whether this instance is dev and test.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dev and test; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDevOrTest => IsEnvironment(Development) || IsEnvironment(Test);

        /// <summary>
        /// Gets a value indicating whether this instance is performance.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is performance; otherwise, <c>false</c>.
        /// </value>
        public static bool IsPerformance => IsEnvironment(Performance);

        /// <summary>
        /// Gets a value indicating whether this instance is production.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is production; otherwise, <c>false</c>.
        /// </value>
        public static bool IsProduction => IsEnvironment(Production);

        /// <summary>
        /// Gets a value indicating whether this instance is staging.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is staging; otherwise, <c>false</c>.
        /// </value>
        public static bool IsStaging => IsEnvironment(Staging);

        /// <summary>
        /// Gets a value indicating whether this instance is test.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is test; otherwise, <c>false</c>.
        /// </value>
        public static bool IsTest => IsEnvironment(Test);

        /// <summary>
        /// Determines whether the specified environment name is environment.
        /// </summary>
        /// <param name="environmentName">Name of the environment.</param>
        /// <returns>
        /// <c>true</c> if the specified environment name is environment; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsEnvironment(string environmentName)
        {
            var hostingEnvironment = Environment.GetEnvironmentVariable(AspNetCoreEnv);
            return hostingEnvironment != null && environmentName != null && hostingEnvironment.Equals(environmentName, StringComparison.OrdinalIgnoreCase);
        }
    }
}