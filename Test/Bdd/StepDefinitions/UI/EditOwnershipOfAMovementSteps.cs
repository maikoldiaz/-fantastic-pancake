// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditOwnershipOfAMovementSteps.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Globalization;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    public class EditOwnershipOfAMovementSteps : EcpWebStepDefinitionBase
    {
        public static Dictionary<string, string> Owners { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Equion", "movements-out" },
            { "Reficar", "inventory-out" },
            { "EcoPetrol", "EcoPetrol" },
            { "Ceplosa", "Ceplosa" },
        };

        [Then(@"I verify I am able to add the owners")]
        public void ThenIVerifyIAmAbleToAddTheOwners()
        {
            var ownercount = 0;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    this.Get<ElementPage>().WaitUntilElementExists(nameof(Resources.MovementOwnerNames), i);
                    var text = this.Get<ElementPage>().GetElementText(nameof(Resources.MovementOwnerNames), i);
                    if (Owners.ContainsKey(text))
                    {
                        ownercount++;
                        continue;
                    }
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    Assert.Fail("Owners are not added");
                }

                if (ownercount == 1)
                {
                    Assert.Fail("Owners are not present or added");
                }
            }
        }

        [When(@"I save without entering required (.*)")]
        public void WhenISaveWithoutEnteringRequired(string field)
        {
            switch (field)
            {
                case "Volume":
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerInputBoxes), 3).SendKeys(string.Empty);
                    break;
                case "Percentage":
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerInputBoxes), 2).SendKeys(string.Empty);
                    break;
                case "Comment":
                    ////this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipCommentBox)).SendKeys(string.Empty);
                    break;
                case "Reason":
                    break;
            }
        }

        [Then(@"I should see the error notification ""(.*)""")]
        public void ThenIShouldSeeTheErrorNotification(string message)
        {
            var text = this.Get<ElementPage>().GetElementText(nameof(Resources.OwnerErrorNotification));
            Assert.AreEqual(message, text);
        }

        [When(@"I update owners volume to more than (.*)")]
        public void WhenIUpdateOwnersVolumeToMoreThan(int value)
        {
            var netVolume = this.Get<ElementPage>().GetElementText(nameof(Resources.NetVolume));
            this.Get<ElementPage>().GetElement(nameof(Resources.OwnerInputBoxes), 3).SendKeys(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
