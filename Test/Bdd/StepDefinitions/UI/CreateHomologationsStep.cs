// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateHomologationsStep.cs" company="Microsoft">
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
    using System;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class CreateHomologationsStep : EcpWebStepDefinitionBase
    {
        [Given(@"I do not have Homologation group in the system")]
        public async Task GivenIRemoveAllHomologationGroupsAsync()
        {
            try
            {
                await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.DeleteAllHomologationData).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
