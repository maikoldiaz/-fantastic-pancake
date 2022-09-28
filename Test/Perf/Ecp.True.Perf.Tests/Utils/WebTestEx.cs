// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebTestEx.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Runtime.CompilerServices;

    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;

    public static class WebTestEx
    {
        internal const string IncludeClass = "Ecp.True.Perf.Tests.LoginHelper";
        internal const string IncludePath = "ecp.true.perf.tests.dll";

        internal const string WebServer1 = "WebServer1";
        internal const string WebServer2 = "https://fs.ecopetrol.com.co";
        internal const string WebServer3 = "https://login.microsoftonline.com";

        internal static readonly string[] CategoryNames = new string[] { "Tipo%20de%20Nodo", "Segmento", "Operador", "Organizacional" };
        internal static readonly string[] CategoryNames2 = new string[] { "Tipo%20Movimiento", "Tipo%20Producto", "Tipo%20Producto", "Tipo%20Producto", "Propietario" };

        internal static readonly Configuration ConfigManager = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = "Ecp.True.Perf.Tests.dll.config" }, ConfigurationUserLevel.None);

        private const string XsrfTokenHeaderKey = "x-xsrf-token";
        private const string StaParam = "param_sta";
        private const string JsonContentType = "application/json; charset=UTF-8";
        private const string PostMethod = "POST";
        private const string HttpPrefix = "http";
        private const string AuthHeaderKey = "Authorization";
        private const string EmbedTokenPrefix = "EmbedToken ";
        private const string TokParam = "param_tok";

        public static void Init(this WebTest webTest)
        {
            webTest.ThrowIfNull(nameof(webTest));
            webTest.Context.Add(nameof(WebServer1), WebServer1.GetConfigValue());
            webTest.Context.Add(nameof(WebServer2), WebServer2);
            webTest.Context.Add(nameof(WebServer3), WebServer3);
            webTest.PreAuthenticate = true;
            webTest.Proxy = "default";

            // TODO: Do we need this?
            if (WebServer1.GetConfigValue().IndexOf("tst", StringComparison.OrdinalIgnoreCase) > 0)
            {
                webTest.PreRequest += new EventHandler<PreRequestEventArgs>(new RequestHelper().PreRequest);
            }
        }

        public static IEnumerable<WebTestRequest> SetupLogin(this WebTest webTest)
        {
            webTest.ThrowIfNull(nameof(webTest));
            foreach (WebTestRequest r in webTest.IncludeWebTest(new LoginHelper(), false))
            {
                yield return r;
            }
        }

        public static WebTestRequest ExecuteTransaction(this WebTest webTest, string name, string endpoint, Action<WebTestRequest> addtionalSetters = null, bool addXsrfHeader = true)
        {
            webTest.ThrowIfNull(nameof(webTest));
            webTest.BeginTransaction(name);
            var request = webTest.ExecuteRequest(endpoint, addtionalSetters, addXsrfHeader);
            webTest.EndTransaction(name);
            return request;
        }

        ////public static IEnumerable<WebTestRequest> ExecuteTransaction(this WebTest webTest, string name, Func<IEnumerable<WebTestRequest>> requests)
        ////{
        ////    webTest.ThrowIfNull(nameof(webTest));
        ////    webTest.BeginTransaction(name);
        ////    foreach (var request in requests?.Invoke())
        ////    {
        ////        yield return request;
        ////    }
        ////    webTest.EndTransaction(name);
        ////}

        public static WebTestRequest ExecuteRequest(this WebTest webTest, string endpoint, Action<WebTestRequest> addtionalSetters = null, bool addXsrfHeader = true)
        {
            webTest.ThrowIfNull(nameof(webTest));
            var request = new WebTestRequest(endpoint?.StartsWith(HttpPrefix, StringComparison.OrdinalIgnoreCase) == true ? endpoint : webTest.GetEndpoint(endpoint));
            if (addXsrfHeader)
            {
                request.Headers.Add(new WebTestRequestHeader(XsrfTokenHeaderKey, webTest.Context[StaParam].ToString()));
            }

            request.ThinkTime = 10; // TODO: Required in case of POST (addtionalSetters != null)?
            addtionalSetters?.Invoke(request);
            return request;
        }

        public static WebTestRequest ExecuteReportRequest(this WebTest webTest, string endpoint, Action<WebTestRequest> addtionalSetters = null, bool addAuth = true, string tokParamName = TokParam)
        {
            webTest.ThrowIfNull(nameof(webTest));
            var request = new WebTestRequest(endpoint);
            if (addAuth)
            {
                request.Headers.Add(AuthHeaderKey, EmbedTokenPrefix + webTest.Context[tokParamName].ToString());
            }
            else
            {
                request.ParseDependentRequests = false;
            }

            addtionalSetters?.Invoke(request);
            return request;
        }

        public static WebTestRequest ExecuteReportPostRequest(this WebTest webTest, string endpoint, string bodyString, WebTestRequest parentRequest = null, string tokParamName = TokParam)
        {
            webTest.ThrowIfNull(nameof(webTest));
            return webTest.ExecuteReportRequest(endpoint, request =>
            {
                PopulatePostRequest(request, bodyString);
                request.Headers.Add(AuthHeaderKey, EmbedTokenPrefix + webTest.Context[tokParamName].ToString());
                parentRequest?.DependentRequests.Add(request);
            });
        }

        public static WebTestRequest ExecutePostTransaction(this WebTest webTest, string name, string endpoint, string bodyString, Action<WebTestRequest> addtionalSetters = null)
        {
            webTest.ThrowIfNull(nameof(webTest));
            return webTest.ExecuteTransaction(name, endpoint, request =>
            {
                PopulatePostRequest(request, bodyString, addtionalSetters);
            });
        }

        public static WebTestRequest ExecutePostRequest(this WebTest webTest, string endpoint, string bodyString, Action<WebTestRequest> addtionalSetters = null)
        {
            webTest.ThrowIfNull(nameof(webTest));
            return webTest.ExecuteRequest(endpoint, request =>
            {
                PopulatePostRequest(request, bodyString, addtionalSetters);
            });
        }

        public static WebTestRequest ExecuteExtractTransaction(this WebTest webTest, string name, string endpoint, string startsWith, string contextParamName, Action<WebTestRequest> addtionalSetters = null)
        {
            webTest.ThrowIfNull(nameof(webTest));
            return webTest.ExecuteTransaction(name, endpoint, request =>
            {
                PopulateExtractRequest(request, startsWith, contextParamName, addtionalSetters);
            });
        }

        public static WebTestRequest ExecuteExtractRequest(this WebTest webTest, string endpoint, string startsWith, string contextParamName, Action<WebTestRequest> addtionalSetters = null)
        {
            webTest.ThrowIfNull(nameof(webTest));
            return webTest.ExecuteRequest(endpoint, request =>
            {
                PopulateExtractRequest(request, startsWith, contextParamName, addtionalSetters);
            });
        }

        public static string GetEndpoint(this WebTest webTest, string route, string server = null)
        {
            webTest.ThrowIfNull(nameof(webTest));
            return webTest.Context[server ?? nameof(WebServer1)] + route;
        }

        public static void ThrowIfNull(this object arg, string argName, [CallerMemberName] string member = "")
        {
            if (arg == null)
            {
                throw new ArgumentNullException($"{member}: {argName}");
            }
        }

        public static string GetConfigValue(this string appSettingsKey)
        {
            appSettingsKey.ThrowIfNull(nameof(appSettingsKey));
            return ConfigManager.AppSettings.Settings[appSettingsKey].Value;
        }

        private static void PopulatePostRequest(WebTestRequest request, string bodyString, Action<WebTestRequest> addtionalSetters = null)
        {
            request.Method = PostMethod;
            addtionalSetters?.Invoke(request);
            var body = new StringHttpBody();
            body.ContentType = JsonContentType;
            body.InsertByteOrderMark = false;
            body.BodyString = bodyString;
            request.Body = body;
        }

        private static void PopulateExtractRequest(WebTestRequest request, string startsWith, string contextParamName, Action<WebTestRequest> addtionalSetters = null, string endsWith = ",")
        {
            addtionalSetters?.Invoke(request);
            var extractText = new ExtractText();
            extractText.StartsWith = startsWith;
            extractText.EndsWith = endsWith;
            extractText.IgnoreCase = false;
            extractText.UseRegularExpression = false;
            extractText.Required = true;
            extractText.ExtractRandomMatch = false;
            extractText.Index = 0;
            extractText.HtmlDecode = true;
            extractText.SearchInHeaders = false;
            extractText.ContextParameterName = contextParamName;
            request.ExtractValues += new EventHandler<ExtractionEventArgs>(extractText.Extract);
        }
    }
}
