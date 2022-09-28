// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalculatedVariable.cs" company="Microsoft">
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
    using System;

    /// <summary>
    /// The class CalculatedVariable.
    /// </summary>
    public class CalculatedVariable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// Return the name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// Return the scope.
        /// </value>
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// Return the type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the StringVal.
        /// </summary>
        /// <value>
        /// Return the StringVal.
        /// </value>
        public string StringVal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the BooleanVal.
        /// </summary>
        /// <value>
        /// Return the BooleanVal.
        /// </value>
        public bool BooleanVal { get; set; }

        /// <summary>
        /// Gets or sets the DoubleVal.
        /// </summary>
        /// <value>
        /// Return the DoubleVal.
        /// </value>
        public decimal DoubleVal { get; set; }

        /// <summary>
        /// Gets or sets the StringVal.
        /// </summary>
        /// <value>
        /// Return the StringVal.
        /// </value>
        public int IntegerVal { get; set; }

        /// <summary>
        /// Gets or sets the DateVal.
        /// </summary>
        /// <value>
        /// Return the DateVal.
        /// </value>
        public DateTime DateVal { get; set; }
    }
}
