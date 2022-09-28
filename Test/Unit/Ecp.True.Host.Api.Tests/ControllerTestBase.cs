// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerTestBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using Ecp.True.Core.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;

    /// <summary>
    /// The controller test base.
    /// </summary>
    public class ControllerTestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerTestBase"/> class.
        /// </summary>
        protected ControllerTestBase()
        {
            this.HttpContext = new DefaultHttpContext();
            this.MockResourceProvider = new Mock<IResourceProvider>();
            this.Routes = new RouteValueDictionary();
            this.RouteData = new RouteData(this.Routes);
            this.ActionDescriptor = new ActionDescriptor();
            this.ActionContext = new ActionContext(this.HttpContext, this.RouteData, this.ActionDescriptor);
        }

        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        /// <value>
        /// The HTTP context.
        /// </value>
        protected HttpContext HttpContext { get; }

        /// <summary>
        /// Gets the routes.
        /// </summary>
        /// <value>
        /// The routes.
        /// </value>
        protected RouteValueDictionary Routes { get; }

        /// <summary>
        /// Gets the route data.
        /// </summary>
        /// <value>
        /// The route data.
        /// </value>
        protected RouteData RouteData { get; }

        /// <summary>
        /// Gets the action descriptor.
        /// </summary>
        /// <value>
        /// The action descriptor.
        /// </value>
        protected ActionDescriptor ActionDescriptor { get; }

        /// <summary>
        /// Gets the mock resource provider.
        /// </summary>
        /// <value>
        /// The mock resource provider.
        /// </value>
        protected Mock<IResourceProvider> MockResourceProvider { get; }

        /// <summary>
        /// Gets the action context.
        /// </summary>
        /// <value>
        /// The action context.
        /// </value>
        protected ActionContext ActionContext { get; }

        /// <summary>
        /// Setups the HTTP context.
        /// </summary>
        protected void SetupHttpContext()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(mockServiceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);

            var loggerMock = new Mock<ILogger<StatusCodeResult>>();
            loggerMock.Setup(ml => ml.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            loggerMock.Setup(x => x.BeginScope(true)).Returns(serviceScope.Object);

            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(a => a.CreateLogger("StatusCodeResult")).Returns(loggerMock.Object);

            mockServiceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            mockServiceProvider
                .Setup(x => x.GetService(typeof(ILoggerFactory)))
                .Returns(loggerFactory.Object);

            this.MockResourceProvider
                .Setup(m => m.GetResource(It.IsAny<string>()))
                .Returns<string>((s) => s);

            mockServiceProvider
                .Setup(m => m.GetService(typeof(IResourceProvider)))
                .Returns(this.MockResourceProvider.Object);

            mockServiceProvider
                .Setup(x => x.GetService(typeof(IActionResultExecutor<ObjectResult>)))
                .Returns(new Mock<IActionResultExecutor<ObjectResult>>().Object);

            this.HttpContext.RequestServices = mockServiceProvider.Object;
        }

        /// <summary>
        /// Creates the SQL exception stub.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>Return the sql exception.</returns>
        protected SqlException CreateSqlExceptionStub(int number = 1, string errorMessage = "error message")
        {
            var collection = this.Construct<SqlErrorCollection>();
            var error = this.Construct<SqlError>(number, (byte)2, (byte)3, "server name", errorMessage , "proc", 100, new Exception());

            typeof(SqlErrorCollection)
                .GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(collection, new object[] { error });

            return typeof(SqlException)
                .GetMethod(
                "CreateException",
                BindingFlags.NonPublic | BindingFlags.Static,
                null,
                CallingConventions.ExplicitThis,
                new[] { typeof(SqlErrorCollection), typeof(string) },
                Array.Empty<ParameterModifier>())
                .Invoke(null, new object[] { collection, "7.0.0" }) as SqlException;
        }

        /// <summary>
        /// Constructs the specified p.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="p">The p.</param>
        /// <returns>Return the type.</returns>
        private T Construct<T>(params object[] p)
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)ctors.First(ctor => ctor.GetParameters().Length == p.Length).Invoke(p);
        }
    }
}
