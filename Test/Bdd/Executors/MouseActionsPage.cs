// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseActionsPage.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Executors
{
    using System;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Ocaramba;
    using Ocaramba.Extensions;
    using Ocaramba.Types;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;

    public class MouseActionsPage : ProjectPageBase
    {
        private readonly Actions actions;

        public MouseActionsPage(DriverContext driverContext)
                    : base(driverContext)
        {
            this.actions = new Actions(driverContext?.Driver);
        }

        public void DragAndDropToARandomPosition(IWebElement element, int x, int y)
        {
            this.actions.ClickAndHold(element).MoveByOffset(x, y).Build().Perform();
        }

        public void DragAndDrop(IWebElement sourceElement, IWebElement destinationElement)
        {
            this.actions.DragAndDrop(sourceElement, destinationElement).Build().Perform();
        }

        public void MouseHover(string resourceKey, params object[] formatArgs)
        {
            ElementLocator elementLocator = this.DriverContext.GetElementLocator(resourceKey, formatArgs);
            this.MouseHover(elementLocator);
        }

        public void MouseHover(ElementLocator elementLocator)
        {
            IWebElement element = this.GetElement(elementLocator);
            this.MouseHover(element);
        }

        public void MouseHover(IWebElement element)
        {
            this.actions.MoveToElement(element).Perform();
        }

        public IWebElement GetElement(ElementLocator elementLocator, double timeout = 15.0)
        {
            WebDriverWait webDriverWait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(timeout));
            return webDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(elementLocator.ToBy()));
        }
    }
}