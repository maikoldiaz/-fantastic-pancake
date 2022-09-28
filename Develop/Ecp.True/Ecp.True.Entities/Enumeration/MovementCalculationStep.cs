// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementCalculationStep.cs" company="Microsoft">
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
    /// Defines the Movement calculation steps.
    /// </summary>
    public enum MovementCalculationStep
    {
        /// <summary>
        /// Defines the Interface
        /// </summary>
        Interface = 1,

        /// <summary>
        /// Defines the BalanceTolerances
        /// </summary>
        BalanceTolerances = 2,

        /// <summary>
        /// Defines the UnidentifiedLosses
        /// </summary>
        UnidentifiedLosses = 3,

        /// <summary>
        /// Defines the Finalize
        /// </summary>
        Unbalance = 4,

        /// <summary>
        /// Defines the Finalize
        /// </summary>
        Finalize = 5,

        /// <summary>
        /// Defines the Failed
        /// </summary>
        Failed = 6,
    }
}
