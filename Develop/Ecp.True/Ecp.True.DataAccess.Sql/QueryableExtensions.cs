// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryableExtensions.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Ecp.True.Core;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.Internal;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// Debug extensions for SQL server query.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class QueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        private static readonly FieldInfo QueryModelGeneratorField = typeof(QueryCompiler).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryModelGenerator");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        /// <summary>
        /// Gets the compiled sql query string for debug purposes.
        /// </summary>
        /// <typeparam name="TEntity">The entity.</typeparam>
        /// <param name="query">The queryable object.</param>
        /// <returns>The SQL query string.</returns>
        public static string ToSql<TEntity>(this IQueryable<TEntity> query)
        {
            ArgumentValidators.ThrowIfNull(query, nameof(query));

            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var queryModelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = queryModelGenerator.ParseQuery(query.Expression);
            var database = DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }
    }
}