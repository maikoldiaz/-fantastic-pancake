// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICompositeBuilder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Builders.Interfaces
{
    /// <summary>
    /// The ICompositeBuilder.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Transform.Builders.Interfaces.IBuilder{TInput}" />
    /// <seealso cref="Transform.Interfaces.IBuilder" />
    public interface ICompositeBuilder<in TInput> : IBuilder<TInput>
    {
        /// <summary>
        /// Gets the builders.
        /// </summary>
        /// <value>
        /// The builders.
        /// </value>
        IBuilder<TInput>[] Builders { get; }
    }
}
