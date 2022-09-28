// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstrumentConstants.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Logging
{
    /// <summary>
    /// Instrumentation class.
    /// </summary>
    public static class InstrumentConstants
    {
        /// <summary>
        /// The activity identifier header.
        /// </summary>
        public static readonly string ActivityIdHeader = "Request-Id";

        /// <summary>
        /// The source file name.
        /// </summary>
        public static readonly string SourceFileName = "SourceFile";

        /// <summary>
        /// The source line name.
        /// </summary>
        public static readonly string SourceLineName = "SourceLine";

        /// <summary>
        /// The user message.
        /// </summary>
        public static readonly string UserMessage = "UserMessage";

        /// <summary>
        /// The calling member name.
        /// </summary>
        public static readonly string CallingMemberName = "CallingMember";

        /// <summary>
        /// The tag unique identifier name.
        /// </summary>
        public static readonly string TagGuidName = "Tag";

        /// <summary>
        /// The GTS logger name.
        /// </summary>
        public static readonly string TrueLoggerName = "TRUE";
    }
}