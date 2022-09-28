// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIToViewOwnershipDetailsForMovementOrInventoriesSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class UIToViewOwnershipDetailsForMovementOrInventoriesSteps : EcpWebStepDefinitionBase
    {
        [Then(@"I verify i am able to see details of the movement")]
        public void ThenIVerifyIAmAbleToSeeDetailsOfTheMovement()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ViewModalTable));
            for (int j = 0; j < 8; j++)
            {
                var text = this.DriverContext.Driver.FindElement(By.XPath("//*[@id='cont_modal_movementOwnership']/div/section/form/section/section[1]/div/table/tbody/tr/td[" + (j + 1) + "]")).Text;
                if (text != null)
                {
#pragma warning disable S3626 // Jump statements should not be redundant
                    continue;
#pragma warning restore S3626 // Jump statements should not be redundant
                }
                else
                {
                    break;
                }
            }
        }

        [Then(@"I verify i am able to see the reason for change comment")]
        public void ThenIVerifyIAmAbleToSeeTheReasonForChangeComment()
        {
            var text = this.Get<ElementPage>().FindElementById(nameof(Resources.OwnershipCommentBox)).Text;
            if (text == null)
            {
                Assert.Fail("Comment Not Found!");
            }
        }

        [Then(@"I verify i am able to see details of the inventory")]
        public void ThenIVerifyIAmAbleToSeeDetailsOfTheInventory()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ViewModalTable));
            for (int j = 0; j < 7; j++)
            {
                var text = this.DriverContext.Driver.FindElement(By.XPath("//*[@id='cont_modal_inventoryOwnership']/div/section/form/section/section[1]/div/table/tbody/tr/td[" + (j + 1) + "]")).Text;
                if (text != null)
                {
#pragma warning disable S3626 // Jump statements should not be redundant
                    continue;
#pragma warning restore S3626 // Jump statements should not be redundant
                }
                else
                {
                    break;
                }
            }
        }
    }
}
