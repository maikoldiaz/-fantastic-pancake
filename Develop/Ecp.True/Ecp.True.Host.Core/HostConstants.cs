// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostConstants.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The host constants.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HostConstants
    {
        /// <summary>
        /// The sap role claim type.
        /// </summary>
        public const string SapRoleClaimType = "true-sap";

        /// <summary>
        /// The flow role claim type.
        /// </summary>
        public const string FlowRoleClaimType = "true-flow";

        /// <summary>
        /// Default session timeout.
        /// </summary>
        public static readonly TimeSpan DefaultSessionTimeout = TimeSpan.FromMinutes(90);

        /// <summary>
        /// Default session IO timeout.
        /// </summary>
        public static readonly TimeSpan DefaultSessionIOTimeout = TimeSpan.FromMinutes(5);

        /// <summary>
        /// The client side request header.
        /// </summary>
        public static readonly string ClientSideRequestHeader = "WhoAmI";

        /// <summary>
        /// The acr claim type.
        /// </summary>
        public static readonly string AcrClaimType = "tfp";

        /// <summary>
        /// The error code route.
        /// </summary>
        public static readonly string ErrorCodeRoute = "/error/details/{0}";

        /// <summary>
        /// The unhandled error message.
        /// </summary>
        public static readonly string UnhandledErrorMessage = "Unhandled error occurred";

        /// <summary>
        /// The default cache timeout.
        /// </summary>
        public static readonly TimeSpan DefaultCacheTimeOut = TimeSpan.FromHours(1);

        /// <summary>
        /// The default auth token cache timeout.
        /// </summary>
        public static readonly TimeSpan DefaultAuthTokenCacheTimeout = TimeSpan.FromMinutes(55);

        /// <summary>
        /// The default last accessed key.
        /// </summary>
        public static readonly string LastAccessedKey = "LastAccessed";

        /// <summary>
        /// The ODATA.
        /// </summary>
        public static readonly string OData = "OData";

        /// <summary>
        /// The disabled site error code.
        /// </summary>
        public static readonly string DisabledSiteErrorCode = "403";

        /// <summary>
        /// The not found error code.
        /// </summary>
        public static readonly string NotFoundErrorCode = "404";

        /// <summary>
        /// The bad request error code.
        /// </summary>
        public static readonly string BadRequestErrorCode = "400";

        /// <summary>
        /// The row concurrency conflict.
        /// </summary>
        public static readonly string RowConcurrencyConflict = "409";
    }
}