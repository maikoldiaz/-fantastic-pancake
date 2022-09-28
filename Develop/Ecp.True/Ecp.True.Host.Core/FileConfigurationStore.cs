// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileConfigurationStore.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The file config store.
    /// </summary>
    [CLSCompliant(false)]
    public class FileConfigurationStore : IFileConfigurationStore
    {
        /// <summary>
        /// The application settings.
        /// </summary>
        private readonly IConfigurationSection applicationSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileConfigurationStore" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public FileConfigurationStore(IConfiguration configuration)
        {
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));
            this.applicationSettings = configuration.GetSection("Settings");
        }

        /// <summary>
        /// The get from store asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The value.
        /// </returns>
        public Task<T> GetFromStoreAsync<T>(string key)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(key, nameof(key));
            var value = this.applicationSettings[key.Replace(ConfigurationConstants.FileConfigurationPrefix, string.Empty, StringComparison.OrdinalIgnoreCase)];
            return string.IsNullOrEmpty(value) ? Task.FromResult(default(T)) : Task.FromResult(ParseType<T>(value));
        }

        /// <summary>
        /// The get from store asynchronously
        /// Considers caching depending on shouldCache parameter.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="shouldCache">if set to <c>true</c> [should cache].</param>
        /// <returns>
        /// The value.
        /// </returns>
        public Task<T> GetFromStoreAsync<T>(string key, bool shouldCache)
        {
            return Task.FromResult(default(T));
        }

        /// <inheritdoc />
        public Task<IEnumerable<T>> GetAllFromStoreAsync<T>()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// The parse type.
        /// </summary>
        /// <param name="strValue">
        /// The string value.
        /// </param>
        /// <typeparam name="T">
        /// Type of setting.
        /// </typeparam>
        /// <returns>
        /// typed value.
        /// </returns>
        protected static T ParseType<T>(string strValue)
        {
            var typedValue = default(T);
            var parseResult = strValue.ParseType(typeof(T));

            if (parseResult.Item2 != null)
            {
                return typedValue;
            }

            if (parseResult.Item1 != null)
            {
                typedValue = (T)parseResult.Item1;
            }

            return typedValue;
        }
    }
}