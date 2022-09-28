// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditedStep.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Stubs.Api.Response
{
    using System.ComponentModel;

    /// <summary>
    /// The class AuditedStep.
    /// </summary>
    [DisplayName("AuditedSteps")]
    public class AuditedStep
    {
        /// <summary>
        /// Gets or sets the step name.
        /// </summary>
        /// <value>
        /// The step name.
        /// </value>
        public string StepName { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the step message.
        /// </summary>
        /// <value>
        /// The step message.
        /// </value>
        public string StepMessage { get; set; }

        /// <summary>
        /// Gets or sets the step number.
        /// </summary>
        /// <value>
        /// The step number.
        /// </value>
        public int StepNumber { get; set; }
    }
}
