// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRolesAndPermissionsReportGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Reporting.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Common;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Microsoft.Graph;
    using Microsoft.Graph.Auth;
    using Microsoft.Identity.Client;
    using Constants = Ecp.True.Repositories.Constants;

    /// <summary>
    /// user, Roles and Permissions report generator.
    /// </summary>
    public class UserRolesAndPermissionsReportGenerator : ReportGenerator
    {
        private IEnumerable<UserRoleType> users;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRolesAndPermissionsReportGenerator" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="logger">The logger.</param>
        public UserRolesAndPermissionsReportGenerator(
            IUnitOfWorkFactory unitOfWorkFactory,
            IConfigurationHandler configurationHandler,
            ITrueLogger<CutoffReportGenerator> logger)
            : base(unitOfWorkFactory, configurationHandler, logger)
        {
        }

        /// <inheritdoc/>
        public override ReportType Type => ReportType.UserRolesAndPermissions;

        /// <inheritdoc/>
        protected override async Task DoGenerateAsync(ReportExecution entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            if (entity.Name != UserRolesReport.UserGroupAccessReport.ToString())
            {
                var rolesConfig = await this.ConfigurationHandler.GetConfigurationAsync<UserRoleSettings>(ConfigurationConstants.UserRoleSettings).ConfigureAwait(false);

                var graphClient = await this.GetAuthenticatedGraphClientAsync().ConfigureAwait(false);

                var taskGroups = rolesConfig.Mapping
                    .Select(item => Task.Run(async () =>
                    {
                        var members = await graphClient.Groups[item.Value]
                            .Members.Request(new List<QueryOption> { new QueryOption("$top", "999") })
                            .Select("id,displayName,mail,userType")
                            .GetAsync()
                    .ConfigureAwait(false);

                        return this.MapToUser(members, item.Key);
                    }))
                    .ToList();

                var resultTasks = await Task.WhenAll(taskGroups).ConfigureAwait(false);

                this.users = resultTasks.SelectMany(x => x).ToList();
            }

            await this.GenerateUserGroupsAsync(entity).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override IDictionary<string, object> GetParameters(ReportExecution entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var parameters = new Dictionary<string, object>
            {
                { "@ExecutionId", entity.ExecutionId },
            };

            if (entity.Name != UserRolesReport.UserGroupAccessReport.ToString())
            {
                parameters.Add("@userGroup", this.users.ToDataTable(Constants.UsersGroupsReportRequestTable));
            }

            return parameters;
        }

        private async Task<GraphServiceClient> GetAuthenticatedGraphClientAsync()
        {
            var graphSettings = await this.ConfigurationHandler.GetConfigurationAsync<GraphSettings>(ConfigurationConstants.GraphSettings).ConfigureAwait(false);
            var authority = $"https://login.microsoftonline.com/{graphSettings.TenantId}/oauth2/v2.0/token";

            var cca = ConfidentialClientApplicationBuilder.Create(graphSettings.ClientId)
                                                      .WithClientSecret(graphSettings.ClientSecret)
                                                      .WithTenantId(graphSettings.TenantId)
                                                      .WithAuthority(new Uri(authority))
                                                      .Build();

            return new GraphServiceClient(new ClientCredentialProvider(cca));
        }

        private Task GenerateUserGroupsAsync(ReportExecution entity)
        {
            // Run wrapper
            var storeProcedure = entity.Name != UserRolesReport.UserGroupAccessReport.ToString() ? Constants.UsersGroupsReportRequestProcedureName : Constants.MenusRolDetailsReportRequestTable;

            return this.ExecuteAsync(entity, storeProcedure);
        }

        private IEnumerable<UserRoleType> MapToUser(
            IEnumerable resultGraphUser, Role role)
        {
            return (
                from Microsoft.Graph.User userCast in resultGraphUser
                select new UserRoleType
                {
                    UserId = userCast.Id,
                    Name = userCast.DisplayName,
                    Email = userCast.Mail,
                    RoleId = (int)role,
                    UserType = userCast.UserType,
                }).ToList();
        }
    }
}