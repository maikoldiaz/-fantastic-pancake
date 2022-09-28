// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportGeneratorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Reporting.Tests.Services
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Moq;

    public class ReportGeneratorTests
    {
        /// <summary>
        /// Creates the SQL exception stub.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>Return the sql exception.</returns>
        protected SqlException CreateSqlExceptionStub(int number = 1, string errorMessage = "error message")
        {
            var collection = this.Construct<SqlErrorCollection>();
            var error = this.Construct<SqlError>(number, (byte)2, (byte)3, "server name", errorMessage, "proc", 100, new Exception());

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
        /// Sets up repository mock.
        /// </summary>
        /// <param name="repositoryMock">The repository mock.</param>
        /// <param name="entity">The entity.</param>
        protected void SetUpRepositoryMock(Mock<IRepository<ReportExecution>> repositoryMock, ReportExecution entity)
        {
            ArgumentValidators.ThrowIfNull(repositoryMock, nameof(repositoryMock));

            repositoryMock.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<ReportExecution, bool>>>())).ReturnsAsync(entity);
            repositoryMock.Setup(m => m.Update(It.IsAny<ReportExecution>()));
        }

        /// <summary>
        /// Verifies the repository mock.
        /// </summary>
        /// <param name="repositoryMock">The repository mock.</param>
        /// <param name="statusType">Type of the status.</param>
        protected void VerifyRepositoryMock(Mock<IRepository<ReportExecution>> repositoryMock, StatusType statusType)
        {
            ArgumentValidators.ThrowIfNull(repositoryMock, nameof(repositoryMock));

            repositoryMock.Verify(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<ReportExecution, bool>>>()), Times.Once);
            repositoryMock.Verify(m => m.Update(It.Is<ReportExecution>(r => r.StatusTypeId == statusType)), Times.Once);
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
