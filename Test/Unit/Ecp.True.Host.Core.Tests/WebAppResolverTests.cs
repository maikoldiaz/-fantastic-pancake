// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebAppResolverTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Tests
{
    using System;
    using Ecp.True.Host.Core.OData;
    using Ecp.True.Ioc.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The tests for web app resolver.
    /// </summary>
    [TestClass]
    public class WebAppResolverTests
    {
        private IResolver webappResolver;

        private Mock<IHttpContextAccessor> httpContextAccessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.httpContextAccessor = new Mock<IHttpContextAccessor>();
            this.webappResolver = new WebAppResolver(this.httpContextAccessor.Object);
        }

        /// <summary>
        /// Should get instance from httpcontext accessor.
        /// </summary>
        [TestMethod]
        public void GetInstance_ShouldCallHttpContextAccessor_WhenCalled()
        {
            // Arrange
            var returnValue = new Mock<IODataService>();
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(It.IsAny<Type>())).Returns(returnValue.Object);
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(x => x.RequestServices).Returns(serviceProvider.Object);
            this.httpContextAccessor.SetupGet(x => x.HttpContext).Returns(httpContext.Object);

            // Act
            var result = this.webappResolver.GetInstance<IODataService>();

            // Assert
            Assert.IsNotNull(result);
            serviceProvider.Verify(x => x.GetService(It.IsAny<Type>()), Times.Once);
        }
    }
}
