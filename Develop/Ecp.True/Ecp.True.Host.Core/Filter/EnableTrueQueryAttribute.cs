// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnableTrueQueryAttribute.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Query;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The enable true query attribute.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.OData.EnableQueryAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class EnableTrueQueryAttribute : EnableQueryAttribute
    {
        /// <summary>
        /// The validators.
        /// </summary>
        private static readonly IDictionary<string, Action<ODataQueryOptions>> Validators =
            new Dictionary<string, Action<ODataQueryOptions>>();

        /// <summary>
        /// The system settings.
        /// </summary>
        private static SystemSettings settings;

        /// <summary>
        /// The route.
        /// </summary>
        private readonly string route;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnableTrueQueryAttribute" /> class.
        /// </summary>
        /// <param name="route">The route.</param>
        public EnableTrueQueryAttribute(string route)
        {
            this.route = route;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="systemSettings">The system settings.</param>
        public static void Initialize(SystemSettings systemSettings)
        {
            settings = systemSettings;
            AddValidator("fileregistrations", ValidateFileRegistration);
        }

        /// <inheritdoc/>
        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)
        {
            base.ValidateQuery(request, queryOptions);

            if (!Validators.ContainsKey(this.route))
            {
                return;
            }

            Validators[this.route](queryOptions);
        }

        private static void ValidateFileRegistration(ODataQueryOptions queryOptions)
        {
            var hasError = false;
            var dateRange = settings.TransportFileUpload.DateRange;

            if (queryOptions.Filter == null)
            {
                return;
            }

            var filters = queryOptions.Filter.RawValue;
            if (!(filters.Contains("createdDate", StringComparison.OrdinalIgnoreCase) && filters.Contains("SystemTypeId", StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var filterOptions = queryOptions.Filter.RawValue.Split("and").Select(s => s.Trim());

            var systemTypeFilter = filterOptions.Single(f => f.Contains("SystemTypeId", StringComparison.OrdinalIgnoreCase));
            var isContractOrEvent = systemTypeFilter.Contains(SystemType.CONTRACT.ToString(), StringComparison.OrdinalIgnoreCase) ||
                                        systemTypeFilter.Contains(SystemType.EVENTS.ToString(), StringComparison.OrdinalIgnoreCase);

            if (!isContractOrEvent)
            {
                return;
            }

            var start = filterOptions.Single(f => f.Contains("createdDate ge", StringComparison.OrdinalIgnoreCase)).Replace("createdDate ge ", string.Empty, StringComparison.OrdinalIgnoreCase);
            var end = filterOptions.Single(f => f.Contains("createdDate le", StringComparison.OrdinalIgnoreCase)).Replace("createdDate le ", string.Empty, StringComparison.OrdinalIgnoreCase);

            if (!DateTime.TryParse(start, out DateTime startDate) ||
                !DateTime.TryParse(end, out DateTime endDate) ||
                endDate.Subtract(startDate).TotalDays > dateRange)
            {
                hasError = true;
            }

            if (hasError)
            {
                throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture, Entities.Constants.DateRangeExceeded, dateRange));
            }
        }

        private static void AddValidator(string key, Action<ODataQueryOptions> validator)
        {
            if (Validators.ContainsKey(key))
            {
                return;
            }

            Validators.Add(key, validator);
        }
    }
}
