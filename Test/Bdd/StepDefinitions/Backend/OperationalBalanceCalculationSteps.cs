// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalBalanceCalculationSteps.cs" company="Microsoft">
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
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TechTalk.SpecFlow;

    [Binding]
    public class OperationalBalanceCalculationSteps : EcpApiStepDefinitionBase
    {
        public OperationalBalanceCalculationSteps(FeatureContext featureContext)
           : base(featureContext)
        {
        }

        [Given(@"I want to calculate the ""(.*)"" in the system")]
        public async Task GivenIWantToCalculateTheInTheSystemAsync(string field)
        {
            ////this.Given("I want create TestData for Operational Cutoff \"SingleDay\"");
            await this.TestDataForOperationCutOffAsync("SingleDay").ConfigureAwait(false);
            Assert.IsNotNull(field);
        }

        [When(@"I receive the input data")]
        public async Task WhenIReceiveTheInputDataAsync()
        {
            bool result = true;
            var list = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow["OperationalBalance"], args: new { NodeId = 1, StartDate = "20190721 10:34:09 AM", EndDate = "20190721 10:34:09 AM" }).ConfigureAwait(false);
            var inputlist = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow["Inputs"], args: new { NodeId = 1, StartDate = "20190721 10:34:09 AM", EndDate = "20190721 10:34:09 AM" }).ConfigureAwait(false);
            var outputlist = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow["Outputs"], args: new { NodeId = 1, StartDate = "20190721 10:34:09 AM", EndDate = "20190721 10:34:09 AM" }).ConfigureAwait(false);
            var losses = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow["Losses"], args: new { NodeId = 1, StartDate = "20190721 10:34:09 AM", EndDate = "20190721 10:34:09 AM" }).ConfigureAwait(false);
            this.LogToReport(list.ToExpandoList());
            foreach (var item in list.ToExpandoList())
            {
                var jsonContent = item.ToJson();
                string productName = jsonContent.JsonGetValueforSP("ProductName");
                float initialinputcalculated = 0;
                float outputcalculated = 0;
                float identifiedlossfromSQL = 0;
                foreach (var item1 in inputlist)
                {
                    var jsonContent1 = item1.ToJson();
                    string destinationProduct = jsonContent1.JsonGetValueforSP("DestinationProduct");
                    if (destinationProduct.EqualsIgnoreCase(productName))
                    {
                        initialinputcalculated = initialinputcalculated + float.Parse(jsonContent1.JsonGetValueforSP("NetStandardVolume"), CultureInfo.InvariantCulture);
                    }
                }

                float initialInventory = float.Parse(jsonContent.JsonGetValueforSP("InitialInventory"), CultureInfo.InvariantCulture);
                float inputs = float.Parse(jsonContent.JsonGetValueforSP("Inputs"), CultureInfo.InvariantCulture);
                if (initialinputcalculated != inputs)
                {
                    result = false;
                }

                float finalInventory = float.Parse(jsonContent.JsonGetValueforSP("FinalInventory"), CultureInfo.InvariantCulture);
                float outputs = float.Parse(jsonContent.JsonGetValueforSP("Outputs"), CultureInfo.InvariantCulture);
                foreach (var item1 in outputlist)
                {
                    var jsonContent1 = item1.ToJson();
                    string sourceProduct = jsonContent1.JsonGetValueforSP("SourceProduct");
                    if (sourceProduct.EqualsIgnoreCase(productName))
                    {
                        outputcalculated = outputcalculated + float.Parse(jsonContent1.JsonGetValueforSP("NetStandardVolume"), CultureInfo.InvariantCulture);
                    }
                }

                if (outputcalculated != outputs)
                {
                    result = false;
                }

                float identifiedLoss = float.Parse(jsonContent.JsonGetValueforSP("IdentifiedLoss"), CultureInfo.InvariantCulture);
                foreach (var item1 in losses)
                {
                    var jsonContent1 = item1.ToJson();
                    string sourceProduct = jsonContent1.JsonGetValueforSP("SourceProduct");
                    if (sourceProduct.EqualsIgnoreCase(productName))
                    {
                        identifiedlossfromSQL = identifiedlossfromSQL + float.Parse(jsonContent1.JsonGetValueforSP("NetStandardVolume"), CultureInfo.InvariantCulture);
                    }
                }

                if (identifiedlossfromSQL != identifiedLoss)
                {
                    result = false;
                }

                float unbalancefromSQL = initialInventory + inputs - finalInventory - identifiedlossfromSQL - outputcalculated;
                float unBalance = float.Parse(jsonContent.JsonGetValueforSP("Unbalance"), CultureInfo.InvariantCulture);
                if ((unbalancefromSQL - unBalance) > 0.1)
                {
                    result = false;
                }

                this.LogToReport(productName);
                this.LogToReport(initialInventory);
                this.LogToReport(inputs);
                this.LogToReport(finalInventory);
                this.LogToReport(outputs);
                this.LogToReport(identifiedLoss);
                this.LogToReport(unBalance);
              }

            this.ScenarioContext["result"] = result;
        }

        [When(@"I don't receive ""(.*)""")]
        public async System.Threading.Tasks.Task WhenIDonTReceiveAsync(string field)
        {
            string snull = null;
            try
            {
                if (field == "StartDate")
                {
                    this.ScenarioContext["lastReadRow"] = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["OperationalBalance"], args: new { NodeId = 1, StartDate = snull, EndDate = "20190721 10:34:09 AM" }).ConfigureAwait(false);
                }
                else if (field == "EndDate")
                {
                    this.ScenarioContext["lastReadRow"] = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["OperationalBalance"], args: new { NodeId = 1, StartDate = "20190721 10:34:09 AM", EndDate = snull }).ConfigureAwait(false);
                }
                else if (field == "NodeId")
                {
                    this.ScenarioContext["lastReadRow"] = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["OperationalBalance"], args: new { NodeId = snull, StartDate = "20190721 10:34:09 AM", EndDate = "20190721 10:34:09 AM" }).ConfigureAwait(false);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                this.SetValue(Keys.Error, e.Message);
            }
        }

        [When(@"I receive ""(.*)"" greater than the ""(.*)""")]
        [When(@"I receive ""(.*)"" less than the ""(.*)""")]
        [When(@"I receive ""(.*)"" greater than or equal to the ""(.*)""")]
        public async System.Threading.Tasks.Task WhenIReceiveGreaterThanTheAsync(string field1, string field2)
        {
            try
            {
                if (field1.EqualsIgnoreCase("StartDate") && field2.EqualsIgnoreCase("EndDate"))
                {
                    this.ScenarioContext["lastReadRow"] = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["OperationalBalance"], args: new { NodeId = 1, StartDate = "20200721 10:34:09 AM", EndDate = "20190721 10:34:09 AM" }).ConfigureAwait(false);
                }
                else if (field1.EqualsIgnoreCase("EndDate") && field2.EqualsIgnoreCase("CurrentDate"))
                {
                    this.ScenarioContext["lastReadRow"] = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["OperationalBalance"], args: new { NodeId = 1, StartDate = "20190721 10:34:09 AM", EndDate = "20250721 10:34:09 AM" }).ConfigureAwait(false);
                }
                else if (field1.EqualsIgnoreCase("EndDate") && field2.EqualsIgnoreCase("StartDate"))
                {
                    this.ScenarioContext["lastReadRow"] = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["OperationalBalance"], args: new { NodeId = 1, StartDate = "20190722 10:34:09 AM", EndDate = "20190721 10:34:09 AM" }).ConfigureAwait(false);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                this.SetValue(Keys.Error, e.Message);
            }
        }

        [Then(@"I should be able to calculate the Operational Balance")]
        public void ThenIShouldBeAbleToCalculateTheOperationalBalance()
        {
            bool result = (bool)this.ScenarioContext["result"];
            Assert.IsTrue(result);
        }

        [Then(@"the result should fail with message ""(.*)""")]
        public void ThenTheResultShouldFailWithMessage(string field)
        {
            Assert.AreEqual(this.GetValue<string>(Keys.Error), field);
        }
    }
}
