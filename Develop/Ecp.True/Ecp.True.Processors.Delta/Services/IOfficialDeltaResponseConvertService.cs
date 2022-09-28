// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOfficialDeltaResponseConvertService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Services
{
    using System.Collections.Generic;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The official delta response convert service.
    /// </summary>
    public interface IOfficialDeltaResponseConvertService
    {
        /// <summary>
        /// Converts the delta response.
        /// </summary>
        /// <param name="deltaResults">The result enumerable.</param>
        /// <param name="deltaData">The official delta data.</param>
        /// <returns>The converted official delta result enumerable.</returns>
        IEnumerable<OfficialResultMovement> ConvertOfficialDeltaResponse(IEnumerable<OfficialDeltaResultMovement> deltaResults, OfficialDeltaData deltaData);
    }
}