﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentValidators.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Ecp.True.Core.Attributes;

    /// <summary>
    /// Validation extensions.
    /// </summary>
    public static class ArgumentValidators
    {
        /// <summary>
        /// Throw argument null exception if value is null.
        /// </summary>
        /// <param name="value">The value to be validated.</param>
        /// <param name="parameterName">The parameter.</param>
        public static void ThrowIfNull([ValidatedNotNull] object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throw argument null exception if value is null or empty.
        /// </summary>
        /// <param name="value">The value to be validated.</param>
        /// <param name="parameterName">The parameter.</param>
        public static void ThrowIfNullOrEmpty([ValidatedNotNull] string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throw argument null exception if value is null or empty.
        /// </summary>
        /// <param name="enumerable">The value to be validated.</param>
        /// <param name="parameterName">The parameter.</param>
        /// <typeparam name="T">The generic type of enumerable.</typeparam>
        /// <returns>The enumerated list.</returns>
        public static IList<T> ThrowIfNullOrEmpty<T>([ValidatedNotNull] this IEnumerable<T> enumerable, string parameterName)
        {
            var list = enumerable?.ToList();
            if (list == null || !list.Any())
            {
                throw new ArgumentNullException(parameterName);
            }

            return list;
        }

        /// <summary>
        /// Throw argument null exception if value is null.
        /// </summary>
        /// <param name="stream">The stream to be validated.</param>
        /// <param name="parameterName">The parameter name. </param>
        public static void ThrowIfStreamEmpty([ValidatedNotNull] Stream stream, string parameterName)
        {
            if (stream != null && stream.Length == 0)
            {
                throw new InvalidDataException(parameterName);
            }
        }
    }
}
