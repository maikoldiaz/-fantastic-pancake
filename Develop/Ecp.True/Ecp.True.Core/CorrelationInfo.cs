// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CorrelationInfo.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core
{
    using System;

    /// <summary>
    /// Correlation Information.
    /// </summary>
    public static class CorrelationInfo
    {
        /// <summary>
        /// The operation identifier.
        /// </summary>
        public static readonly string OperationId = "Request-Id";

        /// <summary>
        /// The API host default.
        /// </summary>
        public static readonly Guid DefaultApiHostId = new Guid("11111111-1111-1111-1111-111111111111");

        /// <summary>
        /// The logger default.
        /// </summary>
        public static readonly Guid DefaultLoggerId = new Guid("99999999-9999-9999-9999-999999999999");

        /// <summary>
        /// The business context.
        /// </summary>
        public static readonly string BusinessContext = "BusinessContext";
    }
}