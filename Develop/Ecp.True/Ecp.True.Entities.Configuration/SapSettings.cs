// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapSettings.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// The Sap Rule Settings.
    /// </summary>
    public class SapSettings
    {
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the transfer point path.
        /// </summary>
        /// <value>
        /// The ownership transfer point.
        /// </value>
        public string TransferPointPath { get; set; }

        /// <summary>
        /// Gets or sets the upload status path.
        /// </summary>
        /// <value>
        /// The upload status path.
        /// </value>
        public string UploadStatusPath { get; set; }

        /// <summary>
        /// Gets or sets the upload status contract path.
        /// </summary>
        /// <value>
        /// The upload status contract path.
        /// </value>
        public string UploadStatusContractPath { get; set; }

        /// <summary>
        /// Gets or sets the mapping path.
        /// </summary>
        /// <value>
        /// The sap mapping path.
        /// </value>
        public string MappingPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        /// <value>
        /// The retry count.
        /// </value>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the retry interval.
        /// </summary>
        /// <value>
        /// The retry interval.
        /// </value>
        public int RetryInterval { get; set; }

        /// <summary>
        /// Gets or sets the sap records max limit.
        /// </summary>
        /// <value>
        /// The sap records max limit.
        /// </value>
        public int? SapRecordsMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets the sales positions max limit.
        /// </summary>
        /// <value>
        /// The sales positions max limit.
        /// </value>
        public int? SalesPositionsMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets the purchases positions records max limit.
        /// </summary>
        /// <value>
        /// The purchases positions max limit.
        /// </value>
        public int? PurchasesPositionsMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets the logistic movement endpoint.
        /// </summary>
        /// <value>
        /// The logistic movement endpoint.
        /// </value>
        public string SendLogisticMovementsPath { get; set; }
    }
}