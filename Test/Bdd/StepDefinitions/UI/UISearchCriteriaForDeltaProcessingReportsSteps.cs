namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class UISearchCriteriaForDeltaProcessingReportsSteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should see sucessful report Generation")]
        public void ThenIShouldSeeSucessfulReportGeneration()
        {
            this.DriverContext.Driver.SwitchTo().Frame(0);
            var reportTitle = this.Get<ElementPage>().GetElement(new ElementLocator(Ocaramba.Locator.XPath, "//*[text()=\"Balance Oficial Inicial Cargado\"]"));
            Assert.IsTrue(reportTitle.Displayed);
        }

        [Then(@"I should see sucessful report Generation ""(.*)""")]
        public void ThenIShouldSeeSucessfulReportGeneration(string reportName)
        {
            this.DriverContext.Driver.SwitchTo().Frame(0);
            var reportTitle = this.Get<ElementPage>().GetElement(new ElementLocator(Ocaramba.Locator.XPath, "//*[text()=\"" + reportName + "\"]"));
            Assert.IsTrue(reportTitle.Displayed);
        }

        [When(@"I select first item from Node dropdown by keyingin ""(.*)""")]
        public void WhenISelectFirstItemFromNodeDropdownByKeyingin(string text)
        {
            this.IEnterValueInto(text, new ElementLocator(Ocaramba.Locator.XPath, "//label[@id=\"lbl_nodeFilter_node\"]/following-sibling::div//input"));
            this.IClickOn(new ElementLocator(Ocaramba.Locator.XPath, "//label[@id=\"lbl_nodeFilter_node\"]/following-sibling::div//input/following-sibling::div//div[contains(text(),\"Automation\")]"));
        }

        [When(@"I select first item from Segment dropdown by keyingin")]
        public async Task WhenISelectFirstItemFromSegmentDropdownByKeyinginAsync()
        {
            var segment = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSegmentForDeltaCalReport).ConfigureAwait(false);
            this.IEnterValueInto(segment[ConstantValues.Name], new ElementLocator(Ocaramba.Locator.XPath, "//label[@id=\"lbl_nodeFilter_element\"]/following-sibling::div//input"));
            this.IClickOn(new ElementLocator(Ocaramba.Locator.XPath, "//div[contains(@class,'ep-select__option ep-select') and text()=\"" + segment[ConstantValues.Name] + "\"]"));
        }

        [When(@"I select first item from Not in Segment dropdown by keyingin")]
        public async Task WhenISelectFirstItemFromNotInSegmentDropdownByKeyinginAsync()
        {
            var segment = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetSegmentNotForDeltaCalReport).ConfigureAwait(false);
            this.IEnterValueInto(segment[ConstantValues.Name], new ElementLocator(Ocaramba.Locator.XPath, "//label[@id=\"lbl_nodeFilter_element\"]/following-sibling::div//input"));
            IWebElement firstItem = this.Get<ElementPage>().GetElement(new ElementLocator(Ocaramba.Locator.XPath, "//div[contains(@class,\"ep-select__option\") and  contains(text(),\"" + segment[ConstantValues.Name] + "\")]"));
            firstItem.Click();
        }

        [When(@"I select first item from Period dropdown")]
        public void WhenISelectFirstItemFromPeriodDropdown()
        {
            this.IClickOn(new ElementLocator(Ocaramba.Locator.XPath, "//*[text()=\"Período\"]/following-sibling::div//*[text()=\"Seleccionar\"]"));
            this.IClickOn(new ElementLocator(Ocaramba.Locator.XPath, "//div[contains(@id,'react-select')]"));
        }

        [Then(@"validation message ""(.*)""")]
        public void ThenValidationMessage(string text)
        {
            IWebElement notificationMessage = this.Get<ElementPage>().GetElement(new ElementLocator(Ocaramba.Locator.XPath, "//div[@class=\"ep-notification ep-notification--error\"]"));
            Assert.IsTrue(text == notificationMessage.GetAttribute("innerText").Replace("\r\n", string.Empty));
        }

        [Then(@"Ver Reporte button should be disabled")]
        public void ThenButtonShouldBeDisabled()
        {
            IWebElement viewReportButton = this.Get<ElementPage>().GetElement(new ElementLocator(Ocaramba.Locator.XPath, "//button[@id=\"btn_nodeFilter_viewReport\"]"));
            Assert.IsFalse(viewReportButton.Enabled);
        }

        [Then(@"I should not see Page")]
        public void ThenIShouldNotSeePage(Table table)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            var dict = table.Rows.ToDictionary(r => r[0]);
#pragma warning restore CA1062 // Validate arguments of public methods
            IWebElement link = this.Get<ElementPage>().GetElement(new ElementLocator(Ocaramba.Locator.XPath, "//span[text()=\"Gestión cadena de suministro\"]"));
            link.Click();
            foreach (var data in dict.Keys)
            {
                int pageCount = this.DriverContext.Driver.FindElements(By.XPath(string.Empty)).Count;
            }
        }

        [Then(@"I should not see ""(.*)""")]
        public void ThenIShouldNotSee(string pageName, Table table)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            var dict = table.Rows.ToDictionary(r => r[0]);
#pragma warning restore CA1062 // Validate arguments of public methods
            IWebElement parentLink = this.Get<ElementPage>().GetElement(new ElementLocator(Ocaramba.Locator.XPath, "//i[@class=\"ep-icn ep-icn--supplyChain\"]"));
            parentLink.Click();
            IWebElement link = this.Get<ElementPage>().GetElement(new ElementLocator(Ocaramba.Locator.XPath, "//span[text()=\"Gestión cadena de suministro\"]"));
            link.Click();
            int pageCount = 1;
            foreach (var data in dict.Keys)
            {
                pageCount = this.DriverContext.Driver.FindElements(By.XPath("//span[text()=\"" + pageName + "\"]")).Count;
            }

            Assert.IsTrue(pageCount == 0);
        }

        [When(@"I click on filter year")]
        public void WhenIClickOnFilterYear()
        {
            this.IClickOn(new ElementLocator(Ocaramba.Locator.XPath, "//*[text()=\"Año\"]/following-sibling::div"));
            IList<IWebElement> years = this.Get<ElementPage>().GetElements(new ElementLocator(Ocaramba.Locator.XPath, "//div[contains(@class,\"ep-select__menu-list\")]//div"));
            List<int> yearsParsed = new List<int>();
            foreach (IWebElement element in years)
            {
#pragma warning disable CA1305 // Specify IFormatProvider
                yearsParsed.Add(Convert.ToInt32(element.Text));
#pragma warning restore CA1305 // Specify IFormatProvider
            }

            NUnit.Framework.Assert.That(yearsParsed, Is.Ordered.Descending);
            years[0].Click();
        }

        [Then(@"Format of the period should be XXX-XX")]
        public void ThenFormatOfThePeriodShouldBe()
        {
            IWebElement period = this.Get<ElementPage>().GetElement(new ElementLocator(Ocaramba.Locator.XPath, "//*[text()=\"Período\"]//following-sibling::div"));
            string[] periodText = period.GetAttribute("innerText").Split(new[] { System.Environment.NewLine }, StringSplitOptions.None);
            string regString = @"(\d+)-([a-zA-Z]+)";
            Regex regex = new Regex(regString);
            bool isFormatMatch = regex.IsMatch(periodText.Last());
            Assert.IsTrue(isFormatMatch);
        }
    }
}