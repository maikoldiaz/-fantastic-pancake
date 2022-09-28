// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketType.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Enumeration
{
    /// <summary>
    /// The ticket type.
    /// </summary>
    public enum TicketType
    {
        /// <summary>
        /// The Conciliation,
        /// Field not exist in database
        /// </summary>
        Conciliation = -1,

        /// <summary>
        /// The cutoff
        /// </summary>
        Cutoff = 1,

        /// <summary>
        /// The ownership
        /// </summary>
        Ownership = 2,

        /// <summary>
        /// The logistics
        /// </summary>
        Logistics = 3,

        /// <summary>
        /// The delta
        /// </summary>
        Delta = 4,

        /// <summary>
        /// The official delta.
        /// </summary>
        OfficialDelta = 5,

        /// <summary>
        /// The official logistics.
        /// </summary>
        OfficialLogistics = 6,

        /// <summary>
        /// The logistic movements.
        /// </summary>
        LogisticMovements = 7,
    }
}