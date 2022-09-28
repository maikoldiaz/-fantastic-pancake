// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Disposable.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The Disposable.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    [ExcludeFromCodeCoverage]
    public class Disposable : IDisposable
    {
        /// <summary>
        /// Finalizes an instance of the <see cref="Disposable" /> class.
        /// </summary>
        ~Disposable()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Disposable" /> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    this.DisposeManagedResources();
                }

                this.DisposeUnmanagedResources();
                this.Disposed = true;
            }
        }

        /// <summary>
        /// Disposes the managed resources.
        /// </summary>
        protected virtual void DisposeManagedResources()
        {
        }

        /// <summary>
        /// Disposes the unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
        }
    }
}