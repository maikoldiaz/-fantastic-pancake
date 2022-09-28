// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceTolerance.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Calculation.Output
{
    /// <summary>
    /// The balance tolerance.
    /// </summary>
    public class BalanceTolerance : OutputBase
    {
        /// <summary>
        /// Gets or sets the tolerance.
        /// </summary>
        /// <value>
        /// The tolerance.
        /// </value>
        public decimal Tolerance { get; set; }

        /// <summary>
        /// Gets or sets the Standard Uncertainty.
        /// </summary>
        /// <value>
        /// The standard Uncertainty.
        /// </value>
        public decimal StandardUncertainty { get; set; }
    }
}
