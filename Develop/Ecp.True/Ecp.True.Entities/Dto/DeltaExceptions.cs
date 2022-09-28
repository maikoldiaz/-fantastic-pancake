// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaExceptions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Origin.
    /// </summary>
    /// <seealso cref="Entity" />
    public class DeltaExceptions : Entity
    {
        /// <summary>Gets or sets the Inventory or Movement Id.</summary>
        /// <value>The Inventory or Movement  node.</value>
        public string Identifier { get; set; }

        /// <summary>Gets or sets the Type.</summary>
        /// <value>The Type.</value>
        public string Type { get; set; }

        /// <summary>Gets or sets the source node.</summary>
        /// <value>The origin source node.</value>
        public string SourceNode { get; set; }

        /// <summary>Gets or sets the destination node.</summary>
        /// <value>The destination node.</value>
        public string DestinationNode { get; set; }

        /// <summary>Gets or sets the source product.</summary>
        /// <value>The source product.</value>
        public string SourceProduct { get; set; }

        /// <summary>Gets or sets the destination product.</summary>
        /// <value>The destination product.</value>
        public string DestinationProduct { get; set; }

        /// <summary>Gets or sets the Quantity.</summary>
        /// <value>The Quantity.</value>
        public decimal Quantity { get; set; }

        /// <summary>Gets or sets the measurement.</summary>
        /// <value>The measurement.</value>
        public string Unit { get; set; }

        /// <summary>Gets or sets the date.</summary>
        /// <value>The date.</value>
        public DateTime? Date { get; set; }

        /// <summary>Gets or sets the Error.</summary>
        /// <value>The Error.</value>
        public string Error { get; set; }
    }
}