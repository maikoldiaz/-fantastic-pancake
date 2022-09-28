// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageInventoriesSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Api
{
    using System;
    using System.Globalization;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class ManageInventoriesSteps : EcpApiStepDefinitionBase
    {
        public ManageInventoriesSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [When(@"I have (.*) inventories")]
        [When(@"I have (.*) inventory")]
        public void WhenIHaveInventory(int count)
        {
            this.SetValue(ConstantValues.TestData, "WithScenarioId");
            this.SetValue(ConstantValues.BATCHID, new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
            this.CommonMethodForInventoryRegistration(count);
        }

        [When(@"I have (.*) inventory with ""(.*)"" products")]
        public void WhenIHaveInventoryWithProducts(int count, string products)
        {
            Assert.IsNotNull(products);
            JArray inventoryArray = new JArray();
            string content = ApiContent.Creates["InventoriesWithMultipleProducts"];
            for (int i = 0; i < count; i++)
            {
                var id = new Faker().Random.Number(9999, 999999).ToString(CultureInfo.InvariantCulture);
                this.SetValue("InventoryId", id);
                content = content.JsonChangePropertyValue("inventoryId", id);
                content = content.JsonChangePropertyValue(ConstantValues.InventoryDate, DateTime.Now.AddDays(-2).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                if (this.GetValue(Entities.Keys.EntityType).ContainsIgnoreCase("Homologated"))
                {
                    content = content.JsonChangePropertyValue("NodeId", this.GetValue("NodeId_1"));
                    content = content.JsonChangePropertyValue(ConstantValues.ProductTypeId, this.GetValue("ProductTypeId"));
                    content = content.JsonChangePropertyValue("Segment", this.GetValue("SegmentId"));
                }

                inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }
    }
}