// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterNodeStatusToBlockchainSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Backend
{
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using TechTalk.SpecFlow;

    [Binding]
    public class RegisterNodeStatusToBlockchainSteps : EcpApiStepDefinitionBase
    {
        public RegisterNodeStatusToBlockchainSteps(FeatureContext featureContext)
           : base(featureContext)
        {
        }

        [When(@"I verify if Node record is created in db")]
        public void WhenIVerifyIfNodeRecordIsCreatedInDb()
        {
            // Yet to be implemented
        }

        [When(@"I update the order of nodes in a segment")]
        public void WhenIUpdateTheOrderOfNodesInASegment()
        {
           // Not yet implemented
        }

        [Then(@"I verify if node connection record is created in blockchain register")]
        public void ThenIVerifyIfNodeConnectionRecordIsCreatedInBlockchainRegister()
        {
           // yet to be implemented
        }
    }
}
