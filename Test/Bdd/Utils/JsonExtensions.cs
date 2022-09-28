// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonExtensions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Utils
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class JsonExtensions
    {
        public static string JsonChangePropertyValue(this string content, string field, string fieldValue = null)
        {
            var model = JsonConvert.DeserializeObject<JObject>(content);
            var split = field?.Split(' ');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            var path = JsonPaths.Mappings.ContainsKey(parent) ? JsonPaths.Mappings[parent] : string.Empty;
            field = path + child.ToCamelCase();
            var token = model.SelectToken(field);
            token.Replace(fieldValue);
            return model.ToString();
        }

        public static string JsonChangePropertyValue(this string content, string field, dynamic fieldValue)
        {
            var model = JsonConvert.DeserializeObject<JObject>(content);
            var split = field?.Split(' ');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            if (parent.EqualsIgnoreCase("productId") && child.EqualsIgnoreCase("productId"))
            {
                field = child.ToCamelCase();
            }
            else
            {
                var path = JsonPaths.Mappings.ContainsKey(parent) ? JsonPaths.Mappings[parent] : string.Empty;
                field = path + child.ToCamelCase();
            }

            var token = model.SelectToken(field);
            token.Replace(fieldValue);
            return model.ToString();
        }

        public static string JsonChangePropertyValue(this string content, string field, bool fieldValue)
        {
            var model = JsonConvert.DeserializeObject<JObject>(content);
            var split = field?.Split(' ');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            var path = JsonPaths.Mappings.ContainsKey(parent) ? JsonPaths.Mappings[parent] : string.Empty;
            field = path + child.ToCamelCase();
            var token = model.SelectToken(field);
            token.Replace(fieldValue);
            return model.ToString();
        }

        public static string JsonGetValue(this string content, string field)
        {
            return JObject.Parse(content)[field.ToCamelCase()].ToString();
        }

        public static string JsonGetFromValue(this string content, string field)
        {
            var model = JsonConvert.DeserializeObject<JObject>(content);
            var split = field?.Split(' ');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            var path = JsonPaths.Mappings.ContainsKey(parent) ? JsonPaths.Mappings[parent] : string.Empty;
            field = path + child.ToCamelCase();
            var token = model.SelectToken(field);
            return token.ToString();
        }

        public static string JsonGetValueforSP(this string content, string field)
        {
            return JObject.Parse(content)[field].ToString();
        }

        public static string JsonChangeValue(this string content, string field = "name", string suffix = "_")
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(content);
            jsonObj[field] = string.Concat($"Automation{suffix}", new Faker().Random.AlphaNumeric(5));
            return JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        }

        public static string JsonChangeValueInArray(this string content, string field, string value)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(content);
            var split = field?.Split('_');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            jsonObj[parent][0][child][0]["productId"] = value;
            return JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        }

        public static string JsonRemoveObject(this string content, string property)
        {
            dynamic model = JsonConvert.DeserializeObject<JObject>(content);
            var split = property?.Split(' ');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            var path = JsonPaths.Mappings.ContainsKey(parent) ? JsonPaths.Mappings[parent] : string.Empty;
            property = child.ToCamelCase();
            if (!string.IsNullOrEmpty(path) && !path.TrimEnd('.').EqualsIgnoreCase(property))
            {
                var validJobject = (JObject)model.SelectToken(path.TrimEnd('.'));
                validJobject.Property(property, StringComparison.InvariantCulture).Remove();
            }
            else if (property == "ownersOfSapPo")
            {
                property = "owners";
                model.Property(property, StringComparison.InvariantCulture).Remove();
            }
            else if (property == "attributesOfSapPo")
            {
                property = "attributes";
                model.Property(property, StringComparison.InvariantCulture).Remove();
            }
            else if (property == "owners1")
            {
                property = "owners";
                model.Property(property, StringComparison.InvariantCulture).Remove();
            }
            else
            {
                model.Property(property, StringComparison.InvariantCulture).Remove();
            }

            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }

        public static string JsonAddObject(this string content, string field = null, string value = null)
        {
            var model = JsonConvert.DeserializeObject<JObject>(content);
            var split = field?.Split(' ');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            var path = JsonPaths.Mappings.ContainsKey(parent) ? JsonPaths.Mappings[parent] : string.Empty;
            field = path;
            var token = model.SelectToken(field + '.' + child + "[0]");
            token.AddAfterSelf(JObject.Parse(value));
            return JsonConvert.SerializeObject(model);
        }

        public static string JsonAddField(this string content, string field = null, dynamic value = default)
        {
            var model = JsonConvert.DeserializeObject<JObject>(content);
            var split = field?.Split(' ');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            var path = JsonPaths.Mappings.ContainsKey(parent) ? JsonPaths.Mappings[parent] : string.Empty;
            field = path + child.ToCamelCase();
            split = field?.Split('.');
#pragma warning disable S1854 // Unused assignments should be removed
            JToken token = JToken.Parse(JsonConvert.SerializeObject(model));
#pragma warning restore S1854 // Unused assignments should be removed
            if (split.Length == 1)
            {
                model[split[0]] = value;
            }
            else if (split.Length == 2)
            {
                token = model.SelectToken(split[0]);
                token[split[1]] = value;
            }
            else if (split.Length == 3)
            {
                token = model.SelectToken(split[0]).SelectToken(split[1]);
                token[split[2]] = value;
            }

            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }

        public static string JsonAddFieldAt(this string content, string toAdd, string field = null, string value = null)
        {
            var model = JsonConvert.DeserializeObject<JObject>(content);
            if (field == null && toAdd != null)
            {
                model[toAdd] = value;
                var token = model;
                return JsonConvert.SerializeObject(token, Formatting.Indented);
            }
            else if (field != null && toAdd == null)
            {
                var split = field?.Split(' ');
                var parent = split.FirstOrDefault();
                var child = split.LastOrDefault();
                var path = JsonPaths.Mappings.ContainsKey(parent) ? JsonPaths.Mappings[parent] : string.Empty;
                field = path + child.ToCamelCase();
                var token = model.SelectToken(field);
                token[field] = value;
                return JsonConvert.SerializeObject(token, Formatting.Indented);
            }

            return JsonConvert.SerializeObject(content, Formatting.Indented);
        }

        public static string JArrayChangePropertyValue(this string content, string field, string fieldValue = null)
        {
            var jArrayResult = JArray.Parse(content);
            var split = field?.Split(' ');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            split = parent?.Split('_');
            var index = split.LastOrDefault();
            var jsonString = jArrayResult[Convert.ToInt32(index, System.Globalization.CultureInfo.InvariantCulture) - 1].ToString();
            var model = JsonConvert.DeserializeObject<JObject>(jsonString);
            var token = model.SelectToken(child.ToCamelCase());
            token.Replace(fieldValue);
            jArrayResult[Convert.ToInt32(index, System.Globalization.CultureInfo.InvariantCulture) - 1] = model.ToString();
            var finalContent = "[";
            for (var i = 0; i < jArrayResult.Count; i++)
            {
                finalContent += jArrayResult[i] + (jArrayResult.Count.Equals(i + 1) ? string.Empty : ",");
            }

            finalContent += "]";
            return finalContent;
        }

        public static string JArrayModifyPropertyValue(this string content, string field, string fieldValue = null)
        {
            var jArrayResult = JArray.Parse(content);
            var split = field?.Split(' ');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            split = parent?.Split('_');
            var index = split.LastOrDefault();
            var jsonString = jArrayResult[Convert.ToInt32(index, System.Globalization.CultureInfo.InvariantCulture) - 1].ToString();
            var model = JsonConvert.DeserializeObject<JObject>(jsonString);
            var path = JsonPaths.Mappings.ContainsKey(split.FirstOrDefault()) ? JsonPaths.Mappings[split.FirstOrDefault()] : string.Empty;
            field = path + child.ToCamelCase();
            var token = model.SelectToken(field);
            token.Replace(fieldValue);
            jArrayResult[Convert.ToInt32(index, System.Globalization.CultureInfo.InvariantCulture) - 1] = model.ToString();
            var finalContent = "[";
            for (var i = 0; i < jArrayResult.Count; i++)
            {
                finalContent += jArrayResult[i] + (jArrayResult.Count.Equals(i + 1) ? string.Empty : ",");
            }

            finalContent += "]";
            return finalContent;
        }

        public static string JarrayGetValue(this string content, string field)
        {
            var jArrayResult = JArray.Parse(content);
            var split = field?.Split(' ');
            var parent = split.FirstOrDefault();
            var child = split.LastOrDefault();
            split = parent?.Split('_');
            var index = split.LastOrDefault();
            var jsonString = jArrayResult[Convert.ToInt32(index, System.Globalization.CultureInfo.InvariantCulture) - 1].ToString();
            var model = JsonConvert.DeserializeObject<JObject>(jsonString);
            var path = JsonPaths.Mappings.ContainsKey(split.FirstOrDefault()) ? JsonPaths.Mappings[split.FirstOrDefault()] : string.Empty;
            field = path + child.ToCamelCase();
            var token = model.SelectToken(field);
            return token.ToString();
        }
    }
}
