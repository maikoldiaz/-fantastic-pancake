// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockNumberConverter.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Ecp.True.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// The block number converter for hex block numbers.
    /// </summary>
    public class BlockNumberConverter : JsonConverter<ulong>
    {
        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override ulong ReadJson(JsonReader reader, Type objectType, [AllowNull] ulong existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            ArgumentValidators.ThrowIfNull(reader, nameof(reader));

            var blockNumber = reader.Value.ToString();
            if (reader.TokenType == JsonToken.String && blockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return blockNumber.ToUlong();
            }

            return Convert.ToUInt64(reader.Value, CultureInfo.InvariantCulture);
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, [AllowNull] ulong value, JsonSerializer serializer)
        {
            // Do nothing
        }
    }
}
