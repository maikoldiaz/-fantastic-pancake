// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Transform
{
    using System;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The registration data.
    /// </summary>
    public class RegistrationData : OrchestratorMetaData
    {
        /// <summary>
        /// Gets or sets the invocation identifier.
        /// </summary>
        /// <value>
        /// The invocation identifier.
        /// </value>
        public Guid InvocationId { get; set; }

        /// <summary>
        /// Gets or sets the true message.
        /// </summary>
        /// <value>
        /// The true message.
        /// </value>
        public TrueMessage TrueMessage { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public JArray Data { get; set; }
    }
}
