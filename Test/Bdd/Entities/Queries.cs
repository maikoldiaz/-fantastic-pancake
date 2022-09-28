// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Queries.cs" company="Microsoft">
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
    public static class Queries
    {
        public const string FetchFicoAuditLogs = "traces | extend itemType = iif(itemType == 'trace', itemType, \"\") | where(itemType == 'trace') and message startswith \"Audited Steps\" | where * has 'OwnershipRulesSync'" +
                                                 "| top 9 by timestamp desc";

        public const string FetchFicoExceptions = "exceptions| extend itemType = iif(itemType == 'exception', itemType,\"\")| where(itemType == 'exception')| where * has 'OwnershipRulesSync'" +
                                                  "| top 1 by timestamp desc";

        public const string FetchFicoResponseTime = "traces | extend itemType = iif(itemType == 'trace', itemType, \"\") | where(itemType == 'trace') and message startswith \"Time Taken\" | where * has 'OwnershipRulesSync'" +
                                                    "| top 2 by timestamp desc";

        public const string CountOfDeadLetteredMessages = "traces | where message contains 'Message received with property deadletter' | where timestamp  > ago(5m)";

        public const string ConsolidationTechnicalExceptionMessage = "traces | where message contains 'Se presentó un error técnico inesperado en la consolidación de movimientos e inventarios del escenario operativo. Por favor ejecute nuevamente el proceso o comuníquese con la mesa de ayuda.' | where timestamp > ago(5m) |top 1 by timestamp desc";
    }
}