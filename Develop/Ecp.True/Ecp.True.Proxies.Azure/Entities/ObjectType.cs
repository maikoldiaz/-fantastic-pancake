// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectType.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure.Entities
{
    /// <summary>
    /// The object type.
    /// </summary>
    public enum ObjectType
    {
        /// <summary>
        /// The cutoff
        /// </summary>
        Cutoff = 0,

        /// <summary>
        /// The ownership calculation.
        /// </summary>
        OwnershipCalculation = 1,

        /// <summary>
        /// The audit.
        /// </summary>
        Audit = 2,

        /// <summary>
        /// The transfer point
        /// </summary>
        TransferPoint = 3,

        /// <summary>
        /// The official delta.
        /// </summary>
        OfficialDelta = 4,
    }
}
