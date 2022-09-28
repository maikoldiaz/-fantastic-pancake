using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ecp.True.Stubs.Api
{
    /// <summary>
    /// Request Handler
    /// </summary>
    public sealed class RequestHandler
    {
        private readonly RequestDelegate next;
        private const string ContentEncodingHeader = "Content-Encoding";
        private const string ContentEncodingGzip = "gzip";

        public RequestHandler(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains(ContentEncodingHeader) && (context.Request.Headers[ContentEncodingHeader] == ContentEncodingGzip))
            {
                var umcompressedPayload = (Stream)new GZipStream(context.Request.Body, CompressionMode.Decompress, true);
                context.Request.Body = umcompressedPayload;
            }

            await next(context).ConfigureAwait(false);
        }
    }
}
