// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Routes.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Perf.Tests
{
    public static class Routes
    {
        public const string CategoryElements = "/v1/categoryelements";
        public const string SearchCategoryElements = "/v1/odata/categoryelements?$expand=category&$top=10&$filter=contains(name,%27{0}%27)%20and%20contains(category/name,%27{1}%27)";
        public const string Nodes = "/v1/nodes";
        public const string SearchNodes = "/v1/odata/nodes?$select=nodeId,name,controlLimit,acceptableBalancePercentage,isActive&$top=10&$filter=contains(name,%27perfnode_{0}_{1}%27)";
        public const string NodeConnections = "/v1/nodeconnections";

        public const string CategoryElementCategories = "/v1/odata/categoryelements?$expand=category&$top=10";
        public const string NodeAttributes = "/v1/odata/nodes?$select=nodeId,name,controlLimit,acceptableBalancePercentage,isActive&$top=10";
        public const string NodeConnectionSourceNodes = "/v1/odata/nodeconnections?$expand=SourceNode($select=name),DestinationNode($select=name)&$select=nodeConnectionId,sourceNodeId,destinationNodeId,controlLimit,isActive&$top=10";

        public const string Homologations = "/v1/homologations";

        public const string FileRegistrations = "/v1/odata/fileregistrations?$top=10&$filter=SystemTypeId%20eq%20Ecp.True.Entities.Dto.SystemType%27EXCEL%27%20and%20createdDate%20gt%202019-12-15T00:00:00.000Z&$orderby=createdDate%20desc";
        public const string FileRegistrationReadAccess = "/v1/fileregistration/readaccessinfo";

        public const string CutoffTickets = "/v1/odata/tickets?$expand=CategoryElement($expand=Category)&$top=10&$filter=createdDate%20gt%202019-11-07T05:50:25.807Z%20and%20ticketTypeId%20eq%20%27Cutoff%27&$orderby=createdDate%20desc";
        public const string OwnershipTickets = "/v1/odata/tickets?$expand=CategoryElement($expand=Category)&$top=10&$filter=createdDate%20gt%202019-11-07T05:55:38.431Z%20and%20ticketTypeId%20eq%20%27Ownership%27&$orderby=createdDate%20desc";
        public const string OwnershipNodes = "/v1/odata/ownershipnodes?$expand=ticket($expand=categoryElement),node($select=name)&$top=10&$filter=createdDate%20gt%202019-11-07T05:55:45.913Z%20and%20((ticket/status%20eq%20%27FAILED%27%20and%20status%20ne%20%27PROCESSING%27)%20or%20(ticket/status%20eq%20%27PROCESSED%27%20or%20ticket/status%20eq%20%27PROCESSING%27))%20&$orderby=createdDate%20desc";
        public const string OwnershipTicket = "/v1/odata/tickets?$top=1&$orderby=createdDate%20desc&$filter=ticketTypeId%20eq%20%27Ownership%27";

        public const string BalanceReportConfig01 = "/v1/reportConfigs/10.10.01BalanceOperativoSinPropiedad01";
        public const string BalanceReportConfig04 = "/v1/reportConfigs/10.10.04BalanceOperativo04";
        public const string EvaluationReportConfig03 = "/v1/reportConfigs/10.10.03EvaluacionModelosAnaliticosPorcentajePropiedad03";
        public const string ReportClusterMetadata = "https://api.powerbi.com/metadata/cluster";
        public const string ReportQueryData = "https://wabi-south-central-us-redirect.analysis.windows.net/explore/querydata";

        public const string Report1 = "https://app.powerbi.com/reportEmbed?reportId=75af10f8-4d1d-402e-88c9-399c06280c37&groupId=66132eec-e218-4b36-ac8f-d479c8bb0b4d&language=es&formatLocale=es&uid=1ksemg";
        public const string Report2 = "https://wabi-south-central-us-redirect.analysis.windows.net/explore/reports/75af10f8-4d1d-402e-88c9-399c06280c37/modelsAndExploration?preferReadOnlySession=true";
        public const string Report3 = "https://wabi-south-central-us-redirect.analysis.windows.net/explore/reports/75af10f8-4d1d-402e-88c9-399c06280c37/conceptualschema?userPreferredLocale=es";
        public const string Report4 = "https://app.powerbi.com/13.0.12371.183/cvSandboxMinimal.html?reportId=75af10f8-4d1d-402e-88c9-399c06280c37&groupId=66132eec-e218-4b36-ac8f-d479c8bb0b4d&language=es&formatLocale=es&uid=1ksemg&plugin=kpiTicker492C9305B9464241B52382527F977DE1&locale=es-ES";
        public const string Report5 = "https://app.powerbi.com/reportEmbed?reportId=75af10f8-4d1d-402e-88c9-399c06280c37&groupId=66132eec-e218-4b36-ac8f-d479c8bb0b4d&language=es&formatLocale=en-US&uid=4irlt";
        public const string Report6 = "https://wabi-south-central-us-redirect.analysis.windows.net//explore/reports/ad97d10c-7088-47a3-a166-29b336cd045f/modelsAndExploration?preferReadOnlySession=true";
        public const string Report7 = "https://wabi-south-central-us-redirect.analysis.windows.net//explore/reports/ad97d10c-7088-47a3-a166-29b336cd045f/conceptualschema?userPreferredLocale=es";
        public const string Report8 = "https://app.powerbi.com/13.0.12371.183/cvSandboxMinimal.html?reportId=ad97d10c-7088-47a3-a166-29b336cd045f&groupId=66132eec-e218-4b36-ac8f-d479c8bb0b4d&language=es&formatLocale=es&uid=1ksemg&plugin=kpiTicker492C9305B9464241B52382527F977DE1&locale=es-ES";

        public const string Report9 = "https://app.powerbi.com/reportEmbed?reportId=f3fc1e98-6673-4d16-b3a1-0ccaa7169aef&groupId=66132eec-e218-4b36-ac8f-d479c8bb0b4d&language=es&formatLocale=en-US&uid=c9nos";
        public const string Report10 = "https://wabi-south-central-us-redirect.analysis.windows.net//explore/reports/f3fc1e98-6673-4d16-b3a1-0ccaa7169aef/modelsAndExploration?preferReadOnlySession=true";
        public const string Report11 = "https://wabi-south-central-us-redirect.analysis.windows.net//explore/reports/f3fc1e98-6673-4d16-b3a1-0ccaa7169aef/conceptualschema?userPreferredLocale=es";
        public const string Report12 = "https://app.powerbi.com/13.0.12594.185/cvSandboxMinimal.html?reportId=f3fc1e98-6673-4d16-b3a1-0ccaa7169aef&groupId=66132eec-e218-4b36-ac8f-d479c8bb0b4d&language=es&formatLocale=en-US&uid=c9nos&plugin=kpiTicker492C9305B9464241B52382527F977DE1&locale=es-ES";
    }
}
