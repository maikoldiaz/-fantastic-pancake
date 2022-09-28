// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementExtensions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Executors;
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Api.Executors;
    using global::Bdd.Core.Entities;
    using global::Bdd.Core.StepDefinitions;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using OpenQA.Selenium.Interactions;

    using TechTalk.SpecFlow;

    public static class ElementExtensions
    {
        private static readonly Dictionary<string, (string Prefix, string Suffix)> Ids = new Dictionary<string, (string Prefix, string Suffix)>(StringComparer.OrdinalIgnoreCase)
        {
            { "combobox", ("dd_", string.Empty) }, // Combination of select-box and text-field
            { "dropdown", ("dd_", string.Empty) }, // Drop-down link
            { "menu", ("react-select-dd_", "--list") }, // Drop-down menu
            { "button", ("btn_", string.Empty) },
            { "toggle", ("tog_", string.Empty) }, // Toggles
            { "textbox", ("txt_", string.Empty) },
            { "date", ("dt_", string.Empty) },
            { "tab", ("tab_", string.Empty) },
            { "textarea", ("txtarea_", string.Empty) },
            { "link", ("lnk_", string.Empty) },
            { "interface", ("frm_", string.Empty) },
            { "list", ("li_", string.Empty) },
            { "container", ("cont_", string.Empty) },
            { "label", ("lbl_", string.Empty) },
            { "form", ("frm_", string.Empty) },
            { "checkbox", ("chk_", string.Empty) },
            { "datepicker", ("dp_", string.Empty) },
            { "color", ("col-", string.Empty) },
            { "modal", (string.Empty, "_modal") },
            { "grid", ("grd_", string.Empty) },
            { "table", ("tbl_", string.Empty) },
            { "header", ("h1_", string.Empty) },
            { "message", ("msg_", string.Empty) },
            { "icon", (string.Empty, "_icon") },
        };

        public static ElementLocator GetLocator(string item, string type)
        {
            var id = string.Empty;
            var items = item?.Split('"');
            if (items.Length > 1)
            {
                var parent = items.FirstOrDefault();
                var child = items.LastOrDefault();
                id = Ids[type].Prefix + (string.IsNullOrWhiteSpace(parent) ? string.Empty : parent.ToCamelCase() + "_") + (items.Length > 5 ? (items[items.Length - 5].ToCamelCase() + "_") : string.Empty) + (items.Length > 3 ? (items[items.Length - 3].ToCamelCase() + "_") : string.Empty) + (items[2] == "categoryElement_name" || items[2] == "node_name" || items[2] == "owner_name" ? child : child.ToCamelCase()) + Ids[type].Suffix;
            }
            else
            {
                id = Ids[type].Prefix + item.ToCamelCase() + Ids[type].Suffix;
            }

            var locator = new ElementLocator(Ocaramba.Locator.XPath, $"//*[contains(@id, '{id}')]");
            return locator;
        }

        public static Credentials GetUserCredentials(FeatureContext featureContext, ScenarioContext scenarioContext, string role)
        {
            var user = scenarioContext.GetCredential<Credentials>(featureContext, role, "input=Credentials.xlsx");
            return user;
        }

        public static async Task IAmAuthenticatedForUserAsync(this StepDefinitionBase step, ApiExecutor apiExecutor, string role)
        {
            step.ThrowIfNull(nameof(step));
            var userCredentials = GetUserCredentials(step.FeatureContext, step.ScenarioContext, role);
            step.ScenarioContext[nameof(step.UserDetails)] = userCredentials;
            await step.TokenForUserAsync(apiExecutor, userCredentials).ConfigureAwait(false);
        }

        public static void LoggedInAsUser(this StepDefinitionBase step, string role)
        {
            step.ThrowIfNull(nameof(step));
            role.ThrowIfNull(nameof(role));
            var user = GetUserCredentials(step.FeatureContext, step.ScenarioContext, role);
            step.LoggedInAsUser(user);
        }

        public static void LoggedInAsUser(this StepDefinitionBase step, Credentials user)
        {
            step.ThrowIfNull(nameof(step));
            user.ThrowIfNull(nameof(user));
            step.ScenarioContext[nameof(step.UserDetails)] = user;
            step.Get<LoginPage>().Login(user);
            step.ScenarioContext["DropDownIndex"] = 1;
        }

        public static void ISelectAnyFromOnInterface(this StepDefinitionBase step, string value, string item, string type, string field)
        {
            var elementLocator = GetLocator(item, type);
            ISelectAnyFromOnInterface(step, value, elementLocator, field);
        }

        public static void ISelectAnyFromOnInterface(this StepDefinitionBase step, string value, ElementLocator elementLocator, string field)
        {
            step.ThrowIfNull(nameof(step));
            elementLocator.ThrowIfNull(nameof(elementLocator));

            step.Get<ElementPage>().Click(elementLocator);

            if ((field.EqualsIgnoreCase("Inventory") && value.EqualsIgnoreCase("SourceProduct")) || value.EqualsIgnoreCase("DestinationProduct") || (field.EqualsIgnoreCase("Movement") && value.EqualsIgnoreCase("SourceProduct")) || value.EqualsIgnoreCase("DestinationProduct"))
            {
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), ConstantValues.ProductName, step.Get<ElementPage>().GetElements(nameof(Resources.SelectBoxOptionByValue), ConstantValues.ProductName).Count);
            }
            else if (value.EqualsIgnoreCase("Unit"))
            {
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), ConstantValues.MeasurementUnit, step.Get<ElementPage>().GetElements(nameof(Resources.SelectBoxOptionByValue), ConstantValues.MeasurementUnit).Count);
            }
            else if (field.EqualsIgnoreCase("Movement") && elementLocator.Value.Contains("origin"))
            {
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), step.ScenarioContext["Destination_NodeName"].ToString());
            }
            else if (field.EqualsIgnoreCase("Movement") && elementLocator.Value.Contains("destination"))
            {
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), step.ScenarioContext["Destination_DestinationName"].ToString());
            }

            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
        }

        public static void IProvideValueForOnInterface(this StepDefinitionBase step, string item, string type, string field)
        {
            var elementLocator = GetLocator(item, type);
            IProvideValueForOnInterface(step, elementLocator, field);
        }

        public static void IProvideValueForOnInterface(this StepDefinitionBase step, ElementLocator elementLocator, string field)
        {
            step.ThrowIfNull(nameof(step));
            elementLocator.ThrowIfNull(nameof(elementLocator));

            if (elementLocator.Value.Contains("origin") && field.EqualsIgnoreCase("Inventory"))
            {
                step.Get<ElementPage>().EnterText(nameof(Resources.SourceNodeNameTextBox), step.ScenarioContext["Origin_NodeName"].ToString(), 1);
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), step.ScenarioContext["Origin_NodeName"].ToString());
            }
            else if (elementLocator.Value.Contains("destination") && field.EqualsIgnoreCase("Inventory"))
            {
                step.Get<ElementPage>().EnterText(nameof(Resources.SourceNodeNameTextBox), step.ScenarioContext["Destination_NodeName"].ToString(), 2);
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), step.ScenarioContext["Destination_NodeName"].ToString());
            }
            else if (elementLocator.Value.Contains("origin") && field.EqualsIgnoreCase("Movement"))
            {
                step.Get<ElementPage>().EnterText(nameof(Resources.SourceNodeNameTextBox), step.ScenarioContext["Origin_NodeName"].ToString(), 1);
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), step.ScenarioContext["Origin_NodeName"].ToString());
            }
            else if (elementLocator.Value.Contains("destination") && field.EqualsIgnoreCase("Movement"))
            {
                step.Get<ElementPage>().EnterText(nameof(Resources.SourceNodeNameTextBox), step.ScenarioContext["Origin_DestinationNodeName"].ToString(), 2);
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), step.ScenarioContext["Origin_DestinationNodeName"].ToString());
            }

            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
        }

        public static void IShouldSee(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            IShouldSee(step, elementLocator);
        }

        public static void IShouldSee(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            Assert.IsTrue(step.Get<ElementPage>().GetElement(elementLocator).Displayed);
        }

        public static void IClickOn(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            IClickOn(step, elementLocator);
        }

        public static void IClickOn(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            step.Get<ElementPage>().Click(elementLocator);
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 60);
        }

        public static void ISelectSegmentFrom(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ISelectSegmentFrom(step, elementLocator);
        }

        public static void ISelectSegmentFrom(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            var page = step.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            page.Click(elementLocator);
            var option = page.GetElement(nameof(Resources.SelectBoxOption), formatArgs: step.ScenarioContext["CategorySegment"].ToString());
            Actions action = new Actions(step.DriverContext.Driver);
            action.MoveToElement(option).Perform();
            option.Click();
            ////step.Get<ElementPage>().Click(nameof(Resources.UploadTypeName), step.ScenarioContext["CategorySegment"));
        }

        public static void ISelectCategoryElementFrom(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ISelectCategoryElementFrom(step, elementLocator);
        }

        public static void ISelectCategoryElementFrom(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            step.Get<ElementPage>().Click(elementLocator);
            step.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), step.ScenarioContext["CategorySegment"].ToString()).Click();
        }

        public static void IEnterValidValueInto(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            IEnterValidValueInto(step, elementLocator);
        }

        public static void IEnterValidValueInto(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            step.EnterValueIntoTextBox(elementLocator, ConstantValues.ValidValue);
        }

        public static void EnterValueIntoTextBox(this StepDefinitionBase step, ElementLocator elementLocator, string data)
        {
            step.Get<ElementPage>().GetElement(elementLocator).SendKeys(data);
        }

        public static void ValidateThatAsEnabled(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ValidateThatAsEnabled(step, elementLocator);
        }

        public static void ValidateThatAsEnabled(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            Assert.IsTrue(step.Get<ElementPage>().GetElement(elementLocator).Enabled);
        }

        public static void IEnterVolumeInto(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            IEnterVolumeInto(step, elementLocator);
        }

        public static void IEnterVolumeInto(this StepDefinitionBase step, ElementLocator elementLocator)
        {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            step.Get<ElementPage>().GetElement(elementLocator).SendKeys(ConstantValues.NewMovementVolume);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            step.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Tab);
        }

        public static async Task ISelectedValueOnCreateMovementFormAsync(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            await ISelectedValueOnCreateMovementFormAsync(step, elementLocator).ConfigureAwait(false);
        }

        public static async Task ISelectedValueOnCreateMovementFormAsync(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            var node = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            if (step.ScenarioContext[ConstantValues.Variable].ToString() == "Input")
            {
                var sourceNode = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetTopInputConnectionsToNode, args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
                step.Get<ElementPage>().Click(elementLocator);
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), formatArgs: sourceNode[ConstantValues.Name]);
                step.ScenarioContext[ConstantValues.SourceNode] = sourceNode[ConstantValues.NodeId];
                step.ScenarioContext[ConstantValues.DestinationNode] = step.ScenarioContext["NodeId_1"].ToString();
            }
            else if (step.ScenarioContext[ConstantValues.Variable].ToString() == "Tolerance" || step.ScenarioContext[ConstantValues.Variable].ToString() == "UnidentifiedLoss")
            {
                step.Get<ElementPage>().Click(elementLocator);
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), node[ConstantValues.Name], step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), node[ConstantValues.Name]).Count);
                step.ScenarioContext[ConstantValues.SourceNode] = step.ScenarioContext["NodeId_1"].ToString();
            }
            else
            {
                step.ScenarioContext[ConstantValues.SourceNode] = step.ScenarioContext["NodeId_1"].ToString();
            }
        }

        public static async Task ISelectedValueOnCreateMovementInterfaceAsync(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            await ISelectedValueOnCreateMovementInterfaceAsync(step, elementLocator).ConfigureAwait(false);
        }

        public static async Task ISelectedValueOnCreateMovementInterfaceAsync(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            var node = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            step.ScenarioContext["NodeName"] = node[ConstantValues.Name];
            if (step.ScenarioContext[ConstantValues.Variable].ToString() == "Output")
            {
                var destinationNode = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetTopOutputConnectionsToNode, args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
                step.Get<ElementPage>().Click(elementLocator);
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), formatArgs: destinationNode[ConstantValues.Name]);
                step.ScenarioContext[ConstantValues.SourceNode] = step.ScenarioContext["NodeId_1"].ToString();
                step.ScenarioContext[ConstantValues.DestinationNode] = destinationNode[ConstantValues.NodeId];
            }
            else if (step.ScenarioContext[ConstantValues.Variable].ToString() == "Tolerance" || step.ScenarioContext[ConstantValues.Variable].ToString() == "UnidentifiedLoss")
            {
                step.Get<ElementPage>().Click(elementLocator);
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), node[ConstantValues.Name], step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), node[ConstantValues.Name]).Count);
                step.ScenarioContext[ConstantValues.DestinationNode] = step.ScenarioContext["NodeId_1"].ToString();
            }
            else
            {
                step.ScenarioContext[ConstantValues.DestinationNode] = step.ScenarioContext["NodeId_1"].ToString();
            }
        }

        public static void ISelectedValueFrom(this StepDefinitionBase step, string inputValue, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ISelectedValueFrom(step, inputValue, elementLocator);
        }

        public static void ISelectedValueFrom(this StepDefinitionBase step, string inputValue, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().Click(elementLocator);
            if (inputValue == "product")
            {
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), ConstantValues.ProductName, step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), ConstantValues.ProductName).Count);
            }
            else if (inputValue == "Tolerance")
            {
                step.ScenarioContext[ConstantValues.Variable] = inputValue;
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), UIContent.Conversion[inputValue], step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), UIContent.Conversion[inputValue]).Count);
            }
            else if (inputValue == "Bbl")
            {
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), ConstantValues.MeasurementUnit, step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), ConstantValues.MeasurementUnit).Count);
            }
            else if (inputValue == "reasonForChange")
            {
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), ConstantValues.ReasonForChange, step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), ConstantValues.ReasonForChange).Count);
            }
            else if (inputValue == "REFICAR")
            {
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), ConstantValues.Reficar, step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), ConstantValues.Reficar).Count);
            }
            else
            {
                step.ScenarioContext[ConstantValues.Variable] = inputValue;
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), formatArgs: UIContent.Conversion[inputValue]);
            }
        }

        public static void IEnterValidInto(this StepDefinitionBase step, string data, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            IEnterValidInto(step, data, elementLocator);
        }

        public static void IEnterValidInto(this StepDefinitionBase step, string data, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            step.EnterValueIntoTextBox(elementLocator, step.ScenarioContext[data].ToString());
            step.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        public static void IEnterMorethanCharactersInto(this StepDefinitionBase step, int limit, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            IEnterMorethanCharactersInto(step, limit, elementLocator);
        }

        public static void IEnterMorethanCharactersInto(this StepDefinitionBase step, int limit, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            step.ScenarioContext[Entities.Keys.RandomFieldValue] = new Faker().Random.AlphaNumeric(limit + 1);
            step.Get<ElementPage>().GetElement(elementLocator).SendKeys(step.ScenarioContext[Entities.Keys.RandomFieldValue].ToString());
            step.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Tab);
        }

        public static void VerifyThatIs(this StepDefinitionBase step, string item, string type, string expectedValue)
        {
            var elementLocator = GetLocator(item, type);
            VerifyThatIs(step, elementLocator, expectedValue);
        }

        public static void VerifyThatIs(this StepDefinitionBase step, ElementLocator elementLocator, string expectedValue)
        {
            step.ThrowIfNull(nameof(step));
            string isDisabled = step.Get<ElementPage>().GetElement(elementLocator).GetAttribute("disabled");
            if (isDisabled == null)
            {
                Assert.AreEqual("enabled", expectedValue);
            }
            else
            {
                Assert.AreEqual("disabled", expectedValue);
            }
        }

        public static void IProvideValueFor(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            IProvideValueFor(step, elementLocator);
        }

        public static void IProvideValueFor(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().GetElement(elementLocator).SendKeys(step.ScenarioContext[Entities.Keys.RandomFieldValue].ToString());
            step.ScenarioContext[Entities.Keys.Field] = elementLocator != null ? elementLocator.Value : string.Empty;
        }

        public static void ValidateSelectedColorIsDisplayingIn(this StepDefinitionBase step, string color, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ValidateSelectedColorIsDisplayingIn(step, color, elementLocator);
        }

        public static void ValidateSelectedColorIsDisplayingIn(this StepDefinitionBase step, string color, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            int r, g, b = 0;
#pragma warning disable CA1062 // Validate arguments of public methods
#pragma warning disable CA1305 // Specify IFormatProvider
            r = int.Parse(color.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            g = int.Parse(color.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            b = int.Parse(color.Substring(4, 2), NumberStyles.AllowHexSpecifier);
#pragma warning restore CA1305 // Specify IFormatProvider
#pragma warning restore CA1062 // Validate arguments of public methods
            var rgbValue = "rgb(" + r + ", " + g + ", " + b + ")";
            var elementAttribute = step.Get<ElementPage>().GetElement(elementLocator).GetAttribute("style");
            Assert.IsTrue(elementAttribute.ContainsIgnoreCase(rgbValue));
        }

        public static void ValidateSelectedIconIsDisplayedIn(this StepDefinitionBase step, string iconName, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ValidateSelectedIconIsDisplayedIn(step, iconName, elementLocator);
        }

        public static void ValidateSelectedIconIsDisplayedIn(this StepDefinitionBase step, string iconName, ElementLocator elementLocator)
        {
            Assert.IsTrue(step.Get<ElementPage>().GetElement(elementLocator).GetAttribute("value").ContainsIgnoreCase(iconName));
        }

        public static void IProvideTheValueForElement(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            IProvideTheValueForElement(step, elementLocator);
        }

        public static void IProvideTheValueForElement(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));

            if (elementLocator is null)
            {
                throw new System.ArgumentNullException(nameof(elementLocator));
            }

            if (elementLocator.Value.ContainsIgnoreCase(ConstantValues.NodeNameText))
            {
                step.ScenarioContext[ConstantValues.CreatedNodeName] = string.Concat($"Automation_", new Faker().Random.AlphaNumeric(5));
                step.Get<ElementPage>().GetElement(elementLocator).SendKeys(step.ScenarioContext[ConstantValues.CreatedNodeName].ToString());
            }
            else if (elementLocator.Value.Contains("txtarea"))
            {
                step.ScenarioContext[ConstantValues.Description] = string.Concat($"Automation_", new Faker().Random.AlphaNumeric(5));
                step.Get<ElementPage>().GetElement(elementLocator).SendKeys(step.ScenarioContext[ConstantValues.Description].ToString());
            }
            else if (elementLocator.Value.Contains("order"))
            {
                step.ScenarioContext[ConstantValues.Order] = new Faker().Random.Number(500);
                step.Get<ElementPage>().GetElement(elementLocator).SendKeys(step.ScenarioContext[ConstantValues.Description].ToString());
            }
            else if (elementLocator.Value.Contains("nodeStorageLocation_name"))
            {
                step.ScenarioContext[Keys.RandomFieldValue] = string.Concat($"Automation_", new Faker().Random.AlphaNumeric(50));
                step.Get<ElementPage>().GetElements(elementLocator).ElementAt(1).SendKeys(step.ScenarioContext[Keys.RandomFieldValue].ToString());
            }
            else
            {
                step.ScenarioContext[Keys.RandomFieldValue] = string.Concat($"Automation_", new Faker().Random.AlphaNumeric(50));
                step.Get<ElementPage>().GetElement(elementLocator).SendKeys(step.ScenarioContext[Keys.RandomFieldValue].ToString());
            }
        }

        public static void ISelectAnyElement(this StepDefinitionBase step, string value, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ISelectAnyElement(step, value, elementLocator);
        }

        public static void ISelectAnyElement(this StepDefinitionBase step, string value, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            if (elementLocator is null)
            {
                throw new System.ArgumentNullException(nameof(elementLocator));
            }

            step.Get<ElementPage>().Click(elementLocator);
            if (elementLocator.Value.Contains("nodeTags_element"))
            {
                value = step.ScenarioContext["NodeTypeName"].ToString();
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), formatArgs: value);
            }
            else if (value.EqualsIgnoreCase("Category"))
            {
                value = step.ScenarioContext["CategoryName"].ToString();
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), formatArgs: value);
            }
            else
            {
                step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), formatArgs: UIContent.Conversion[value]);
            }
        }

        public static void IEnterValueInto(this StepDefinitionBase step, string value, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            IEnterValueInto(step, value, elementLocator);
        }

        public static void IEnterValueInto(this StepDefinitionBase step, string value, ElementLocator elementLocator)
        {
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            step.EnterValueIntoTextBox(elementLocator, value);
        }

        public static async Task ISelectSegmentValueFromAsync(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            await ISelectSegmentValueFromAsync(step, elementLocator).ConfigureAwait(false);
        }

        public static async Task ISelectSegmentValueFromAsync(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            var categoryElement = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetCategoryElementByName, args: new { name = step.ScenarioContext[ConstantValues.CategorySegment].ToString() }).ConfigureAwait(false);
            if (int.Parse(categoryElement[ConstantValues.CategoryId], CultureInfo.InvariantCulture) == 2)
            {
                step.Get<ElementPage>().Click(elementLocator);
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), step.ScenarioContext[ConstantValues.CategorySegment].ToString(), step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), step.ScenarioContext[ConstantValues.CategorySegment].ToString()).Count);
            }
            else if (int.Parse(categoryElement[ConstantValues.CategoryId], CultureInfo.InvariantCulture) == 8)
            {
                step.Get<ElementPage>().Click(elementLocator);
                categoryElement = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTopCategorySegment).ConfigureAwait(false);
                step.Get<ElementPage>().Click(nameof(Resources.UploadType), categoryElement[ConstantValues.Name], step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), categoryElement[ConstantValues.Name]).Count);
            }
        }

        public static void ISelectOwnershipCalculatedSegmentFrom(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ISelectOwnershipCalculatedSegmentFrom(step, elementLocator);
        }

        public static void ISelectOwnershipCalculatedSegmentFrom(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().Click(elementLocator);
            step.Get<ElementPage>().Click(nameof(Resources.UploadType), step.ScenarioContext[ConstantValues.Segment].ToString(), step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), step.ScenarioContext[ConstantValues.Segment].ToString()).Count);
        }

        public static void ISelectRequiredDateFrom(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ISelectRequiredDateFrom(step, elementLocator);
        }

        public static void ISelectRequiredDateFrom(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            elementLocator.ThrowIfNull(nameof(elementLocator));
            var futureDate = DateTime.Now.AddDays(50).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            step.ScenarioContext["FutureDate"] = futureDate;
            step.Get<ElementPage>().SetValue(elementLocator.Value, futureDate);
        }

        public static void IClickOnForProduct(this StepDefinitionBase step, string item, string type, int productNumber)
        {
            var elementLocator = GetLocator(item, type);
            IClickOnForProduct(step, elementLocator, productNumber);
        }

        public static void IClickOnForProduct(this StepDefinitionBase step, ElementLocator elementLocator, int productNumber)
        {
            var productRow = step.Get<ElementPage>().GetElements(elementLocator);
            productRow[productNumber - 1].Click();
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
        }

        public static void ISelectOfficialDeltaSegmentFrom(this StepDefinitionBase step, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ISelectOfficialDeltaSegmentFrom(step, elementLocator);
        }

        public static void ISelectOfficialDeltaSegmentFrom(this StepDefinitionBase step, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            var page = step.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            page.Click(elementLocator);
            page.GetElement(nameof(Resources.SelectBoxOptionByValue), formatArgs: step.ScenarioContext["DeltaCategorySegment"].ToString()).Click();
        }

        public static void ISelectAPeriodFromDropDown(this StepDefinitionBase step, string month)
        {
            step.ThrowIfNull(nameof(step));
            var page = step.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            page.GetElement(nameof(Resources.SelectBox), formatArgs: "Período del procesamiento").Click();
            page.GetElement(nameof(Resources.SelectBoxOptionByValue), formatArgs: "01-" + month).Click();
            step.ScenarioContext["Period"] = page.GetElement(nameof(Resources.SelectBox), formatArgs: "Período del procesamiento").Text;
            step.ScenarioContext["Mon"] = month;
        }

        public static void ISelectYearFromDropDown(this StepDefinitionBase step, string year)
        {
            step.ThrowIfNull(nameof(step));
            var page = step.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            page.GetElement(nameof(Resources.SelectBox), formatArgs: "Año").Click();
            page.GetElement(nameof(Resources.SelectBoxOptionByValue), formatArgs: year).Click();
            step.ScenarioContext["Year"] = page.GetElement(nameof(Resources.SelectBox), formatArgs: "Año").Text;
        }

        public static void ISelectNodeFromTheNodoDropdownOnOfficialLogistics(this StepDefinitionBase step, string nodeCount)
        {
            step.ThrowIfNull(nameof(step));
            ////step.When("I click on Node textbox on criteria step");
            step.Get<ElementPage>().Click(nameof(Resources.NodeFieldOnCreateLogisticsInterface));
            var page = step.Get<ElementPage>();
            string nodeName = nodeCount.EqualsIgnoreCase("Todos") ? "Todos" : step.GetValueInternal("NodeName_1");
            var nodeSelection = page.GetElement(nameof(Resources.NodeInputOnCreateLogisticsInterface));
            nodeSelection.SendKeys(nodeName);
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.SelectItemAtIndex(nodeSelection, 1);
            Assert.AreEqual(nodeName, page.GetElement(nameof(Resources.NodeInputOnCreateLogisticsInterface)).GetAttribute(ConstantValues.Value));
        }

        public static void ISelectSegmentFromOfficialLogistics(this StepDefinitionBase step, string segmentName, string item, string type)
        {
            var elementLocator = GetLocator(item, type);
            ISelectSegmentFromOfficialLogistics(step, segmentName, elementLocator);
        }

        public static void ISelectSegmentFromOfficialLogistics(this StepDefinitionBase step, string segmentName, ElementLocator elementLocator)
        {
            step.ThrowIfNull(nameof(step));
            string segmentToBeSelected = segmentName.EqualsIgnoreCase("random") ? step.ScenarioContext["CategorySegment"].ToString() : segmentName;
            var page = step.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            page.Click(elementLocator);
            var option = page.GetElement(nameof(Resources.SelectBoxOption), formatArgs: segmentToBeSelected);
            Actions action = new Actions(step.DriverContext.Driver);
            action.MoveToElement(option).Perform();
            option.Click();
        }

        public static void ISelectOwnerOnOfficialLogistics(this StepDefinitionBase step, string ownerName)
        {
            step.Get<ElementPage>().Click(nameof(Resources.OwnerOnOfficialSivWizard), formatArgs: ownerName);
        }

        public static void ISelectAPeriodFromDropDownForParticularPeriod(this StepDefinitionBase step, string month)
        {
            step.ThrowIfNull(nameof(step));
            var page = step.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            page.GetElement(nameof(Resources.SelectBox), formatArgs: "Período del procesamiento").Click();
            page.GetElement(nameof(Resources.SelectBoxOptionByValue), formatArgs: "29-" + month).Click();
            step.ScenarioContext["Period"] = page.GetElement(nameof(Resources.SelectBox), formatArgs: "Período del procesamiento").Text;
            step.ScenarioContext["Mon"] = month;
        }
    }
}