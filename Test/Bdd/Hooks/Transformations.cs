// <copyright file="Transformations.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace Ecp.True.Bdd.Tests.Hooks
{
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Entities;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class Transformations
    {
        public Transformations(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.FeatureContext = featureContext;
            this.ScenarioContext = scenarioContext;
        }

        public ScenarioContext ScenarioContext { get; }

        protected FeatureContext FeatureContext { get; }

        // Step-Argument Transformation!
        // When I select an item from "inputs" "dropdown"
        // When I click on "some panel" "save" "button"
        // When I enter some text into "text" "textbox"
        [StepArgumentTransformation(@"from ""(.*)"" ""(.*)""")] // Drop-down
        [StepArgumentTransformation(@"on ""(.*)"" ""(.*)""")] // Button
        [StepArgumentTransformation(@"into ""(.*)"" ""(.*)""")] // Text-box
        [StepArgumentTransformation(@"for ""(.*)"" ""(.*)""")] // Text-box
        [StepArgumentTransformation(@"see ""(.*)"" ""(.*)""")] // Modal Window
        [StepArgumentTransformation(@"in ""(.*)"" ""(.*)""")] // Label
        [StepArgumentTransformation(@"that ""(.*)"" ""(.*)""")] // Validate
        [StepArgumentTransformation(@"""(.*)"" ""(.*)"" should")] // Button
        public ElementLocator GetElementLocator(string item, string type)
        {
            var locator = ElementExtensions.GetLocator(item, type);
            return locator;
        }

        [StepArgumentTransformation(@"authenticated as ""(.*)""")]
        [StepArgumentTransformation(@"logged in as ""(.*)""")]
        public Credentials GetUser(string role)
        {
            var user = ElementExtensions.GetUserCredentials(this.FeatureContext, this.ScenarioContext, role);
            return user;
        }
    }
}