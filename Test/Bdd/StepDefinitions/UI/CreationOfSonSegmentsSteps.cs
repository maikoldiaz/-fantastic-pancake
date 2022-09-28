namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ocaramba.Types;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class CreationOfSonSegmentsSteps : EcpWebStepDefinitionBase
    {
        private readonly List<string> requiredSegmentsList = new List<string>();

        [Then(@"I validate that only expected segments are listed inside the ""(.*)"" segments list and that they are in alphabetical order")]
        public async Task ThenIValidateThatOnlyExpectedSegmentsAreListedInsideTheSegmentsListAndThatTheyAreInAlphabeticalOrderAsync(string segmentType)
        {
            List<string> segmentsListFromUI = new List<string>();

            switch (segmentType)
            {
                case "chain":

                    await this.GetTheActiveChainOrSonSegmentsListAsync(segmentType).ConfigureAwait(false);

                    var chainSegmentsElementsOnUI = this.Get<ElementPage>().GetElements(nameof(Resources.ChainSegmentsElements));
                    int chainSegmentsCountOnUI = chainSegmentsElementsOnUI.Count;

                    for (int currentIndex = 0; currentIndex < chainSegmentsCountOnUI; currentIndex++)
                    {
                        segmentsListFromUI.Add(chainSegmentsElementsOnUI[currentIndex].Text.Trim());
                    }

                    break;
                case "son":
                    await this.GetTheActiveChainOrSonSegmentsListAsync(segmentType).ConfigureAwait(false);

                    var sonSegmentsElementsOnUI = this.Get<ElementPage>().GetElements(nameof(Resources.SonSegmentsElements));
                    int sonSegmentsCountOnUI = sonSegmentsElementsOnUI.Count;

                    for (int currentIndex = 0; currentIndex < sonSegmentsCountOnUI; currentIndex++)
                    {
                        segmentsListFromUI.Add(sonSegmentsElementsOnUI[currentIndex].Text.Trim());
                    }

                    break;
            }

            if (this.requiredSegmentsList.Count == segmentsListFromUI.Count)
            {
                Console.WriteLine("\nList of mismatched values between Db and UI if any : \n");

                for (int i = 0; i < segmentsListFromUI.Count; i++)
                {
                    if (!this.requiredSegmentsList[i].EqualsIgnoreCase(segmentsListFromUI[i]))
                    {
                        Console.WriteLine("Expected value from Db : " + this.requiredSegmentsList[i] + "; Actual value on UI : " + segmentsListFromUI[i]);
                    }
                }
            }

            Assert.IsTrue(this.requiredSegmentsList.SequenceEqual(segmentsListFromUI, StringComparer.OrdinalIgnoreCase), "Validation of segments and their order inside '" + segmentType + "' list failed");
        }

        [Then(@"I validate that all the segments (from "".*"" "".*"" "".*"") dropdown are active SON segments")]
        public async Task ThenIValidateThatAllTheSegmentsFromDropdownAreActiveSONSegmentsAsync(ElementLocator elementLocator)
        {
            List<string> segmentsListFromUI = new List<string>();

            await Task.Delay(2000).ConfigureAwait(false);
            await this.GetTheActiveChainOrSonSegmentsListAsync("son").ConfigureAwait(false);

            this.Get<ElementPage>().Click(elementLocator);
            var sonSegmentsElementsOnUI = this.Get<ElementPage>().GetElements(nameof(Resources.DropdowmSegmentsList));
            int sonSegmentsCountOnUI = sonSegmentsElementsOnUI.Count;

            for (int currentIndex = 0; currentIndex < sonSegmentsCountOnUI; currentIndex++)
            {
                segmentsListFromUI.Add(sonSegmentsElementsOnUI[currentIndex].Text.Trim());
            }

            segmentsListFromUI.Sort();

            Assert.IsTrue(this.requiredSegmentsList.SequenceEqual(segmentsListFromUI, StringComparer.OrdinalIgnoreCase), "Validation of segment dropdown displaying only active SON segments failed");
        }

        [When(@"I move ""(.*)"" segments from ""(.*)"" list by searching (in "".*"" "".*"" "".*"" "".*"") and by clicking (on "".*"" "".*"" "".*"" "".*"")")]
        public async Task WhenIMoveSegmentsFromListBySearchingInAndByClickingOnAsync(int numberOfSegments, string fromToListName, ElementLocator searchBox, ElementLocator moveButton)
        {
            List<string> segmentNames = new List<string>();

            switch (fromToListName)
            {
                case "chain-son":
                    segmentNames = this.GetValue("ChainSegmentsAdded").Split(':').ToList();
                    break;
                case "son-chain":
                    segmentNames = this.GetValue("SonSegmentsAdded").Split(':').ToList();
                    break;
            }

            int currentSegmentIndex = 0;

            foreach (string segmentName in segmentNames)
            {
                if (currentSegmentIndex < numberOfSegments)
                {
                    this.Get<ElementPage>().EnterText(searchBox, segmentName);

                    switch (fromToListName)
                    {
                        case "chain-son":
                            this.Get<ElementPage>().Click(nameof(Resources.ChainSegmentsElements));
                            break;
                        case "son-chain":
                            this.Get<ElementPage>().Click(nameof(Resources.SonSegmentsElements));
                            break;
                    }

                    this.Get<ElementPage>().Click(moveButton);
                    this.Get<ElementPage>().ClearText(searchBox);
                    currentSegmentIndex++;
                }
                else
                {
                    break;
                }
            }

            await Task.Delay(3000).ConfigureAwait(false);
        }

        [Then(@"I validate that ""(.*)"" configured segments are updated as ""(.*)"" in the Db")]
        public async Task ThenIValidateThatConfiguredSegmentsAreUpdatedAsInTheDbAsync(int numberOfSegments, string segmentType)
        {
            bool validationFailed = false;
            bool resultComparision;
            List<string> segmentNames = new List<string>();

            switch (segmentType)
            {
                case "chain segment":
                    segmentNames = this.GetValue("SonSegmentsAdded").Split(':').ToList();
                    break;
                case "son segment":
                    segmentNames = this.GetValue("ChainSegmentsAdded").Split(':').ToList();
                    break;
            }

            int currentSegmentIndex = 0;

            foreach (string segmentName in segmentNames)
            {
                if (currentSegmentIndex < numberOfSegments)
                {
                    var resultTable = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetIsOperationalSegmentValue, args: new { name = segmentName }).ConfigureAwait(false);
                    resultComparision = resultTable[ConstantValues.IsOperationalSegment].Equals("true", System.StringComparison.OrdinalIgnoreCase);

                    if (segmentType.Equals("chain segment", System.StringComparison.OrdinalIgnoreCase))
                    {
                        if (resultComparision)
                        {
                            validationFailed = true;
                            break;
                        }
                    }
                    else
                    {
                        if (!resultComparision)
                        {
                            validationFailed = true;
                            break;
                        }
                    }

                    currentSegmentIndex++;
                }
                else
                {
                    break;
                }
            }

            Assert.IsFalse(validationFailed, "Failed to validate the saved segments in Db");
        }

        [Then(@"I validate that ""(.*)"" moved segments are ""(.*)"" in ""(.*)"" segments list by searching (in "".*"" "".*"" "".*"" "".*"")")]
        public void ThenIValidateThatMovedSegmentsAreInSegmentsListBySearchingIn(int numberOfSegments, string availabilityState, string segmentType, ElementLocator searchBox)
        {
            bool validationFailed = false;
            bool resultComparision;
            List<string> segmentNames;

            try
            {
                segmentNames = this.GetValue("ChainSegmentsAdded").Split(':').ToList();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                segmentNames = this.GetValue("SonSegmentsAdded").Split(':').ToList();
            }

            int currentSegmentIndex = 0;

            foreach (string segmentName in segmentNames)
            {
                if (currentSegmentIndex < numberOfSegments)
                {
                    this.Get<ElementPage>().EnterText(searchBox, segmentName);

                    switch (segmentType)
                    {
                        case "chain":

                            resultComparision = this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ChainSegmentsElementsByText), formatArgs: segmentName);

                            if (availabilityState.Equals("available", System.StringComparison.OrdinalIgnoreCase))
                            {
                                if (!resultComparision)
                                {
                                    validationFailed = true;
                                }
                            }
                            else
                            {
                                if (resultComparision)
                                {
                                    validationFailed = true;
                                }
                            }

                            break;

                        case "son":

                            resultComparision = this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.SonSegmentsElementsByText), formatArgs: segmentName);

                            if (availabilityState.Equals("available", System.StringComparison.OrdinalIgnoreCase))
                            {
                                if (!resultComparision)
                                {
                                    validationFailed = true;
                                }
                            }
                            else
                            {
                                if (resultComparision)
                                {
                                    validationFailed = true;
                                }
                            }

                            break;
                    }

                    this.Get<ElementPage>().ClearText(searchBox);
                    currentSegmentIndex++;
                }
                else
                {
                    break;
                }
            }

            Assert.IsFalse(validationFailed, "Failed to validate movement of segment from other list to '" + segmentType + "'");
        }

        [Then(@"I validate the filter functionality of ""(.*)"" text for ""(.*)"" segments by searching (in "".*"" "".*"" "".*"" "".*"")")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "CodeAsExpected")]
        public void ThenIValidateTheFilterFunctionalityOfTextForSegmentsBySearchingIn(string searchCriteria, string segmentType, ElementLocator searchBox)
        {
            bool validationFailed = false;
            List<string> segmentNames;
            IList<IWebElement> resultsOfFilterCriteria;

            switch (segmentType)
            {
                case "chain":
                    segmentNames = this.GetValue("ChainSegmentsAdded").Split(':').ToList();

                    if (searchCriteria.Equals("partial", System.StringComparison.OrdinalIgnoreCase))
                    {
                        this.Get<ElementPage>().EnterText(searchBox, segmentNames[0]);
                        resultsOfFilterCriteria = this.Get<ElementPage>().GetElements(nameof(Resources.ChainSegmentsElements), formatArgs: segmentNames[0]);

                        if (resultsOfFilterCriteria.Count != 1)
                        {
                            validationFailed = true;
                            break;
                        }
                    }
                    else
                    {
                        this.Get<ElementPage>().EnterText(searchBox, ConstantValues.SegmentCategoryNameSuffix);
                        resultsOfFilterCriteria = this.Get<ElementPage>().GetElements(nameof(Resources.ChainSegmentsElements), formatArgs: ConstantValues.SegmentCategoryNameSuffix);

                        if (resultsOfFilterCriteria.Count <= 1)
                        {
                            validationFailed = true;
                            break;
                        }
                    }

                    this.Get<ElementPage>().ClearText(searchBox);

                    break;

                case "son":
                    segmentNames = this.GetValue("SonSegmentsAdded").Split(':').ToList();

                    if (searchCriteria.Equals("partial", System.StringComparison.OrdinalIgnoreCase))
                    {
                        this.Get<ElementPage>().EnterText(searchBox, segmentNames[0]);
                        resultsOfFilterCriteria = this.Get<ElementPage>().GetElements(nameof(Resources.SonSegmentsElements), formatArgs: segmentNames[0]);

                        if (resultsOfFilterCriteria.Count != 1)
                        {
                            validationFailed = true;
                            break;
                        }
                    }
                    else
                    {
                        this.Get<ElementPage>().EnterText(searchBox, ConstantValues.SegmentCategoryNameSuffix);
                        resultsOfFilterCriteria = this.Get<ElementPage>().GetElements(nameof(Resources.SonSegmentsElements), formatArgs: ConstantValues.SegmentCategoryNameSuffix);

                        if (resultsOfFilterCriteria.Count <= 1)
                        {
                            validationFailed = true;
                            break;
                        }
                    }

                    this.Get<ElementPage>().ClearText(searchBox);
                    break;
            }

            Assert.IsFalse(validationFailed, "Validation of filter criteria for '" + searchCriteria + "' text failed for '" + segmentType + "' segment");
        }

        public async Task GetTheActiveChainOrSonSegmentsListAsync(string segmentType)
        {
            this.requiredSegmentsList.Clear();

            switch (segmentType)
            {
                case "chain":
                    var tabledataOfChainSegments = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetListOfActiveChainSegments).ConfigureAwait(false);
                    var chainSegmentNameTableColumn = tabledataOfChainSegments.ToDictionaryList();
                    int chainSegmentsCount = chainSegmentNameTableColumn.Count;

                    for (int currentIndex = 0; currentIndex < chainSegmentsCount; currentIndex++)
                    {
                        this.requiredSegmentsList.Add((string)chainSegmentNameTableColumn[currentIndex]["Name"]);
                    }

                    break;
                case "son":
                    var tabledataOfSonSegments = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetListOfActiveSonSegments).ConfigureAwait(false);
                    var sonSegmentNameTableColumn = tabledataOfSonSegments.ToDictionaryList();
                    int sonSegmentsCount = sonSegmentNameTableColumn.Count;

                    for (int currentIndex = 0; currentIndex < sonSegmentsCount; currentIndex++)
                    {
                        this.requiredSegmentsList.Add((string)sonSegmentNameTableColumn[currentIndex]["Name"]);
                    }

                    break;
            }
        }
    }
}
