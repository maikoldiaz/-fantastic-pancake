// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReconciliationSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    using System;
    using Ecp.True.Core;

    /// <summary>
    /// The reconciliation settings.
    /// </summary>
    public class ReconciliationSettings
    {
        /// <summary>
        /// Gets the failure batch.
        /// </summary>
        /// <value>
        /// The failure batch.
        /// </value>
        public static int FailureBatch => 100;

        /// <summary>
        /// Gets or sets the reconcile interval in minutes.
        /// </summary>
        /// <value>
        /// The reconcile interval in minutes.
        /// </value>
        public int? ReconcileIntervalInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the maximum retry count.
        /// </summary>
        /// <value>
        /// The maximum retry count.
        /// </value>
        public int? MaxRetryCount { get; set; }

        /// <summary>
        /// Gets or sets the batch size.
        /// </summary>
        /// <value>
        /// The batch size.
        /// </value>
        public int? BatchSize { get; set; }

        /// <summary>
        /// Gets the default interval.
        /// </summary>
        /// <value>
        /// The default interval.
        /// </value>
        public int DefaultInterval => this.ReconcileIntervalInMinutes.GetValueOrDefault(120);

        /// <summary>
        /// Gets the default retries.
        /// </summary>
        /// <value>
        /// The default retries.
        /// </value>
        public int DefaultRetries => this.MaxRetryCount.GetValueOrDefault(3);

        /// <summary>
        /// Gets the default batch.
        /// </summary>
        /// <value>
        /// The default batch.
        /// </value>
        public int DefaultBatch => this.BatchSize.GetValueOrDefault(1000);

        /// <summary>
        /// Gets the maximum date time.
        /// </summary>
        /// <value>
        /// The maximum date time.
        /// </value>
        public DateTime MaxDateTime => DateTime.UtcNow.ToTrue().AddMinutes(-this.DefaultInterval);
    }
}
