// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiExceptionFilterAttribute.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Text.RegularExpressions;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Core;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.EntityFrameworkCore;
    using Nethereum.JsonRpc.Client;
    using Newtonsoft.Json;
    using EntityConstants = Ecp.True.Entities.Constants;
    using SqlConstants = Ecp.True.DataAccess.Sql.Constants;

    /// <summary>
    /// The ADAL token acquisition exception filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [CLSCompliant(false)]
    public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// The validation messages.
        /// </summary>
        private static readonly IDictionary<string, string> Messages = new Dictionary<string, string>
        {
            { "'Admin.Category'", EntityConstants.CategoryNameAlreadyExists },
            { "'Admin.CategoryElement'", EntityConstants.CategoryElementNameAlreadyExists },
            { "'Admin.Node'", EntityConstants.NodeNameMustBeUnique },
            { "'Admin.NodeStorageLocation'", EntityConstants.StorageNameMustBeUnique },
            { "'Admin.FileRegistration'", EntityConstants.RegisterFileUploadIdMustBeUnique },
            { "'Admin.HomologationObject'", EntityConstants.HomologationObjectShouldNotRepeat },
        };

        /// <summary>
        /// The SQL messages.
        /// </summary>
        private static readonly IDictionary<int, string> SqlMessages = new Dictionary<int, string>
        {
            { SqlConstants.NotNullConstraintCode, EntityConstants.NotNullConstraintsError },
            { SqlConstants.IdentityInsertCode, EntityConstants.DuplicateEntityInsert },
            { SqlConstants.ForeignKeyConstraintCode, EntityConstants.EntityNotExists },
            { SqlConstants.NodeNameMustBeUnique, EntityConstants.NodeNameMustBeUnique },
            { SqlConstants.OneOrMoreStorageLocationsDoesNotExistsCode, EntityConstants.OneOrMoreStorageLocationDoesNotExists },
            { SqlConstants.OneOrMoreProductsDoesNotBelongToDatabaseStorageLocationCode, EntityConstants.OneOrMoreProductDoesNotBelongToStorageLocation },
            { SqlConstants.OneOrMoreProductsDoesNotBelogToDatatableStorageLocationCode, EntityConstants.OneOrMoreProductDoesNotBelongToPassedStorageLocation },
            { SqlConstants.DuplicateNodeStorageLocationIdCode, EntityConstants.DuplicateNodeStorageLocationId },
            { SqlConstants.DuplicateStorageLocationProductIdCode, EntityConstants.DuplicateStorageLocationProductId },
            { SqlConstants.StorageLocationNameAlreadyExistsWithinNodeCode, EntityConstants.StorageNameMustBeUnique },
            { SqlConstants.DuplicateStorageLocationNameForSameNode, EntityConstants.StorageNameMustBeUnique },
            { SqlConstants.ConcurrentUpdate, EntityConstants.RowConcurrencyConflict },
        };

        /// <summary>
        /// On Exception.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var httpContext = context.HttpContext;
            var resourceProvider = (IResourceProvider)httpContext.RequestServices.GetService(typeof(IResourceProvider));

            var unauthorizedAccessException = context.Exception as UnauthorizedAccessException;
            if (unauthorizedAccessException != null)
            {
                context.Result = new UnauthorizedResult((int)System.Net.HttpStatusCode.Forbidden, AuthorizationErrorCode.NoAccess);
                return;
            }

            var argumentNullException = context.Exception as ArgumentNullException;
            if (argumentNullException != null)
            {
                context.Result = httpContext.BuildErrorResult(string.Empty);
                return;
            }

            var keyNotFoundException = context.Exception as KeyNotFoundException;
            if (keyNotFoundException != null)
            {
                var error = resourceProvider.GetResource(keyNotFoundException.Message);
                context.Result = httpContext.BuildErrorResult(error);
                return;
            }

            var invalidDataException = context.Exception as InvalidDataException;
            if (invalidDataException != null)
            {
                var error = resourceProvider.GetResource(invalidDataException.Message);
                context.Result = httpContext.BuildErrorResult(error);
                return;
            }

            var rpcTimeoutException = context.Exception as RpcClientTimeoutException;
            if (rpcTimeoutException != null)
            {
                context.Result = httpContext.BuildErrorResult(EntityConstants.RpcTimeoutException);
            }

            var jsonReaderException = context.Exception as JsonReaderException;
            if (jsonReaderException != null)
            {
                context.Result = context.HttpContext.BuildErrorResult(HandleJsonReaderError(jsonReaderException));
            }

            HandleDatabaseError(context, resourceProvider);
        }

        private static void HandleDatabaseError(ExceptionContext context, IResourceProvider resourceProvider)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var httpContext = context.HttpContext;
            if (context.Exception is DBConcurrencyException)
            {
                context.Result = httpContext.BuildConflictError(EntityConstants.RowConcurrencyConflict);
            }

            var sqlException = context.Exception as SqlException;
            if (sqlException != null)
            {
                var error = HandleSqlErrors(sqlException, resourceProvider);
                context.Result = error == EntityConstants.RowConcurrencyConflict
                ? httpContext.BuildConflictError(EntityConstants.RowConcurrencyConflict)
                : httpContext.BuildErrorResult(error);
            }

            var dbUpdateConcurrencyException = context.Exception as DbUpdateConcurrencyException;
            if (dbUpdateConcurrencyException != null)
            {
                context.Result = httpContext.BuildConflictError(EntityConstants.RowConcurrencyConflict);
            }

            var dbUpdateException = context.Exception as DbUpdateException;
            if (dbUpdateException != null)
            {
                sqlException = dbUpdateException.InnerException as SqlException;
                if (sqlException != null)
                {
                    var error = HandleSqlErrors(sqlException, resourceProvider);
                    context.Result = httpContext.BuildErrorResult(error);
                }
            }
        }

        private static string HandleSqlErrors(SqlException sqlException, IResourceProvider resourceProvider)
        {
            string error = null;

            if (sqlException.Number == SqlConstants.UniqueConstraintCode)
            {
                foreach (var item in Messages)
                {
                    if (sqlException.Message.Contains(item.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        error = resourceProvider.GetResource(item.Value);
                    }
                }
            }
            else if (SqlMessages.ContainsKey(sqlException.Number))
            {
                error = resourceProvider.GetResource(SqlMessages[sqlException.Number]);
            }
            else if (sqlException.Number >= SqlConstants.SqlCustomErrorCode)
            {
                error = resourceProvider.GetResource(sqlException.Message);
            }
            else
            {
                error = "Unknown-" + sqlException.Message;
            }

            return error;
        }

        private static IList<ErrorInfo> HandleJsonReaderError(JsonReaderException ex)
        {
            var errors = new List<ErrorInfo>();
            var regex = @"(\w+) valid (\w+)";
            var m = Regex.Match(ex.Message, regex);
            if (m.Groups.Count < 2)
            {
                regex = @"(\w+) to (\w+)";
                m = Regex.Match(ex.Message, regex);
            }

            errors.Add(new ErrorInfo("4000", $"{ex.Path} {Constants.InvalidDataType} {m.Groups[2]}"));
            return errors;
        }
    }
}