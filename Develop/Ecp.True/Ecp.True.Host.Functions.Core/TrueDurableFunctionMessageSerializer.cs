// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrueDurableFunctionMessageSerializer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core
{
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Newtonsoft.Json;

    /// <summary>
    /// The Message Serializer.
    /// </summary>
    /// <seealso cref="Microsoft.Azure.WebJobs.Extensions.DurableTask.IMessageSerializerSettingsFactory" />
    public class TrueDurableFunctionMessageSerializer : IMessageSerializerSettingsFactory
    {
        /// <summary>
        /// Creates or retrieves settings to be used throughout the extension for message serialization.
        /// </summary>
        /// <returns>
        ///   The serializer to be used by the Durable Task Extension for message serialization.
        /// </returns>
        public JsonSerializerSettings CreateJsonSerializerSettings() => new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        };
    }
}
