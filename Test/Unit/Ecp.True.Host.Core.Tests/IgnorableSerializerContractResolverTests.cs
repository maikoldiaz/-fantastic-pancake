// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IgnorableSerializerContractResolverTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Tests
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Query;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The custom validation attribute adapter provider tests.
    /// </summary>
    [TestClass]
    public class IgnorableSerializerContractResolverTests
    {
        /// <summary>
        /// Serializers the should ignore configured properties.
        /// </summary>
        [TestMethod]
        public void Serializer_ShouldIgnoreConfiguredProperties()
        {
            var contractResolver = new IgnorableSerializerContractResolver();
            contractResolver.Ignore(typeof(PreviousInventoryOperationalData), nameof(PreviousInventoryOperationalData.IsOwnershipCalculated));
            contractResolver.Ignore(typeof(PreviousInventoryOperationalData), nameof(PreviousInventoryOperationalData.OwnershipPercentage));
            contractResolver.Ignore(typeof(PreviousMovementOperationalData), nameof(PreviousMovementOperationalData.AppliedRule));
            contractResolver.Ignore(typeof(PreviousMovementOperationalData), nameof(PreviousMovementOperationalData.OwnershipPercentage));

            var serializer = new JsonSerializer { ContractResolver = contractResolver, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var prevInventoryOperationalDataData = new PreviousInventoryOperationalData
            {
                InventoryId = 1344,
                IsOwnershipCalculated = true,
                NodeId = 2,
                OwnershipPercentage = 34.54M,
                OwnerId = 3,
            };

            var prevMovementOperationalDataData = new PreviousMovementOperationalData
            {
                MovementId = 1344,
                AppliedRule = 0,
                OwnershipPercentage = 34.54M,
                OwnerId = 3,
            };

            var serializedPreviousInventoryOperationalData = JObject.FromObject(prevInventoryOperationalDataData, serializer);
            var serializedPreviousMovementOperationalData = JObject.FromObject(prevMovementOperationalDataData, serializer);

            Assert.IsFalse(serializedPreviousInventoryOperationalData.ContainsKey(nameof(PreviousInventoryOperationalData.IsOwnershipCalculated)));
            Assert.IsFalse(serializedPreviousInventoryOperationalData.ContainsKey(nameof(PreviousInventoryOperationalData.OwnershipPercentage)));
            Assert.IsFalse(serializedPreviousMovementOperationalData.ContainsKey(nameof(PreviousMovementOperationalData.AppliedRule)));
            Assert.IsFalse(serializedPreviousMovementOperationalData.ContainsKey(nameof(PreviousMovementOperationalData.OwnershipPercentage)));
            Assert.IsTrue(serializedPreviousInventoryOperationalData.ContainsKey("idNodo"));
        }
    }
}
