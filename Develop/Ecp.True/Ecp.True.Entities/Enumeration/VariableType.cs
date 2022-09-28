// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VariableType.cs" company="Microsoft">
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
    /// The File Action type.
    /// </summary>
    public enum VariableType
    {
        /// <summary>
        /// The Interfase.
        /// </summary>
        Interface = 1,

        /// <summary>
        /// The balance tolerance
        /// </summary>
        BalanceTolerance = 2,

        /// <summary>
        /// The unidentified losses
        /// </summary>
        UnidentifiedLosses = 3,

        /// <summary>
        /// The initial inventory
        /// </summary>
        InitialInventory = 4,

        /// <summary>
        /// The input
        /// </summary>
        Input = 5,

        /// <summary>
        /// The output
        /// </summary>
        Output = 6,

        /// <summary>
        /// The identified losses
        /// </summary>
        IdentifiedLosses = 7,

        /// <summary>
        /// The final inventory
        /// </summary>
        FinalInventory = 8,
    }
}
