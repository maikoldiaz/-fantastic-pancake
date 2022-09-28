// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementOwnershipDetails.cs" company="Microsoft">
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
    using Nethereum.ABI.FunctionEncoding.Attributes;

    [FunctionOutput]
    public class MovementOwnershipDetailsAfterCalculation
    {
        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [Parameter("string", "BlockchainMovementTransactionId", 1)]
        public string BlockchainMovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Parameter("string", "OwnershipId", 2)]
        public string OwnershipId { get; set; }

        [Parameter("int64", "OwnerId", 3)]
        public long OwnerId { get; set; }

        [Parameter("int256", "OwnershipVolume", 4)]
        public long OwnershipVolume { get; set; }

        [Parameter("int256", "OwnershipPercentage", 5)]
        public long OwnershipPercentage { get; set; }

        [Parameter("string", "AppliedRule", 6)]
        public string AppliedRule { get; set; }

        [Parameter("string", "RuleVersion", 7)]
        public string RuleVersion { get; set; }

        [Parameter("int64", "TicketId", 8)]
        public int TicketId { get; set; }
    }
}