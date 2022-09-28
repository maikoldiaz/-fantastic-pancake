// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Tests
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;

    /// <summary>
    /// The extension tests.
    /// </summary>
    [TestClass]
    public class ExtensionTests
    {
        /// <summary>
        /// Converts to truedecimalpositive_shouldreturndecimal.
        /// </summary>
        [TestMethod]
        public void ToTrueDecimalPositive_ShouldCheckDigitsAfterDecimal()
        {
            decimal val = 12345.7893M;
            val.ToTrueDecimal();
            string strvalue = Convert.ToString(val.ToTrueDecimal(), CultureInfo.InvariantCulture);
            strvalue = strvalue.Split(".")[1];
            Assert.AreEqual(2, strvalue.Length);
        }

        /// <summary>
        /// Converts to truedecimalnegative_shouldreturndecimal.
        /// </summary>
        [TestMethod]
        public void ToTrueDecimalNegative_ShouldCheckDigitsAfterDecimal()
        {
            decimal val = -12345.7893M;
            val.ToTrueDecimal();
            string strvalue = Convert.ToString(val.ToTrueDecimal(), CultureInfo.InvariantCulture);
            strvalue = strvalue.Split(".")[1];
            Assert.AreEqual(2, strvalue.Length);
        }

        /// <summary>
        /// Converts to blockchainnumber_shouldcheckdigitsafterdecimal.
        /// </summary>
        [TestMethod]
        public void ToBlockChainNumber_ShouldCheckDigitsAfterDecimal()
        {
            decimal val = 12345.7893M;
            string str = Convert.ToString(val, CultureInfo.InvariantCulture);
            decimal? newDecimal = str.ToTrueDecimal();
            string strvalue = Convert.ToString(newDecimal, CultureInfo.InvariantCulture);
            strvalue = strvalue.Split(".")[1];
            Assert.AreEqual(2, strvalue.Length);
        }

        /// <summary>
        /// Converts entity enumerable to datatable.
        /// </summary>
        [TestMethod]
        public void ToDatatable_ShouldConvertEnumerableOfValueTypes_ToDatatable()
        {
            var source = new[] { 1, 2 };
            var table = source.ToDataTable("Column", "Table");

            Assert.IsNotNull(table);
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(1, table.Columns.Count);
            Assert.AreEqual("Column", table.Columns[0].ColumnName);
            Assert.AreEqual("Table", table.TableName);
            Assert.AreEqual(1, table.Rows[0][0]);
            Assert.AreEqual(2, table.Rows[1][0]);
        }

        /// <summary>
        /// Converts empty entity enumerable to datatable.
        /// </summary>
        [TestMethod]
        public void ToDatatable_ShouldConvertEmptyEnumerableOfValueTypes_ToDatatable()
        {
            var source = Array.Empty<int>();
            var table = source.ToDataTable("Column", "Table");

            Assert.IsNotNull(table);
            Assert.AreEqual(0, table.Rows.Count);
            Assert.AreEqual(1, table.Columns.Count);
            Assert.AreEqual("Column", table.Columns[0].ColumnName);
            Assert.AreEqual("Table", table.TableName);
        }

        /// <summary>
        /// Converts entity enumerable to datatable.
        /// </summary>
        [TestMethod]
        public void ToDatatable_ShouldConvertEnumerableOfString_ToDatatable()
        {
            var source = new[] { "One", "Two" };
            var table = source.ToDataTable("Column", "Table");

            Assert.IsNotNull(table);
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(1, table.Columns.Count);
            Assert.AreEqual("Column", table.Columns[0].ColumnName);
            Assert.AreEqual("Table", table.TableName);
            Assert.AreEqual("One", table.Rows[0][0]);
            Assert.AreEqual("Two", table.Rows[1][0]);
        }

        /// <summary>
        /// Converts empty entity enumerable to datatable.
        /// </summary>
        [TestMethod]
        public void ToDatatable_ShouldConvertEmptyEnumerableOfString_ToDatatable()
        {
            var source = Array.Empty<string>();
            var table = source.ToDataTable("Column", "Table");

            Assert.IsNotNull(table);
            Assert.AreEqual(0, table.Rows.Count);
            Assert.AreEqual(1, table.Columns.Count);
            Assert.AreEqual("Column", table.Columns[0].ColumnName);
            Assert.AreEqual("Table", table.TableName);
        }

        /// <summary>
        /// Converts entity enumerable to datatable.
        /// </summary>
        [TestMethod]
        public void ToDatatable_ShouldConvertEnumerableOfReferenceType_ToDatatable()
        {
            var source = new[] { Tuple.Create(1) };
            var table = source.ToDataTable(typeof(Tuple<int>).Name);

            Assert.IsNotNull(table);
            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual(1, table.Columns.Count);
            Assert.AreEqual("Item1", table.Columns[0].ColumnName);
            Assert.AreEqual(typeof(Tuple<int>).Name, table.TableName);

            Assert.AreEqual(1, table.Rows[0][0]);
        }

        /// <summary>
        /// Converts entity to datatable.
        /// </summary>
        [TestMethod]
        public void ToDatatable_ShouldConvertReferenceType_ToDatatable()
        {
            var source = Tuple.Create(1);
            var table = source.ToTable();

            Assert.IsNotNull(table);
            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual(1, table.Columns.Count);
            Assert.AreEqual("Item1", table.Columns[0].ColumnName);
            Assert.AreEqual(typeof(Tuple<int>).Name, table.TableName);

            Assert.AreEqual(1, table.Rows[0][0]);
        }

        /// <summary>
        /// Converts value type to datatable.
        /// </summary>
        [TestMethod]
        public void ToDatatable_ShouldConvertValueType_ToDatatable()
        {
            var table = 1.ToStructTable();

            Assert.IsNotNull(table);
            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual(1, table.Columns.Count);
            Assert.AreEqual("C1", table.Columns[0].ColumnName);
            Assert.AreEqual("T1", table.TableName);

            Assert.AreEqual(1, table.Rows[0][0]);
        }

        /// <summary>
        /// Gets the property information should return property information from expression.
        /// </summary>
        [TestMethod]
        public void GetPropertyInfo_ShouldReturnPropertyInfo_FromExpression()
        {
            Expression<Func<Tuple<bool>, bool>> exp = t => t.Item1;
            var pi = exp.GetPropertyInfo();

            Assert.IsNotNull(pi);
            Assert.AreEqual("Item1", pi.Name);
        }

        /// <summary>
        /// Deserialize should deserialize json stream.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Deserialize_ShouldDeserializeJsonStreamAsync()
        {
            var obj = Tuple.Create(1);
            var json = JsonConvert.SerializeObject(obj);

            var byteArray = Encoding.ASCII.GetBytes(json);
            var stream = new MemoryStream(byteArray);

            using (var content = new StreamContent(stream))
            {
                var result = await content.DeserializeHttpContentAsync<Tuple<int>>().ConfigureAwait(false);
                Assert.AreEqual(obj.Item1, result.Item1);
            }
        }

        /// <summary>
        /// Deserialize should deserialize json stream.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Deserialize_ShouldThrowNullException_WhenContentIsNullAsync()
        {
            var content = default(StreamContent);
            await content.DeserializeHttpContentAsync<Tuple<int>>().ConfigureAwait(false);
        }

        /// <summary>
        /// Deserialize should deserialize json stream.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Deserialize_ShouldDeserializeJsonStreamToDefaultValue_WhenStreamIsNotReadableAsync()
        {
            var obj = Tuple.Create(1);
            var json = JsonConvert.SerializeObject(obj);

            var byteArray = Encoding.ASCII.GetBytes(json);
            var stream = new MemoryStream(byteArray);

            using (var reader = new StreamReader(stream))
            {
                await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            using (var content = new StreamContent(stream))
            {
                var result = await content.DeserializeHttpContentAsync<Tuple<int>>().ConfigureAwait(false);
                Assert.AreEqual(null, result);
            }
        }

        /// <summary>
        /// Deserialize should deserialize json stream.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Deserialize_ShouldDeserializeJsonStream_ToStringContentAsync()
        {
            var obj = Tuple.Create(1);
            var json = JsonConvert.SerializeObject(obj);

            var byteArray = Encoding.ASCII.GetBytes(json);
            var stream = new MemoryStream(byteArray);

            using (var content = new StreamContent(stream))
            {
                var result = await content.DeserializeHttpContentAsync().ConfigureAwait(false);
                Assert.AreEqual(json, result);
            }
        }

        /// <summary>
        /// Deserialize should deserialize json stream.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Deserialize_ShouldDeserializeJsonStream_ToGZipContentAsync()
        {
            var obj = Tuple.Create(1);
            var json = JsonConvert.SerializeObject(obj);

            var byteArray = Encoding.ASCII.GetBytes(json);
            var stream = new MemoryStream(byteArray);

            using (var content = new StreamContent(stream))
            {
                using (var gzipContent = new GzipContent(content))
                {
                    var result = await gzipContent.DeserializeHttpContentAsync().ConfigureAwait(false);
                    Assert.AreEqual(json, result);
                }
            }
        }

        /// <summary>
        /// Deserialize should deserialize json stream.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Deserialize_ShouldThrowNullException_WhenStringContentIsNullAsync()
        {
            var content = default(StreamContent);
            await content.DeserializeHttpContentAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Deserialize should deserialize json stream.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Deserialize_ShouldDeserializeJsonStreamToEmptyString_WhenStringStreamIsNotReadableAsync()
        {
            var obj = Tuple.Create(1);
            var json = JsonConvert.SerializeObject(obj);

            var byteArray = Encoding.ASCII.GetBytes(json);
            var stream = new MemoryStream(byteArray);

            using (var reader = new StreamReader(stream))
            {
                await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            using (var content = new StreamContent(stream))
            {
                var result = await content.DeserializeHttpContentAsync().ConfigureAwait(false);
                Assert.AreEqual(string.Empty, result);
            }
        }

        /// <summary>
        /// Determines whether [contains should validate string contains].
        /// </summary>
        [TestMethod]
        public void Contains_ShouldValidate_StringContains()
        {
            var original = "Some Test String";

            Assert.IsTrue(original.Contains("some", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(original.Contains("Some", StringComparison.Ordinal));
            Assert.IsFalse(string.Empty.Contains("some", StringComparison.OrdinalIgnoreCase));
            Assert.IsFalse(" ".Contains("some", StringComparison.OrdinalIgnoreCase));
            Assert.IsFalse(original.Contains("invalid", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Pluralize should pluralize text.
        /// </summary>
        [TestMethod]
        public void Pluralize_ShouldPluralize_Text()
        {
            Assert.AreEqual("cats", "cat".Pluralize());
            Assert.AreEqual("boxes", "box".Pluralize("es"));
            Assert.AreEqual("Locals", DateTimeKind.Local.Pluralize());
            Assert.AreEqual("Classes", AttributeTargets.Class.Pluralize("es"));
        }

        /// <summary>
        /// To Enum should convert string to enum.
        /// </summary>
        [TestMethod]
        public void ToEnum_ShouldConvertStringToEnum()
        {
            Assert.AreEqual(DateTimeKind.Local, "Local".ToEnum<DateTimeKind>());
            Assert.AreEqual(DateTimeKind.Utc, string.Empty.ToEnum(DateTimeKind.Utc));
            Assert.AreEqual(DateTimeKind.Utc, " ".ToEnum(DateTimeKind.Utc));
            Assert.AreEqual(DateTimeKind.Utc, "INVALID".ToEnum(DateTimeKind.Utc));
            Assert.AreEqual(DateTimeKind.Unspecified, "Unspecified".ToEnum(DateTimeKind.Utc));
        }

        /// <summary>
        /// Date Format should format dates.
        /// </summary>
        [TestMethod]
        public void DateFormat_ShouldFormatDates()
        {
            var dt = DateTime.UtcNow.ToTrue();
            Assert.AreEqual(dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), dt.ToLongDateFormat());
            Assert.AreEqual(dt.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), dt.ToIsoDateFormat());
        }

        /// <summary>
        /// For each should iterate on enumerable when invoked.
        /// </summary>
        [TestMethod]
        public void ForEach_ShouldIterateOnEnumerable_WhenInvoked()
        {
            var counter = 0;
            Enumerable.Range(1, 3).ForEach(i => counter++);

            Assert.AreEqual(3, counter);
        }

        /// <summary>
        /// Equals ignore case should equate case insensitive.
        /// </summary>
        [TestMethod]
        public void EqualsIgnoreCase_ShouldEquateCaseInsensitive()
        {
            var original = "Test";
            Assert.IsTrue(original.EqualsIgnoreCase("test"));
            Assert.IsTrue(original.EqualsIgnoreCase("Test"));
        }

        /// <summary>
        /// Replace should replace empty or default.
        /// </summary>
        [TestMethod]
        public void Replace_ShouldReplaceEmptyOrDefault()
        {
            Assert.AreEqual("New", string.Empty.ReplaceIfEmpty("New"));
            Assert.AreEqual("New", " ".ReplaceIfEmpty("New"));
            Assert.AreEqual("Value", "Value".ReplaceIfEmpty("New"));

            Assert.AreEqual(3, 0.ReplaceIfDefault(3));
            Assert.AreEqual(1, 1.ReplaceIfDefault(3));
        }

        /// <summary>
        /// To lower case should return lower case string.
        /// </summary>
        [TestMethod]
        public void ToLowerCase_ShouldReturn_LowerCaseString()
        {
            Assert.AreEqual("lower", "Lower".ToLowerCase());
            Assert.AreEqual('l', 'L'.ToLowerCase());
        }

        /// <summary>
        /// To sentence should return sentence from pascal case.
        /// </summary>
        [TestMethod]
        public void ToSentence_ShouldReturnSentence_FromPascalCase()
        {
            Assert.AreEqual("Get by id", "GetById".ToSentence());
        }

        /// <summary>
        /// To true string should return formatted datetime string from datetime.
        /// </summary>
        [TestMethod]
        public void ToTrueString_ShouldReturnDateimeString_FromDatetime()
        {
            Assert.AreEqual("20-Jun-20", new DateTime(20, 6, 20).ToTrueString());
        }

        /// <summary>
        /// Converts to ulong_shouldconverthexadecimal_toulong.
        /// </summary>
        [TestMethod]
        public void ToUlong_ShouldConvertHexaDecimal_ToULong()
        {
            ulong number = 1501484;
            Assert.AreEqual(number, "0x16e92c".ToUlong());
        }

        /// <summary>
        /// Converts to ulong_shouldconvertinvalidhexadecimal_todefaultulong.
        /// </summary>
        [TestMethod]
        public void ToUlong_ShouldConvertInvalidHexaDecimal_ToDefaultULong()
        {
            ulong number = 0;
            Assert.AreEqual(number, "0x16e92K".ToUlong());
        }
    }
}
