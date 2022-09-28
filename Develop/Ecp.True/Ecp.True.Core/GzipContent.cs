// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GzipContent.cs" company="Microsoft">
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
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Custom Gzip Content.
    /// </summary>
    /// <seealso cref="System.Net.Http.HttpContent" />
    public class GzipContent : HttpContent
    {
        private readonly HttpContent content;

        /// <summary>
        /// Initializes a new instance of the <see cref="GzipContent"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public GzipContent(HttpContent content)
        {
            this.content = content;

            if (this.content != null)
            {
                foreach (var header in this.content.Headers)
                {
                    this.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                this.Headers.ContentEncoding.Add(Constants.GzipContent);
            }
        }

        /// <inheritdoc/>
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using (var gzip = new GZipStream(stream, CompressionMode.Compress, true))
            {
                return this.content.CopyToAsync(gzip);
            }
        }

        /// <inheritdoc/>
        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}
