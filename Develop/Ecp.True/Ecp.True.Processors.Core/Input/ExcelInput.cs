// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelInput.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Input
{
    using System;
    using System.Data;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The excel input.
    /// </summary>
    public class ExcelInput : InputBase<DataSet>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelInput"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public ExcelInput(DataSet element)
            : base(element)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelInput" /> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="element">The element.</param>
        public ExcelInput(Tuple<DataRow, string, bool> args, DataSet element)
            : base(args, element)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelInput" /> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="message">The message.</param>
        public ExcelInput(DataSet element, TrueMessage message)
            : base(element, message)
        {
        }
    }
}
