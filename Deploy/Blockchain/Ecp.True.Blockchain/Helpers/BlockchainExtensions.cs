// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainExtensions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Blockchain.SetUp;

    /// <summary>
    /// The BlockchainExtensions.
    /// </summary>
    public static class BlockchainExtensions
    {
        /// <summary>
        /// Equals the ignore case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="that">The that.</param>
        /// <returns>The value is returned.</returns>
        public static bool EqualsIgnoreCase(this string value, string that)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(value, nameof(value));
            return value.Equals(that, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// The extension method for ForEach.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            ArgumentValidators.ThrowIfNull(source, nameof(source));

            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Converts to true.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The columbian datetime.</returns>
        public static DateTime ToTrue(this DateTime dateTime)
        {
            var timezones = TimeZoneInfo.GetSystemTimeZones();
            var trueTimeZone = timezones.FirstOrDefault(x => x.Id == "SA Pacific Standard Time" || x.Id == "America/Bogota");
            if (trueTimeZone == null)
            {
                return dateTime;
            }

            return dateTime.Kind == DateTimeKind.Utc ? TimeZoneInfo.ConvertTimeFromUtc(dateTime, trueTimeZone) : TimeZoneInfo.ConvertTime(dateTime, trueTimeZone);
        }
    }
}
