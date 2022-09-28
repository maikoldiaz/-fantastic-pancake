// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Microsoft">
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
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts the IEnumerable of type object to data table.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="columnName">The columnName.</param>
        /// <param name="tableName">The tableName.</param>
        /// <returns>The data table.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source, string columnName, string tableName)
            where T : struct
        {
            var dataTable = new DataTable(tableName) { Locale = CultureInfo.InvariantCulture };
            dataTable.Columns.Add(columnName, typeof(T));

            if (source == null)
            {
                return dataTable;
            }

            foreach (var item in source)
            {
                var dr = dataTable.NewRow();
                dr[columnName] = item;

                dataTable.Rows.Add(dr);
            }

            return dataTable;
        }

        /// <summary>
        /// Converts the IEnumerable of type string to data table.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="columnName">The columnName.</param>
        /// <param name="tableName">The tableName.</param>
        /// <returns>The data table.</returns>
        public static DataTable ToDataTable(this IEnumerable<string> source, string columnName, string tableName)
        {
            var dataTable = new DataTable(tableName) { Locale = CultureInfo.InvariantCulture };
            dataTable.Columns.Add(columnName, typeof(string));

            if (source == null)
            {
                return dataTable;
            }

            foreach (var item in source)
            {
                var dr = dataTable.NewRow();
                dr[columnName] = item;

                dataTable.Rows.Add(dr);
            }

            return dataTable;
        }

        /// <summary>
        /// Converts the IEnumerable of type object to data table.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>
        /// The data table.
        /// </returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source, string tableName)
           where T : class
        {
            var isDisplayNameAvailable = typeof(T).GetCustomAttributes().Any(a => string.Equals(a.GetType().Name, nameof(DisplayNameAttribute), StringComparison.Ordinal));
            string dataTableName = string.IsNullOrWhiteSpace(tableName) ? typeof(T).Name : tableName;
            dataTableName = isDisplayNameAvailable ? GetTableDisplayName<T>() : dataTableName;

            var dataTable = new DataTable(dataTableName) { Locale = CultureInfo.InvariantCulture };
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var propsinfo = props.Where(p => !p.GetCustomAttributes().Any(a => string.Equals(a.GetType().Name, "ColumnIgnoreAttribute", StringComparison.Ordinal)));

            GenerateDataColumns(isDisplayNameAvailable, dataTable, propsinfo);

            if (source != null)
            {
                GenerateDataRows(source, isDisplayNameAvailable, dataTable, propsinfo);
            }

            return dataTable;
        }

        /// <summary>
        /// Adds one enumerable to another.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>
        /// The concatenated collection.
        /// </returns>
        public static IEnumerable<T> AddRange<T>(this IEnumerable<T> source, IEnumerable<T> destination)
           where T : class
        {
            var sourceList = source.ToList();
            sourceList.AddRange(destination);
            return sourceList;
        }

        /// <summary>
        /// Converts the object to data table.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>The data table.</returns>
        public static DataTable ToTable<T>(this T source)
            where T : class
        {
            var dataTable = new DataTable(typeof(T).Name) { Locale = CultureInfo.InvariantCulture };
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props.Where(p => !p.GetCustomAttributes().Any(a => string.Equals(a.GetType().Name, "ColumnIgnoreAttribute", StringComparison.Ordinal))))
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            if (source != null)
            {
                var dr = dataTable.NewRow();
                foreach (DataColumn dataTableColumn in dataTable.Columns)
                {
                    var propertyInfo = props.Single(p => p.Name.Equals(dataTableColumn.ColumnName, StringComparison.OrdinalIgnoreCase));
                    dr[dataTableColumn] = propertyInfo.GetValue(source) ?? DBNull.Value;
                }

                dataTable.Rows.Add(dr);
            }

            return dataTable;
        }

        /// <summary>
        /// Converts the object to data table.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>The data table.</returns>
        public static DataTable ToStructTable<T>(this T item)
            where T : struct
        {
            var dataTable = new DataTable("T1") { Locale = CultureInfo.InvariantCulture };
            const string columnName = "C1";
            dataTable.Columns.Add(columnName, typeof(T));

            var dr = dataTable.NewRow();
            dr[columnName] = item;

            dataTable.Rows.Add(dr);

            return dataTable;
        }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="exp">The exp.</param>
        /// <returns>The property info from expression.</returns>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> exp)
        {
            ArgumentValidators.ThrowIfNull(exp, nameof(exp));

            var member = exp.Body as MemberExpression;
            return member?.Member as PropertyInfo;
        }

        /// <summary>
        /// Deserialize the HTTP content asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of content.</typeparam>
        /// <param name="content">The content.</param>
        /// <returns>The task.</returns>
        public static async Task<T> DeserializeHttpContentAsync<T>(this HttpContent content)
        {
            ArgumentValidators.ThrowIfNull(content, nameof(content));
            var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            if (stream == null || !stream.CanRead)
            {
                return default;
            }

            using var sr = new StreamReader(stream);
            using var jtr = new JsonTextReader(sr);

            var js = new JsonSerializer { FloatParseHandling = FloatParseHandling.Decimal, };
            return js.Deserialize<T>(jtr);
        }

        /// <summary>
        /// Deserialize the HTTP content asynchronous.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The task.</returns>
        public static async Task<string> DeserializeHttpContentAsync(this HttpContent content)
        {
            ArgumentValidators.ThrowIfNull(content, nameof(content));
            using var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            if (stream == null || !stream.CanRead)
            {
                return string.Empty;
            }

            if (!content.Headers.ContentEncoding.Any(x => x == Constants.GzipContent))
            {
                using (var sr = new StreamReader(stream))
                {
                    return await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            }

            using (var decompressed = new GZipStream(stream, CompressionMode.Decompress))
            {
                using (var reader = new StreamReader(decompressed))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Deserialize the stream.
        /// </summary>
        /// <typeparam name="T">The Type.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>The Task.</returns>
        public static T DeserializeStream<T>(this Stream stream)
        {
            var serializer = new JsonSerializer();
            using (var textStream = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(textStream))
                {
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

        /// <summary>
        /// Determines whether the given value is contained in the string also checking the case.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>True if value is present in the source string as per the comparison type.</returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return !string.IsNullOrWhiteSpace(source) && source.Contains(value, comparisonType);
        }

        /// <summary>
        /// Pluralizes the specified plural string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The plural enum name.
        /// </returns>
        public static string Pluralize(this Enum value)
        {
            return Pluralize(value, "s");
        }

        /// <summary>
        /// Pluralizes the specified plural string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="pluralString">The plural string.</param>
        /// <returns>
        /// The plural enum name.
        /// </returns>
        public static string Pluralize(this Enum value, string pluralString)
        {
            ArgumentValidators.ThrowIfNull(value, nameof(value));
            return $"{value.ToString()}{pluralString}";
        }

        /// <summary>
        /// Pluralizes the specified plural string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The plural enum name.
        /// </returns>
        public static string Pluralize(this string value)
        {
            return Pluralize(value, "s");
        }

        /// <summary>
        /// Pluralizes the specified plural string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="pluralString">The plural string.</param>
        /// <returns>
        /// The plural enum name.
        /// </returns>
        public static string Pluralize(this string value, string pluralString)
        {
            return $"{value}{pluralString}";
        }

        /// <summary>
        /// To convert to ENUM.
        /// </summary>
        /// <typeparam name="T">The type T.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The type.</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            return ToEnum(value, default(T));
        }

        /// <summary>
        /// To convert to ENUM.
        /// </summary>
        /// <typeparam name="T">The type T.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The type.</returns>
        public static T ToEnum<T>(this string value, T defaultValue)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return Enum.TryParse(value, true, out T result) ? result : defaultValue;
        }

        /// <summary>
        /// To the long date format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The long date format.</returns>
        public static string ToLongDateFormat(this DateTime value) => value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        /// <summary>
        /// To the ISO date format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The ISO date format.</returns>
        public static string ToIsoDateFormat(this DateTime value) => value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);

        /// <summary>
        /// The extension method for ForEach.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            ArgumentValidators.ThrowIfNull(source, nameof(source));
            ArgumentValidators.ThrowIfNull(action, nameof(action));
            foreach (var item in source)
            {
                action(item);
            }
        }

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
        /// The Non zero.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="replaceValue">
        /// The replace value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReplaceIfEmpty(this string input, string replaceValue)
        {
            return string.IsNullOrWhiteSpace(input) ? replaceValue : input;
        }

        /// <summary>
        /// Gets the or default.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Returns target tokens string value from JToken.</returns>
        public static T GetOrDefault<T>(this JToken input, string key, T defaultValue)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            return input[key] != null && !string.IsNullOrWhiteSpace(input[key].ToString()) ? input[key].ToObject<T>() : defaultValue;
        }

        /// <summary>
        /// Replaces if default.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>The updated value.</returns>
        public static TValue ReplaceIfDefault<TValue>(this TValue value, TValue newValue)
            where TValue : struct
        {
            return value.Equals(default(TValue)) ? newValue : value;
        }

        /// <summary>
        /// Converts to lowercase.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The lower case string.</returns>
        public static string ToLowerCase(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? value : value.ToLowerInvariant();
        }

        /// <summary>
        /// Converts to lowercase.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Converts a character to lower case.</returns>
        public static char ToLowerCase(this char value)
        {
            return char.ToLowerInvariant(value);
        }

        /// <summary>
        /// Converts to pascal case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The pascal case string.</returns>
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var charArray = value.ToCharArray();
            charArray[0] = char.ToUpperInvariant(charArray[0]);
            return new string(charArray);
        }

        /// <summary>
        /// Converts to sentence.
        /// </summary>
        /// <param name="pascal">The pascal.</param>
        /// <returns>The sentence from pascal case.</returns>
        public static string ToSentence(this string pascal)
        {
            return Regex.Replace(pascal, "[a-z][A-Z]", m => $"{m.Value[0]} {m.Value[1].ToLowerCase()}");
        }

        /// <summary>
        /// Gets the ticks.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Returns ticks.</returns>
        public static int GetTicks(this DateTime dateTime)
        {
            ArgumentValidators.ThrowIfNull(dateTime, nameof(dateTime));

            var ticksValue = dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc));

            return (int)ticksValue.TotalSeconds;
        }

        /// <summary>
        /// Gets the ticks.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Returns ticks.</returns>
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

        /// <summary>
        /// Gets the ticks.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Returns ticks.</returns>
        public static string ToTrueString(this DateTime dateTime)
        {
            var trueDate = dateTime.ToString("dd-MMM-yy", new CultureInfo("es-CO"));
            var dateParts = trueDate.Split("-");
            var formattedMonth = dateParts[1].ToPascalCase().Substring(0, 3);
            return string.Join("-", new List<string> { dateParts[0], formattedMonth, dateParts[2] });
        }

        /// <summary>
        /// Checks if it has value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>true or false.</returns>
        public static bool HasStringValue(this JToken input, string key, string value)
        {
            return !string.IsNullOrEmpty(input.GetOrDefault(key, string.Empty)) && input.GetOrDefault(key, string.Empty).EqualsIgnoreCase(value);
        }

        /// <summary>
        /// Determines whether this instance is number.
        /// </summary>
        /// <param name="input">The object.</param>
        /// <returns>
        ///   <c>true</c> if the specified object is number; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumber(this object input)
        {
            return input is double || input is decimal || input is int;
        }

        /// <summary>
        /// Determines whether this instance is date.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if the specified input is date; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDate(this object input)
        {
            return input is DateTime;
        }

        /// <summary>Rounds the off decimal.</summary>
        /// <param name="input">The input.</param>
        /// <returns>The truncated value.</returns>
        public static decimal? ToTrueDecimal(this decimal input)
        {
            return Math.Round(input, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>Converts to truedecimal.</summary>
        /// <param name="input">The input.</param>
        /// <returns>returns rounded off value.</returns>
        public static decimal? ToTrueDecimal(this decimal? input)
        {
            return input.HasValue ? input.Value.ToTrueDecimal() : null;
        }

        /// <summary>Truncates the decimal.</summary>
        /// <param name="input">The input.</param>
        /// <returns>The truncated value.</returns>
        public static decimal? ToTrueDecimal(this string input)
        {
            decimal number;
            if (decimal.TryParse(input, out number))
            {
                return ToTrueDecimal(number);
            }

            return null;
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The nullable integer value.</returns>
        public static int? ToNullableInt(this string input)
        {
            if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out var output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// Determines whether this instance is negative.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is negative; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNegative(this decimal? value)
        {
            return value < 0;
        }

        /// <summary>
        /// Replaces the title tag.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The string.</returns>
        public static string ReplaceTitleTag(this string value)
        {
            var regex = new Regex("(<title>.*?</title>)");
            return regex.Replace(value, string.Empty);
        }

        /// <summary>
        /// From the base64.
        /// Converts the base64 to byte array.
        /// </summary>
        /// <param name="encoded">The encoded.</param>
        /// <returns>The byte array.</returns>
        public static byte[] FromBase64(this string encoded)
        {
            return Convert.FromBase64String(encoded);
        }

        /// <summary>
        /// To the base64.
        /// </summary>
        /// <param name="arr">The array.</param>
        /// <returns>The encoded string.</returns>
        public static string ToBase64(this byte[] arr)
        {
            return Convert.ToBase64String(arr);
        }

        /// <summary>
        /// Converts to bearer.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The bearer token auth header.</returns>
        public static AuthenticationHeaderValue ToBearer(this string token)
        {
            return new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        /// <summary>
        /// Converts to basicauth.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="password">The password.</param>
        /// <returns>The auth header.</returns>
        public static AuthenticationHeaderValue ToBasicAuth(this string name, string password)
        {
            return new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", name, password))));
        }

        /// <summary>
        /// Converts to compressed stream.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The gzip content.</returns>
        public static GzipContent ToCompressedStream(this HttpContent content)
        {
            return new GzipContent(content);
        }

        /// <summary>
        /// Copies the property values.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="sourceItem">The source item.</param>
        /// <returns>The mapped object.</returns>
        public static TTarget CopyPropertyValuesWithName<TSource, TTarget>(this TSource sourceItem)
        {
            var serializationSettings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new FullPropertyNameContractResolver(),
            };

            var serializedObject = JsonConvert.SerializeObject(sourceItem, serializationSettings);
            return JsonConvert.DeserializeObject<TTarget>(serializedObject, serializationSettings);
        }

        /// <summary>
        /// Gets the Is Valid Json.
        /// </summary>
        /// <param name="strInput ">The input.</param>
        /// <returns>The string.</returns>
        public static bool IsValidJson(this string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput))
            {
                return false;
            }

            try
            {
                JToken.Parse(strInput);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <param name="input ">The input.</param>
        /// <param name="length">The length.</param>
        /// <returns>The string.</returns>
        public static string GetHash(this string input, int length)
        {
            using var crypt = new SHA256Managed();
            var hashBuilder = new StringBuilder();

            var bytes = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            bytes.ForEach(b => hashBuilder.Append(b.ToString("x2", CultureInfo.InvariantCulture)));

            var hash = hashBuilder.ToString();
            return hash.Substring(hash.Length - length);
        }

        /// <summary>
        /// Converts to ulong.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns>The unsigned long from hex.</returns>
        public static ulong ToUlong(this string hex)
        {
            try
            {
                return Convert.ToUInt64(hex, 16);
            }
            catch
            {
                return default;
            }
        }

        private static void GenerateDataRows<T>(IEnumerable<T> source, bool isDisplayNameAvailable, DataTable dataTable, IEnumerable<PropertyInfo> propsinfo)
            where T : class
        {
            foreach (var item in source)
            {
                var dr = dataTable.NewRow();
                foreach (DataColumn dataTableColumn in dataTable.Columns)
                {
                    PropertyInfo propertyInfo = isDisplayNameAvailable
                        ? propsinfo.Single(p => p.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>()
                        .FirstOrDefault().DisplayName.Equals(dataTableColumn.ColumnName, StringComparison.OrdinalIgnoreCase))
                        : propsinfo.Single(p => p.Name.Equals(dataTableColumn.ColumnName, StringComparison.OrdinalIgnoreCase));
                    dr[dataTableColumn] = Convert.ToBoolean(Nullable.GetUnderlyingType(propertyInfo.PropertyType)?.IsEnum, CultureInfo.InvariantCulture)
                            ? (int)propertyInfo.GetValue(item)
                            : propertyInfo.GetValue(item) ?? DBNull.Value;
                }

                dataTable.Rows.Add(dr);
            }
        }

        private static void GenerateDataColumns(bool isDisplayNameAvailable, DataTable dataTable, IEnumerable<PropertyInfo> propsinfo)
        {
            foreach (var prop in propsinfo)
            {
                string propName = string.Empty;
                propName = isDisplayNameAvailable ? GetPropertyDisplayName(prop) : prop.Name;
                dataTable.Columns.Add(propName, GetType(prop));
            }
        }

        private static string GetTableDisplayName<T>()
            where T : class
        {
            return typeof(T).GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().FirstOrDefault().DisplayName;
        }

        private static string GetPropertyDisplayName(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().FirstOrDefault().DisplayName;
        }

        /// <summary>
        /// Gets the prop type.
        /// </summary>
        /// <param name="prop">The prop.</param>
        /// <returns>The type.</returns>
        private static Type GetType(PropertyInfo prop)
        {
            return Convert.ToBoolean(Nullable.GetUnderlyingType(prop.PropertyType)?.IsEnum, CultureInfo.InvariantCulture)
                ? typeof(int)
                : Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
        }
    }
}