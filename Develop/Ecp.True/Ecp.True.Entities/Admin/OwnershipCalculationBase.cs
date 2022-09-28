// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipCalculationBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// SegmentOwnershipCalculation Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class OwnershipCalculationBase : Entity
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the Measurement Unit identifier.
        /// </summary>
        /// <value>
        /// The Measurement Unit identifier.
        /// </value>
        public int? MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership ticket identifier.
        /// </summary>
        /// <value>
        /// The ownership ticket identifier.
        /// </value>
        public int? OwnershipTicketId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the initial inventory volume.
        /// </summary>
        /// <value>
        /// The initial inventory volume.
        /// </value>
        public decimal? InitialInventoryVolume { get; set; }

        /// <summary>
        /// Gets or sets the initial inventory percentage.
        /// </summary>
        /// <value>
        /// The initial inventory percentage.
        /// </value>
        public decimal? InitialInventoryPercentage { get; set; }

        /// <summary>
        /// Gets or sets the final inventory volume.
        /// </summary>
        /// <value>
        /// The final inventory volume.
        /// </value>
        public decimal? FinalInventoryVolume { get; set; }

        /// <summary>
        /// Gets or sets the final inventory percentage.
        /// </summary>
        /// <value>
        /// The final inventory percentage.
        /// </value>
        public decimal? FinalInventoryPercentage { get; set; }

        /// <summary>
        /// Gets or sets the input volume.
        /// </summary>
        /// <value>
        /// The input volume.
        /// </value>
        public decimal? InputVolume { get; set; }

        /// <summary>
        /// Gets or sets the input percentage.
        /// </summary>
        /// <value>
        /// The input percentage.
        /// </value>
        public decimal? InputPercentage { get; set; }

        /// <summary>
        /// Gets or sets the output volume.
        /// </summary>
        /// <value>
        /// The output volume.
        /// </value>
        public decimal? OutputVolume { get; set; }

        /// <summary>
        /// Gets or sets the output percentage.
        /// </summary>
        /// <value>
        /// The output percentage.
        /// </value>
        public decimal? OutputPercentage { get; set; }

        /// <summary>
        /// Gets or sets the identified losses volume.
        /// </summary>
        /// <value>
        /// The identified losses volume.
        /// </value>
        public decimal? IdentifiedLossesVolume { get; set; }

        /// <summary>
        /// Gets or sets the identified losses percentage.
        /// </summary>
        /// <value>
        /// The identified losses percentage.
        /// </value>
        public decimal? IdentifiedLossesPercentage { get; set; }

        /// <summary>
        /// Gets or sets the unbalance volume.
        /// </summary>
        /// <value>
        /// The unbalance volume.
        /// </value>
        public decimal? UnbalanceVolume { get; set; }

        /// <summary>
        /// Gets or sets the unbalance percentage.
        /// </summary>
        /// <value>
        /// The unbalance percentage.
        /// </value>
        public decimal? UnbalancePercentage { get; set; }

        /// <summary>
        /// Gets or sets the interface volume.
        /// </summary>
        /// <value>
        /// The interface volume.
        /// </value>
        public decimal? InterfaceVolume { get; set; }

        /// <summary>
        /// Gets or sets the interface percentage.
        /// </summary>
        /// <value>
        /// The interface percentage.
        /// </value>
        public decimal? InterfacePercentage { get; set; }

        /// <summary>
        /// Gets or sets the tolerance volume.
        /// </summary>
        /// <value>
        /// The tolerance volume.
        /// </value>
        public decimal? ToleranceVolume { get; set; }

        /// <summary>
        /// Gets or sets the tolerance percentage.
        /// </summary>
        /// <value>
        /// The tolerance percentage.
        /// </value>
        public decimal? TolerancePercentage { get; set; }

        /// <summary>
        /// Gets or sets the unidentified losses volume.
        /// </summary>
        /// <value>
        /// The unidentified losses volume.
        /// </value>
        public decimal? UnidentifiedLossesVolume { get; set; }

        /// <summary>
        /// Gets or sets the unidentified losses percentage.
        /// </summary>
        /// <value>
        /// The unidentified losses percentage.
        /// </value>
        public decimal? UnidentifiedLossesPercentage { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public virtual CategoryElement Owner { get; set; }

        /// <summary>
        /// Gets or sets the ownership ticket.
        /// </summary>
        /// <value>
        /// The ownership ticket.
        /// </value>
        public virtual Ticket OwnershipTicket { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var ownershipCalculation = (OwnershipCalculationBase)entity;
            this.InputVolume = ownershipCalculation.InputVolume;
            this.InputPercentage = ownershipCalculation.InputPercentage;
            this.OutputVolume = ownershipCalculation.OutputVolume;
            this.OutputPercentage = ownershipCalculation.OutputPercentage;
            this.InitialInventoryVolume = ownershipCalculation.InitialInventoryVolume;
            this.InitialInventoryPercentage = ownershipCalculation.InitialInventoryPercentage;
            this.FinalInventoryVolume = ownershipCalculation.FinalInventoryVolume;
            this.FinalInventoryPercentage = ownershipCalculation.FinalInventoryPercentage;
            this.InterfaceVolume = ownershipCalculation.InterfaceVolume;
            this.InterfacePercentage = ownershipCalculation.InterfacePercentage;
            this.ToleranceVolume = ownershipCalculation.ToleranceVolume;
            this.TolerancePercentage = ownershipCalculation.TolerancePercentage;
            this.UnidentifiedLossesVolume = ownershipCalculation.UnidentifiedLossesVolume;
            this.UnidentifiedLossesPercentage = ownershipCalculation.UnidentifiedLossesPercentage;
            this.IdentifiedLossesVolume = ownershipCalculation.IdentifiedLossesVolume;
            this.IdentifiedLossesPercentage = ownershipCalculation.IdentifiedLossesPercentage;
            this.UnbalanceVolume = ownershipCalculation.UnbalanceVolume;
            this.UnbalancePercentage = ownershipCalculation.UnbalancePercentage;
        }
    }
}
