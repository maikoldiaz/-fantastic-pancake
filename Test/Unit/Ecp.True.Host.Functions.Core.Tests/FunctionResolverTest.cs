// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionResolverTest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Tests
{
    using System;
    using Ecp.True.Host.Core.OData;
    using Ecp.True.Ioc.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The function resolver.
    /// </summary>
    [TestClass]
    public class FunctionResolverTest
    {
        private IResolver functionResolver;

        private Mock<IServiceProvider> serviceProviderMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.serviceProviderMock = new Mock<IServiceProvider>();
            this.functionResolver = new FunctionResolver(this.serviceProviderMock.Object);
        }

        /// <summary>
        /// Should get instance from httpcontext accessor.
        /// </summary>
        [TestMethod]
        public void GetInstance_ShouldCallServiceProvider_WhenCalled()
        {
            // Arrange
            var returnValue = new Mock<IODataService>();
            this.serviceProviderMock.Setup(x => x.GetService(It.IsAny<Type>())).Returns(returnValue.Object);

            // Act
            var result = this.functionResolver.GetInstance<IODataService>();

            // Assert
            Assert.IsNotNull(result);
            this.serviceProviderMock.Verify(x => x.GetService(It.IsAny<Type>()), Times.Once);
        }
    }
}
