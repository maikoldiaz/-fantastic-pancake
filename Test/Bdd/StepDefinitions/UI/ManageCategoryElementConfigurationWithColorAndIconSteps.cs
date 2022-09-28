// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageCategoryElementConfigurationWithColorAndIconSteps.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class ManageCategoryElementConfigurationWithColorAndIconSteps : EcpWebStepDefinitionBase
    {
        [When(@"I select color ""(.*)"" (from "".*"" "".*"")")]
        public void WhenISelectColorFrom(string color, ElementLocator elementLocator)
        {
            Assert.IsNotNull(elementLocator);
            this.Get<ElementPage>().Click(nameof(Resources.Color), formatArgs: color);
        }

        [StepDefinition(@"validate selected color ""(.*)"" is displaying (in "".*"" "".*"")")]
        public void ThenValidateSelectedColorIsDisplayingIn(string color, ElementLocator elementLocator)
        {
            this.ValidateSelectedColorIsDisplayingIn(color, elementLocator);
        }

        [Then(@"validate no color is displaying (in "".*"" "".*"")")]
        [When(@"validate no color is displaying (in "".*"" "".*"")")]
        public void WhenValidateNoColorIsDisplayingIn(ElementLocator elementLocator)
        {
            var elementAttribute = this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("style");
            Assert.IsTrue(elementAttribute.EqualsIgnoreCase(string.Empty));
        }

        [When(@"I should not (see "".*"" "".*"")")]
        [Then(@"I should not (see "".*"" "".*"")")]
        public void ThenIShouldNotSee(ElementLocator elementLocator)
        {
            try
            {
                Assert.IsFalse(this.Get<ElementPage>().GetElement(elementLocator).Displayed);
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException)
            {
                Assert.IsTrue(true);
            }
        }

        [StepDefinition(@"validate available icons are displayed")]
        public async Task ThenValidateAvailableIconsAreDisplayedAsync()
        {
            var iconElements = this.Get<ElementPage>().GetElements(nameof(Resources.AllIcons));
            var iconDB = await this.ReadSqlScalarAsync<int>(ApiContent.Counts["Icons"]).ConfigureAwait(false);
            Assert.AreEqual(iconElements.Count, iconDB);
        }

        [When(@"I click on Icon Search Button")]
        public void WhenIClickOnIconSearchButton()
        {
            this.Get<ElementPage>().GetElement(nameof(Resources.IconSearch)).Click();
        }

        [When(@"click on ""(.*)"" icon")]
        [Then(@"click on ""(.*)"" icon")]
        public async Task ThenClickOnIconAsync(string icon)
        {
            var iconRow = await this.ReadSqlAsStringDictionaryAsync(input: UIContent.GetRow["IconByName"], args: new { iconName = icon }).ConfigureAwait(false);
            var iconValue = int.Parse(iconRow["IconId"], CultureInfo.InvariantCulture) - 1;
            this.Get<ElementPage>().GetElement(nameof(Resources.Icon), formatArgs: iconValue).Click();
        }

        [StepDefinition(@"validate selected icon ""(.*)"" is displayed (in "".*"" "".*"")")]
        public void ThenValidateSelectedIconIsDisplayedIn(string iconName, ElementLocator elementLocator)
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("value").ContainsIgnoreCase(iconName));
        }

        [StepDefinition(@"validate ""(.*)"" saved with ""(.*)"" and ""(.*)""")]
        public async Task ThenValidateSavedWithAndAsync(string entity, string icon, string color)
        {
            Assert.IsNotNull(entity);
            string categoryElementName;
            categoryElementName = this.GetValue(Entities.Keys.RandomFieldValue);
            ////            try
            ////            {
            ////                categoryElementName = this.GetValue("CategoryElementName");
            ////            }
            ////#pragma warning disable CA1031 // Do not catch general exception types
            ////            catch (Exception)
            ////#pragma warning restore CA1031 // Do not catch general exception types
            ////            {
            ////                categoryElementName = this.GetValue(Entities.Keys.RandomFieldValue);
            ////            }

            var categoryRow = await this.ReadSqlAsStringDictionaryAsync(UIContent.GetRow["CategoryElementByName"], args: new { name = categoryElementName }).ConfigureAwait(false);
            Assert.IsTrue(categoryRow["Color"].ContainsIgnoreCase(color));
            var iconRow = await this.ReadSqlAsStringDictionaryAsync(input: UIContent.GetRow["IconByName"], args: new { iconName = icon }).ConfigureAwait(false);
            var iconValue = iconRow["IconId"];
            Assert.IsTrue(categoryRow["IconId"].EqualsIgnoreCase(iconValue));
            this.SetValue("ElementName", categoryElementName);
            this.SetValue("CategoryElementName", categoryElementName);
        }

        [Given(@"I have ""(.*)"" with a color for a particular category")]
        public async Task GivenIHaveWithAColorForAParticularCategoryAsync(string entity)
        {
            Assert.IsNotNull(entity);
            ////this.When("I navigate to \"Category Elements\" page");
            this.UiNavigation("Category Elements");
            ////this.And("I click on \"Create Element\" \"button\"");
            this.IClickOn("Create Element", "button");
            ////this.And("I should see \"Create Element\" interface");
            this.IShouldSeeInterface("Create Element");
            ////this.And("I select \"Sistema\" from \"Category\"");
            this.SelectValueFromDropDown("Sistema", "Category");
            ////this.And("I provide value for \"element\" \"name\" \"textbox\"");
            this.IProvideValueFor("element\" \"name", "textbox");
            ////this.And("I provide value for \"element\" \"description\" \"textarea\"");
            this.IProvideValueFor("element\" \"description", "textarea");
            ////this.And("I click on Icon Search Button");
            this.Get<ElementPage>().GetElement(nameof(Resources.IconSearch)).Click();
            ////this.And("I should see \"Icon\" \"modal\"");
            this.IShouldSee("Icon", "modal");
            ////this.And("validate available icons are displayed");
            var iconElements = this.Get<ElementPage>().GetElements(nameof(Resources.AllIcons));
            var iconDB = await this.ReadSqlScalarAsync<int>(ApiContent.Counts["Icons"]).ConfigureAwait(false);
            Assert.AreEqual(iconElements.Count, iconDB);
            ////this.And("click on \"Barril\" icon");
            await this.ThenClickOnIconAsync("Barril").ConfigureAwait(false);
            ////this.And("I click on \"Submit\" \"Icon\" \"button\"");
            this.IClickOn("Submit\" \"Icon", "button");
            ////this.And("validate selected icon \"Barril\" is displayed in \"icon\" \"textbox\"");
            this.ValidateSelectedIconIsDisplayedIn("Barril", "icon", "textbox");
            ////this.And("I click on \"Picker\" \"Control\" \"color\"");
            this.IClickOn("Picker\" \"Control", "color");
            ////this.And("I should see \"Picker\" \"Overlay\" \"color\"");
            this.IShouldSee("Picker\" \"Overlay", "color");
            ////this.And("I select color \"9c27b0\" from \"Picker\" \"Overlay\" \"color\"");
            this.Get<ElementPage>().Click(nameof(Resources.Color), formatArgs: "9c27b0");
            ////this.And("validate selected color \"9c27b0\" is displaying in \"Picker\" \"Control\" \"color\"");
            this.ValidateSelectedColorIsDisplayingIn("9c27b0", "Picker\" \"Control", "color");
            ////this.And("I click on \"element\" \"submit\" \"button\"");
            this.IClickOn("element\" \"submit", "button");
            this.SetValue("SistemaName", this.GetValue(Entities.Keys.RandomFieldValue));
        }

        [Then(@"validate the error message ""(.*)""")]
        public void ThenValidateTheErrorMessage(string message)
        {
            var errorMsgElement = this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessageInInicioTab)).Text;
            var expectedMsg = message?.Split('[')[0] + this.GetValue("ElementName");
            Assert.IsTrue(errorMsgElement.EqualsIgnoreCase(expectedMsg));
        }
    }
}
