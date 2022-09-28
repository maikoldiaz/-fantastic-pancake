// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ADataFactoryClient.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Bdd.Tests.Entities
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Threading.Tasks;

    using global::Bdd.Core.Utils;

    using Microsoft.Azure.Management.DataFactory;
    using Microsoft.Azure.Management.DataFactory.Models;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Rest;

    public static class ADataFactoryClient
    {
        private static readonly NameValueCollection Settings = ConfigurationManager.GetSection("adfpipeline") as NameValueCollection;

        public static async Task<string> RunADFPipelineAsync(string pipelineName, dynamic pipelineParameterValue = default)
        {
            PipelineRun pipelineRun;
            var accessToken = await ADataFactoryClient.GetADFAccessTokenAsync().ConfigureAwait(false);
            ServiceClientCredentials cred = new TokenCredentials(accessToken);

            using (DataFactoryManagementClient clientDF = new DataFactoryManagementClient(cred) { SubscriptionId = GenericExtensions.GetValue(Settings, "SubscriptionId", prefix: "ADF", true) })
            {
                var resourceGroupName = GenericExtensions.GetValue(Settings, "ResourceGroupName", prefix: "ADF");
                var dataFactoryName = GenericExtensions.GetValue(Settings, "DataFactoryName");
                string runId;

                if (pipelineParameterValue != null && (pipelineName == "ADF_OperativeMovementsHistory" || pipelineName == "ADF_OperativeMovementswithOwnerShip"))
                {
                    var pipelineParameters = new Dictionary<string, object>
                    {
                        { "LoadType", pipelineParameterValue },
                    };

                    runId = clientDF.Pipelines.CreateRun(resourceGroupName, dataFactoryName, pipelineName, parameters: pipelineParameters).RunId;
                }
                else
                {
                    runId = clientDF.Pipelines.CreateRun(resourceGroupName, dataFactoryName, pipelineName).RunId;
                }

                while (true)
                {
                    pipelineRun = clientDF.PipelineRuns.Get(resourceGroupName, dataFactoryName, runId);
                    if (pipelineRun.Status == "InProgress" || pipelineRun.Status == "Queued")
                    {
                        System.Threading.Thread.Sleep(4000);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return pipelineRun.Status;
        }

        public static async Task<string> GetADFAccessTokenAsync()
        {
            var tenantId = GenericExtensions.GetValue(Settings, "TenantId", prefix: "ADF", true);
            var context = new AuthenticationContext("https://login.windows.net/" + tenantId);
            ClientCredential cc = new ClientCredential(GenericExtensions.GetValue(Settings, "ClientId", prefix: "ADF", true), GenericExtensions.GetValue(Settings, "ClientSecret", prefix: "ADF", true));
            AuthenticationResult result = await context.AcquireTokenAsync("https://management.azure.com/", cc).ConfigureAwait(false);
            return result.AccessToken;
        }
    }
}