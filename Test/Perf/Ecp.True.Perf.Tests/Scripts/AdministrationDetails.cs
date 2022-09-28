// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdministrationDetails.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    [SuppressMessage("Design", "CA1010:Collections should implement generic interface", Justification = "Pending")]
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Pending")]
    [Priority(0)]
    //// [IncludeCodedWebTest(WebTestEx.IncludeClass, WebTestEx.IncludePath)]
    public class AdministrationDetails : WebTest
    {
        //// private RequestHelper requestHelper = new RequestHelper();

        public AdministrationDetails()
        {
            //// this.PreRequest += new EventHandler<PreRequestEventArgs>(this.requestHelper.PreRequest);
            this.Init();
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            foreach (var login in this.SetupLogin())
            {
                yield return login;
            }

            yield return this.ExecuteTransaction("Category Element Page", Routes.CategoryElementCategories, addXsrfHeader: false);
            yield return this.ExecuteTransaction("Node Attribute Page", Routes.NodeAttributes, addXsrfHeader: false);
            yield return this.ExecuteTransaction("Node Connection Page", Routes.NodeConnectionSourceNodes, addXsrfHeader: false);
        }
    }
}
