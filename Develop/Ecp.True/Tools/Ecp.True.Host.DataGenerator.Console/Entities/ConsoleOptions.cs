// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleOptions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Entities
{
    using CommandLine;

    /// <summary>
    /// The ConsoleOptions.
    /// </summary>
    public class ConsoleOptions
    {
        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        [Option('s', "segment", Required = false, HelpText = "Segment is required.")]
        public int Segment { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Option('i', "startDate", Required = false, HelpText = "StartDate is required.")]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Option('f', "endDate", Required = false, HelpText = "EndDate is required.")]
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is cancellation case.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is cancellation case; otherwise, <c>false</c>.
        /// </value>
        [Option('c', "isCancellationCase", Required = false, HelpText = "IsCancellationCase is required.")]
        public bool IsCancellationCase { get; set; }
    }
}
