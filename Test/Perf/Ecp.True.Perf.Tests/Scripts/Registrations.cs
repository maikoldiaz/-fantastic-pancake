// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Registrations.cs" company="Microsoft">
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
    public class Registrations : WebTest
    {
        //// private RequestHelper requestHelper = new RequestHelper();

        public Registrations()
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

            this.BeginTransaction("Cargue de Movimientos e inventarios Page");
            yield return this.ExecuteRequest(Routes.FileRegistrations);
            yield return this.ExecuteRequest(Routes.FileRegistrationReadAccess);
            this.EndTransaction("Cargue de Movimientos e inventarios Page");

            this.BeginTransaction("Corte Operativo Page");
            yield return this.ExecuteRequest(Routes.FileRegistrations);
            yield return this.ExecuteRequest(Routes.CutoffTickets);
            this.EndTransaction("Corte Operativo Page");

            yield return this.ExecuteTransaction("Balance Volumetrico con propiedid Page", Routes.OwnershipTickets);

            this.BeginTransaction("Balance Volumetrico con propiedid por nodo Page");
            yield return this.ExecuteRequest(Routes.OwnershipNodes);
            yield return this.ExecuteRequest(Routes.OwnershipTicket);
            this.EndTransaction("Balance Volumetrico con propiedid por nodo Page");
        }
    }
}
