// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryDesignUpdateSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System.Linq;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class InventoryDesignUpdateSteps : EcpWebStepDefinitionBase
    {
        [Then(@"validate the unbalances are calculated in the grid")]
        public void ThenValidateTheUnbalancesAreCalculatedInTheGrid()
        {
            var connectionGridRow = this.Get<ElementPage>().GetElements(nameof(Resources.GridRow), formatArgs: "unbalances");
            Assert.IsNotNull(connectionGridRow);
        }

        [Then(@"validate inventory is registered with ""(.*)"" products")]
        public async System.Threading.Tasks.Task ThenValidateInventoryIsRegisteredWithProductsAsync(string product)
        {
            Assert.IsNotNull(product);
            var lastCreatedRow = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow["IventoryWithMultipleProducts"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            Assert.AreEqual(2, lastCreatedRow.Count());
        }
    }
}
