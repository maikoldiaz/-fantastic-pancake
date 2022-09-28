// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransformationOfficialDeltaNode.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Approval.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// Interface ITransformationOfficialDeltaNode.
    /// </summary>
    public interface ITransformationOfficialDeltaNode
    {
        /// <summary>
        /// Apply Transformation Official Delta.
        /// </summary>
        /// <param name="movements">movements.</param>
        /// <param name="dateCutOff">date cut off.</param>
        /// <returns>IEnumerable Movement.</returns>
        IEnumerable<Movement> ApplyTransformationOfficialDelta(IEnumerable<OfficialDeltaNodeMovement> movements, DateTime dateCutOff);
    }
}
