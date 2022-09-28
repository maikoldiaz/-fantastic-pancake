// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdjustmentsToOperatingBalanceProcessesSteps.cs" company="Microsoft">
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
    using TechTalk.SpecFlow;

    [Binding]
    public class AdjustmentsToOperatingBalanceProcessesSteps : EcpWebStepDefinitionBase
    {
        [When(@"I select Current Date from ""(.*)"" ""(.*)"" ""(.*)""")]
        public void WhenISelectCurrentDateFrom(string p0, string p1, string p2)
        {
            Console.WriteLine(p0 + p1 + p2);
        }
    }
}
