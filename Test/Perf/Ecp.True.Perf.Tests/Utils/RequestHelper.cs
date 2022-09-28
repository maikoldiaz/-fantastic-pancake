// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestHelper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// Based on https://social.msdn.microsoft.com/Forums/en-US/e639498b-3e3b-4c47-8a38-b910a8be70fc/ssl-in-webtests-vsts-2008-and-2010?forum=vstswebtest
namespace Ecp.True.Perf.Tests
{
    using System;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    public class RequestHelper : WebTestPlugin
    {
        private const string TraceIdHeaderKey = "request-id";

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // TODO: Fix the hard-coding
            if (e.Request.Url.Contains(WebTestEx.WebServer1.GetConfigValue()))
            {
                Guid obj = Guid.NewGuid();
                e.Request.Headers.Add(TraceIdHeaderKey, obj.ToString());
            }
        }
    }
}