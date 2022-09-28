// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Option.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The Option.
    /// </summary>
    public class Option
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Option" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="processCallbackAsync">The process callback asynchronous.</param>
        public Option(string name, Func<Task> processCallbackAsync)
        {
            this.Name = name;
            this.ProcessCallbackAsync = processCallbackAsync;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the process callback asynchronous.
        /// </summary>
        /// <value>
        /// The process callback asynchronous.
        /// </value>
        public Func<Task> ProcessCallbackAsync { get; private set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A string that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
