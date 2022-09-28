// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance
{
    /// <summary>
    /// The constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The under score punctuation.
        /// </summary>
        public static readonly string BalanceCalculationGeneralError = $"Se presentó un error inesperado " +
            "durante la ejecución del corte operativo. Si este tipo de errores son frecuentes, utilice el siguiente identificador " +
            "{0} para reportar el error al administrador de la plataforma.";

        /// <summary>
        /// The calculation BLOB path.
        /// </summary>
        public static readonly string CalculationBlobPath = "system/json/movements/";

        /// <summary>
        /// The container name.
        /// </summary>
        public static readonly string ContainerName = "true";
    }
}
