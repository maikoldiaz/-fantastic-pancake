// <copyright file="ExcelBuilder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Builders.Excel.Common
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Builder Service.
    /// </summary>
    public abstract class ExcelBuilder : IExcelBuilder
    {
        /// <summary>
        /// Builds the excel asynchronous.
        /// </summary>
        /// <param name="element">The excel element.</param>
        /// <returns>
        /// The built object.
        /// </returns>
        public abstract Task<JObject> BuildAsync(ExcelInput element);

        /// <summary>Gets the value.</summary>
        /// <returns>The value.</returns>
        /// <param name="row">The row.</param>
        /// <param name="key">The node.</param>
        protected static string GetValue(DataRow row, string key)
        {
            ArgumentValidators.ThrowIfNull(row, nameof(row));
            return row[key].ToString();
        }

        /// <summary>Gets the value.</summary>
        /// <returns>The value.</returns>
        /// <param name="row">The row.</param>
        /// <param name="key">The node.</param>
        /// <param name="isRequired">The isRequired.</param>
        /// <param name="errorMessage">The error message.</param>
        protected static string GetValue(DataRow row, string key, bool isRequired, string errorMessage)
        {
            ArgumentValidators.ThrowIfNull(row, nameof(row));
            if (isRequired && !row.Table.Columns.Contains(key))
            {
                throw new ArgumentException($"{row.Table.TableName} does not contain {key} column.");
            }

            if (isRequired && string.IsNullOrEmpty(row[key].ToString()))
            {
                throw new MissingFieldException(errorMessage);
            }

            return row[key].ToString();
        }

        /// <summary>Gets the value.</summary>
        /// <returns>The value.</returns>
        /// <param name="row">The row.</param>
        /// <param name="key">The key.</param>
        /// <param name="mappingKey">The mapping key.</param>
        /// <param name="dataType">The datatype.</param>
        /// <param name="isValueString">The is Value string.</param>
        protected static JProperty GetValue(DataRow row, string key, string mappingKey, JTokenType dataType, bool isValueString = false)
        {
            ArgumentValidators.ThrowIfNull(row, nameof(row));
            if ((dataType == JTokenType.Date && !row[key].IsDate()) || (dataType == JTokenType.Float && !row[key].IsNumber()))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Constants.IncorrectDatatypeFailureMessage, key));
            }

            return new JProperty(mappingKey, !isValueString ? row[key] : row[key].ToString());
        }

        /// <summary>Gets the value.</summary>
        /// <returns>The value.</returns>
        /// <param name="row">The row.</param>
        /// <param name="key">The key.</param>
        /// <param name="mappingKey">The mapping key.</param>
        /// <param name="dataType">The datatype.</param>
        /// <param name="isValueString">The is Value string.</param>
        /// <param name="isValueNull">The is Value null.</param>
        protected static JProperty GetValue(DataRow row, string key, string mappingKey, JTokenType dataType, bool isValueString, bool isValueNull)
        {
            ArgumentValidators.ThrowIfNull(row, nameof(row));
            if ((dataType == JTokenType.Date && !row[key].IsDate()) || (!isValueNull && dataType == JTokenType.Float && !row[key].IsNumber()))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Constants.IncorrectDatatypeFailureMessage, key));
            }

            return new JProperty(mappingKey, !isValueString ? row[key] : row[key].ToString());
        }

        /// <summary>ValidateNull.</summary>
        /// <returns>The value.</returns>
        /// <param name="row">The Origen.</param>
        protected static JProperty ValidateOrigenNull(DataRow row)
        {
            var origen = GetValue(row, "SistemaOrigen", "SourceSystemId", JTokenType.None);
            if (string.IsNullOrEmpty(origen.Value.ToString()))
            {
                throw new InvalidDataException(Entities.Constants.ContractSourceSystemRequired);
            }

            return origen;
        }
    }
}