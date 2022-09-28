// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlType.cs" company="Microsoft">
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
    /// The file upload status.
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// The initial inventory.
        /// </summary>
        INITIALINVENTORY = 1,

        /// <summary>
        /// The final inventory.
        /// </summary>
        FINALINVENTORY = 2,

        /// <summary>
        /// The input.
        /// </summary>
        INPUT = 3,

        /// <summary>
        /// The output
        /// </summary>
        OUTPUT = 4,
    }
}
