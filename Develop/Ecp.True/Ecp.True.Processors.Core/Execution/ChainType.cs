// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChainType.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Execution
{
    /// <summary>
    /// The chain type.
    /// </summary>
    public enum ChainType
    {
        /// <summary>
        /// True process analytics.
        /// </summary>
        ProcessAnalytics = 1,

        /// <summary>
        /// The request ownership data.
        /// </summary>
        RequestOwnershipData = 2,

        /// <summary>
        /// The register.
        /// </summary>
        Register = 3,

        /// <summary>
        /// The calculate ownership data.
        /// </summary>
        CalculateOwnershipData = 4,

        /// <summary>
        /// True get delta.
        /// </summary>
        GetDelta = 5,

        /// <summary>
        /// The request ownership data.
        /// </summary>
        RequestDelta = 6,

        /// <summary>
        /// The register.
        /// </summary>
        ProcessDelta = 7,

        /// <summary>
        /// The calculate ownership data.
        /// </summary>
        CompleteDelta = 8,

        /// <summary>
        /// The Build official data.
        /// </summary>
        BuildOfficialData = 9,

        /// <summary>
        /// The  exclude data.
        /// </summary>
        ExcludeData = 10,

        /// <summary>
        /// The RegisterNodeActivity.
        /// </summary>
        RegisterNodeActivity = 11,

        /// <summary>
        /// The request ownership data.
        /// </summary>
        RequestOfficialDelta = 12,

        /// <summary>
        /// The register.
        /// </summary>
        ProcessOfficialDelta = 13,

        /// <summary>
        /// The calculate ownership data.
        /// </summary>
        RegisterMovementsOfficialDelta = 14,

        /// <summary>
        /// The calculate official delta.
        /// </summary>
        CalculateOfficialDelta = 15,
    }
}
