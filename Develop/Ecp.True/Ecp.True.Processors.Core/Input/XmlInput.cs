﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlInput.cs" company="Microsoft">
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
    using System.Xml.Linq;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The XML input type.
    /// </summary>
    public class XmlInput : InputBase<XElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlInput"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public XmlInput(XElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlInput" /> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="message">The message.</param>
        public XmlInput(XElement element, TrueMessage message)
            : base(element, message)
        {
        }
    }
}
