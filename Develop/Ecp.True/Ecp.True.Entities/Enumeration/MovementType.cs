// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementType.cs" company="Microsoft">
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
    /// The movement type.
    /// </summary>
    public enum MovementType
    {
        /// <summary>
        /// Interface
        /// </summary>
        Interface = 42,

        /// <summary>
        /// Tolerance
        /// </summary>
        Tolerance = 43,

        /// <summary>
        /// Unidentified Loss
        /// </summary>
        UnidentifiedLoss = 44,

        /// <summary>
        /// Transfer
        /// </summary>
        Transfer = 48,

        /// <summary>
        /// Purchase
        /// </summary>
        Purchase = 49,

        /// <summary>
        /// Sale
        /// </summary>
        Sale = 50,

        /// <summary>
        /// EVACUACION ENTRADA MOVEMENT
        /// </summary>
        InputEvacuation = 153,

        /// <summary>
        /// EVACUACION SALIDA MOVEMENT
        /// </summary>
        OutputEvacuation = 154,

        /// <summary>
        /// ANULACION ENTRADA MOVEMENT
        /// </summary>
        InputCancellation = 155,

        /// <summary>
        /// ANULACION SALIDA MOVEMENT
        /// </summary>
        OutputCancellation = 156,

        /// <summary>
        /// Input Collaboration Agreements
        /// </summary>
        InputCollaborationAgreement = 157,

        /// <summary>
        /// Output Collaboration Agreements
        /// </summary>
        OutputCollaborationAgreement = 158,

        /// <summary>
        /// DeltaInventory
        /// </summary>
        DeltaInventory = 187,

        /// <summary>
        /// SelfConsumption
        /// </summary>
        SelfConsumption = 191,

        /// <summary>
        /// CancellationTransferConciliation
        /// </summary>
        CancellationTransferConciliation = 228,

        /// <summary>
        /// SMConciliation
        /// </summary>
        SMConciliation = 229,

        /// <summary>
        /// EMConciliation
        /// </summary>
        EMConciliation = 230,

        /// <summary>
        /// ConciliationTransfer
        /// </summary>
        ConciliationTransfer = 231,

        /// <summary>
        /// Delta Annulation EM Evacuation
        /// </summary>
        DeltaAnnulationEMEvacuation = 232,

        /// <summary>
        /// Delta Annulation SM Evacuation
        /// </summary>
        DeltaAnnulationSMEvacuation = 233,
    }
}
