namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System.Linq;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class PagesUnavailableSteps : EcpWebStepDefinitionBase
    {
        [Then(@"I validate below error details are displayed")]
        public void ThenIValidateBelowErrorDetailsAreDisplayed(Table table)
        {
            var dict = table?.Rows.ToDictionary(r => r[0], r => r[1]);

            foreach (string key in dict.Keys)
            {
                string keyValue = dict[key].ToString();

                if (key.Contains("code"))
                {
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.Error), formatArgs: key);
                    Assert.AreEqual(keyValue, this.Get<ElementPage>().GetElement(nameof(Resources.Error), formatArgs: key).Text);
                }
                else if (key.Contains("title"))
                {
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.Error), formatArgs: key);
                    Assert.AreEqual(keyValue, this.Get<ElementPage>().GetElement(nameof(Resources.Error), formatArgs: key).Text);
                }
                else if (key.Contains("desc"))
                {
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.Error), formatArgs: key);
                    Assert.AreEqual(keyValue, this.Get<ElementPage>().GetElement(nameof(Resources.Error), formatArgs: key).Text);
                }
                else if (key.Contains("link"))
                {
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.Error), formatArgs: key);
                    Assert.IsTrue(keyValue.EqualsIgnoreCase(this.Get<ElementPage>().GetElement(nameof(Resources.Error), formatArgs: key).Text));
                }
            }
        }

        [When(@"I click on Más información ""(.*)""")]
        public void WhenIClickOn(string type)
        {
            this.Get<ElementPage>().Click(nameof(Resources.Error), formatArgs: type);
        }

        [Then(@"I validate below options available under ""(.*)"" section of Attention Model pop-up ""(.*)""")]
        public void ThenIValidateBelowOptionsAvailableUnderSectionOfAttentionModelPopUp(string title, string modelTitle, Table table)
        {
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.UploadTypeName), formatArgs: modelTitle);

            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.UploadTypeName), formatArgs: title);

            var dict = table?.Rows.ToDictionary(r => r[0]);

            foreach (string key in dict.Keys)
            {
                this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.UploadTypeName), formatArgs: key);
            }
        }

        [Then(@"I validate ""(.*)"" option has ""(.*)"" displayed from system parameters")]
        public void ThenIValidateOption(string item, string option)
        {
            string hrefvalue = this.Get<ElementPage>().GetAttributeValue(nameof(Resources.PhoneorEmailForOption), "href", formatArgs: item);
            Assert.True(hrefvalue.Contains(option));
        }

        [Then(@"I validate ""(.*)"" option has phone number with extension as ""(.*)"" displayed")]
        public void ThenIValidateOptionHasExtensionAsIsDisplayed(string item, string option)
        {
            string extension = this.Get<ElementPage>().GetElement(nameof(Resources.PhoneExtension), formatArgs: item).Text;
            Assert.AreEqual(option, this.Get<ElementPage>().GetElement(nameof(Resources.PhoneExtension), formatArgs: item).Text);
        }

        [Then(@"I validate below report steps with descriptions are displayed under ""(.*)"" section of Attention Model pop-up")]
        public void ThenIValidateBelowReportStepsWithDescriptionsAreDisplayedOnAttentionModelPopUp(string title, Table table)
        {
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.UploadTypeName), formatArgs: title);

            var dict = table?.Rows.ToDictionary(r => r[0], r => r[1]);

            foreach (string key in dict.Keys)
            {
                this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.UploadTypeName), formatArgs: key);
            }
        }
    }
}
